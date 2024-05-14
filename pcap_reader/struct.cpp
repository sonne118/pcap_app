#include <string>
#include <iostream>
#include <memory>
#include <struct.h>
using namespace std;

OneStruct::OneStruct(int _id,
	string _source_ip,
	string _dest_ip,
	string _mac_source,
	string _mac_destin,
	string _user_agent)
{
	id = _id;
	source_ip = _source_ip;
	dest_ip = _dest_ip;
	mac_source = _mac_source;
	mac_destin = _mac_destin;
	user_agent = _user_agent;
}

SecondStruct::SecondStruct(int _id,
	const char* _source_ip,
	const char* _dest_ip,
	const char* _mac_source,
	const char* _mac_destin,
	const char* _user_agent)
{
	id = _id;
	source_ip = _source_ip;
	dest_ip = _dest_ip;
	mac_source = _mac_source;
	mac_destin = _mac_destin;
	user_agent = _user_agent;
}