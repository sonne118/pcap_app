#ifndef IPC_H
#define IPC_H

#include <tchar.h>

inline HANDLE hPipe = ::CreateNamedPipe(_T("\\\\.\\pipe\\testpipe"),
	PIPE_ACCESS_DUPLEX,
	PIPE_TYPE_BYTE | PIPE_READMODE_BYTE,
	PIPE_UNLIMITED_INSTANCES,
	4096,
	4096,
	0,
	NULL);

#endif 

