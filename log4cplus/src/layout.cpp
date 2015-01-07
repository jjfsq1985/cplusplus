// Module:  Log4CPLUS
// File:    layout.cpp


#include "log4cplus/layout.h"
#include "log4cplus/stringhelper.h"
#include "log4cplus/timehelper.h"
#include "log4cplus/loggingevent.h"
#include "log4cplus/property.h"
#include <ostream>
#include <iomanip>

using namespace std;
using namespace log4cplus;

void formatRelativeTimestamp (ostream & output, InternalLoggingEvent const& loggingEvent)
{
	TimeHelper const rel_time = loggingEvent.getTimestamp () - getLayoutTimeBase ();
	char const old_fill = output.fill ();
	time_t const sec = rel_time.sec ();

	if (sec != 0)
		output << sec << std::setfill ('0') << std::setw (3);

	output << rel_time.usec () / 1000;
	output.fill (old_fill);
}


Layout::Layout () : _llmCache(getLogLevelManager()) {}

Layout::Layout (const Properties&) : _llmCache(getLogLevelManager()) {}

Layout::~Layout() {}


///////////////////////////////////////////////////////////////////////////////
// SimpleLayout 
///////////////////////////////////////////////////////////////////////////////

SimpleLayout::SimpleLayout () {}

SimpleLayout::SimpleLayout (const Properties& properties) : Layout (properties) {}

SimpleLayout::~SimpleLayout() {}

void SimpleLayout::formatAndAppend(ostream& output, const InternalLoggingEvent& loggingEvent)
{
	output.clear();
	formatRelativeTimestamp (output, loggingEvent);

	output << " - "
		<< _llmCache.toString(loggingEvent.getLogLevel()) 
		<< " - "
		<< loggingEvent.getMessage() 
		<< "\n"
		<< std::ends;
}

