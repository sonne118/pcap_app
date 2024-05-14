#ifndef ABSTRACTHANDLER_H
#define ABSTRACTHANDLER_H

#include "HttpLayer.h"
#include <string>
#include<struct.h>
#include<Handler.h>

using namespace std;

class AbstractHandler : public Handler {

protected:
	Handler* next_handler_;

public:
	AbstractHandler() : next_handler_(nullptr) {}

	~AbstractHandler() {}

	Handler* SetNext(Handler* handler) override {
		this->next_handler_ = handler;
		return handler;
	}

	void Handle(std::vector<std::unique_ptr<vStruct>>& request) override {
		if (this->next_handler_) {
			return this->next_handler_->Handle(request);
		}
	}

protected:
	std::string getAgentAsString(pcpp::HttpRequestLayer* httpRequestLayer)
	{
		std::string agent = httpRequestLayer->getFieldByName(PCPP_HTTP_USER_AGENT_FIELD)->getFieldValue();
		return agent;
	}

protected:
	const char* CopyToChar(const std::string& bar) {
		char* tmp = new char[bar.length() + 1];
		strcpy_s(tmp, strlen(tmp), bar.c_str());
		return  tmp;
	}
protected:
	Handler* getHandle() const { return next_handler_; }
};

#endif