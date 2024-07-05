#pragma once
#ifndef OpDevices_HEADER
#define OpDevices_HEADER

#include <pcap.h>
#include <iostream>
#include <string>


char errbuf[PCAP_ERRBUF_SIZE];

class OpDevices
{
public:
	OpDevices() {};

	~OpDevices() {
		alldevs = nullptr;
		d = nullptr;
		_adhandle = nullptr;
	};

public:
	void SetInum(int dev);
	int Findalldevs();
	pcap_t* OpenDevices();
	std::vector<std::string>listalldevs();
	int OpenFile();


private:
	int i = 0;
	pcap_if_t* alldevs;
	pcap_if_t* d;
	int inum;	

public :
	pcap_t* _adhandle;
};

void OpDevices::SetInum(int dev)
{
	inum = dev;
};

inline int OpDevices::Findalldevs()
{
	if (pcap_findalldevs_ex(PCAP_SRC_IF_STRING, NULL, &alldevs, errbuf) == -1)
	{
		std::cout << "Error in pcap_findalldevs" << errbuf << std::endl;
		exit(1);
	}

	for (d = alldevs; d; d = d->next)
	{
		std::cout << ++i << "." << d->name << std::endl;
		if (d->description)
			std::cout << d->description << std::endl;
		else
			std::cout << "No description available" << std::endl;
	}
	if (i == 0)
	{
		std::cout << "No interfaces found! Make sure WinPcap is installed" << std::endl;
		return -1;
	}

	std::cout << "Enter the interface number: 1-" << i << std::endl;

	std::cout << "You entered the interface number: " << inum << std::endl;
	return  inum;
}

inline pcap_t* OpDevices::OpenDevices()
{
	std::cout << " entered the interface number: " << inum << std::endl;

	_adhandle = nullptr;
	if (inum < 1 || inum > i)
	{
		std::cout << "Interface number out of range" << std::endl;
		pcap_freealldevs(alldevs);		
	}
	// Jump to the selected adapter 
	for (d = alldevs, i = 0; i < inum - 1;d = d->next, i++);
	// Open the device 
	if ((_adhandle = pcap_open(d->name,
		65536,
		PCAP_OPENFLAG_PROMISCUOUS,
		1000,
		NULL,
		errbuf)) == NULL)
	{
		fprintf(stderr, "\nUnable to open the adapter. %s is not supported by WinPcap\n", d->name);
		pcap_freealldevs(alldevs);
	}

	std::cout << "listening on  ......" << d->description << std::endl;

	return _adhandle;
}

inline std::vector<std::string>OpDevices::listalldevs()
{
	std::vector<std::string> list;int i = 0;

	if (pcap_findalldevs_ex(PCAP_SRC_IF_STRING, NULL, &alldevs, errbuf) == -1)
	{
		std::cout << "Error in pcap_findalldevs" << errbuf << std::endl;
		exit(1);
	}

	for (d = alldevs; d; d = d->next)
	{
		++i;
		if (d->description)
		{
			auto str = std::to_string(i).append("_").append(d->name).append(d->description);
			list.push_back(str);
		}
		else
			std::cout << "No description available" << std::endl;
	}
	if (i == 0)
	{
		std::cout << "No interfaces found! Make sure WinPcap is installed" << std::endl;
	}
	return  list;
}

inline int OpDevices::OpenFile()
{
	_adhandle = pcap_open_offline("D:/repo/test2/pcap_app/pcap_app/sniffer_packages/http.pcap", errbuf);
	if (_adhandle == NULL) {
		std::cout << "pcap_open_live() failed: " << errbuf << std::endl;
		return 1;
	}
}

#endif

