
// Module:  Log4CPLUS
// File:    Layout.h

#ifndef LOG4CPLUS_LAYOUT_HEADER_
#define LOG4CPLUS_LAYOUT_HEADER_


#include "log4cplus/platform.h"
#include "log4cplus/loglevel.h"
#include "log4cplus/property.h"

#include <vector>


namespace log4cplus {
	

class PatternConverter;
class TimeHelper;
class InternalLoggingEvent;

/**
* This class is used to layout std::strings sent to an {@link Appender}.
*/
class LOG4CPLUS_EXPORT Layout
{
public:
	Layout();
	Layout(const Properties& properties);
	virtual ~Layout() = 0;

	virtual void formatAndAppend(std::ostream& output, const InternalLoggingEvent& loggingEvent) = 0;

protected:
	LogLevelManager& _llmCache;

private:
	// Disable copy
	Layout(const Layout&);
	Layout& operator= (Layout const&);
};


/**
* SimpleLayout consists of the LogLevel of the log statement,
* followed by " - " and then the log message itself. For example,
*/
class LOG4CPLUS_EXPORT SimpleLayout : public Layout
{
public:
	SimpleLayout();
	SimpleLayout(const Properties& properties);
	virtual ~SimpleLayout();

	virtual void formatAndAppend(std::ostream& output, const InternalLoggingEvent& loggingEvent);

private: 
	// Disallow copying of instances of this class
	SimpleLayout(const SimpleLayout&);
	SimpleLayout& operator= (const SimpleLayout&);
};


LOG4CPLUS_EXPORT TimeHelper const& getLayoutTimeBase();


/**
* A flexible layout configurable with pattern std::string.
*/
class LOG4CPLUS_EXPORT PatternLayout : public Layout
{
public:
		 
	PatternLayout(const std::string& pattern);
	PatternLayout(const Properties& properties);
	virtual ~PatternLayout();

	virtual void formatAndAppend(std::ostream& output, const InternalLoggingEvent& loggingEvent);

protected:
	void init(const std::string& pattern);
	
	std::string _pattern;
	std::vector<PatternConverter*> _parsedPattern;

private: 
	// Disallow copying of instances of this class
	PatternLayout(const PatternLayout&);
	PatternLayout& operator = (const PatternLayout&);
};


} // namespace log4cplus

#endif // LOG4CPLUS_LAYOUT_HEADER_

