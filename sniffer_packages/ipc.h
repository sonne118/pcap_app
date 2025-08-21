#ifndef IPC_H
#define IPC_H

#include <atomic>
#include <condition_variable>
#include <mutex>
#include <string>
#include <vector>
#include <thread>
#include <pcap.h>

#ifdef _WIN32
  #include <windows.h>
  #include <winsock2.h>
  #include <tchar.h>
#else
  typedef void* HANDLE;
  #ifndef __stdcall
    #define __stdcall
  #endif
#endif

// Export macro
#ifdef _WIN32
  #define IPC_EXPORT extern "C" __declspec(dllexport)
#else
  #define IPC_EXPORT extern "C"
#endif

// Main capture function
int mainFunc(HANDLE eventHandle);

// Extern globals (defined in ipc.cpp)
extern std::atomic_bool quit_flag;
extern std::atomic<int> d1;
extern std::mutex m;
extern std::condition_variable cv;
extern pcap_t* _adhandle1;
#ifdef _WIN32
extern HANDLE hPipe;
#endif

// Exported API declarations
IPC_EXPORT void fnDevCPPDLL(char** data, int* sizes, int* count);
#ifdef _WIN32
IPC_EXPORT void __stdcall fnCPPDLL(int d);
IPC_EXPORT void __stdcall fnPutdevCPPDLL(int dev);
#else
IPC_EXPORT void fnCPPDLL(int d);
IPC_EXPORT void fnPutdevCPPDLL(int dev);
#endif

#endif // IPC_H
