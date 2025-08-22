#!/usr/bin/env bash
set -euo pipefail

# Usage: ./build.sh [Debug|Release] [target]
BUILD_TYPE="${1:-Debug}"
TARGET="${2:-}"

command -v cmake >/dev/null 2>&1 || { echo "cmake not found. Install with: brew install cmake"; exit 1; }

# Determine parallel job count (macOS/Linux) and sanitize to a single integer
CORES="$(
  command -v getconf >/dev/null 2>&1 && getconf _NPROCESSORS_ONLN 2>/dev/null ||
  command -v sysctl >/dev/null 2>&1 && sysctl -n hw.ncpu 2>/dev/null ||
  echo 4
)"
# Keep only the first line and digits to avoid cases like "12\n12"
CORES="$(printf '%s\n' "$CORES" | head -n1 | tr -cd '0-9')"
[ -z "$CORES" ] && CORES=4

# Ensure we're in the right directory
if [[ ! -f "CMakeLists.txt" && -d "sniffer_packages" ]]; then
  echo "Warning: No CMakeLists.txt found in current directory. Is this the project root?"
  echo "Contents of current directory: $(ls -la)"
fi

mkdir -p build
cd build

# Configure with verbose output
if [ ! -f CMakeCache.txt ] || ! grep -q "CMAKE_BUILD_TYPE:STRING=${BUILD_TYPE}" CMakeCache.txt; then
  echo "Configuring (${BUILD_TYPE})..."
  cmake .. -DCMAKE_BUILD_TYPE="${BUILD_TYPE}" -DCMAKE_VERBOSE_MAKEFILE=ON
fi

echo "Building with ${CORES} cores..."
if [ -n "${TARGET}" ]; then
  echo "Target: ${TARGET}"
  # Use CMake's native -j to control parallelism; avoid passing -j directly to make
  cmake --build . --target "${TARGET}" -j "${CORES}" -- VERBOSE=1
else
  echo "Building all targets..."
  # List available targets
  echo "Available targets:"
  cmake --build . --target help
  
  # Build default target
  # Use CMake's native -j to control parallelism; avoid passing -j directly to make
  cmake --build . -j "${CORES}" -- VERBOSE=1
fi

# Check if build succeeded for sniffer_packages
if [ -z "${TARGET}" ] || [ "${TARGET}" = "sniffer_packages" ]; then
  echo "Checking sniffer_packages build results..."
  if [ -d "lib" ]; then
    echo "Library directory exists: $(ls -la lib)"
    if [ -f "lib/libsniffer_packages.so" ] || [ -f "lib/libsniffer_packages.dylib" ] || [ -f "lib/sniffer_packages.dll" ]; then
      echo "✅ sniffer_packages library was built successfully."
    else
      echo "❌ sniffer_packages library was not built."
      echo "Please check:"
      echo "1. All source files exist (mainFunc.cpp, ipc.cpp, packages_globals.cpp)"
      echo "2. Source files are in the correct location: $(pwd)/../sniffer_packages/"
    fi
  else
    echo "❌ Library directory not found!"
  fi
fi

echo "Build complete (type: ${BUILD_TYPE})"

# Library location hint (after CMakeLists change sets lib dir)
if [ -d "lib" ]; then
  echo "Libraries in: $(pwd)/lib"
fi
