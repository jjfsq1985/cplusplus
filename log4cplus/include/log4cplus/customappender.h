// Module:  Log4CPLUS
// File:    customappender.h

#ifndef _LOG4CPLUS_OSP_APPENDER_HEADER_
#define _LOG4CPLUS_OSP_APPENDER_HEADER_

#include "log4cplus/platform.h"
#include "log4cplus/appender.h"

#ifdef WIN32
typedef void (__stdcall *pCustomFuncCallBack)(const char*);
#else
typedef void (*pCustomFuncCallBack)(const char*);
#endif	


namespace log4cplus {


class LOG4CPLUS_EXPORT CustomAppender : public Appender
{
public:
	CustomAppender();

	CustomAppender(const Properties& properties);

	virtual ~CustomAppender();

	virtual void close();

	static void setCustomFunc(pCustomFuncCallBack pCustomFunc);	

protected:
	virtual void append(const InternalLoggingEvent& loggingEvent);

private:
	CustomAppender(const CustomAppender&);

	CustomAppender& operator=(const CustomAppender&);

	static pCustomFuncCallBack _pCustomFunc;
};


} // namespace log4cplus

#endif // _LOG4CPLUS_OSP_APPENDER_HEADER_

