cmake_minimum_required(VERSION 3.21)

set_source_files_properties(
    ${CMAKE_SOURCE_DIR}/sniffer_packages/x64/Debug/.NETFramework,Version=v4.7.2.AssemblyAttributes.cpp
    PROPERTIES
    COMPILE_OPTIONS "/clr"
)
project(sniffer_packages LANGUAGES C CXX)

# Sources from sniffer_packages/
# Add this line to include the directory containing packages.h
include_directories(${CMAKE_SOURCE_DIR}/sniffer_packages)
set(SNIFFER_SOURCES
    mainFunc.cpp
    builderDevice.h
    ether_ntoa.h
    handleProto.h
    ipc.h
    packages.h
    struct.h
)

add_library(sniffer_packages SHARED ${SNIFFER_SOURCES})

# Language/standard
target_compile_features(sniffer_packages PUBLIC cxx_std_20)

# Definitions (mirror vcxproj)
target_compile_definitions(sniffer_packages
    PRIVATE
        $<$<CONFIG:Debug>:_DEBUG>
        _CONSOLE
        _CRT_SECURE_NO_WARNINGS
)

# Define WINx64 on 64-bit like in Debug|x64
if(CMAKE_SIZEOF_VOID_P EQUAL 8)
  target_compile_definitions(sniffer_packages PRIVATE WINx64)
endif()

# Includes: project local
target_include_directories(sniffer_packages
    PRIVATE
        ${CMAKE_CURRENT_SOURCE_DIR}
)

# Warnings and runtime per-compiler
if(MSVC)
  # clang-cl also sets MSVC=ON; restrict MSVC-only flags to the MSVC compiler
  if(CMAKE_CXX_COMPILER_ID STREQUAL "MSVC")
    target_compile_options(sniffer_packages PRIVATE /permissive- /Zc:twoPhase- /Zc:strictStrings- /W4)
  else()
    # clang-cl: use MSVC-style warning level
    target_compile_options(sniffer_packages PRIVATE /W4)
  endif()
  # MultiThreadedDebugDLL (/MDd) in Debug, MultiThreadedDLL (/MD) in others
  set_property(TARGET sniffer_packages PROPERTY MSVC_RUNTIME_LIBRARY "MultiThreaded$<$<CONFIG:Debug>:Debug>DLL")
elseif(CMAKE_CXX_COMPILER_ID MATCHES "Clang|GNU")
  target_compile_options(sniffer_packages PRIVATE -Wall -Wextra -Wpedantic)
endif()

# On Windows, export all symbols when building a shared lib (helps when no .def file is provided)
if(WIN32)
  set(CMAKE_WINDOWS_EXPORT_ALL_SYMBOLS ON)
endif()

# Try to use vcpkg (recommended)
# - Requires: vcpkg integrate via toolchain, e.g. -DCMAKE_TOOLCHAIN_FILE=.../vcpkg.cmake
# - Ports: pcapplusplus, pthreads (or you can rely on system pthreadVC2.lib), and system libs (ws2_32, iphlpapi)
find_package(PcapPlusPlus CONFIG QUIET)

# Select pthreads library name depending on toolchain
set(PTHREAD_LIB pthreadVC2)
if(CMAKE_CXX_COMPILER_ID MATCHES "Clang|GNU" AND NOT MSVC)
  set(PTHREAD_LIB pthread)
endif()

if(PcapPlusPlus_FOUND)
  target_link_libraries(sniffer_packages
      PRIVATE
        PcapPlusPlus::Pcap++
        PcapPlusPlus::Packet++
        PcapPlusPlus::Common++
        ws2_32
        iphlpapi
  )
else()
  # Fallback to local SDK paths (mirrors .vcxproj)
  # 1) WpdPack (WinPcap/Npcap SDK)
  set(WPDPACK_ROOT "${CMAKE_CURRENT_LIST_DIR}/../header/WpdPack/WpdPack" CACHE PATH "Path to WpdPack root")
  target_include_directories(sniffer_packages PRIVATE
      "${WPDPACK_ROOT}/Include"
      "${WPDPACK_ROOT}/Include/pcap"
  )
  # Library paths; add both in case
  if(CMAKE_SIZEOF_VOID_P EQUAL 8)
    link_directories("${WPDPACK_ROOT}/Lib/x64")
  endif()
  link_directories("${WPDPACK_ROOT}/Lib")

  # 2) PcapPlusPlus (prebuilt libs) — provide directory via cache var
  set(PCAPPP_LIB_DIR "" CACHE PATH "Path to PcapPlusPlus prebuilt lib directory (containing Common++.lib, Packet++.lib, Pcap++.lib)")
  if(PCAPPP_LIB_DIR)
    link_directories("${PCAPPP_LIB_DIR}")
  endif()

  target_link_libraries(sniffer_packages
      PRIVATE
        ws2_32
        iphlpapi
        wpcap
        Packet
        ${PTHREAD_LIB}
        Pcap++
        Packet++
        Common++
  )
endif()
