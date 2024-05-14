#include<atlsafe.h>
#include<vector>
#include<struct.h>
#include<Handler.h>
#include<AbstractHandler.h>
#include<VectorOneHandler.h>
#include<VectorTwoHandler.h>

static std::vector<SecondStruct> ClientCode(int* size)
	{
	    std::vector<SecondStruct> vec{};
		std::vector<std::unique_ptr<vStruct>> request{};
		vStruct* tmpBase = nullptr;
		SecondStruct* tmp;

		VectorOneHandler* vectorOne = new VectorOneHandler();
		VectorTwoHandler* vectorTwo = new VectorTwoHandler();

		vectorOne->SetNext(vectorTwo);
		vectorOne->Handle(request);

		for (auto& v : request) {
			tmpBase = v.get();
			tmp = static_cast<SecondStruct*>(tmpBase);
			SecondStruct st{};
			st.id = tmp->id;
			st.source_ip = tmp->source_ip;
			st.dest_ip = tmp->dest_ip;
			st.mac_source = tmp->mac_source;
			st.mac_destin = tmp->mac_destin;
			st.user_agent = tmp->user_agent;
			vec.push_back(st);
			v.reset();
		}
		*size = request.size();
		delete vectorOne;
		delete vectorTwo;
		return vec;
	}

std::vector<SecondStruct> result{};
extern "C" __declspec(dllexport) SecondStruct * Predict(int* size1, const char* path2D)
{
	int size;
	result = ClientCode(&size);
	*size1 = size;

	return result.data();
}




