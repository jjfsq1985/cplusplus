// Module:  Log4CPLUS
// File:    loggerimpl.cpp


#include "log4cplus/loggerimpl.h"
#include "log4cplus/appender.h"
#include "log4cplus/hierarchy.h"
#include "log4cplus/loglog.h"
#include "log4cplus/loggingevent.h"
#include "log4cplus/rootlogger.h"

using namespace std;
using namespace log4cplus;	


LoggerImpl::LoggerImpl(const string& name_, Hierarchy& h)
	: _name(name_), _ll(NOT_SET_LOG_LEVEL), _parent(NULL), _hierarchy(h)
{
}

LoggerImpl::~LoggerImpl() 
{ 
}

void LoggerImpl::callAppenders(const InternalLoggingEvent& loggingEvent)
{
	int writes = 0;
	for(const LoggerImpl* c = this; c != NULL; c=c->_parent.get())
	{
		writes += c->appendLoopOnAppenders(loggingEvent);
	}

	// No appenders in hierarchy, warn user only once.
	if(!_hierarchy._isEmittedNoAppenderWarning && writes == 0) 
	{
		LogLog::getLogLog()->error("No appenders could be found for logger(" + getName() + ").");
		LogLog::getLogLog()->error("Please initialize the log4cplus system properly.");
		_hierarchy._isEmittedNoAppenderWarning = true;
	}
}


void LoggerImpl::closeNestedAppenders()
{
	SharedAppenderPtrList appenders = getAllAppenders();
	for(SharedAppenderPtrList::iterator it=appenders.begin(); it!=appenders.end(); ++it)
	{
		Appender & appender = **it;
		if(!appender.isClosed())
			appender.close();
	}
}


bool LoggerImpl::isEnabledFor(LogLevel loglevel) const
{
	if(_hierarchy._nDisableValue >= loglevel) 
	{
		return false;
	}
	return loglevel >= getChainedLogLevel();
}


void LoggerImpl::log(LogLevel loglevel, const string& message)
{
	if(isEnabledFor(loglevel))
	{
		forcedLog(loglevel, message);
	}
}

void LoggerImpl::log(InternalLoggingEvent const& ev)
{
	if(isEnabledFor(ev.getLogLevel()))
		forcedLog(ev);
}


LogLevel LoggerImpl::getChainedLogLevel() const
{
	for(const LoggerImpl *c=this; c != NULL; c=c->_parent.get()) 
	{
		if(c->_ll != NOT_SET_LOG_LEVEL) 
		{
			return c->_ll;
		}
	}

	LogLog::getLogLog()->error("LoggerImpl::getChainedLogLevel()- No valid LogLevel found", true);
	return NOT_SET_LOG_LEVEL;
}


Hierarchy& LoggerImpl::getHierarchy() const
{ 
	return _hierarchy; 
}


void LoggerImpl::forcedLog(LogLevel loglevel, const string& message)
{
	InternalLoggingEvent& loggingEvent = *getInternalLoggingEvent();
	loggingEvent.setLoggingEvent(this->getName(), loglevel, message);
	callAppenders(loggingEvent);
}


void LoggerImpl::forcedLog(InternalLoggingEvent const& ev)
{
	callAppenders(ev);
}

