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
#include <list>

using namespace std;
std::string getAgentAsString(pcpp::HttpRequestLayer* httpRequestLayer);

#define API __declspec(dllexport)

struct vStruct {
	
	int id;
};

struct OneStruct : public vStruct
{	
	int id;
	string source_ip;
	string dest_ip;
	string mac_source;
	string mac_destin;
	string user_agent;

public:
	OneStruct(int _id, string _source_ip, string _dest_ip, string _mac_source, string _mac_destin, string _user_agent)
	{
		id = _id;
		source_ip = _source_ip;
		dest_ip = _dest_ip;
		mac_source = _mac_source;
		mac_destin = _mac_destin;
		user_agent = _user_agent;
	}
};

struct SecondStruct : public vStruct
{	
	int id;
	const char* source_ip;
	const char* dest_ip;
	const char* mac_source;
	const char* mac_destin;
	const char* user_agent;

public:
	SecondStruct(int _id = 1000, const char* _source_ip = "", const char* _dest_ip = "", const char* _mac_source = "", const char* _mac_destin = "", const char* _user_agent = "none")
	{
		id = _id;
		source_ip = _source_ip;
		dest_ip = _dest_ip;
		mac_source = _mac_source;
		mac_destin = _mac_destin;
		user_agent = _user_agent;
	}
};

class Handler
{
public:
	virtual	Handler* SetNext(Handler* handler) = 0;

	virtual	void Handle(std::vector<std::unique_ptr<vStruct>>& request) = 0;
};

class AbstractHandler : public Handler {

protected:
	Handler* next_handler_;

public:
	AbstractHandler() : next_handler_(nullptr) {
	}

	Handler* SetNext(Handler* handler) override {
		this->next_handler_ = handler;
		return handler;
	}


	void Handle(std::vector<std::unique_ptr<vStruct>>& request) override {   
		if (this->next_handler_) {
			return this->next_handler_->Handle(request);
		}	
	}

protected:
	std::string getAgentAsString(pcpp::HttpRequestLayer* httpRequestLayer)
	{
		std::string agent = httpRequestLayer->getFieldByName(PCPP_HTTP_USER_AGENT_FIELD)->getFieldValue();
		return agent;
	}

protected:
	const char* CopyToChar(const std::string& bar) {
		char* tmp = new char[bar.length() + 1];
		strcpy_s(tmp, strlen(tmp), bar.c_str());
		return  tmp;
	}
	protected:
		Handler* getHandle() const { return next_handler_; }
};


class VectorOneHandler : public AbstractHandler
{
	~VectorOneHandler() = default;
	std::vector<std::unique_ptr<vStruct>> request;

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


class VectorTwoHandler : public AbstractHandler
{
	~VectorTwoHandler() = default;
	std::vector<std::unique_ptr<vStruct>> _request;	

public:
	void Handle(std::vector<std::unique_ptr<vStruct>>& request) override {

		vStruct* tmpBase = nullptr;
		OneStruct* tmp;

		int max_elelments = request.size();
		request.reserve(max_elelments * 2);

		for (const std::unique_ptr<vStruct>& v : request)
		{
			tmpBase = v.get();
			tmp = static_cast<OneStruct*>(tmpBase);

			SecondStruct* st = new SecondStruct(tmp->id,
				                                CopyToChar(tmp->source_ip),
				                                CopyToChar(tmp->dest_ip),
				                                CopyToChar(tmp->mac_source),
				                                CopyToChar(tmp->mac_destin));

			request.emplace_back(std::make_unique<SecondStruct>(*st));
		}

		request.erase(request.begin(), request.begin() + max_elelments);

		if (getHandle() != nullptr)
		{
			AbstractHandler::Handle(request);
		}
	}
};

std::vector<SecondStruct> vec{};
std::vector<SecondStruct> convertPtrVecToList(int* size)
{
	std::vector<std::unique_ptr<vStruct>> request{};
	vStruct* tmpBase = nullptr;
	SecondStruct* tmp;

	VectorOneHandler* vectorOne = new VectorOneHandler();
	VectorTwoHandler* vectorTwo = new VectorTwoHandler();

	vectorOne->SetNext(vectorTwo);
	vectorOne->Handle(request);

	for (auto& v : request) {

		tmpBase = v.get();
		tmp = static_cast<SecondStruct*>(tmpBase);
		SecondStruct st{};
		st.id = tmp->id;
		st.source_ip = tmp->source_ip;
		st.dest_ip = tmp->dest_ip;
		st.mac_source = tmp->mac_source;
		st.mac_destin = tmp->mac_destin;
		st.user_agent = tmp->user_agent;
		vec.push_back(st);
		v.reset();
	}
	*size = request.size();	
	return vec;
}

std::vector<SecondStruct> result{};
extern "C" __declspec(dllexport) SecondStruct * Predict(int* size1, const char* path2D)
{
	int size;
	result = convertPtrVecToList(&size);
	*size1 = size;

	return result.data();
}








