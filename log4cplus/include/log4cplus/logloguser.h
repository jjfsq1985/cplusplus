
// Module:  Log4CPLUS
// File:    logloguser.h

#ifndef LOG4CPLUS_HELPERS_LOGLOG_USER
#define LOG4CPLUS_HELPERS_LOGLOG_USER


#include "log4cplus/platform.h"


namespace log4cplus { 
	

class LogLog;


/**
* This class used to simplify the use of the LogLog class.  Any class
* that uses the LogLog class should extend this class and retrieve
* their reference to LogLog using the method provided.
*/
class LOG4CPLUS_EXPORT LogLogUser
{
public:
		 
	LogLogUser();
	LogLogUser(const LogLogUser&);
	virtual ~LogLogUser();

	// public methods
	LogLog& getLogLog() const;

	// operators
	LogLogUser& operator= (const LogLogUser& rhs);
};


} // namespace log4cplus 


#endif // LOG4CPLUS_HELPERS_LOGLOG_USER

