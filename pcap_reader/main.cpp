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
#include <string>

using namespace std;
std::string getAgentAsString(pcpp::HttpRequestLayer* httpRequestLayer);

struct myStruct
{
	int id;
	string source_ip;
	string dest_ip;
	string mac_source;
	string mac_destin;
};

struct myStruct1
{
	int id;
	const char* source_ip;
	const char* dest_ip;
	const char* mac_source;
	const char* mac_destin;
};

#define API __declspec(dllexport)

std::vector<myStruct> _result{};

std::vector<myStruct> GetVector(int *size)
{	
	// open a pcap file for reading
	std::string  path = "D:/repo/test2/pcap_app/pcap_app/pcap_reader/message2.pcap";

	pcpp::PcapFileReaderDevice reader(path.c_str());
	if (!reader.open())
	{
		printf("Error opening the pcap file\n");
		return _result;
	}

	// read the first (and only) packet from the file
	pcpp::RawPacket rawPacket;
	if (!reader.getNextPacket(rawPacket))
	{
		printf("Couldn't read the first packet in the file\n");
		return _result;
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

		//myStruct st;
		// parse the raw packet into a parsed packet
		pcpp::Packet parsedPacket(&rawPacket);
		ethernetLayer = parsedPacket.getLayerOfType<pcpp::EthLayer>();
		httpRequestLayer = parsedPacket.getLayerOfType<pcpp::HttpRequestLayer>();
		curLayer = parsedPacket.getFirstLayer();


		// verify the packet is IPv4
		if (parsedPacket.isPacketOfType(pcpp::IPv4))
		{

			myStruct st{};
			st.id = i;
			st.source_ip  = parsedPacket.getLayerOfType<pcpp::IPv4Layer>()->getSrcIpAddress().toString();;
			st.dest_ip = parsedPacket.getLayerOfType<pcpp::IPv4Layer>()->getDstIpAddress().toString();
			st.mac_source = ethernetLayer->getSourceMac().toString();
			st.mac_destin = ethernetLayer->getDestMac().toString();

			_result.push_back(st);

			if (httpRequestLayer == NULL)
			{
				//std::cerr << "user_agent:" << "none" << std::endl;
			}
			else
			{
				//std::string user_agent = httpRequestLayer->getFieldByName(PCPP_HTTP_USER_AGENT_FIELD)->getFieldValue().c_str();
				//user_agent = getAgentAsString(httpRequestLayer);
			}
		}
	}

	*size = _result.size();
	// close the file
	reader.close();
	
	return _result;
}

std::string getAgentAsString(pcpp::HttpRequestLayer* httpRequestLayer)
{
	std::string agent = httpRequestLayer->getFieldByName(PCPP_HTTP_USER_AGENT_FIELD)->getFieldValue();
	return agent;
}

const char* CopyToChar(const std::string& bar) {
	char *tmp = new char[bar.length() + 1];
	strcpy_s(tmp, strlen(tmp), bar.c_str());
	return  tmp;
}

std::vector<myStruct1> GetVector1(int *size1)
{	
	int size;	
	std::vector<myStruct>vec = GetVector(&size);
	*size1 = size; 
	std::vector<myStruct1> lresult; 

	vector<myStruct>::iterator iter = vec.begin();		
    for (std::vector<myStruct>::size_type i = 0; i != vec.size(); i++) 
	{
		myStruct1 st{}; 
		st.id = iter[i].id;  		
		st.source_ip = CopyToChar(iter[i].source_ip);
		st.dest_ip = CopyToChar(iter[i].dest_ip);
		st.mac_source = CopyToChar(iter[i].mac_source);
		st.mac_destin = CopyToChar(iter[i].mac_destin);
		lresult.push_back(st);		
	}
	vec.erase(vec.begin(), vec.begin() + size-1);	
	return lresult;	
}

std::vector<myStruct1> result{};

extern "C" __declspec(dllexport) myStruct1 * Predict(int* size1, const char* path2D)
{
	int size;
	result = GetVector1(&size);	
	*size1 = size;
	return result.data();
}



