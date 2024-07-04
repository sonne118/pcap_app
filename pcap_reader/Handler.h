#ifndef HANDLER_H
#define HANDLER_H
#include <vector>
#include<struct.h>
#include<Handler.h>

using namespace std;

class Handler
{
public:
	virtual ~Handler() = default;
	virtual	Handler* SetNext(Handler* handler) = 0;
	virtual	void Handle(std::vector<std::unique_ptr<vStruct>>& request) = 0;
};

#endif
