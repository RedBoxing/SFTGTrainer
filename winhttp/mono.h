#pragma once

#include <Windows.h>
#include <stdint.h>

typedef struct _MonoClass MonoClass;
typedef struct _MonoImage MonoImage;
typedef struct _MonoMethod MonoMethod;
typedef struct _MonoObject MonoObject;
typedef struct _MonoThread MonoThread;
typedef struct _MonoDomain MonoDomain;
typedef struct _MonoAssembly MonoAssembly;
typedef struct _MonoProperty MonoProperty;

typedef enum {
	MONO_IMAGE_OK,
	MONO_IMAGE_ERROR_ERRNO,
	MONO_IMAGE_MISSING_ASSEMBLYREF,
	MONO_IMAGE_IMAGE_INVALID
} MonoImageOpenStatus;

HMODULE hMono = NULL;

//Class
typedef MonoClass* (__cdecl* mono_class_from_name_t)(MonoImage* image, const char* name_space, const char* name);
typedef MonoMethod* (__cdecl* mono_class_get_method_from_name_t)(MonoClass* mclass, const char* name, int param_count);

//Assembly
typedef MonoAssembly* (__cdecl* mono_assembly_open_t)(const char* filename, MonoImageOpenStatus* status);
typedef MonoAssembly* (__cdecl* mono_assembly_load_from_full_t)(MonoImage* image, const char* fname, MonoImageOpenStatus* status, bool refonly);
typedef MonoImage* (__cdecl* mono_assembly_get_image_t)(MonoAssembly* assembly);
typedef MonoAssembly* (__cdecl* mono_assembly_load_from_t)(MonoImage* image, const char* fname, MonoImageOpenStatus* status);

//Image
typedef MonoImage* (__cdecl* mono_image_open_from_data_t)(char* data, uint32_t data_len, bool need_copy, MonoImageOpenStatus* status);
typedef MonoImage* (__cdecl* mono_image_open_from_data_full_t)(char* data, uint32_t data_len, bool need_copy, MonoImageOpenStatus* status, bool refonly);
typedef MonoImage* (__cdecl* mono_image_open_t)(const char* fname, MonoImageOpenStatus* status);

//Object
typedef MonoObject* (__cdecl* mono_runtime_invoke_t)(MonoMethod* method, void* obj, void** params, MonoObject** exc);

//Thread
typedef MonoThread* (__cdecl* mono_thread_attach_t)(MonoDomain* domain);

//Domain
typedef MonoDomain* (__cdecl* mono_get_root_domain_t)(void);
typedef MonoDomain* (__cdecl* mono_domain_get_t)(void);

//Security
typedef void* (__cdecl* mono_security_set_mode_t)(DWORD);

//Property
typedef MonoProperty* (__cdecl* mono_class_get_property_from_name_t)(MonoClass* mclass, const char* name);
typedef MonoMethod* (__cdecl* mono_property_get_set_method_t)(MonoProperty* prop);
typedef MonoMethod* (__cdecl* mono_property_get_get_method_t)(MonoProperty* prop);

mono_class_from_name_t mono_class_from_name;
mono_class_get_method_from_name_t mono_class_get_method_from_name;

mono_assembly_open_t mono_assembly_open;
mono_assembly_load_from_full_t mono_assembly_load_from_full;
mono_assembly_get_image_t mono_assembly_get_image;
mono_assembly_load_from_t mono_assembly_load_from;

mono_image_open_from_data_t mono_image_open_from_data;
mono_image_open_from_data_full_t mono_image_open_from_data_full;
mono_image_open_t mono_image_open;

mono_runtime_invoke_t mono_runtime_invoke;

mono_thread_attach_t mono_thread_attach;

mono_get_root_domain_t mono_get_root_domain;
mono_domain_get_t mono_domain_get;

mono_security_set_mode_t mono_security_set_mode;

mono_class_get_property_from_name_t mono_class_get_property_from_name;
mono_property_get_set_method_t mono_property_get_set_method;
mono_property_get_get_method_t mono_property_get_get_method;

bool InitializeMono()
{
	hMono = GetModuleHandle("mono.dll");
	return hMono != NULL;
}

DWORD GetMonoFunction(const char* funcname)
{
	return (DWORD)GetProcAddress(hMono, funcname);
}

void InitializeMonoFunctions()
{
	mono_class_from_name = (mono_class_from_name_t)GetMonoFunction("mono_class_from_name");
	mono_class_get_method_from_name = (mono_class_get_method_from_name_t)GetMonoFunction("mono_class_get_method_from_name");

	mono_assembly_open = (mono_assembly_open_t)GetMonoFunction("mono_assembly_open");
	mono_assembly_load_from_full = (mono_assembly_load_from_full_t)GetMonoFunction("mono_assembly_load_from_full");
	mono_assembly_get_image = (mono_assembly_get_image_t)GetMonoFunction("mono_assembly_get_image");
	mono_assembly_load_from = (mono_assembly_load_from_t)GetMonoFunction("mono_assembly_load_from");

	mono_image_open_from_data = (mono_image_open_from_data_t)GetMonoFunction("mono_image_open_from_data");
	mono_image_open_from_data_full = (mono_image_open_from_data_full_t)GetMonoFunction("mono_image_open_from_data_full");
	mono_image_open = (mono_image_open_t)GetMonoFunction("mono_image_open");

	mono_runtime_invoke = (mono_runtime_invoke_t)GetMonoFunction("mono_runtime_invoke");

	mono_thread_attach = (mono_thread_attach_t)GetMonoFunction("mono_thread_attach");

	mono_get_root_domain = (mono_get_root_domain_t)GetMonoFunction("mono_get_root_domain");
	mono_domain_get = (mono_domain_get_t)GetMonoFunction("mono_domain_get");

	mono_security_set_mode = (mono_security_set_mode_t)GetMonoFunction("mono_security_set_mode");

	mono_class_get_property_from_name = (mono_class_get_property_from_name_t)GetMonoFunction("mono_class_get_property_from_name");
	mono_property_get_set_method = (mono_property_get_set_method_t)GetMonoFunction("mono_property_get_set_method");
	mono_property_get_get_method = (mono_property_get_get_method_t)GetMonoFunction("mono_property_get_get_method");
}