#include <packages.h>
#include <iostream>
#include <string.h>
#include <vector>
#include <thread> 
#include <memory>
#include <ipc.h>
#include <atlsafe.h>
#include <random>
#include <functional>
#include <openDevices.h>

int mainFunc(HANDLE eventHandle) {

	int file = 0; bool dev = true;
	handleProto p;
	int  src_port = 0;
	int  dst_port = 0;
	Packages pack(src_port, dst_port, p);
	auto lmd = [&pack](HANDLE eventHandle) {pack.setHandler(eventHandle); };
	lmd(eventHandle);

	if (dev || file)
	{
		std::vector<std::unique_ptr<std::thread>> threads;
		threads.emplace_back(std::make_unique<std::thread>([&pack]() {pack.producer(std::ref(quit_flag));}));
		threads.emplace_back(std::make_unique<std::thread>([&pack]() {pack.consumer();}));

		for (auto& thread : threads) {
			thread->join();
		}
	}
	[&pack]() {pack.~Packages();};
	free(&pack);
	std::cout << "capture finished" << std::endl;
	return 0;
}



























