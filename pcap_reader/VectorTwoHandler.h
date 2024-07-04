#ifndef VECTORTWOHANDLER_H
#define VECTORTWOHANDLER_H
#include <vector>
#include <iostream>
#include<struct.h>
#include<Handler.h>
#include<AbstractHandler.h>

using namespace std;

class VectorTwoHandler : public AbstractHandler
{
	std::vector<std::unique_ptr<vStruct>> _request;
public:
	~VectorTwoHandler() {};

public:
	void Handle(std::vector<std::unique_ptr<vStruct>>& request) override {

		vStruct* tmpBase = nullptr;
		OneStruct* tmp;

		int max_elelments = request.size();
		request.reserve(max_elelments * 2);

		for (const std::unique_ptr<vStruct>& v : request)
		{
			tmpBase = v.get();
			tmp = static_cast<OneStruct*>(tmpBase);

			SecondStruct* st = new SecondStruct(tmp->id,
				CopyToChar(tmp->source_ip),
				CopyToChar(tmp->dest_ip),
				CopyToChar(tmp->mac_source),
				CopyToChar(tmp->mac_destin));

			request.emplace_back(std::make_unique<SecondStruct>(*st));
		}

		request.erase(request.begin(), request.begin() + max_elelments);

		if (getHandle() != nullptr)
		{
			AbstractHandler::Handle(request);
		}
	}
};
#endif
