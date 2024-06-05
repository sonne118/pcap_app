#include <packages.h>
#include <iostream>
#include <string.h>
#include <vector>
#include <thread> 
#include <memory>
#include <ipc.h>
#include <packages.h>
#include <packages.cpp>


int main1() {

	int file = 0, dev = 0;
	pcap_t* adhandle = nullptr;
	struct pcap_pkthdr* pkthdr = nullptr;
	const u_char* packet = nullptr;

	Packages* pack = new Packages(adhandle, pkthdr, packet);

	dev = pack->findalldevs();
	if (dev)
		pack->OpenDevices();

	// file =pack->OpenFile();

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


extern "C"
{
	void __declspec(dllexport) __stdcall fnCPPDLL(void)
	{
		DWORD IDThread;
		HANDLE hEvent;
		HANDLE hThread;
		hEvent = CreateEvent(
			NULL,
			TRUE,
			FALSE,
			_T("MyEvent")
		);
		hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)main1, &hEvent, 0, &IDThread);
		if (WAIT_TIMEOUT == WaitForSingleObject(hEvent, 10000))
			std::cout << "Time Out" << std::endl;
		WaitForSingleObject(hThread, INFINITE);//This line is used

		//return 99;
	}
}









