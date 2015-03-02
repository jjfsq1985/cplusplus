
// Module:  Log4CPLUS
// File:    rootlogger.h

#ifndef LOG4CPLUS_SPI_ROOT_LOGGER_HEADER_
#define LOG4CPLUS_SPI_ROOT_LOGGER_HEADER_


#include "log4cplus/platform.h"
#include "log4cplus/loggerimpl.h"


namespace log4cplus {


/**
* RootLogger sits at the top of the logger hierachy. It is a
* regular logger except that it provides several guarantees.
*
* First, it cannot be assigned a <code>NOT_SET_LOG_LEVEL</code>
* LogLevel. Second, since root logger cannot have a parent, the
* getChainedLogLevel method always returns the value of the
* ll field without walking the hierarchy.
*/
class LOG4CPLUS_EXPORT RootLogger : public LoggerImpl 
{
public:
		
	/**
	* The root logger names itself as "root". However, the root
	* logger cannot be retrieved by name.  
	*/
	RootLogger(Hierarchy& h, LogLevel ll);

		
	/**
	* Return the assigned LogLevel value without walking the logger hierarchy.
	*/
	virtual LogLevel getAssignedLogLevel() const;

	/**
	* Setting a NOT_SET_LOG_LEVEL value to the LogLevel of the root logger 
	* may have catastrophic results. We prevent this here.
	*/
	void setLogLevel(LogLevel ll);

};


} // namespace log4cplus

#endif // LOG4CPLUS_SPI_ROOT_LOGGER_HEADER_

