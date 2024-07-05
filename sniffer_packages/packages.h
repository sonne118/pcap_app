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
#include <string>
#include <atomic>

using namespace std;

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

Packages::Packages() {};

Packages ::~Packages() {
	_adhandle = nullptr;
	eventHandles = nullptr;
};

inline void* Packages::consumer() {
	tagSnapshot consumed_item{};
	tagSnapshot snapshot;


	while (true) {
		while (free_index == full_index) {
			std::this_thread::sleep_for(std::chrono::milliseconds(100));
		}
		mtx.lock();

		while (free_index != full_index)
		{
			consumed_item = shared_buff[full_index];
			full_index = (full_index + 1) mod buff_max;
			snapshot = consumed_item;
			std::this_thread::sleep_for(std::chrono::milliseconds(500));
			WriteFile(hPipe, &snapshot, sizeof(tagSnapshot), NULL, NULL);
		}
		mtx.unlock();

		std::this_thread::sleep_for(std::chrono::milliseconds(100));
	}
	CloseHandle(hPipe);
}

inline void* Packages::producer(std::atomic<bool>& on) {

	int res; tagSnapshot new_item{};

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

		while ((res = pcap_next_ex(_adhandle, &_pkthdr, &_packet)) >= 0 && on) {

			WaitForSingleObject(eventHandles, INFINITE);

			if (res == 0) {			
				strcpy_s(new_item.source_ip, "192.168.1.1");
				strcpy_s(new_item.dest_ip, "192.168.1.100");
				strcpy_s(new_item.source_mac, "FF:FF:FF:FF:FF:FF");
				strcpy_s(new_item.dest_mac, "FF:FF:FF:FF:FF:FF");
				new_item.dest_port = 8080;
				new_item.source_port = 8081;
				shared_buff[free_index] = new_item;
				free_index = (free_index + 1) mod buff_max;
				continue;
			}

			const struct ether_header* ethernetHeader;
			const struct eth_header* eth_Header;
			const struct ip* ipHeader;
			const struct tcphdr* tcpHeader;
			char sourceIp[INET_ADDRSTRLEN];
			char destIp[INET_ADDRSTRLEN];

			const unsigned char* ptr1;
			const unsigned char* ptr2;
			u_int sourcePort, destPort;
			u_char* data;
			int dataLength = 0;
			std::string dataStr = "";

			ethernetHeader = (struct ether_header*)_packet;
			if (ntohs(ethernetHeader->ether_type) == ETHERTYPE_IP) {
				ipHeader = (struct ip*)(_packet + sizeof(struct ether_header));
				inet_ntop(AF_INET, &(ipHeader->ip_src), sourceIp, INET_ADDRSTRLEN);
				inet_ntop(AF_INET, &(ipHeader->ip_dst), destIp, INET_ADDRSTRLEN);

				ptr1 = ethernetHeader->ether_shost;
				ptr2 = ethernetHeader->ether_dhost;

				char source_mac[32]; char dest_mac[32];
				ether_ntoa(ptr1, source_mac, sizeof source_mac);
				ether_ntoa(ptr2, dest_mac, sizeof dest_mac);

				//ntohs(th->seq), ntohs(th->ack_seq));

				std::string proto = "";
				if (ipHeader->ip_p == 0x06)
					proto = "Protocol: TCP (6)";
				else if (ipHeader->ip_p == 0x11)
					proto = "Protocol: UDP";
				else if (ipHeader->ip_p == 0x0501)
					proto = "Protocol: ICMP";
				else if (ipHeader->ip_p == 0x0501)
					proto = "Protocol: IPV4";


				//if (ipHeader->ip_p == IPPROTO_TCP )//|| ipHeader->ip_p == IPPROTO_ICMP)
				{
					tcpHeader = (tcphdr*)(_packet + sizeof(struct ether_header) + sizeof(struct ip));
					sourcePort = ntohs(tcpHeader->sport);
					destPort = ntohs(tcpHeader->dport);
					data = (u_char*)(_packet + sizeof(struct ether_header) + sizeof(struct ip) + sizeof(struct tcphdr));
					dataLength = _pkthdr->len - (sizeof(struct ether_header) + sizeof(struct ip) + sizeof(struct tcphdr));


					u_int hlen, version;
					int i;
					int len;
					len = ntohs(ipHeader->tlen);
					//hlen = IP_HL(ipHeader); /* header length */
					//version = IP_V(ipHeader);/* ip version */
					//off = ntohs(ipHeader->ip_off);

					while (((free_index + 1) mod buff_max) == full_index) {
						std::this_thread::sleep_for(std::chrono::milliseconds(100));
					}
					mtx.lock();
					//std::cout << "sourceIp"<< sourceIp << std::endl; ///tester
					strcpy_s(new_item.source_ip, sourceIp);
					strcpy_s(new_item.dest_ip, destIp);
					strcpy_s(new_item.source_mac, source_mac);
					strcpy_s(new_item.dest_mac, dest_mac);
					new_item.dest_port = destPort;
					new_item.source_port = sourcePort;

					shared_buff[free_index] = new_item;
					free_index = (free_index + 1) mod buff_max;
					mtx.unlock();

				}
			}
		}
	}
}

#endif