#include <WinSock2.h>
#include <pcap.h>
#include <time.h>
#include <iostream>
#include <string.h>
#include <ipc.h>
#include <sstream>
#include <struct.h>
#include <ether_ntoa.h>

using namespace std;

int main() {

	pcap_t* descr;
	char errbuf[PCAP_ERRBUF_SIZE];
	pcap_if_t* alldevs;
	pcap_if_t* d;
	int inum = 4;
	int i = 0;
	pcap_t* adhandle;
	struct pcap_pkthdr* pkthdr;
	const u_char* packet;


	if (pcap_findalldevs_ex(PCAP_SRC_IF_STRING, NULL, &alldevs, errbuf) == -1)
	{
		fprintf(stderr, "Error in pcap_findalldevs: %s\n", errbuf);
		exit(1);
	}

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
	//scanf_s("%d", &inum);

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
	descr = pcap_open_offline("D:/repo/test2/pcap_app/pcap_app/sniffer_packages/http.pcap", errbuf);
	if (descr == NULL) {
		cout << "pcap_open_live() failed: " << errbuf << endl;
		return 1;
	}


	std::vector<std::thread> threads;
	threads.emplace_back(producer, adhandle, pkthdr, packet);
	threads.emplace_back(consumer);

	for (auto& thread : threads) {
		thread.join();
	}

	cout << "capture finished" << endl;
	return 0;
}





