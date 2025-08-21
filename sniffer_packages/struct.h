#ifndef STRUCT_H
#define STRUCT_H

#ifdef _WIN32
#include <WinSock2.h>
#include <tchar.h>
#include <pcap.h>

#define SIZE_ETHERNET 14
#define ETHER_ADDR_LEN 6

typedef struct ether_header {
    unsigned char ether_dhost[ETHER_ADDR_LEN];
    unsigned char ether_shost[ETHER_ADDR_LEN];
    unsigned short ether_type;
} ETHHEADER, *PETHHEADER;

struct ip {
    u_char  ip_vhl;
    u_char  ip_tos;
    u_short ip_len;
    u_short ip_id;
    u_short ip_off;
    #define IP_RF 0x8000
    #define IP_DF 0x4000
    #define IP_MF 0x2000
    #define IP_OFFMASK 0x1fff
    u_char  ip_ttl;
    u_char  ip_p;
    u_short ip_sum;
    struct in_addr ip_src, ip_dst;
};
#define IP_HL(ip)  (((ip)->ip_vhl) & 0x0f)
#define IP_V(ip)   (((ip)->ip_vhl) >> 4)

typedef u_int tcp_seq;
struct tcphdr {
    u_short sport;
    u_short dport;
    tcp_seq seq;
    tcp_seq ack;
    u_char offx2;
    u_short len;
    u_short crc;
    #define TH_OFF(th)  (((th)->offx2 & 0xf0) >> 4)
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
    u_short th_win;
    u_short th_sum;
    u_short th_urp;
};

struct udphdr {
    u_short uh_sport;
    u_short uh_dport;
    u_short uh_ulen;
    u_short uh_sum;
};

struct icmp {
    u_char  icmp_type;
    u_char  icmp_code;
    u_short icmp_cksum;
    union {
        u_char ih_pptr;
        struct in_addr ih_gwaddr;
        struct ih_idseq { u_short icd_id; u_short icd_seq; } ih_idseq;
        int32_t ih_void;
        struct ih_pmtu { u_short ipm_void; u_short ipm_nextmtu; } ih_pmtu;
        struct ih_rtradv { u_char irt_num_addrs; u_char irt_wpa; u_short irt_lifetime; } ih_rtradv;
    } icmp_hun;
};

#else // Unix-like
#include <arpa/inet.h>
#include <netinet/if_ether.h>
#include <netinet/ip.h>
#include <netinet/tcp.h>
#include <netinet/udp.h>
#include <netinet/ip_icmp.h>
#endif

#pragma pack(push, 2)
typedef struct tagSnapshot {
    int id;
    int source_port;
    int dest_port;
    char proto[22];
    char source_ip[22];
    char dest_ip[22];
    char source_mac[22];
    char dest_mac[22];
    char host_name[22];
} Snapshot;
#pragma pack(pop)

#endif // STRUCT_H
