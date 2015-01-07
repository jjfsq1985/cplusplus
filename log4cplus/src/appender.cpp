// Module:  Log4CPLUS
// File:    appender.cpp

#include "log4cplus/appender.h"
#include "log4cplus/layout.h"
#include "log4cplus/loglog.h"
#include "log4cplus/sharedptr.h"
#include "log4cplus/stringhelper.h"
#include "log4cplus/property.h"
#include "log4cplus/factory.h"
#include "log4cplus/loggingevent.h"

#include <stdexcept>

using namespace std;
using namespace log4cplus;

ErrorHandler::ErrorHandler() {}

ErrorHandler::~ErrorHandler() {}

///////////////////////////////////////////////////////////////////////////////
// log4cplus::OnlyOnceErrorHandler 
///////////////////////////////////////////////////////////////////////////////

OnlyOnceErrorHandler::OnlyOnceErrorHandler() : firstTime(true) {}

OnlyOnceErrorHandler::~OnlyOnceErrorHandler() {}

void OnlyOnceErrorHandler::error(const string& err)
{
	if(firstTime) 
	{
		LogLog::getLogLog()->error(err);
		firstTime = false;
	}
}

void OnlyOnceErrorHandler::reset()
{
	firstTime = true;
}


Appender::Appender()
	: _layout(new SimpleLayout()),
	_name(""),
	_threshold(NOT_SET_LOG_LEVEL),
	_errorHandler(new OnlyOnceErrorHandler),
	_isClosed(false)
{
}

Appender::Appender(const log4cplus::Properties & properties)
	: _layout(new SimpleLayout())
	, _name()
	, _threshold(NOT_SET_LOG_LEVEL)
	, _errorHandler(new OnlyOnceErrorHandler)
	, _isClosed(false)
{
	if(properties.exists("layout"))
	{
		string const& factoryName = properties.getProperty("layout");
		LayoutFactory* factory = getLayoutFactoryRegistry().get(factoryName);
		if(factory == 0) 
		{
			LogLog::getLogLog()->error("Cannot find LayoutFactory: " + factoryName );
			return;
		}

		Properties layoutProperties = properties.getPropertySubset("layout.");
		try 
		{
			std::auto_ptr<Layout> newLayout(factory->createObject(layoutProperties));
			if(newLayout.get() == 0)
			{
				LogLog::getLogLog()->error("Failed to create appender: " + factoryName);
			}
			else 
			{
				_layout = newLayout;
			}
		}
		catch(std::exception const& e) 
		{
			LogLog::getLogLog()->error("Error while creating Layout: " + string(e.what()));
			return;
		}

	}

	// Support for appender.Threshold in properties configuration file
	if(properties.exists("Threshold")) 
	{
		string tmp = properties.getProperty("Threshold");
		tmp = log4cplus::toUpper(tmp);
		_threshold = getLogLevelManager().fromString(tmp);
	}

	// Configure the filters
	Properties filterProps = properties.getPropertySubset("filters.");
	unsigned filterCount = 0;
	FilterPtr filterChain;
	string filterName;
	while(filterProps.exists(filterName = convertIntegerToString(++filterCount)))
	{
		string const& factoryName = filterProps.getProperty(filterName);
		FilterFactory* factory = getFilterFactoryRegistry().get(factoryName);

		if(!factory)
		{
			string err = "Appender::ctor()- Cannot find FilterFactory: ";
			LogLog::getLogLog()->error(err + factoryName);
			continue;
		}
		FilterPtr tmpFilter = factory->createObject(filterProps.getPropertySubset(filterName + "."));
		if(!tmpFilter)
		{
			string err = "Appender::ctor()- Failed to create filter: ";
			LogLog::getLogLog()->error(err + filterName);
		}
		if(!filterChain)
			filterChain = tmpFilter;
		else
			filterChain->appendFilter(tmpFilter);
	}
	setFilter(filterChain);
}

Appender::~Appender()
{
	if(!_isClosed)
		LogLog::getLogLog()->error("Derived Appender did not call destructorImpl().");
}


void Appender::destructorImpl()
{
	// An appender might be closed then destroyed. There is no point
	// in closing twice. It can actually be a wrong thing to do, e.g.,
	// files get rolled more than once.
	if(_isClosed)
		return;

	close();
	_isClosed = true;
}

bool Appender::isClosed() const
{
	return _isClosed;
}


void Appender::doAppend(const log4cplus::InternalLoggingEvent& loggingEvent)
{
	MutexLock lock(&_mutex);

	if(_isClosed) 
	{
		LogLog::getLogLog()->error("Attempted to append to closed appender named [" + _name + "].");
		return;
	}

	// Check appender's threshold logging level.

	if(!isAsSevereAsThreshold(loggingEvent.getLogLevel()))
		return;

	// Evaluate filters attached to this appender.

	if(checkFilter(_filter.get(), loggingEvent) == DENY)
		return;
	// Finally append given loggingEvent.

	append(loggingEvent);
}


string Appender::getName()
{
	return _name;
}

void Appender::setName(const string& n)
{
	this->_name = n;
}

ErrorHandler* Appender::getErrorHandler()
{
	return _errorHandler.get();
}

void Appender::setErrorHandler(std::auto_ptr<ErrorHandler> eh)
{
	if(!eh.get())
	{
		// We do not throw exception here since the cause is probably a
		// bad config file.
		LogLog::getLogLog()->error("You have tried to set a null error-handler.");
		return;
	}

	MutexLock lock(&_mutex);

	this->_errorHandler = eh;
}

void Appender::setLayout(std::auto_ptr<Layout> lo)
{
	MutexLock lock(&_mutex);

	this->_layout = lo;
}

Layout* Appender::getLayout()
{
	return _layout.get();
}

