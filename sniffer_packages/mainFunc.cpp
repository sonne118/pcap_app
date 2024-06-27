#include <packages.h>
#include <packages.cpp>
#include <iostream>
#include <string.h>
#include <vector>
#include <thread> 
#include <memory>
#include <ipc.h>
#include <atlsafe.h>

int mainFunc(HANDLE eventHandle, int d) {

	int file = 0, dev = 0;
	pcap_t* adhandle = nullptr;
	struct pcap_pkthdr* pkthdr = nullptr;
	const u_char* packet = nullptr;

	Packages* pack = new Packages(adhandle, pkthdr, packet);
	pack->eventHandles = eventHandle;
	pack->inum = d;

	dev = pack->findalldevs();
	if (dev)
		pack->OpenDevices();
	// file = pack->Packages::OpenFile();
	if (dev || file)
	{
		std::vector<std::unique_ptr<std::thread>> threads;
		threads.emplace_back(std::make_unique<std::thread>(&Packages::producer, pack));
		threads.emplace_back(std::make_unique<std::thread>(&Packages::consumer, pack));

		for (auto& thread : threads) {
			thread->join();
		}
	}
	pack->~Packages();
	std::cout << "capture finished" << std::endl;
	return 0;
}


extern "C" __declspec(dllexport) LPSAFEARRAY fnDevCPPDLL();
LPSAFEARRAY fnDevCPPDLL()
{
	std::vector<string> listdev;
	Packages* pack = new Packages(nullptr, nullptr, nullptr);
	listdev = pack->listalldevs();
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

		hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)mainFunc(eventHandle,d), &eventHandle, 0, &IDThread);

		std::cout << " main thread " << std::endl;

		CloseHandle(eventHandle);
		CloseHandle(hThread);

	}

}





















