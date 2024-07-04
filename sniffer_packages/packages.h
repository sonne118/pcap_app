#pragma once
#ifndef PACKAGES_H
#define PACKAGES_H
#include <pcap.h>
#include <vector>
#include <string>
#include <atomic>

class Packages
{
public:
	Packages(); 
	~Packages();
	void* producer(std::atomic<bool>& on);
	void* consumer();


private:
	pcap_t* descr;
	pcap_if_t* alldevs;
	pcap_if_t* d;
	int i = 0;
	struct pcap_pkthdr* _pkthdr;
	const u_char* _packet;
public:
	pcap_t* _adhandle;
	HANDLE eventHandles;	
};
#endif