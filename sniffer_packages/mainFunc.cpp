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

#include "packages.h"
#include <iostream>
#include <string.h>
#include <vector>
#include <thread> 
#include <memory>
#include <ipc.h>
#include <random>
#include <functional>
#include <builderDevice.h>
#include <handleProto.h>

int mainFunc(HANDLE eventHandle) {

	int file = 0; bool dev = true;
	handleProto p;
	Packages pack(p);
	auto lmd = [&pack](HANDLE eventHandle) {pack.setHandler(eventHandle); };
	lmd(eventHandle);

	if (dev || file)
	{
		std::vector<std::unique_ptr<std::thread>> threads;
		threads.emplace_back(std::make_unique<std::thread>([&pack]() {pack.producer(std::ref(quit_flag));}));
		threads.emplace_back(std::make_unique<std::thread>([&pack]() {pack.consumer();}));

		for (auto& thread : threads) {
			thread->join();
		}
	}
	
	std::cout << "capture finished" << std::endl;
	return 0;
}

// Placeholder implementation - replace with actual code
int mainFunction() {
    // Main function implementation
    return 0;
}
