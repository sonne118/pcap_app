#ifndef IPC_H
#define IPC_H
#include <tchar.h>
#include <packages.h>
#include <thread> 
#include <memory>
#include <ipc.h>
#include <atlsafe.h>
#include <random>
#include <functional>
#include <openDevices.h>
#include <vector>
#include <string.h>
#include <mutex>

int mainFunc(HANDLE eventHandle);

std::atomic_bool quit_flag(false);
std::atomic<int> d1;

std::mutex m;
std::condition_variable cv;

pcap_t* _adhandle1;

HANDLE hPipe = ::CreateNamedPipe(_T("\\\\.\\pipe\\testpipe"),
	PIPE_ACCESS_DUPLEX,
	PIPE_TYPE_BYTE | PIPE_READMODE_BYTE,
	PIPE_UNLIMITED_INSTANCES,
	4096,
	4096,
	0,
	NULL);

extern "C" __declspec(dllexport) LPSAFEARRAY fnDevCPPDLL();

LPSAFEARRAY fnDevCPPDLL()
{
	std::vector<std::string> listdev; OpDevices* oPdev;
	oPdev = new OpDevices();
	listdev = oPdev->listalldevs();
	CComSafeArray<BSTR> a(listdev.size());

	std::vector<std::string>::const_iterator it;
	int i = 0;
	for (it = listdev.begin(); it != listdev.end(); ++it, ++i)
	{
		a.SetAt(i, A2BSTR_EX((*it).c_str()), FALSE);
	}
	return a.Detach();
}


extern "C" {
	void __declspec(dllexport) __stdcall fnCPPDLL(int d)
	{
		HANDLE eventHandle = NULL;
		DWORD IDThread;
		HANDLE hThread;

		const WCHAR* initializedEventName = L"Global\\sniffer";
		int attempt = 0;

		while ((eventHandle = OpenEventW(EVENT_MODIFY_STATE | SYNCHRONIZE, FALSE, initializedEventName)) == NULL && attempt < 200000)
		{
			Sleep(1000);
			++attempt;
			std::cout << "Attempt:   " << attempt << std::endl;//tester
		}
		if (eventHandle != NULL)
		{
			std::cout << "Event" << eventHandle << std::endl;//tester
		}

		hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)mainFunc(eventHandle), &eventHandle, 0, &IDThread);

		std::cout << " main thread " << std::endl;

		CloseHandle(eventHandle);
		CloseHandle(hThread);
	}
}

extern "C"
{
	void __declspec(dllexport) __stdcall fnPutdevCPPDLL(int dev) {

		d1 = dev;
		OpDevices* oPdev;
		pcap_t* _adhandle2 = NULL;
		_adhandle1 = NULL;

		if (quit_flag)
			quit_flag = false;

		std::this_thread::sleep_for(std::chrono::milliseconds(2000));
		//TODO: pattern builder
		oPdev = new OpDevices();
		oPdev->SetInum(dev);
		oPdev->Findalldevs();
		_adhandle1 = oPdev->OpenDevices();
		{
			std::unique_lock lk(m);
			quit_flag = true;
		}
		oPdev->~OpDevices();
		cv.notify_one();
	}
}
#endif 

