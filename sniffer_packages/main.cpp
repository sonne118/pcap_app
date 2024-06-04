#pragma once
#include <packages.h>
#include <iostream>
#include <string.h>
#include <vector>
#include <thread> 
#include <ipc.h>
#include <packages.cpp>


int main() {

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
		std::vector<std::thread> threads;
		threads.emplace_back(&Packages::producer, pack);
		threads.emplace_back(&Packages::consumer, pack);

		for (auto& thread : threads) {
			thread.join();
		}
	}
	std::cout << "capture finished" << std::endl;
	return 0;
}

// Packages::Packages(pcap_t* adhandle, struct pcap_pkthdr* pkthdr, const u_char* packet)
//{
//	_adhandle = adhandle;
//	_pkthdr = pkthdr;
//	_packet = packet;
//}
//
// int Packages::findalldevs()
//{
//	if (pcap_findalldevs_ex(PCAP_SRC_IF_STRING, NULL, &alldevs, errbuf) == -1)
//	{
//		fprintf(stderr, "Error in pcap_findalldevs: %s\n", errbuf);
//		exit(1);
//	}
//
//	for (d = alldevs; d; d = d->next)
//	{
//		printf("%d. %s", ++i, d->name);
//		if (d->description)
//			printf(" (%s)\n", d->description);
//		else
//			printf(" (No description available)\n");
//	}
//	if (i == 0)
//	{
//		printf("\nNo interfaces found! Make sure WinPcap is installed.\n");
//		return -1;
//	}
//
//	printf("Enter the interface number (1-%d):", i);
//	//scanf_s("%d", &inum);
//	return  inum;
//}
//
// int Packages::OpenDevices()
//{
//	if (inum < 1 || inum > i)
//	{
//		printf("\nInterface number out of range.\n");
//		pcap_freealldevs(alldevs);
//		return -1;
//	}
//
//	// Jump to the selected adapter 
//	for (d = alldevs, i = 0; i < inum - 1;d = d->next, i++);
//	// Open the device 
//	if ((_adhandle = pcap_open(d->name,          // name of the device
//		65536,            // portion of the packet to capture	
//		PCAP_OPENFLAG_PROMISCUOUS,    // promiscuous mode
//		1000,             // read timeout
//		NULL,             // authentication on the remote machine
//		errbuf            // error buffer
//	)) == NULL)
//	{
//		fprintf(stderr, "\nUnable to open the adapter. %s is not supported by WinPcap\n", d->name);
//		pcap_freealldevs(alldevs);
//		return -1;
//	}
//	printf("\nlistening on %s...\n", d->description);
//
//	return 1;
//}
//
// int Packages::OpenFile()
//{
//	_adhandle = pcap_open_offline("D:/repo/test2/pcap_app/pcap_app/sniffer_packages/http.pcap", errbuf);
//	if (_adhandle == NULL) {
//		std::cout << "pcap_open_live() failed: " << errbuf << std::endl;
//		return 1;
//	}
//}






