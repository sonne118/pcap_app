#ifndef STRUCT_H
#define STRUCT_H
#include <iostream>
#include <WinSock2.h>

#define ETHER_ADDR_LEN 6
#ifndef ETHERTYPE_IP
#define ETHERTYPE_IP		0x0800	
#endif

//Ethernet Header
typedef struct ether_header {
	unsigned char ether_dhost[ETHER_ADDR_LEN];
	unsigned char ether_shost[ETHER_ADDR_LEN];
	unsigned short ether_type;
}ETHHEADER, * PETHHEADER;

//IPv4 Header
typedef struct ip {
	// unsigned char	ip_vhl;		/* header length, version *///
	unsigned char ver_ihl;
	unsigned char tos;
	unsigned short tlen;
	unsigned short identification;
	unsigned short flags_fo;
	unsigned char ttl;
	//unsigned short	ip_off; /* fragment offset field */
	unsigned char ip_p;
	unsigned char crc;
	u_char ip_src[4];
	u_char ip_dst[4];

}IPHEADER, * PIPHEADER;
#define IP_HL(ip)  (((ip)->ip_vhl) & 0x0f)
#define IP_V(ip)  (((ip)->ip_vhl) >> 4)

typedef struct tcphdr {
	u_short sport;
	u_short dport;
	u_short len;
	u_short crc;
}TCPHEADER, * PTCPHEADER;


#pragma pack(push, 2)
typedef struct tagSnapshot {
	int  source_port;
	int  dest_port;
	char source_ip[22];
	char dest_ip[22];
	char source_mac[22];
	char dest_mac[22];
}Snapshot;
#pragma pack(pop)

#endif
