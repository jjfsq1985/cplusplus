// Module:  Log4CPLUS
// File:    loglevel.cpp


#include "log4cplus/loglevel.h"
#include "log4cplus/loglog.h"
#include "log4cplus/stringhelper.h"
#include "log4cplus/loggingevent.h"
#include <algorithm>

using namespace std;
using namespace log4cplus;

static string const ALL_STRING ("ALL");
static string const DEBUG_STRING ("DEBUG");
static string const INFO_STRING ("INFO");
static string const ERROR_STRING ("ERROR");
static string const FATAL_STRING ("FATAL");
static string const OFF_STRING ("OFF");
static string const NOTSET_STRING ("NOTSET");
static string const UNKNOWN_STRING ("UNKNOWN");


LogLevelManager::LogLevelManager() {}


LogLevelManager::~LogLevelManager() {}


string const& LogLevelManager::toString(LogLevel ll) const
{
	switch(ll) 
	{
	case OFF_LOG_LEVEL:     return OFF_STRING;
	case FATAL_LOG_LEVEL:   return FATAL_STRING;
	case ERROR_LOG_LEVEL:   return ERROR_STRING;
	case INFO_LOG_LEVEL:    return INFO_STRING;
	case DEBUG_LOG_LEVEL:   return DEBUG_STRING;
	case NOT_SET_LOG_LEVEL: return NOTSET_STRING;
	};

	return UNKNOWN_STRING;
}


LogLevel LogLevelManager::fromString(const string& llstring) const
{
	if (llstring == OFF_STRING)
		return OFF_LOG_LEVEL;
	else if (llstring == FATAL_STRING)
		return FATAL_LOG_LEVEL;
	else if (llstring == ERROR_STRING)
		return ERROR_LOG_LEVEL;
	else if (llstring == INFO_STRING)
		return INFO_LOG_LEVEL;
	else if (llstring == DEBUG_STRING)
		return DEBUG_LOG_LEVEL;
	else 
		return NOT_SET_LOG_LEVEL;

}

