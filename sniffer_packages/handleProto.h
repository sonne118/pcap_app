#pragma once
#ifndef HANDLE_PROTO_H
#define HANDLE_PROTO_H
#include <iostream>
#include <map>
#include <functional>
#include <cstring>

#pragma warning(disable:4996) 

class handleProto
{
public:
	~handleProto();
	handleProto();
	handleProto(handleProto* proto);

public:
	void* initialize();
	void handlePROTO_IP();
	void handlePROTO_TCP();
	void handlePROTO_UDP();
	void handlePROTO_PUP();
	void handlePROTO_ICMP();
	void handlePROTO_IGMP();
	void handlePROTO_GGP();
	void handlePROTO_IDP();
	void handlePROTO_ST();
	void handlePROTO_RDP();
	void handlePROTO_ND();
	void handlePROTO_L2TP();
	void handlePROTO_PIM();
	void handlePROTO_PGM();
	void handlePROTO_SCTP();
	void handlePROTO_CBT();
	void handlePROTO_EGP();
	void handlePROTO_IGP();
	void handlePROTO_IPV4();
	void handlePROTO_IPV6();
	void handlePROTO_ROUTING();
	void handlePROTO_FRAGMENT();
	void handlePROTO_ESP();
	void handlePROTO_AH();
	void handlePROTO_ICMPV6();
	void handlePROTO_NONE();
	void handlePROTO_DSTOPTS();
	void handlePROTO_RAW();
	void handlePROTO_MAX();
	void handlePROTO_RESERVED_IPSEC();
	void* handleDefault();

public:
	std::map<int, std::function<void()>> caseMap;
	int* _src_port;
	int* _dst_port;
	char* protoStr;
private:
	handleProto* p;
};

inline handleProto::~handleProto()
{
	_src_port = NULL;
	_dst_port = NULL;
	p = NULL;
	delete _src_port;
	delete _dst_port;
	delete p;
}

inline handleProto::handleProto() :protoStr() {
	_src_port = 0;
	_dst_port = 0;
	 p = nullptr;
	//initialize();
}

inline handleProto::handleProto(handleProto* proto) {

	//this->~handleProto();
	//new (this) handleProto();
	proto = this;
	p = proto;
	initialize();
}

 inline void* handleProto::initialize()
{
	caseMap[0] = std::bind(&handleProto::handlePROTO_IP, p);
	caseMap[6] = std::bind(&handleProto::handlePROTO_TCP, p);
	caseMap[17] = std::bind(&handleProto::handlePROTO_UDP, p);
	caseMap[12] = std::bind(&handleProto::handlePROTO_PUP, p);
	caseMap[1] = std::bind(&handleProto::handlePROTO_ICMP, p);
	caseMap[2] = std::bind(&handleProto::handlePROTO_IGMP, p);
	caseMap[3] = std::bind(&handleProto::handlePROTO_GGP, p);
	caseMap[22] = std::bind(&handleProto::handlePROTO_IDP, p);
	caseMap[5] = std::bind(&handleProto::handlePROTO_ST, p);
	caseMap[27] = std::bind(&handleProto::handlePROTO_RDP, p);
	caseMap[77] = std::bind(&handleProto::handlePROTO_ND, p);
	caseMap[115] = std::bind(&handleProto::handlePROTO_L2TP, p);
	caseMap[103] = std::bind(&handleProto::handlePROTO_PIM, p);
	caseMap[113] = std::bind(&handleProto::handlePROTO_PGM, p);
	caseMap[132] = std::bind(&handleProto::handlePROTO_SCTP, p);
	caseMap[7] = std::bind(&handleProto::handlePROTO_CBT, p);
	caseMap[8] = std::bind(&handleProto::handlePROTO_EGP, p);
	caseMap[9] = std::bind(&handleProto::handlePROTO_IGP, p);
	caseMap[4] = std::bind(&handleProto::handlePROTO_IPV4, p);
	caseMap[58] = std::bind(&handleProto::handlePROTO_IPV6, p);
	caseMap[59] = std::bind(&handleProto::handlePROTO_NONE, p);
	caseMap[60] = std::bind(&handleProto::handlePROTO_DSTOPTS, p);
	caseMap[257] = std::bind(&handleProto::handlePROTO_RAW, p);
	caseMap[256] = std::bind(&handleProto::handlePROTO_MAX, p);
	caseMap[258] = std::bind(&handleProto::handlePROTO_RESERVED_IPSEC, p);
	caseMap[43] = std::bind(&handleProto::handlePROTO_ROUTING, p);
	caseMap[44] = std::bind(&handleProto::handlePROTO_FRAGMENT, p);
	caseMap[50] = std::bind(&handleProto::handlePROTO_ESP, p);
	caseMap[51] = std::bind(&handleProto::handlePROTO_AH, p);
	return 0;
}

 inline void handleProto::handlePROTO_IP()
{
	strcpy(protoStr, "IP");
}

