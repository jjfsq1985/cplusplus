
#ifndef LOG4CPLUS_MUTEX_H_
#define LOG4CPLUS_MUTEX_H_

#include "log4cplus/platform.h"

#include <cassert>

namespace log4cplus
{


#ifdef _MSC_VER
	typedef CRITICAL_SECTION MutexType;
#else	
	typedef pthread_mutex_t MutexType;
#endif


class Mutex 
{
public:
	Mutex();
	~Mutex();

	void Lock();    
	void Unlock(); 

private:
	MutexType _mutex;

	Mutex(Mutex* /*ignored*/) {}
	Mutex(const Mutex&);
	void operator=(const Mutex&);

};	


// MutexLock(mu) acquires mu when constructed and releases it when destroyed.
class MutexLock 
{
public:
	explicit MutexLock(Mutex *mu) : m_pMutex(mu), _isUnlocked(false) 
	{
		assert(0 != m_pMutex);
		m_pMutex->Lock(); 
	}

	~MutexLock() { this->Unlock(); }

	void Unlock() 
	{
		if (!_isUnlocked)
		{
			m_pMutex->Unlock();
			_isUnlocked = true;
		}
	}

private:		
	Mutex* const m_pMutex;
	bool _isUnlocked;
	MutexLock(const MutexLock&);
	void operator=(const MutexLock&);
};


}  // namespace log4cplus

#endif  // #define LOG4CPLUS_MUTEX_H_ 
