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
#include <string>


using namespace std;

std::string getAgentAsString(pcpp::HttpRequestLayer* httpRequestLayer);

int main(int argc, char* argv[])
{
	// open a pcap file for reading
	pcpp::PcapFileReaderDevice reader("message2.pcap");
	if (!reader.open())
	{
		printf("Error opening the pcap file\n");
		return 1;
	}

	// read the first (and only) packet from the file
	pcpp::RawPacket rawPacket;
	if (!reader.getNextPacket(rawPacket))
	{
		printf("Couldn't read the first packet in the file\n");
		return 1;
	}

	int i = 1;
	std::string user_agent;
	pcpp::Packet parsedPacket(&rawPacket);
	pcpp::EthLayer* ethernetLayer = parsedPacket.getLayerOfType<pcpp::EthLayer>();
	pcpp::HttpRequestLayer* httpRequestLayer = parsedPacket.getLayerOfType<pcpp::HttpRequestLayer>();
	pcpp::Layer* curLayer = parsedPacket.getFirstLayer();


	std::cout << std::endl
		<< "#         : " << i << std::endl
		<< "source ip :" << parsedPacket.getLayerOfType<pcpp::IPv4Layer>()->getSrcIpAddress().toString().c_str() << std::endl
		<< "dest ip   :" << parsedPacket.getLayerOfType<pcpp::IPv4Layer>()->getDstIpAddress().toString().c_str() << std::endl
		<< "mac_source:" << ethernetLayer->getSourceMac().toString().c_str() << std::endl
		<< "mac_destin:" << ethernetLayer->getDestMac().toString().c_str() << std::endl;
		

	while (reader.getNextPacket(rawPacket))
	{
		i++;
		// parse the raw packet into a parsed packet
		pcpp::Packet parsedPacket(&rawPacket);
		ethernetLayer = parsedPacket.getLayerOfType<pcpp::EthLayer>();
		httpRequestLayer = parsedPacket.getLayerOfType<pcpp::HttpRequestLayer>();
		curLayer = parsedPacket.getFirstLayer();


		// verify the packet is IPv4
		if (parsedPacket.isPacketOfType(pcpp::IPv4))
		{

			std::cout << std::endl
				<< "#         : " << i << std::endl
				<< "source ip :" << parsedPacket.getLayerOfType<pcpp::IPv4Layer>()->getSrcIpAddress().toString().c_str() << std::endl
				<< "dest ip   :" << parsedPacket.getLayerOfType<pcpp::IPv4Layer>()->getDstIpAddress().toString().c_str() << std::endl
				<< "mac_source:" << ethernetLayer->getSourceMac().toString().c_str() << std::endl
				<< "mac_destin:" << ethernetLayer->getDestMac().toString().c_str() << std::endl;


			if (httpRequestLayer == NULL)
			{
				std::cerr << "user_agent:" << "none" << std::endl;
			}
			else
			{
				//std::string user_agent = httpRequestLayer->getFieldByName(PCPP_HTTP_USER_AGENT_FIELD)->getFieldValue().c_str();
				user_agent = getAgentAsString(httpRequestLayer);
				std::cerr << "http user-agent: " << user_agent << std::endl;
				//delete httpRequestLayer;
			}
		}		
	}

	// close the file
	reader.close();

	return 0;
}

std::string getAgentAsString(pcpp::HttpRequestLayer* httpRequestLayer)
{
	std::string agent = httpRequestLayer->getFieldByName(PCPP_HTTP_USER_AGENT_FIELD)->getFieldValue();
	return agent;
}
	

