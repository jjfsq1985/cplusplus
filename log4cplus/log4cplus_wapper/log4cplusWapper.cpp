
#include "log4cplusWapper.h"
#include "log4cplus/configurator.h"
#include "log4cplus/loggingmacros.h"
#include "log4cplus/customappender.h"

#include <string>
#include <map>
#include <algorithm>
#include <sys/stat.h>
#include <cstring>
#include <stdexcept>
#include <stdarg.h>
#include <cstdio>
#ifdef _WIN32
#include <tchar.h>
#endif


using namespace std;
using namespace log4cplus;
using namespace log4cplusWapper;

#pragma warning(disable : 4127)


#ifdef _WIN32
#define LOGGER_MACO(loggerName, fmt, MACO)			\
do{													\
	if (NULL == fmt || NULL == loggerName || 0 == *loggerName) break;		\
	va_list args = NULL;							\
	va_start(args, fmt);							\
	size_t len = _vscprintf(fmt, args) + 1;			\
	std::vector<char> buffer(len, '\0');			\
	int nWritten = _vsnprintf_s(&buffer[0], buffer.size(), len, fmt, args);	\
	if (nWritten > 0)	MACO(getLogger(loggerName), &buffer[0]);			\
	va_end(args);									\
}while(0)											

#else

#define LOGGER_MACO(loggerName, fmt, MACO)			\
do{												    \
	if (NULL == fmt || NULL == loggerName || 0 == *loggerName) break; \
	va_list args;							\
	va_start(args, fmt);							\
	std::vector<char> buffer;						\
	std::size_t const fmt_len = std::char_traits<char>::length(fmt);\
	std::size_t const output_estimate = fmt_len + fmt_len / 2 + 1;	\
	buffer.resize(output_estimate);					\
	int nWritten = std::vsprintf(&buffer[0], fmt, args);\
	buffer[nWritten] = 0;							\
	if (nWritten > 0)	MACO(getLogger(loggerName), &buffer[0]);	\
	va_end(args);									\
}while(0)										
#endif

typedef map<string, Logger> LoggerMap;
LoggerMap s_loggerMap;

#ifdef _WIN32
void __stdcall OutputDebugStringFunc(const char* sz)
{
	OutputDebugString((char*)sz);
}
#endif

static bool FileExists(string const& name)
{
#ifdef _WIN32
	struct _stat fileStatus;
	if(_tstat(name.c_str(), &fileStatus) == -1)
		return false;
#else
	struct stat fileStatus;
	if(stat(name.c_str(), &fileStatus) == -1)
		return false;
#endif

	return true;
}

static Logger getLogger(const char* szModuleName)
{
	assert (NULL != szModuleName);

	string moduleName(szModuleName);
	LoggerMap::const_iterator it = s_loggerMap.find(moduleName);
	if (it == s_loggerMap.end())
	{
		if(!tryInitLoggerInstance(szModuleName))
			return Logger::getRoot();
	}

	return s_loggerMap[moduleName];
}

void log4cplusWapper::StartLogSystem(const char* properties_filename)
{
    if (NULL == properties_filename || 0 == strlen(properties_filename))
	{
		throw std::invalid_argument("invalid log4cplus properties filename");
	}

	if (!FileExists(properties_filename))
	{
		throw std::invalid_argument(string(string(properties_filename) + " not exists").c_str());
	}

	log4cplus::PropertyConfigurator::doConfigure(properties_filename);

#ifdef _WIN32
	log4cplus::CustomAppender::setCustomFunc(OutputDebugStringFunc);
#endif
}


bool log4cplusWapper::tryInitLoggerInstance(const char* szModuleName)
{
	if (NULL == szModuleName || 0 == strlen(szModuleName))
	{
		throw std::invalid_argument("invalid log4cplus logger name");
	}

	string moduleName(szModuleName);
	LoggerMap::const_iterator it = s_loggerMap.find(moduleName);
	if (it == s_loggerMap.end())
	{
		vector<string> loggerName = PropertyConfigurator::getLoggerNames();
		vector<string>::iterator result = std::find(loggerName.begin(), loggerName.end(), moduleName);
		if (result != loggerName.end())
		{
			Logger logger = Logger::getInstance(moduleName);
			s_loggerMap[moduleName] = logger;
		}
		else
		{
			return false;
		}
	}

	return true;
}

void log4cplusWapper::StopLogSystem()
{
	
}

void log4cplusWapper::PrintDebug(const char* loggerName, const char* pszFormat, ...)
{
	LOGGER_MACO(loggerName, pszFormat, LOG4CPLUS_DEBUG);
}


void log4cplusWapper::PrintInfo(const char* loggerName, const char* pszFormat, ...)
{
	LOGGER_MACO(loggerName, pszFormat, LOG4CPLUS_INFO);
}


void log4cplusWapper::PrintError(const char* loggerName, const char* pszFormat, ...)
{
	LOGGER_MACO(loggerName, pszFormat, LOG4CPLUS_ERROR);
}


void log4cplusWapper::PrintFatal(const char* loggerName, const char* pszFormat, ...)
{
	LOGGER_MACO(loggerName, pszFormat, LOG4CPLUS_FATAL);
}

