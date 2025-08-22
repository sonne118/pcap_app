// Inter-process communication implementation
#include "ipc.h"

// Placeholder implementation - replace with actual code
void initializeIPC() {
    // IPC initialization code
}

#include <iostream>
#include <cstring>
#include "builderDevice.h"

#ifdef _WIN32
#include <atlsafe.h>
#include <tchar.h>
#include <windows.h>
#include <thread>

// Global variable definitions
std::atomic_bool quit_flag(false);
std::atomic<int> d1;
std::mutex m;
std::condition_variable cv;
pcap_t* _adhandle1 = nullptr;

// Named pipe server handle (created later in fnCPPDLL)
HANDLE hPipe = INVALID_HANDLE_VALUE;

// Exported functions
IPC_EXPORT void fnDevCPPDLL(char** data, int* sizes, int* count) {
    std::vector<std::string> listdev = builderDevice::Builder(0).ListDev().build().getDevices();
    *count = listdev.size();
    if (data) {
        for (int i = 0; i < *count; ++i) {
            sizes[i] = listdev[i].size();
            data[i] = new char[sizes[i]+1];
            std::strcpy(data[i], listdev[i].c_str());
        }
    }
}

IPC_EXPORT void __stdcall fnCPPDLL(int d) {
    HANDLE eventHandle = NULL;
    DWORD IDThread;
    const WCHAR* name = L"Global\\sniffer";
    int attempt = 0;
    while ((eventHandle = OpenEventW(EVENT_MODIFY_STATE | SYNCHRONIZE, FALSE, name)) == NULL && attempt < 200000) {
        Sleep(1000); ++attempt;
        std::cout << "Attempt: " << attempt << std::endl;
    }

    // Create and connect the named pipe server if not already done
    if (hPipe == INVALID_HANDLE_VALUE) {
        hPipe = CreateNamedPipeW(
            L"\\\\.\\pipe\\testpipe",
            PIPE_ACCESS_DUPLEX,
            PIPE_TYPE_BYTE | PIPE_READMODE_BYTE | PIPE_WAIT,
            1,                 // max instances
            4096, 4096,        // out/in buffer sizes
            0,                 // default timeout
            NULL);             // default security

        if (hPipe == INVALID_HANDLE_VALUE) {
            DWORD le = GetLastError();
            std::cout << "CreateNamedPipe failed. GLE=" << le << std::endl;
        } else {
            BOOL connected = ConnectNamedPipe(hPipe, NULL) ? TRUE : (GetLastError() == ERROR_PIPE_CONNECTED);
            if (!connected) {
                DWORD le = GetLastError();
                std::cout << "ConnectNamedPipe failed. GLE=" << le << std::endl;
                CloseHandle(hPipe);
                hPipe = INVALID_HANDLE_VALUE;
            } else {
                std::cout << "Named pipe connected" << std::endl;
            }
        }
    }

    std::thread t(mainFunc, eventHandle);
    t.detach();
    std::cout << "main thread started" << std::endl;
}

IPC_EXPORT void __stdcall fnPutdevCPPDLL(int dev) {
    d1 = dev; _adhandle1 = nullptr;
    builderDevice::Builder builder(dev);
    builderDevice builderdev(builder);
    if (quit_flag) quit_flag = false;
    std::this_thread::sleep_for(std::chrono::milliseconds(2000));
    _adhandle1 = builder.Finddev().OpenDevices().build().getHandler();
    {
        std::unique_lock<std::mutex> lock(m);
        quit_flag = true;
    }
    cv.notify_one();
    std::this_thread::sleep_for(std::chrono::milliseconds(20));
}

#else // Unix/Linux
#include <chrono>
#include <thread>
#include <sys/stat.h>
#include <sys/types.h>
#include <fcntl.h>
#include <unistd.h>
#include <errno.h>

// Global variable definitions
std::atomic_bool quit_flag(false);
std::atomic<int> d1;
std::mutex m;
std::condition_variable cv;
pcap_t* _adhandle1 = nullptr;

// POSIX FIFO path
static const char* PIPE_PATH = "/tmp/testpipe";
int hPipe = -1;

// Initialize and open FIFO
static void init_pipe() {
    mkfifo(PIPE_PATH, 0666);
    hPipe = open(PIPE_PATH, O_WRONLY | O_NONBLOCK);
    if (hPipe == -1) std::cerr << "FIFO open error: " << strerror(errno) << std::endl;
}

// Exported functions
IPC_EXPORT void fnDevCPPDLL(char** data, int* sizes, int* count) {
    std::vector<std::string> listdev = builderDevice::Builder(0).ListDev().build().getDevices();
    *count = listdev.size();
    if (data) {
        for (int i = 0; i < *count; ++i) {
            sizes[i] = listdev[i].size();
            data[i] = new char[sizes[i]+1];
            std::strcpy(data[i], listdev[i].c_str());
        }
    }
}

IPC_EXPORT void fnCPPDLL(int d) {
    std::thread t([](){
        init_pipe();
        mainFunc((HANDLE)0);
    });
    t.detach();
    std::cout << "main thread started (Linux)" << std::endl;
}

IPC_EXPORT void fnPutdevCPPDLL(int dev) {
    d1 = dev; _adhandle1 = nullptr;
    builderDevice::Builder builder(dev);
    builderDevice builderdev(builder);
    if (quit_flag) quit_flag = false;
    std::this_thread::sleep_for(std::chrono::milliseconds(2000));
    _adhandle1 = builder.Finddev().OpenDevices().build().getHandler();
    {
        std::unique_lock<std::mutex> lock(m);
        quit_flag = true;
    }
    cv.notify_one();
    std::this_thread::sleep_for(std::chrono::milliseconds(20));
}
#endif
