#ifndef STRUCT_H
#define STRUCT_H
#include <iostream>
#include <winsock2.h>

#define SIZE_ETHERNET 14
#define ETHER_ADDR_LEN 6
#define SIZE_ETHERNET 14
#define ETHERTYPE_IP   0x0800	
#define ETHERTYPE_ARP  0x0806 
#define IPv4_ETHERTYPE 0x800
#define ETHERTYPE_ARP       0x0806 
#define ETHERTYPE_REVARP    0x8035
#define ETHERTYPE_NS        0x0600 

typedef struct ether_header {
	unsigned char ether_dhost[ETHER_ADDR_LEN];
	unsigned char ether_shost[ETHER_ADDR_LEN];
	unsigned short ether_type;
}ETHHEADER, * PETHHEADER;

struct ip {
	u_char  ip_vhl;                 /* version << 4 | header length >> 2 */
	u_char  ip_tos;                 /* type of service */
	u_short ip_len;                 /* total length */
	u_short ip_id;                  /* identification */
	u_short ip_off;                 /* fragment offset field */
#define IP_RF 0x8000            /* reserved fragment flag */
#define IP_DF 0x4000            /* dont fragment flag */
#define IP_MF 0x2000            /* more fragments flag */
#define IP_OFFMASK 0x1fff       /* mask for fragmenting bits */
	u_char  ip_ttl;                 /* time to live */
	u_char  ip_p;                   /* protocol */
	u_short ip_sum;                 /* checksum */
	struct  in_addr ip_src, ip_dst;  /* source and dest address */
};
#define IP_HL(ip)               (((ip)->ip_vhl) & 0x0f)
#define IP_V(ip)                (((ip)->ip_vhl) >> 4)

typedef u_int tcp_seq;
typedef struct tcphdr {
	u_short sport;       /* source port */
	u_short dport;       /* destination port */
	tcp_seq seq;		/* sequence number */
	tcp_seq ack;		/* acknowledgement number */
	u_char offx2;	/* data offset, rsvd */
	u_short len;
	uint16_t id;
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

struct udphdr {
	u_short	uh_sport;		/* source port */
	u_short	uh_dport;		/* destination port */
	u_short	uh_ulen;		/* udp length */
	u_short	uh_sum;			/* udp checksum */
};

struct icmp {
	uint8_t  icmp_type;		/* type of message, see below */
	uint8_t  icmp_code;		/* type sub code */
	uint16_t icmp_cksum;		/* ones complement cksum of struct */
	union {
		uint8_t   ih_pptr;		/* ICMP_PARAMPROB */
		struct in_addr ih_gwaddr;	/* ICMP_REDIRECT */
		struct ih_idseq {
			uint16_t  icd_id;
			uint16_t  icd_seq;
		} ih_idseq;
		int32_t   ih_void;
		/* ICMP_UNREACH_NEEDFRAG -- Path MTU Discovery (RFC1191) */
		struct ih_pmtu {
			uint16_t  ipm_void;
			uint16_t  ipm_nextmtu;
		} ih_pmtu;
		struct ih_rtradv {
			uint8_t   irt_num_addrs;
			uint8_t   irt_wpa;
			uint16_t  irt_lifetime;
		} ih_rtradv;
	} icmp_hun;
};

#pragma pack(push, 2)
typedef struct tagSnapshot {
	int id;
	int  source_port;
	int  dest_port;
	char proto[22];
	char source_ip[22];
	char dest_ip[22];
	char source_mac[22];
	char dest_mac[22];
	char host_name[22];
}Snapshot;
#pragma pack(pop)

#endif
