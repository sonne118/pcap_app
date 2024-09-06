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
	unsigned char	ip_vhl;	 /* version << 4 | header length >> 2 */
	unsigned char ver_ihl;
	unsigned char tos;       /* type of service */
	unsigned short tlen;     /* total length */
	u_short ip_id;		/* identification */
	unsigned short identification;
	unsigned short flags_fo;
	unsigned char ttl;
	unsigned short	ip_off; /* fragment offset field */
#define IP_RF 0x8000		/* reserved fragment flag */
#define IP_DF 0x4000		/* don't fragment flag */
#define IP_MF 0x2000		/* more fragments flag */
#define IP_OFFMASK 0x1fff	/* mask for fragmenting bits */
	unsigned char ip_p;  /* protocol */
	u_short ip_sum;		/* checksum */
	u_char ip_ttl;		/* time to live */	
	unsigned char crc;
	//u_char ip_src[4];
	//u_char ip_dst[4];
	struct in_addr ip_src, ip_dst; /* source and dest address */

}IPHEADER, * PIPHEADER;
#define IP_HL(ip)  (((ip)->ip_vhl) & 0x0f)
#define IP_V(ip)  (((ip)->ip_vhl) >> 4)

typedef u_int tcp_seq;

typedef struct tcphdr {
	u_short sport;       /* source port */
	u_short dport;       /* destination port */
	tcp_seq seq;		/* sequence number */
	tcp_seq ack;		/* acknowledgement number */
	u_char offx2;	/* data offset, rsvd */
	u_short len;
	u_short crc;
#define TH_OFF(th)	(((th)->offx2 & 0xf0) >> 4)
	u_char flags;
#define TH_FIN 0x01
#define TH_SYN 0x02
#define TH_RST 0x04
#define TH_PUSH 0x08
#define TH_ACK 0x10
#define TH_URG 0x20
#define TH_ECE 0x40
#define TH_CWR 0x80
#define TH_FLAGS (TH_FIN|TH_SYN|TH_RST|TH_ACK|TH_URG|TH_ECE|TH_CWR)
	u_short th_win;		/* window */
	u_short th_sum;		/* checksum */
	u_short th_urp;		/* urgent pointer */
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
