// Module:  Log4CPLUS
// File:    loggingevent.cpp


#include "log4cplus/loggingevent.h"
#include <algorithm>

using namespace std;
using namespace log4cplus;


static const int LOG4CPLUS_DEFAULT_TYPE = 1;


InternalLoggingEvent::InternalLoggingEvent(const string& logger,
	LogLevel loglevel, const string& message_)
	: _message(message_)
	, _loggerName(logger)
	, _ll(loglevel)
	, _timestamp(TimeHelper::gettimeofday())
{
}

InternalLoggingEvent::InternalLoggingEvent(const string& logger,
	LogLevel loglevel, const string& message_, TimeHelper time)
	: _message(message_), _loggerName(logger)
	, _ll(loglevel), _timestamp(time)
{
}

InternalLoggingEvent::InternalLoggingEvent()
	: _ll(NOT_SET_LOG_LEVEL)
{}

InternalLoggingEvent::InternalLoggingEvent(const InternalLoggingEvent& rhs)
	: _message(rhs.getMessage())
	, _loggerName(rhs.getLoggerName())
	, _ll(rhs.getLogLevel())
	, _timestamp(rhs.getTimestamp())
{
}

InternalLoggingEvent::~InternalLoggingEvent()
{
}

///////////////////////////////////////////////////////////////////////////////
// InternalLoggingEvent static methods
///////////////////////////////////////////////////////////////////////////////

unsigned int InternalLoggingEvent::getDefaultType()
{
	return LOG4CPLUS_DEFAULT_TYPE;
}


void InternalLoggingEvent::setLoggingEvent(const string& logger, LogLevel loglevel, const string& msg)
{
	// This could be imlemented using the swap idiom:
	//
	// InternalLoggingEvent(logger, loglevel, msg, filename, fline).swap(*this);
	//
	// But that defeats the optimization of using thread local instance
	// of InternalLoggingEvent to avoid memory allocation.

	_loggerName = logger;
	_ll = loglevel;
	_message = msg;
	_timestamp = TimeHelper::gettimeofday();
}

const string& InternalLoggingEvent::getMessage() const
{
	return _message;
}


unsigned int InternalLoggingEvent::getType() const
{
	return LOG4CPLUS_DEFAULT_TYPE;
}

std::auto_ptr<InternalLoggingEvent> InternalLoggingEvent::clone() const
{
	std::auto_ptr<InternalLoggingEvent> tmp(new InternalLoggingEvent(*this));
	return tmp;
}

InternalLoggingEvent & InternalLoggingEvent::operator = (const InternalLoggingEvent& rhs)
{
	InternalLoggingEvent(rhs).swap(*this);
	return *this;
}

void InternalLoggingEvent::swap(InternalLoggingEvent & other)
{
	using std::swap;

	swap(_message, other._message);
	swap(_loggerName, other._loggerName);
	swap(_ll, other._ll);
	swap(_timestamp, other._timestamp);
}

