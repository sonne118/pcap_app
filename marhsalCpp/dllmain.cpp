// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include "framework.h"
#include <array>
#include <stdio.h>
#include <strsafe.h>
#include <combaseapi.h>
#include <string>	
#include <IPv4Layer.h>
#include <TcpLayer.h>
#include <Packet.h>
#include <PcapFileDevice.h>
#include "EthLayer.h"
#include "HttpLayer.h"
#include "stdlib.h"
#include <iostream>
#include "SystemUtils.h"
#include "PcapLiveDeviceList.h"
#include <cstdlib>
#include <string>
#define API __declspec(dllexport)

extern "C"
{

	struct Name
	{
		char* FirstName;
		const char* LastName;
	};

	API void  GetName(struct Name** ppStruct)
	{
		const int cArraySize = 5;
		//*pSize = cArraySize;
		*ppStruct = (Name*)CoTaskMemAlloc(cArraySize * sizeof(Name));	
		Name* pCurStruct = *ppStruct;
		char* buffer;
		for (int i = 0; i < cArraySize; i++, pCurStruct++)
		{
			pCurStruct->LastName = "bla bla bla";
			buffer = (char*)CoTaskMemAlloc(sizeof(char) * (i + 2));
			::ZeroMemory(buffer, sizeof(char) * (i + 2));
			strncpy_s(buffer, i + 2, "*********", i + 1);
			pCurStruct->FirstName = buffer;			
		}
	}
}