inline void handleProto::handlePROTO_TCP()
{
	int sp = *_src_port;
	int dp = *_dst_port;
	if (sp == 443 || dp == 443)
		strcpy(protoStr, "TLS");
	else if (sp == 80 || dp == 80)
		strcpy(protoStr, "Http");
	else if (sp == 53 || dp == 53)
		strcpy(protoStr, "DNS");
	else
		strcpy(protoStr, "TCP");
}

inline void handleProto::handlePROTO_UDP()
{
	int sp = *_src_port;
	int dp = *_dst_port;
	if (sp == 53 || dp == 53)
		strcpy(protoStr, "DNS");
	else if (sp == 67 || dp == 68)
		strcpy(protoStr, "DHCP");
	else
		strcpy(protoStr, "UDP");
}

inline void handleProto::handlePROTO_PUP()
{
	strcpy(protoStr, "PUP");
}

inline void handleProto::handlePROTO_ICMP()
{
	strcpy(protoStr, "ICMP");
}

inline void handleProto::handlePROTO_IGMP()
{
	strcpy(protoStr, "IGMP");
}

inline void handleProto::handlePROTO_GGP()
{
	strcpy(protoStr, "GGP");
}

inline void handleProto::handlePROTO_IDP()
{
	strcpy(protoStr, "IDP");
}

inline void handleProto::handlePROTO_ST()
{
	strcpy(protoStr, "ST");
}

inline void handleProto::handlePROTO_RDP()
{
	strcpy(protoStr, "RDP");
}

inline void handleProto::handlePROTO_ND()
{
	strcpy(protoStr, "ND");
}

inline void handleProto::handlePROTO_L2TP()
{
	strcpy(protoStr, "L2TP");
}

inline void handleProto::handlePROTO_PIM()
{
	strcpy(protoStr, "PIM");
}

inline void handleProto::handlePROTO_PGM()
{
	strcpy(protoStr, "PGM");
}

inline void handleProto::handlePROTO_SCTP()
{
	strcpy(protoStr, "SCTP");
}

inline void handleProto::handlePROTO_CBT()
{
	strcpy(protoStr, "CBT");
}

inline void handleProto::handlePROTO_EGP()
{
	strcpy(protoStr, "EGP");
}

inline void handleProto::handlePROTO_IGP()
{
	strcpy(protoStr, "IGP");
}

inline void handleProto::handlePROTO_IPV4()
{
	strcpy(protoStr, "IPV4");
}

inline void handleProto::handlePROTO_IPV6()
{
	strcpy(protoStr, "IPV6");
}

inline void handleProto::handlePROTO_ROUTING()
{
	strcpy(protoStr, "ROUTING");
}

inline void handleProto::handlePROTO_FRAGMENT()
{
	strcpy(protoStr, "FRAGMENT");
}

inline void handleProto::handlePROTO_ESP()
{
	strcpy(protoStr, "ESP");
}

inline void handleProto::handlePROTO_AH()
{
	strcpy(protoStr, "AH");
}

inline void handleProto::handlePROTO_RESERVED_IPSEC()
{
	strcpy(protoStr, "RESERVED_IPSEC");
}

inline void handleProto::handlePROTO_ICMPV6()
{
	strcpy(protoStr, "ICMPV6");
}

inline void handleProto::handlePROTO_NONE()
{
	strcpy(protoStr, "NONE");
}

inline void handleProto::handlePROTO_DSTOPTS()
{
	strcpy(protoStr, "PROTO_DSTOPTS");
}

inline void handleProto::handlePROTO_RAW()
{
	strcpy(protoStr, "PROTO_RAW");
}

inline void handleProto::handlePROTO_MAX()
{
	strcpy(protoStr, "PROTO_MAX");
}

inline void* handleProto::handleDefault()
{
	return 0;
}
#endif