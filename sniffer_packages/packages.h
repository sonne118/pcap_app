#pragma once
#ifndef PACKAGES_H
#define PACKAGES_H
#include <pcap.h>

class Packages
{
public:
	Packages(pcap_t* adhandle, struct pcap_pkthdr* pkthdr, const u_char* packet);
	//~Packages();
	void* producer();
	void* consumer();
	int findalldevs();
	int OpenDevices();
	int OpenFile();


private:
	pcap_t* descr;
	pcap_if_t* alldevs;
	pcap_if_t* d;
	int inum = 4;
	int i = 0;
	pcap_t* _adhandle;
	struct pcap_pkthdr* _pkthdr;
	const u_char* _packet;
};


#endif