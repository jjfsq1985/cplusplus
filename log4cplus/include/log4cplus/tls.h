

#ifndef LOG4CPLUS_THREAD_IMPL_TLS_H
#define LOG4CPLUS_THREAD_IMPL_TLS_H

#include "log4cplus/platform.h"

#include <new>
#include <cassert>

#ifdef __linux__
#include <pthread.h>
#endif


namespace log4cplus { 


	typedef void * TLSValueType;
	typedef void (* TLSInitCleanupFunc)(void *);

#ifdef _MSC_VER
	typedef DWORD TLSKeyType;
#else	//__linux__
	typedef pthread_key_t * TLSKeyType;
#endif

	inline TLSKeyType TLSInit(TLSInitCleanupFunc);
	inline TLSValueType TLSGetValue(TLSKeyType);
	inline void TLSSetValue(TLSKeyType, TLSValueType);
	inline void TLSCleanup(TLSKeyType);

#ifdef _MSC_VER

	TLSKeyType TLSInit(TLSInitCleanupFunc)
	{
		return TlsAlloc();
	}

	TLSValueType TLSGetValue(TLSKeyType key)
	{
		return TlsGetValue(key);
	}

	void TLSSetValue(TLSKeyType key, TLSValueType val)
	{
		TlsSetValue(key, val);
	}

	void TLSCleanup(TLSKeyType key)
	{
		TlsFree(key);
	}
#else	//__linux__

	TLSKeyType TLSInit(TLSInitCleanupFunc cleanupfunc)
	{
		pthread_key_t * key = new pthread_key_t;
		pthread_key_create(key, cleanupfunc);
		return key;
	}

	TLSValueType TLSGetValue(TLSKeyType key)
	{
		return pthread_getspecific(*key);
	}

	void TLSSetValue(TLSKeyType key, TLSValueType value)
	{
		pthread_setspecific(*key, value);
	}

	void TLSCleanup(TLSKeyType key)
	{
		pthread_key_delete(*key);
		delete key;
	}

#endif

}  // namespace log4cplus 

#endif // LOG4CPLUS_THREAD_IMPL_TLS_H
