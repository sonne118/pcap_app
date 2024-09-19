#pragma once
#ifndef HANDLEPROTO_H
#define HANDLEPROTO_H
#include <iostream>
#include <map>
#include <functional>
#include <cstringt.h>

#pragma warning(disable:4996) 

class handleProto
{
public:
	~handleProto() = default;
	handleProto();
	handleProto(handleProto* proto);

public:
	void* handlePROTO_IP();
	void* handlePROTO_TCP();
	void* handlePROTO_UDP();
	void* handlePROTO_PUP();
	void* handlePROTO_ICMP();
	void* handlePROTO_IGMP();
	void* handlePROTO_GGP();
	void* handlePROTO_IDP();
	void* handlePROTO_ST();
	void* handlePROTO_RDP();
	void* handlePROTO_ND();
	void* handlePROTO_L2TP();
	void* handlePROTO_PIM();
	void* handlePROTO_PGM();
	void* handlePROTO_SCTP();
	void* handlePROTO_CBT();
	void* handlePROTO_EGP();
	void* handlePROTO_IGP();
	void* handlePROTO_IPV4();
	void* handlePROTO_IPV6();
	void* handlePROTO_ROUTING();
	void* handlePROTO_FRAGMENT();
	void* handlePROTO_ESP();
	void* handlePROTO_AH();
	void* handlePROTO_ICMPV6();
	void* handlePROTO_NONE();
	void* handlePROTO_DSTOPTS();
	void* handlePROTO_RAW();
	void* handlePROTO_MAX();
	void* handlePROTO_RESERVED_IPSEC();
	void* handleDefault();

public:
	std::map<int, std::function<void()>> caseMap;
	int* _src_port;
	int* _dst_port;
	char protoStr[22];
private:
	handleProto* p;
};

inline handleProto::handleProto() :protoStr() {
	_src_port = nullptr;
	_dst_port = nullptr;
}

inline handleProto::handleProto(handleProto* proto) {
	p = proto;
	caseMap[0] = std::bind(&handleProto::handlePROTO_IP, proto);
	caseMap[6] = std::bind(&handleProto::handlePROTO_TCP, proto);
	caseMap[17] = std::bind(&handleProto::handlePROTO_UDP, proto);
	caseMap[12] = std::bind(&handleProto::handlePROTO_PUP, proto);
	caseMap[1] = std::bind(&handleProto::handlePROTO_ICMP, proto);
	caseMap[2] = std::bind(&handleProto::handlePROTO_IGMP, proto);
	caseMap[3] = std::bind(&handleProto::handlePROTO_GGP, proto);
	caseMap[22] = std::bind(&handleProto::handlePROTO_IDP, proto);
	caseMap[5] = std::bind(&handleProto::handlePROTO_ST, proto);
	caseMap[27] = std::bind(&handleProto::handlePROTO_RDP, proto);
	caseMap[77] = std::bind(&handleProto::handlePROTO_ND, proto);
	caseMap[115] = std::bind(&handleProto::handlePROTO_L2TP, proto);
	caseMap[103] = std::bind(&handleProto::handlePROTO_PIM, proto);
	caseMap[113] = std::bind(&handleProto::handlePROTO_PGM, proto);
	caseMap[132] = std::bind(&handleProto::handlePROTO_SCTP, proto);
	caseMap[7] = std::bind(&handleProto::handlePROTO_CBT, proto);
	caseMap[8] = std::bind(&handleProto::handlePROTO_EGP, proto);
	caseMap[9] = std::bind(&handleProto::handlePROTO_IGP, proto);
	caseMap[4] = std::bind(&handleProto::handlePROTO_IPV4, proto);
	caseMap[58] = std::bind(&handleProto::handlePROTO_IPV6, proto);
	caseMap[59] = std::bind(&handleProto::handlePROTO_NONE, proto);
	caseMap[60] = std::bind(&handleProto::handlePROTO_DSTOPTS, proto);
	caseMap[257] = std::bind(&handleProto::handlePROTO_RAW, proto);
	caseMap[256] = std::bind(&handleProto::handlePROTO_MAX, proto);
	caseMap[258] = std::bind(&handleProto::handlePROTO_RESERVED_IPSEC, proto);
	caseMap[43] = std::bind(&handleProto::handlePROTO_ROUTING, proto);
	caseMap[44] = std::bind(&handleProto::handlePROTO_FRAGMENT, proto);
	caseMap[50] = std::bind(&handleProto::handlePROTO_ESP, proto);
	caseMap[51] = std::bind(&handleProto::handlePROTO_AH, proto);
}

