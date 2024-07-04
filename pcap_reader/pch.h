#include <vector>
#include <VectorTwoHandler.h>
#ifndef PCH_H
#define PCH_H


static std::vector<SecondStruct> ClientCode(int* size);

extern "C" __declspec(dllexport) SecondStruct * Predict(int* size1, const char* path2D);

#endif
