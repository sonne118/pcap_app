#pragma once
#ifndef BUILDER_DEVICE_H
#define BUILDER_DEVICE_H

#include <pcap.h>
#include <iostream>
#include <string>

char errbuf[PCAP_ERRBUF_SIZE];

class  builderDevice {
public:

	class Builder
	{
	public:
		Builder(int dev);
		~Builder();
	public:
		Builder& Finddev();
		Builder& OpenDevices();
		Builder& ListDev();
		Builder& OpenFile();
		builderDevice build();
	private:
		int i;
		int inum;
		pcap_if_t* alldevs;
		pcap_if_t* d;
		pcap_t* _adhandle;
		std::vector<std::string> _list;
		
		friend class builderDevice;
	};
private:
	int inum;
	pcap_t* adhandle;
	std::vector<std::string> list;

public:
	pcap_t* getHandler();
	std::vector<std::string> getDevices();

public:
	builderDevice(const Builder& builder);
	~builderDevice();
};

inline builderDevice::Builder::Builder(int dev) : inum(dev), i(0), d(nullptr), alldevs(nullptr), _adhandle(nullptr)
{
};

inline builderDevice::Builder::~Builder()
{
	alldevs = NULL;
	d = NULL;
	_adhandle = NULL;
	delete alldevs;
	delete d;
	//delete _adhandle;
};

inline builderDevice::Builder& builderDevice::Builder::Finddev()
{
	if (pcap_findalldevs_ex(PCAP_SRC_IF_STRING, NULL, &alldevs, errbuf) == -1)
	{
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
	return *this;
};

inline builderDevice::Builder& builderDevice::Builder::OpenDevices()
{
	_adhandle = nullptr;
	if (inum < 1 || inum > i)
	{
		std::cout << "Interface number out of range" << std::endl;
		pcap_freealldevs(alldevs);
	}
	// Jump to the selected adapter 
	for (d = alldevs, i = 0; i < inum - 1;d = d->next, i++);
	// Open the device 
	if ((this->_adhandle = pcap_open(d->name,
		65536,
		PCAP_OPENFLAG_PROMISCUOUS,
		1000,
		NULL,
		errbuf)) == NULL)
	{
		pcap_freealldevs(alldevs);
	}
	return *this;
};

inline builderDevice::Builder& builderDevice::Builder::ListDev()
{
	int i = 0;std::vector<std::string> list;
	size_t lastWhitespace;

	if (pcap_findalldevs_ex(PCAP_SRC_IF_STRING, NULL, &alldevs, errbuf) == -1)
	{
		exit(1);
	}

	for (d = alldevs; d; d = d->next)
	{
		++i;
		if (d->description)
		{
			auto str = std::to_string(i)
				.append("_")
				.append(d->description)
				.substr(0, 53);            //.append(d->name)
			lastWhitespace = str.find_last_of(" \t\n\r");
				            
			list.push_back(str.erase(lastWhitespace));
			this->_list = list;
		}
		else
			std::cout << "No description available" << std::endl;
	}
	return *this;
};

inline builderDevice::Builder& builderDevice::Builder::OpenFile()
{
	pcap_t* adhandle;
	adhandle = pcap_open_offline("D:/repo/test2/pcap_app/pcap_app/sniffer_packages/http.pcap", errbuf);
	if (_adhandle == NULL) {
		std::cout << "pcap_open_live() failed: " << errbuf << std::endl;
		//return 1;
	}
	this->_adhandle = adhandle;
	return *this;
}

inline builderDevice builderDevice::Builder::build() {
	return builderDevice(*this);
};

inline pcap_t* builderDevice::getHandler() {
	return adhandle;
}

inline std::vector<std::string> builderDevice::getDevices() {
	return list;
}

inline builderDevice::builderDevice(const Builder& builder)
	: inum(builder.inum), list(builder._list), adhandle(builder._adhandle) {};

inline builderDevice::~builderDevice()
{
	inum = NULL;
	adhandle = nullptr;
}
#endif