void* handleProto::handlePROTO_IP()
{
	strcpy(protoStr, "IP");
	return 0;
}

void* handleProto::handlePROTO_TCP()
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
	return 0;
}

void* handleProto::handlePROTO_UDP()
{
	int sp = *_src_port;
	int dp = *_dst_port;
	if (sp == 53 || dp == 53)
		strcpy(protoStr, "DNS");
	else if (sp == 67 || dp == 68)
		strcpy(protoStr, "DHCP");
	else
		strcpy(protoStr, "UDP");
	return 0;
}

void* handleProto::handlePROTO_PUP()
{
	strcpy(protoStr, "PUP");
	return 0;
}

void* handleProto::handlePROTO_ICMP()
{
	strcpy(protoStr, "ICMP");
	return 0;
}

void* handleProto::handlePROTO_IGMP()
{
	strcpy(protoStr, "IGMP");
	return 0;
}

void* handleProto::handlePROTO_GGP()
{
	strcpy(protoStr, "GGP");
	return 0;
}

void* handleProto::handlePROTO_IDP()
{
	strcpy(protoStr, "IDP");
	return 0;
}

void* handleProto::handlePROTO_ST()
{
	strcpy(protoStr, "ST");
	return 0;
}

void* handleProto::handlePROTO_RDP()
{
	strcpy(protoStr, "RDP");
	return 0;
}

void* handleProto::handlePROTO_ND()
{
	strcpy(protoStr, "ND");
	return 0;
}

void* handleProto::handlePROTO_L2TP()
{
	strcpy(protoStr, "L2TP");
	return 0;
}

void* handleProto::handlePROTO_PIM()
{
	strcpy(protoStr, "PIM");
	return 0;
}

void* handleProto::handlePROTO_PGM()
{
	strcpy(protoStr, "PGM");
	return 0;
}

void* handleProto::handlePROTO_SCTP()
{
	strcpy(protoStr, "SCTP");
	return 0;
}

void* handleProto::handlePROTO_CBT()
{
	strcpy(protoStr, "CBT");
	return 0;
}

void* handleProto::handlePROTO_EGP()
{
	strcpy(protoStr, "EGP");
	return 0;
}

void* handleProto::handlePROTO_IGP()
{
	strcpy(protoStr, "IGP");
	return 0;
}

void* handleProto::handlePROTO_IPV4()
{
	strcpy(protoStr, "IPV4");
	return 0;
}

void* handleProto::handlePROTO_IPV6()
{
	strcpy(protoStr, "IPV6");
	return 0;
}

void* handleProto::handlePROTO_ROUTING()
{
	strcpy(protoStr, "ROUTING");
	return 0;
}

void* handleProto::handlePROTO_FRAGMENT()
{
	strcpy(protoStr, "FRAGMENT");
	return 0;
}

void* handleProto::handlePROTO_ESP()
{
	strcpy(protoStr, "ESP");
	return 0;
}

void* handleProto::handlePROTO_AH()
{
	strcpy(protoStr, "AH");
	return 0;
}

void* handleProto::handlePROTO_RESERVED_IPSEC()
{
	strcpy(protoStr, "RESERVED_IPSEC");
	return 0;
}

void* handleProto::handlePROTO_ICMPV6()
{
	strcpy(protoStr, "ICMPV6");
	return 0;
}

void* handleProto::handlePROTO_NONE()
{
	strcpy(protoStr, "NONE");
	return 0;
}

void* handleProto::handlePROTO_DSTOPTS()
{
	strcpy(protoStr, "PROTO_DSTOPTS");
	return 0;
}

void* handleProto::handlePROTO_RAW()
{
	strcpy(protoStr, "PROTO_RAW");
	return 0;
}

void* handleProto::handlePROTO_MAX()
{
	strcpy(protoStr, "PROTO_MAX");
	return 0;
}

void* handleProto::handleDefault()
{
	return 0;
}
#endif