// Module:  Log4CPLUS
// File:    rootlogger.cpp

#include "log4cplus/rootlogger.h"
#include "log4cplus/loglog.h"
#include "log4cplus/fileappender.h"


using namespace log4cplus;


RootLogger::RootLogger(Hierarchy& h, LogLevel loglevel) : LoggerImpl("root", h)
{
	setLogLevel(loglevel);
}


LogLevel RootLogger::getAssignedLogLevel() const
{
	return _ll;
}

void RootLogger::setLogLevel(LogLevel loglevel)
{
	if(loglevel == NOT_SET_LOG_LEVEL) 
	{
		LogLog::getLogLog()->error("You have tried to set NOT_SET_LOG_LEVEL to root.");
	}
	else 
	{
		LoggerImpl::setLogLevel(loglevel);
	}
}

