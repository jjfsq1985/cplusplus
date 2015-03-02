
// Module:  Log4CPLUS
// File:    loggerfactory.h

#ifndef LOG4CPLUS_SPI_LOGGER_FACTORY_HEADER
#define LOG4CPLUS_SPI_LOGGER_FACTORY_HEADER

#include "log4cplus/platform.h"

namespace log4cplus {


class Logger;
class Hierarchy;


/**
* Implement this interface to create new instances of Logger or a sub-class of Logger.
*/
class LOG4CPLUS_EXPORT LoggerFactory 
{
public:
	/**
	* Creates a new <code>Logger</code> object.
	*/
	virtual Logger makeNewLoggerInstance(const std::string& name, Hierarchy& h) = 0; 
	virtual ~LoggerFactory() = 0;
};

} // namespace log4cplus

#endif // LOG4CPLUS_SPI_LOGGER_FACTORY_HEADER

