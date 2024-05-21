#include <WinSock2.h>
#include <pcap.h>
#include <time.h>
#include <iostream>
#include <string.h>

using namespace std;

#ifndef ETHERTYPE_IP
#define ETHERTYPE_IP		0x0800	/* IP protocol */
#endif
#define ETHER_ADDR_LEN 6


//Ethernet Header
typedef struct ether_header {
    unsigned char ether_dhost [ETHER_ADDR_LEN];	
    unsigned char ether_shost[ETHER_ADDR_LEN];	
    unsigned short ether_type;
}ETHHEADER, * PETHHEADER;

//IPv4 Header
typedef struct ip {
    unsigned char ver_ihl;		
    unsigned char tos;			
    unsigned short tlen;		
    unsigned short identification;
    unsigned short flags_fo;	
    unsigned char ttl;			
    unsigned char ip_p;  
    unsigned char crc;			
    u_char ip_src[4];			
    u_char ip_dst[4];			
}IPHEADER, * PIPHEADER;

typedef struct tcphdr {   
    u_short sport;          
    u_short dport;          
    u_short len;            
    u_short crc;            
}TCPHEADER, * PTCPHEADER;

void packetHandler(u_char* userData, const struct pcap_pkthdr* pkthdr, const u_char* packet);

int main() {
    pcap_t* descr;
    char errbuf[PCAP_ERRBUF_SIZE];
    pcap_if_t* alldevs;
    pcap_if_t* d;
    int inum;
    int i = 0;
    pcap_t* adhandle;

    if (pcap_findalldevs_ex(PCAP_SRC_IF_STRING, NULL, &alldevs, errbuf) == -1)
    {
        fprintf(stderr, "Error in pcap_findalldevs: %s\n", errbuf);
        exit(1);
    }

    /* Print the list */
    for (d = alldevs; d; d = d->next)
    {
        printf("%d. %s", ++i, d->name);
        if (d->description)
            printf(" (%s)\n", d->description);
        else
            printf(" (No description available)\n");
    }

    if (i == 0)
    {
        printf("\nNo interfaces found! Make sure WinPcap is installed.\n");
        return -1;
    }

    printf("Enter the interface number (1-%d):", i);
    scanf_s("%d", &inum);

    if (inum < 1 || inum > i)
    {
        printf("\nInterface number out of range.\n");
        /* Free the device list */
        pcap_freealldevs(alldevs);
        return -1;
    }

    /* Jump to the selected adapter */
    for (d = alldevs, i = 0; i < inum - 1;d = d->next, i++);

    /* Open the device */
    if ((adhandle = pcap_open(d->name,          // name of the device
        65536,            // portion of the packet to capture
        // 65536 guarantees that the whole packet will be captured on all the link layers
        PCAP_OPENFLAG_PROMISCUOUS,    // promiscuous mode
        1000,             // read timeout
        NULL,             // authentication on the remote machine
        errbuf            // error buffer
    )) == NULL)
    {
        fprintf(stderr, "\nUnable to open the adapter. %s is not supported by WinPcap\n", d->name);
        /* Free the device list */
        pcap_freealldevs(alldevs);
        return -1;
    }

    printf("\nlistening on %s...\n", d->description);

   
    pcap_freealldevs(alldevs);
    descr = pcap_open_offline("D:/repo/test2/pcap_app/pcap_app/get_packages/http.pcap", errbuf);
    if (descr == NULL) {
        cout << "pcap_open_live() failed: " << errbuf << endl;
        return 1;
    }
    
    //if (pcap_loop(descr, 0, packetHandler, NULL) < 0) {
      if (pcap_loop(adhandle, 0, packetHandler, NULL) < 0) {
        //cout << "pcap_loop() failed: " << pcap_geterr(descr);
        return 1;
    }

    cout << "capture finished" << endl;

    return 0;
}

int ether_ntoa(const unsigned char etheraddr[ETHER_ADDR_LEN], char* dest, size_t len)
{
    return snprintf(dest, len, "%02x:%02x:%02x:%02x:%02x:%02x",
        (unsigned)etheraddr[0],
        (unsigned)etheraddr[1],
        (unsigned)etheraddr[2],
        (unsigned)etheraddr[3],
        (unsigned)etheraddr[4],
        (unsigned)etheraddr[5]);
}

void packetHandler(u_char* userData, const struct pcap_pkthdr* pkthdr, const u_char* packet) {
    const struct ether_header* ethernetHeader;

    const struct eth_header* eth_Header;
    const struct ip* ipHeader;
    const struct tcphdr* tcpHeader;
    char sourceIp[INET_ADDRSTRLEN];
    char destIp[INET_ADDRSTRLEN];
    char* ethernet_shost = nullptr;
    char* ethernet_dhost = nullptr;
    std::string check = "";
    const unsigned char* ptr;
    const unsigned char* ptr1;
    const unsigned char* ptr2;
    int i;


    u_int sourcePort, destPort;
    u_char* data;
    int dataLength = 0;
    string dataStr = "";

    ethernetHeader = (struct ether_header*)packet;
    if (ntohs(ethernetHeader->ether_type) == ETHERTYPE_IP) {
        ipHeader = (struct ip*)(packet + sizeof(struct ether_header));
        inet_ntop(AF_INET, &(ipHeader->ip_src), sourceIp, INET_ADDRSTRLEN);
        inet_ntop(AF_INET, &(ipHeader->ip_dst), destIp, INET_ADDRSTRLEN);
        
        ptr1 = ethernetHeader->ether_shost;
        ptr2 = ethernetHeader->ether_dhost;

        char eout1[32]; char eout2[32];
        ether_ntoa(ptr1, eout1, sizeof eout1);
        ether_ntoa(ptr2, eout2, sizeof eout2);


        if (ipHeader->ip_p == IPPROTO_TCP) {
            tcpHeader = (tcphdr*)(packet + sizeof(struct ether_header) + sizeof(struct ip));
            sourcePort = ntohs(tcpHeader->sport);
            destPort = ntohs(tcpHeader->dport);
            data = (u_char*)(packet + sizeof(struct ether_header) + sizeof(struct ip) + sizeof(struct tcphdr));
            dataLength = pkthdr->len - (sizeof(struct ether_header) + sizeof(struct ip) + sizeof(struct tcphdr));

            // convert non-printable characters, other than carriage return, line feed,
            // or tab into periods when displayed.
            for (int i = 0; i < dataLength; i++) {
                if ((data[i] >= 32 && data[i] <= 126) || data[i] == 10 || data[i] == 11 || data[i] == 13) {
                    dataStr += (char)data[i];
                }
                else {
                    dataStr += ".";
                }
            }
           
            cout << sourceIp << ":" << sourcePort << " -> " << destIp << ":" << destPort <<"->  D_Address:   "<< eout1 << "->  S_Address:     " << eout2 << endl;
            if (dataLength > 0) {
                //cout << dataStr << endl;
            }
        }
    }
}

