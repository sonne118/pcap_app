#pragma once
#ifndef PACKAGES_H
#define PACKAGES_H
#include <pcap.h>
#include <vector>
#include <string>
#include <atomic>
#include <mutex>
#include <ether_ntoa.h>
#include <WinSock2.h>
#include <struct.h>
#include <pcap.h>
#include <ipc.h>
#include <list>
#include <vector>
#include <iostream>
#include <atomic>
#include <handleProto.h>
#include <ipc.h>

#pragma warning(disable:4996) 
#pragma comment(lib, "wpcap.lib")
#pragma comment(lib, "ws2_32.lib")

#define buff_max 5
#define mod %

tagSnapshot shared_buff[buff_max];

std::atomic<int> free_index(0);
std::atomic<int> full_index(0);
std::mutex mtx;

class Packages
{
public:
	Packages();
	Packages(handleProto p);
	~Packages();
	void* producer(std::atomic<bool>& on);
	void* consumer();
	void setHandler(HANDLE eventHandle);
	void addToStruct(char proto[22], char packet_srcip[INET_ADDRSTRLEN], char packet_dstip[INET_ADDRSTRLEN], char source_mac[32], char dest_mac[32], int packet_id, int dst_port, int src_port, tagSnapshot& item);
	void defaultToStruct(tagSnapshot& item);
	handleProto _proto;

private:
	pcap_t* descr;
	pcap_if_t* alldevs;
	pcap_if_t* d;
	int i = 0;
	struct pcap_pkthdr* _pkthdr;
	const u_char* packetd_ptr;
	pcap_t* _adhandle;
	HANDLE _eventHandles;
public:
	int  src_port;
	int  dst_port;
	int* src_porth;
	int* dst_porth;
};

inline Packages::Packages()
{
	src_port = 0;
	dst_port = 0;
}

inline Packages::Packages(handleProto p) :_proto(&_proto) {
	p = _proto;
	_proto._dst_port = &dst_port;
	_proto._src_port = &src_port;
	src_porth = &src_port;
	dst_porth = &dst_port;
};

Packages ::~Packages() {
	_adhandle = NULL;
	_eventHandles = NULL;
	delete _adhandle;
	delete _eventHandles;
};

inline void Packages::setHandler(HANDLE eventHandle) {
	_eventHandles = eventHandle;
}

inline void* Packages::consumer() {
	tagSnapshot consumed_item{};
	tagSnapshot snapshot;


	while (true) {
		while (free_index == full_index) {
			std::this_thread::sleep_for(std::chrono::milliseconds(10));
		}
		mtx.lock();

		while (free_index != full_index)
		{
			consumed_item = shared_buff[full_index];
			full_index = (full_index + 1) mod buff_max;
			snapshot = consumed_item;
			std::this_thread::sleep_for(std::chrono::milliseconds(100));
			WriteFile(hPipe, &snapshot, sizeof(tagSnapshot), NULL, NULL);
		}
		mtx.unlock();

		std::this_thread::sleep_for(std::chrono::milliseconds(10));
	}
	CloseHandle(hPipe);
}

