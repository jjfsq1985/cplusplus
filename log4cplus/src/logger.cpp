// Module:  Log4CPLUS
// File:    logger.cpp


#include "log4cplus/logger.h"
#include "log4cplus/appender.h"
#include "log4cplus/hierarchy.h"
#include "log4cplus/loglog.h"
#include "log4cplus/loggerimpl.h"

#include <utility>


using namespace std;
using namespace log4cplus;


Logger DefaultLoggerFactory::makeNewLoggerInstance(const string& name, Hierarchy& h)
{ 
	return Logger(new LoggerImpl(name, h));
}

//
Hierarchy& Logger::getDefaultHierarchy()
{
	return log4cplus::getDefaultHierarchy();
}


bool Logger::exists(const string& name) 
{
	return getDefaultHierarchy().exists(name); 
}


LoggerList Logger::getCurrentLoggers() 
{
	return getDefaultHierarchy().getCurrentLoggers();
}


Logger Logger::getInstance(const string& name) 
{ 
	return getDefaultHierarchy().getInstance(name); 
}

Logger Logger::getInstance(const string& name, LoggerFactory& factory)
{ 
	return getDefaultHierarchy().getInstance(name, factory); 
}

Logger Logger::getRoot() 
{ 
	return getDefaultHierarchy().getRoot(); 
}

Logger::Logger() : _pLoggerImpl(0)
{}


Logger::Logger(LoggerImpl * ptr) : _pLoggerImpl(ptr)
{
}


Logger::Logger(const Logger& rhs): AppenderAttachable(rhs) , _pLoggerImpl(rhs._pLoggerImpl)
{
}


Logger & Logger::operator = (const Logger& rhs)
{
	Logger(rhs).swap(*this);
	return *this;
}


Logger::~Logger() 
{
}


//////////////////////////////////////////////////////////////////////////////
// Logger Methods
//////////////////////////////////////////////////////////////////////////////

void Logger::swap(Logger & other)
{
	std::swap(_pLoggerImpl, other._pLoggerImpl);
}


Logger Logger::getParent() const 
{
	if(_pLoggerImpl->_parent)
	{
		return Logger(_pLoggerImpl->_parent.get());
	}
	else
	{
		LogLog::getLogLog()->error("********* This logger has no parent: " + getName());
		return *this;
	}
}


void Logger::addAppender(SharedAppenderPtr newAppender)
{
	_pLoggerImpl->addAppender(newAppender);
}


SharedAppenderPtrList Logger::getAllAppenders()
{
	return _pLoggerImpl->getAllAppenders();
}


SharedAppenderPtr Logger::getAppender(const string& name)
{
	return _pLoggerImpl->getAppender(name);
}


void Logger::removeAllAppenders()
{
	_pLoggerImpl->removeAllAppenders();
}


void Logger::removeAppender(SharedAppenderPtr appender)
{
	_pLoggerImpl->removeAppender(appender);
}


void Logger::removeAppender(const string& name)
{
	_pLoggerImpl->removeAppender(name);
}


void Logger::closeNestedAppenders() const
{
	_pLoggerImpl->closeNestedAppenders();
}


bool Logger::isEnabledFor(LogLevel ll) const
{
	return _pLoggerImpl->isEnabledFor(ll);
}


void Logger::log(InternalLoggingEvent const& ev) const
{
	_pLoggerImpl->log(ev);
}


void Logger::forcedLog(LogLevel ll, const string& message) const
{
	_pLoggerImpl->forcedLog(ll, message);
}


void Logger::forcedLog(InternalLoggingEvent const& ev) const
{
	_pLoggerImpl->forcedLog(ev);
}


void Logger::callAppenders(const InternalLoggingEvent& loggingEvent) const
{
	_pLoggerImpl->callAppenders(loggingEvent);
}


LogLevel Logger::getChainedLogLevel() const
{
	return _pLoggerImpl->getChainedLogLevel();
}


LogLevel Logger::getLogLevel() const
{
	return _pLoggerImpl->getLogLevel();
}


void Logger::setLogLevel(LogLevel ll)
{
	_pLoggerImpl->setLogLevel(ll);
}


Hierarchy& Logger::getHierarchy() const
{ 
	return _pLoggerImpl->getHierarchy();
}


string const& Logger::getName() const
{
	return _pLoggerImpl->getName();
}


