#ifndef STRUCT_H
#define STRUCT_H
#include <iostream>
using namespace std;

struct vStruct {

	int id;
};

struct OneStruct : public vStruct
{
	int id;
	string source_ip;
	string dest_ip;
	string mac_source;
	string mac_destin;
	string user_agent;

public:
	OneStruct(int _id,
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
};

struct SecondStruct : public vStruct
{
	int id;
	const char* source_ip;
	const char* dest_ip;
	const char* mac_source;
	const char* mac_destin;
	const char* user_agent;

public:
	SecondStruct(int _id = 1000,
		const char* _source_ip = "",
		const char* _dest_ip = "",
		const char* _mac_source = "",
		const char* _mac_destin = "",
		const char* _user_agent = "none")
	{
		id = _id;
		source_ip = _source_ip;
		dest_ip = _dest_ip;
		mac_source = _mac_source;
		mac_destin = _mac_destin;
		user_agent = _user_agent;
	}
};

#endif