inline void* Packages::producer(std::atomic<bool>& on) {

	int link_hdr_length = 0;
	int res;

	while (true) {
		std::this_thread::sleep_for(std::chrono::milliseconds(500));

		std::unique_lock lk(m);
		_adhandle = nullptr;
		cv.wait(lk);
		if (_adhandle1 != NULL && _adhandle == NULL)
			_adhandle = _adhandle1;

		lk.unlock();
		cv.notify_one();

		std::this_thread::sleep_for(std::chrono::milliseconds(500));

		while ((res = pcap_next_ex(_adhandle, &_pkthdr, &packetd_ptr)) >= 0 && on.load()) {
			tagSnapshot new_item;char proto[22];u_int size_ip;

			WaitForSingleObject(_eventHandles, INFINITE);

			if (res == 0) {
				Packages::defaultToStruct(new_item);
				shared_buff[free_index] = new_item;
				free_index = (free_index + 1) mod buff_max;
				continue;
			}

			struct ether_header* eptr{};
			struct tcphdr* tcp_header;
			struct udphdr* udp_header;
			struct icmp* icmp_header;
			struct ip* ip_hdr;
			struct tcphdr* tcpip_header;
			const unsigned char* dst_ptr_mac;
			const unsigned char* src_ptr_mac;
			char source_mac[32]; char dest_mac[32];
			packetd_ptr += link_hdr_length;
			eptr = (struct ether_header*)packetd_ptr;
			ip_hdr = (struct ip*)(packetd_ptr + sizeof(struct ether_header));


			char packet_srcip[INET_ADDRSTRLEN];
			char packet_dstip[INET_ADDRSTRLEN];
			strcpy_s(packet_srcip, inet_ntoa(ip_hdr->ip_src));
			strcpy_s(packet_dstip, inet_ntoa(ip_hdr->ip_dst));

			dst_ptr_mac = eptr->ether_shost;
			src_ptr_mac = eptr->ether_dhost;
			ether_ntoa(src_ptr_mac, source_mac, sizeof source_mac);
			ether_ntoa(src_ptr_mac, dest_mac, sizeof dest_mac);


			int packet_id = ntohs(ip_hdr->ip_id),
				packet_ttl = ip_hdr->ip_ttl,
				packet_tos = ip_hdr->ip_tos,
				packet_len = ntohs(ip_hdr->ip_len),
				packet_hlen = ip_hdr->ip_vhl;


			int protocol_type = ip_hdr->ip_p;
			*dst_porth = std::stoi(inet_ntoa(ip_hdr->ip_dst));
			*src_porth = std::stoi(inet_ntoa(ip_hdr->ip_src));


			switch (protocol_type) {

			case IPPROTO_TCP:
				tcpip_header = (tcphdr*)(packetd_ptr + sizeof(struct ether_header) + sizeof(struct ip));
				*dst_porth = ntohs(tcpip_header->dport);
				*src_porth = ntohs(tcpip_header->sport);
				break;
			case IPPROTO_UDP:
				udp_header = (struct udphdr*)packetd_ptr;
				*src_porth = udp_header->uh_sport;
				*dst_porth = udp_header->uh_dport;
				break;
			case IPPROTO_ICMP:
				icmp_header = (struct icmp*)packetd_ptr;
				int icmp_type = icmp_header->icmp_type;
				int icmp_type_code = icmp_header->icmp_code;
				break;
			}

			auto iter = _proto.caseMap.find(protocol_type);

			if (iter != _proto.caseMap.end()) {
				iter->second();
				strcpy(proto, _proto.protoStr);
			}

			if (ntohs(eptr->ether_type) == IPv4_ETHERTYPE) {
				ip_hdr = (struct ip*)(packetd_ptr + SIZE_ETHERNET);
				size_ip = IP_HL(ip_hdr) * 4;
				if (IP_V(ip_hdr) == 4) {

				}
			}

			while (((free_index + 1) mod buff_max) == full_index) {
				std::this_thread::sleep_for(std::chrono::milliseconds(500));
			}
			mtx.lock();

			Packages::addToStruct(proto, packet_srcip, packet_dstip, source_mac, dest_mac, packet_id, dst_port, src_port, new_item);
			shared_buff[free_index] = new_item;
			free_index = (free_index + 1) mod buff_max;
			mtx.unlock();
		}
	}
	return 0;
};

inline void Packages::addToStruct(char proto[22], char packet_srcip[INET_ADDRSTRLEN], char packet_dstip[INET_ADDRSTRLEN], char source_mac[32],
	char dest_mac[32], int packet_id, int dst_port, int src_port, tagSnapshot& item)
{
	strcpy(item.proto, proto);
	strcpy(item.source_ip, packet_srcip);
	strcpy(item.dest_ip, packet_dstip);
	strcpy(item.source_mac, source_mac);
	strcpy(item.dest_mac, dest_mac);
	item.id = packet_id;
	item.dest_port = dst_port;
	item.source_port = src_port;
};
inline void Packages::defaultToStruct(tagSnapshot& item) {

	item.id = 1000;
	strcpy(item.source_ip, "192.168.1.1");
	strcpy(item.dest_ip, "192.168.1.100");
	strcpy(item.source_mac, "ff:ff:ff:ff:ff:ff");
	strcpy(item.dest_mac, "ff:ff:ff:ff:ff:ff");
	item.dest_port = 8080;
	item.source_port = 8081;
};

#endif