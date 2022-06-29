#include "main.h"
#include "mono.h"
#include <psapi.h>
#include <tchar.h>

#define CURL_STATICLIB

#include <curl/curl.h>
#include <string>
#include <iostream>
#include <sstream>
#include <vector>
#include <iomanip>

#pragma comment(lib, "libcurl_a.lib")
#pragma comment(lib, "Ws2_32.lib")
#pragma comment(lib, "Wldap32.lib")
#pragma comment(lib, "Crypt32.lib")
#pragma comment(lib, "Normaliz.lib")

struct MemoryStruct {
	char* memory;
	size_t size;
};

static size_t WriteMemoryCallback(void* contents, size_t size, size_t nmemb, void* userp)
{
	size_t realsize = size * nmemb;
	struct MemoryStruct* mem = (struct MemoryStruct*)userp;

	char* ptr = (char*)realloc(mem->memory, mem->size + realsize + 1);
	if (ptr == NULL) {
		MessageBoxA(NULL, "Out Of Memory", "Loader", MB_OK);
		return 0;
	}

	mem->memory = ptr;
	memcpy(&(mem->memory[mem->size]), contents, realsize);
	mem->size += realsize;
	mem->memory[mem->size] = 0;

	return realsize;
}

void Run() 
{
	TCHAR hostExe[MAX_PATH];
	GetModuleFileName(NULL, hostExe, MAX_PATH);
	char filename[_MAX_FNAME];
	_splitpath_s(hostExe, NULL, 0, NULL, 0, filename, _MAX_FNAME, NULL, 0);
	if (strcmp(filename, "StickFight") == 0)
	{
		AllocConsole();
		FILE* f;
		freopen_s(&f, "CONOUT$", "w", stdout);

		MessageBoxA(NULL, "Downloading...", "Loader", MB_OK);

		CURL* curl_handle;
		CURLcode res;

		struct MemoryStruct data;
		data.memory = (char*)malloc(1);
		data.size = 0;

		curl_global_init(CURL_GLOBAL_ALL);
		curl_handle = curl_easy_init();
		curl_easy_setopt(curl_handle, CURLOPT_URL, "https://resources.redboxing.fr/stickFight/SFTGTrainer.dll");
		curl_easy_setopt(curl_handle, CURLOPT_WRITEFUNCTION, WriteMemoryCallback);
		curl_easy_setopt(curl_handle, CURLOPT_WRITEDATA, (void*)&data);
		curl_easy_setopt(curl_handle, CURLOPT_USERAGENT, "sftg-loader/1.0");

		res = curl_easy_perform(curl_handle);
		if (res != CURLE_OK) {
			MessageBoxA(NULL, "Failed to download DLL !", "Loader", MB_ICONERROR);
			ExitProcess(0);
			return;
		}

		MessageBoxA(NULL, "Initializing...", "Loader", MB_OK);
		while (!InitializeMono()) {
			//MessageBoxA(NULL, "Failed to initialize mono !", "Loader", MB_ICONERROR);
			std::cout << "Initializing..." << std::endl;
			Sleep(200);
		}

		InitializeMonoFunctions();

		std::cout << GetLastError() << std::endl;
		std::cout << "hMono : " << hMono << std::endl;

		std::cout << "mono_get_root_domain : " << mono_get_root_domain << std::endl;
		std::cout << "mono_thread_attach : " << mono_thread_attach << std::endl;

		std::cout << "mono_security_set_mode : " << mono_security_set_mode << std::endl;
		std::cout << "mono_image_open_from_data : " << mono_image_open_from_data << std::endl;

		MessageBoxA(NULL, "Injecting...", "Loader", MB_OK);

	//	mono_thread_attach(mono_get_root_domain());
		MonoDomain* root_domain = mono_get_root_domain();
		std::cout << root_domain << std::endl;
		MessageBoxA(NULL, "0", "Loader", MB_OK);
		mono_thread_attach(root_domain);


		MessageBoxA(NULL, "0", "Loader", MB_OK);
		mono_security_set_mode(NULL);

		MonoImageOpenStatus status;

		MessageBoxA(NULL, "0.5", "Loader", MB_OK);
		MonoImage* raw_img = mono_image_open_from_data(data.memory, data.size, true, &status);

		MessageBoxA(NULL, "1", "Loader", MB_OK);

		MonoAssembly* assembly = mono_assembly_load_from_full(raw_img, "UNSED", &status, 0);

		MessageBoxA(NULL, "2", "Loader", MB_OK);

		MonoImage* img = mono_assembly_get_image(assembly);

		MessageBoxA(NULL, "3", "Loader", MB_OK);

		MonoClass* mainClass = mono_class_from_name(img, "SFTGTrainer", "Loader");

		MessageBoxA(NULL, "4", "Loader", MB_OK);

		MonoMethod* method = mono_class_get_method_from_name(mainClass, "Load", 0);

		MessageBoxA(NULL, "5", "Loader", MB_OK);

		mono_runtime_invoke(method, NULL, NULL, NULL);

		MessageBoxA(NULL, "Injected !", "Loader", MB_OK);
	}
}