// Module:  Log4CPLUS
// File:    filter.cpp


#include "log4cplus/filter.h"
#include "log4cplus/loglog.h"
#include "log4cplus/stringhelper.h"
#include "log4cplus/property.h"
#include "log4cplus/loggingevent.h"

using namespace std;
using namespace log4cplus;

FilterResult log4cplus::checkFilter(const Filter* filter, const InternalLoggingEvent& loggingEvent)
{
	const Filter* currentFilter = filter;
	while(currentFilter) 
	{
		FilterResult result = currentFilter->decide(loggingEvent);
		if(result != NEUTRAL) 
		{
			return result;
		}

		currentFilter = currentFilter->_nextFilter.get();
	}

	return ACCEPT;
}


Filter::Filter() {}

Filter::~Filter() {}

void Filter::appendFilter(FilterPtr filter)
{
	if(!_nextFilter)
		_nextFilter = filter;
	else
		_nextFilter->appendFilter(filter);
}


DenyAllFilter::DenyAllFilter() {}


DenyAllFilter::DenyAllFilter(const Properties&) {}


FilterResult DenyAllFilter::decide(const InternalLoggingEvent&) const
{
	return DENY;
}



LogLevelMatchFilter::LogLevelMatchFilter()
{
	init();
}

LogLevelMatchFilter::LogLevelMatchFilter(const Properties& properties)
{
	init();

	string const& log_level_to_match = properties.getProperty("LogLevelToMatch");
	_logLevelToMatch = getLogLevelManager().fromString(log_level_to_match);
}


void LogLevelMatchFilter::init()
{
	_logLevelToMatch = NOT_SET_LOG_LEVEL;
}


FilterResult LogLevelMatchFilter::decide(const InternalLoggingEvent& loggingEvent) const
{
	if(_logLevelToMatch == NOT_SET_LOG_LEVEL) 
	{
		return NEUTRAL;
	}

	bool matchOccured = (_logLevelToMatch == loggingEvent.getLogLevel());

	if(matchOccured)
	{
		return ACCEPT;
	}
	else
	{
		return NEUTRAL;
	}
}


LogLevelRangeFilter::LogLevelRangeFilter()
{
	init();
}


LogLevelRangeFilter::LogLevelRangeFilter(const Properties& properties)
{
	init();

	string const& log_level_min = properties.getProperty("LogLevelMin");
	_logLevelMin = getLogLevelManager().fromString(log_level_min);

	string const& log_level_max = properties.getProperty("LogLevelMax");
	_logLevelMax = getLogLevelManager().fromString(log_level_max);
}


void LogLevelRangeFilter::init()
{
	_logLevelMin = NOT_SET_LOG_LEVEL;
	_logLevelMax = NOT_SET_LOG_LEVEL;
}


FilterResult LogLevelRangeFilter::decide(const InternalLoggingEvent& loggingEvent) const
{
	if((_logLevelMin != NOT_SET_LOG_LEVEL) &&(loggingEvent.getLogLevel() < _logLevelMin)) 
	{
		// priority of loggingEvent is less than minimum
		return DENY;
	}

	if((_logLevelMax != NOT_SET_LOG_LEVEL) &&(loggingEvent.getLogLevel() > _logLevelMax)) 
	{
		// priority of loggingEvent is greater than maximum
		return DENY;
	}

	return ACCEPT;
}

