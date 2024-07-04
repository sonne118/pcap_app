#include <packages.h>
#include <packages.cpp>
#include <iostream>
#include <string.h>
#include <vector>
#include <thread> 
#include <memory>
#include <ipc.h>
#include <atlsafe.h>
#include <random>
#include <functional>
#include <OpenDevices.h>

std::atomic<int> d1;
std::atomic_bool quit_flag(false);


int mainFunc(HANDLE eventHandle) {

	int file = 0, dev = 4;
	pcap_t* adhandle = nullptr;
	struct pcap_pkthdr* pkthdr = nullptr;
	const u_char* packet = nullptr;

	Packages* pack = new Packages();
	pack->eventHandles = eventHandle;

	 //file = oPdev->OpDevices::OpenFile();
	if (dev || file)
	{
		std::vector<std::thread> threads;
		threads.emplace_back(std::thread(&Packages::producer, pack, std::ref(quit_flag)));
		threads.emplace_back(std::thread(&Packages::consumer, pack));
		cv.notify_one();
		for (auto& thread : threads) {
			thread.join();
		}
	}
	pack->~Packages();
	std::cout << "capture finished" << std::endl;
	return 0;
}


extern "C" __declspec(dllexport) LPSAFEARRAY fnDevCPPDLL();
LPSAFEARRAY fnDevCPPDLL()
{
	std::vector<string> listdev;OpDevices* oPdev;
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
		pcap_t* _adhandle2= NULL ;
		_adhandle1 = NULL;

		if (quit_flag)
			quit_flag = false;

		oPdev = new OpDevices();
		oPdev->Findalldevs(dev);
		_adhandle1 = oPdev->OpenDevices();
		{
			std::unique_lock lk(m);
			quit_flag = true;
		}
		oPdev->~OpDevices();
		cv.notify_one();
	}
}
























