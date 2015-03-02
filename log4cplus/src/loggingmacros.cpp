// Module:  Log4CPLUS
// File:    loggingmacros.cpp


#include "log4cplus/loggingevent.h"
#include "log4cplus/loggingmacros.h"

using namespace std;
using namespace log4cplus;


void log4cplus::macro_forcedLog(Logger const& logger, LogLevel logLevel, string const& msg)
{
	InternalLoggingEvent& loggingEvent = *getInternalLoggingEvent();
	loggingEvent.setLoggingEvent(logger.getName(), logLevel, msg);
	logger.forcedLog(loggingEvent);
}

