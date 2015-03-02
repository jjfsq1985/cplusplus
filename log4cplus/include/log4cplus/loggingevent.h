
// Module:  Log4CPLUS
// File:    loggingevent.h

#ifndef LOG4CPLUS_SPI_INTERNAL_LOGGING_EVENT_HEADER_
#define LOG4CPLUS_SPI_INTERNAL_LOGGING_EVENT_HEADER_

#include "log4cplus/platform.h"
#include "log4cplus/loglevel.h"
#include "log4cplus/timehelper.h"
#include "log4cplus/tls.h"

#include <memory>

namespace log4cplus { 


class TimeHelper;


/**
* The internal representation of logging events. When an affirmative
* decision is made to log then a <code>InternalLoggingEvent</code>
* instance is created. This instance is passed around to the
* different log4cplus components.
*
* This class is of concern to those wishing to extend log4cplus.
*/
class LOG4CPLUS_EXPORT InternalLoggingEvent 
{
public:
	
	/**
	* Instantiate a LoggingEvent from the supplied parameters.
	*
	* @param logger   The logger of this loggingEvent.
	* @param loglevel The LogLevel of this loggingEvent.TimeHelper
	* @param message  The message of this loggingEvent.
	*/
	InternalLoggingEvent(const std::string& logger, LogLevel loglevel, const std::string& message);

	InternalLoggingEvent(const std::string& logger, LogLevel loglevel,
		const std::string& message, TimeHelper time);

	InternalLoggingEvent();

	InternalLoggingEvent(const InternalLoggingEvent& rhs);

	virtual ~InternalLoggingEvent();

	void setLoggingEvent(const std::string& logger, LogLevel ll, const std::string& message);

	// public virtual methods
	/** The application supplied message of logging loggingEvent. */
	virtual const std::string& getMessage() const;

	/** Returns the 'type' of InternalLoggingEvent.  Derived classes
	*  should override this method. (NOTE: Values <= 1000 are
	*  reserved for log4cplus and should not be used.)
	*/
	virtual unsigned int getType() const;

	/** Returns a copy of this object.  Derived classes
	*  should override this method.
	*/
	virtual std::auto_ptr<InternalLoggingEvent> clone() const;



	// public methods
	/** The logger of the logging loggingEvent. It is set by
	*  the LoggingEvent constructor.
	*/
	const std::string& getLoggerName() const
	{
		return _loggerName;
	}

	/** LogLevel of logging loggingEvent. */
	LogLevel getLogLevel() const
	{
		return _ll;
	}

	/** The number of milliseconds elapsed from 1/1/1970 until
	*  logging loggingEvent was created. */
	const TimeHelper& getTimestamp() const
	{
		return _timestamp;
	}

	void swap(InternalLoggingEvent &);

	// public operators
	InternalLoggingEvent& operator=(const InternalLoggingEvent& rhs);

	// static methods
	static unsigned int getDefaultType();

protected:
	
	std::string _message;
	std::string _loggerName;
	LogLevel _ll;
	TimeHelper _timestamp;
};


extern TLSKeyType g_TLS_StorageKey;


inline void setInternalLoggingEvent(InternalLoggingEvent* p)
{
	TLSSetValue(g_TLS_StorageKey, p);
}


inline InternalLoggingEvent* allocInternalLoggingEvent()
{
	InternalLoggingEvent* p = new InternalLoggingEvent;
	setInternalLoggingEvent(p);
	return p;
}


inline InternalLoggingEvent* getInternalLoggingEvent(bool alloc = true)
{
	InternalLoggingEvent* p = reinterpret_cast<InternalLoggingEvent*>(TLSGetValue(g_TLS_StorageKey));

	if(!p && alloc)
		return allocInternalLoggingEvent();

	return p;
}


} // namespace log4cplus

#endif // LOG4CPLUS_SPI_INTERNAL_LOGGING_EVENT_HEADER_
