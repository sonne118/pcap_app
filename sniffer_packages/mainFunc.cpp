#include <packages.h>
#include <packages.cpp>
#include <iostream>
#include <string.h>
#include <vector>
#include <thread> 
#include <memory>
#include <ipc.h>


int mainFunc(HANDLE eventHandle) {

	int file = 0, dev = 0;
	pcap_t* adhandle = nullptr;
	struct pcap_pkthdr* pkthdr = nullptr;
	const u_char* packet = nullptr;

	Packages* pack = new Packages(adhandle, pkthdr, packet);
	pack->eventHandles = eventHandle;

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

extern "C" {
	void __declspec(dllexport) __stdcall fnCPPDLL(int flag)
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





















