
#include <stdlib.h>      // for abort()

#include "log4cplus/mutex.h"

using namespace log4cplus;

#ifdef _MSC_VER

Mutex::Mutex()             { InitializeCriticalSection(&_mutex); }
Mutex::~Mutex()            { DeleteCriticalSection(&_mutex); }
void Mutex::Lock()         { EnterCriticalSection(&_mutex); }
void Mutex::Unlock()       { LeaveCriticalSection(&_mutex); }

#else

#define SAFE_PTHREAD(fncall)  do {    \
	if (fncall(&_mutex) != 0) abort();  \
} while (0)

Mutex::Mutex()             
{
	if (pthread_mutex_init(&_mutex, NULL) != 0) abort();
}
Mutex::~Mutex()            { SAFE_PTHREAD(pthread_mutex_destroy); }
void Mutex::Lock()         { SAFE_PTHREAD(pthread_mutex_lock); }
void Mutex::Unlock()       { SAFE_PTHREAD(pthread_mutex_unlock); }
#undef SAFE_PTHREAD

#endif