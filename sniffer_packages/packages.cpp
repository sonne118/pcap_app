#include <packages.h>
#include <iostream>
#include <struct.h>
#include <mutex>
#include <ether_ntoa.h>
#include <WinSock2.h>
#include <struct.h>
#include <pcap.h>
#include <ipc.h>

#define buff_max 5
#define mod %

inline char errbuf[PCAP_ERRBUF_SIZE];
inline tagSnapshot shared_buff[buff_max];

inline std::atomic<int> free_index(0);
inline std::atomic<int> full_index(0);
inline std::mutex mtx;


inline Packages::Packages(pcap_t* adhandle, struct pcap_pkthdr* pkthdr, const u_char* packet)
{
	_adhandle = adhandle;
	_pkthdr = pkthdr;
	_packet = packet;
}

inline Packages ::~Packages() {};

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

inline void* Packages::producer() {

	int res; tagSnapshot new_item{};

	while (true) {
		std::this_thread::sleep_for(std::chrono::milliseconds(100));

		while ((res = pcap_next_ex(_adhandle, &_pkthdr, &_packet)) >= 0) {

			WaitForSingleObject(eventHandles, INFINITE);

			if (res == 0) {
				//std::cout << "timeout" << "no packages now" << std::endl; //tester
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

inline int Packages::findalldevs()
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

	//std::cin >> inum; 
	std::cout << "You entered the interface number: " << inum << std::endl;
	return  inum;
}

inline int Packages::OpenDevices()
{
	if (inum < 1 || inum > i)
	{
		std::cout << "Interface number out of range" << std::endl;
		pcap_freealldevs(alldevs);
		return -1;
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
		return -1;
	}
	std::cout << "listening on  ......" << d->description << std::endl;

	return 1;
}

inline int Packages::OpenFile()
{
	_adhandle = pcap_open_offline("D:/repo/test2/pcap_app/pcap_app/sniffer_packages/http.pcap", errbuf);
	if (_adhandle == NULL) {
		std::cout << "pcap_open_live() failed: " << errbuf << std::endl;
		return 1;
	}
}