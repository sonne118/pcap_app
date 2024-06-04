#ifndef ETHER_NTOA_H
#define ETHER_NTOA_H
#include <stdio.h>

#define ETHER_ADDR_LEN 6
#ifndef ETHERTYPE_IP
#define ETHERTYPE_IP		0x0800	
#endif

inline int ether_ntoa(const unsigned char etheraddr[ETHER_ADDR_LEN], char* dest, size_t len)
{
	return snprintf(dest, len, "%02x:%02x:%02x:%02x:%02x:%02x",
		(unsigned)etheraddr[0],
		(unsigned)etheraddr[1],
		(unsigned)etheraddr[2],
		(unsigned)etheraddr[3],
		(unsigned)etheraddr[4],
		(unsigned)etheraddr[5]);
}
#endif

