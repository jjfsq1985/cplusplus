
// Module:  Log4CPLUS
// File:    loglevel.h

#ifndef LOG4CPLUS_LOGLEVEL_HEADER_
#define LOG4CPLUS_LOGLEVEL_HEADER_


#include "log4cplus/platform.h"

#include <vector>


namespace log4cplus {


enum LogLevel 
{
	OFF_LOG_LEVEL     = 60000,
	FATAL_LOG_LEVEL   = 50000,
	ERROR_LOG_LEVEL   = 40000,
	INFO_LOG_LEVEL    = 20000,
	DEBUG_LOG_LEVEL   = 10000,
	ALL_LOG_LEVEL	= DEBUG_LOG_LEVEL,
	NOT_SET_LOG_LEVEL = -1,
};


/**
* This class is used to "manage" LogLevel definitions.  This class is also
* how "derived" LogLevels are created. Here are the steps to creating a "derived" LogLevel:
*/
class LOG4CPLUS_EXPORT LogLevelManager 
{
public:
	LogLevelManager();
	~LogLevelManager();

	/**
	* This method is called by all Layout classes to convert a LogLevel
	* into a string.
	* 
	* Note: It traverses the list of <code>LogLevelToStringMethod</code>
	*       to do this, so all "derived" LogLevels are recognized as well.
	*/
	std::string const& toString(LogLevel ll) const;

	/**
	* This method is called by all classes internally to log4cplus to
	* convert a string into a LogLevel.
	* 
	* Note: It traverses the list of <code>StringToLogLevelMethod</code>
	*       to do this, so all "derived" LogLevels are recognized as well.
	*/
	LogLevel fromString(const std::string& s) const;

private:
		
	// Disable Copy
	LogLevelManager(const LogLevelManager&);
	LogLevelManager& operator= (const LogLevelManager&);
};

/**
* Returns the singleton LogLevelManager.
*/
LOG4CPLUS_EXPORT LogLevelManager& getLogLevelManager();

}


#endif // LOG4CPLUS_LOGLEVEL_HEADER_

