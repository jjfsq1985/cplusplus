
// Module:  Log4CPLUS
// File:    nullappender.h

#ifndef LOG4CPLUS_NULL_APPENDER_HEADER_
#define LOG4CPLUS_NULL_APPENDER_HEADER_


#include "log4cplus/platform.h"
#include "log4cplus/appender.h"


namespace log4cplus {


/**
* Appends log events to a file. 
*/
class LOG4CPLUS_EXPORT NullAppender : public Appender
{
public:

	NullAppender();

	NullAppender(const log4cplus::Properties&);

	virtual ~NullAppender();

	
	virtual void close();

protected:
	virtual void append(const log4cplus::InternalLoggingEvent& loggingEvent);

private:
	// Disallow copying of instances of this class
	NullAppender(const NullAppender&);
	NullAppender& operator= (const NullAppender&);
};

} // namespace log4cplus

#endif // LOG4CPLUS_NULL_APPENDER_HEADER_

