#ifndef VECTORONEHANDLER_H
#define VECTORONEHANDLER_H
#include <IPv4Layer.h>
#include <TcpLayer.h>
#include <Packet.h>
#include <PcapFileDevice.h>
#include "EthLayer.h"
#include "HttpLayer.h"
#include "stdlib.h"
#include <iostream>
#include "SystemUtils.h"
#include "PcapLiveDeviceList.h"
#include <cstdlib>
#include <Windows.h>
#include <array>
#include <stdio.h>
#include <strsafe.h>
#include <combaseapi.h>
#include <string>
#include <vector>
#include <iostream>
#include <charconv>
#include <cstddef>
#include <atlsafe.h>
#include <combaseapi.h>
#include<struct.h>
#include<Handler.h>
#include<AbstractHandler.h>

using namespace std;
std::string getAgentAsString(pcpp::HttpRequestLayer* httpRequestLayer);

class VectorOneHandler : public AbstractHandler
{
	std::vector<std::unique_ptr<vStruct>> request;
public:
	~VectorOneHandler() {};

public:
	void Handle(std::vector<std::unique_ptr<vStruct>>& request)
	{
		// open a pcap file for reading
		std::string  path = "D:/repo/test2/pcap_app/pcap_app/pcap_reader/message2.pcap";

		pcpp::PcapFileReaderDevice reader(path.c_str());
		if (!reader.open())
		{
			printf("Error opening the pcap file\n");
			//return request;
		}

		// read the first (and only) packet from the file
		pcpp::RawPacket rawPacket;
		if (!reader.getNextPacket(rawPacket))
		{
			printf("Couldn't read the first packet in the file\n");
			//return request;
		}

		int i = 1;
		std::string user_agent;
		pcpp::Packet parsedPacket(&rawPacket);
		pcpp::EthLayer* ethernetLayer = parsedPacket.getLayerOfType<pcpp::EthLayer>();
		pcpp::HttpRequestLayer* httpRequestLayer = parsedPacket.getLayerOfType<pcpp::HttpRequestLayer>();
		pcpp::Layer* curLayer = parsedPacket.getFirstLayer();
		while (reader.getNextPacket(rawPacket))
		{
			i++;
			// parse the raw packet into a parsed packet
			pcpp::Packet parsedPacket(&rawPacket);
			ethernetLayer = parsedPacket.getLayerOfType<pcpp::EthLayer>();
			httpRequestLayer = parsedPacket.getLayerOfType<pcpp::HttpRequestLayer>();
			curLayer = parsedPacket.getFirstLayer();

			// verify the packet is IPv4
			if (httpRequestLayer == NULL)
			{
				user_agent = "none";
			}
			else
			{
				std::string user_agent = httpRequestLayer->getFieldByName(PCPP_HTTP_USER_AGENT_FIELD)->getFieldValue();
				user_agent = getAgentAsString(httpRequestLayer);
			}

			if (parsedPacket.isPacketOfType(pcpp::IPv4))
			{
				OneStruct* st = new OneStruct(i,
					parsedPacket.getLayerOfType<pcpp::IPv4Layer>()->getSrcIpAddress().toString(),
					parsedPacket.getLayerOfType<pcpp::IPv4Layer>()->getDstIpAddress().toString(),
					ethernetLayer->getSourceMac().toString(),
					ethernetLayer->getDestMac().toString(),
					user_agent);
				request.emplace_back(std::make_unique<OneStruct>(*st));
			}
		}
		reader.close();

		if (getHandle() != nullptr)
		{
			AbstractHandler::Handle(request);
		}
	}
};

#endif

