@echo off
REM Usage: build.bat [Debug|Release] [target]
set BUILD_TYPE=%1
if "%BUILD_TYPE%"=="" set BUILD_TYPE=Debug
set TARGET=%2
REM Create and enter build directory
if not exist build mkdir build
cd build
REM Generate build system (adjust generator/version as needed)
cmake .. -G "Visual Studio 17 2022" -A x64 -DCMAKE_BUILD_TYPE=%BUILD_TYPE%
REM Build solution (optionally target)
if "%TARGET%"=="" (
  cmake --build . --config %BUILD_TYPE%
) else (
  cmake --build . --config %BUILD_TYPE% --target %TARGET%
)
echo Build complete (%BUILD_TYPE%)
echo Libraries (if any) in: %cd%\%BUILD_TYPE%
