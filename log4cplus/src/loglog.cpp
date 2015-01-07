// Module:  Log4CPLUS
// File:    loglog.cpp



#include "log4cplus/loglog.h"
#include "log4cplus/environment.h"
#include "log4cplus/consoleappender.h"
#include <ostream>
#include <stdexcept>

using namespace std;
using namespace log4cplus;

static char const PREFIX[] = "log4cplus: ";
static char const ERR_PREFIX[] = "log4cplus:ERROR ";

LogLog* LogLog::s_pLogLog = NULL;


LogLog* LogLog::getLogLog()
{
	if (NULL == s_pLogLog)
		s_pLogLog = new LogLog();
	return s_pLogLog;
}


LogLog::LogLog() : _isDebugEnabled(false) {}

LogLog::~LogLog() {}

void LogLog::setInternalDebugging(bool enabled)
{
     MutexLock lock(&_mutex);

    _isDebugEnabled = enabled ;
}

void LogLog::debug(const string& msg) const
{
    loggingWorker(PREFIX, msg);
}


void LogLog::debug(char const* msg) const
{
    loggingWorker(PREFIX, msg);
}


void LogLog::error(const string& msg, bool throw_flag) const
{
    loggingWorker(ERR_PREFIX, msg, throw_flag);
}

void LogLog::error(char const* msg, bool throw_flag) const
{
    loggingWorker(ERR_PREFIX, msg, throw_flag);
}

void LogLog::loggingWorker(char const* prefix, string const& msg, bool throw_flag) const
{
    if(!_isDebugEnabled)
    {
#ifdef _MSC_VER
		OutputDebugString((prefix + msg + "\n").c_str());
#else	//__linux__
		cout << prefix << msg << endl;
#endif
	
    }

    if(throw_flag)
        throw std::runtime_error(msg);
}
 
