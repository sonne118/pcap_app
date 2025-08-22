// macOS compatibility shims must appear before including packages.h
#ifdef __APPLE__
  #include <net/ethernet.h>
  #include <netinet/ip.h>
  #include <netinet/tcp.h>
  #include <arpa/inet.h>
  #ifndef ETHER_HDR_LEN
    #define ETHER_HDR_LEN 14
  #endif
  #ifndef SIZE_ETHERNET
    #define SIZE_ETHERNET ETHER_HDR_LEN
  #endif
  #ifndef IPv4_ETHERTYPE
    #define IPv4_ETHERTYPE ETHERTYPE_IP
  #endif
  #ifndef IP_HL
    #define IP_HL(ip) ((ip)->ip_hl)
  #endif
  #ifndef IP_V
    #define IP_V(ip) ((ip)->ip_v)
  #endif
  // Some code may use ip_vhl (Linux/Windows). Map it to ip_hl on macOS.
  #ifndef ip_vhl
    #define ip_vhl ip_hl
  #endif
  // Map Linux/Win field tokens to BSD/macOS names in struct tcphdr
  #ifndef sport
    #define sport th_sport
  #endif
  #ifndef dport
    #define dport th_dport
  #endif
#endif

// Global variables and functions for package handling
#include "packages.h"

// Placeholder implementation - replace with actual code
void initializePackageGlobals() {
    // Initialize global variables
}

// Global variable definitions for packages.h
tagSnapshot shared_buff[buff_max];
std::atomic<int> free_index(0);
std::atomic<int> full_index(0);
std::mutex mtx;
// Single definition for pcap error buffer (declared extern in builderDevice.h)
char errbuf[PCAP_ERRBUF_SIZE];
