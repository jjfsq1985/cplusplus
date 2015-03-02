
// Module:  Log4CPLUS
// File:    consoleappender.h

#ifndef LOG4CPLUS_CONSOLE_APPENDER_HEADER_
#define LOG4CPLUS_CONSOLE_APPENDER_HEADER_


#include "log4cplus/platform.h"
#include "log4cplus/appender.h"
#include "log4cplus/mutex.h"


namespace log4cplus {


/**
    * ConsoleAppender appends log events to <code>std::cout</code> or
    * <code>std::cerr</code> using a layout specified by the
    * user. The default target is <code>std::cout</code>.
    *
    * <h3>Properties</h3>
    * <dl>
    * <dt><tt>logToStdErr</tt></dt>
    * <dd>When it is set true, the output stream will be
    * <code>std::cerr</code> instead of <code>std::cout</code>.</dd>
    *
    * <dt><tt>ImmediateFlush</tt></dt>
    * <dd>When it is set true, output stream will be flushed after
    * each appended loggingEvent.</dd>
    * 
    * </dl>
    * \sa Appender
    */
class LOG4CPLUS_EXPORT ConsoleAppender : public Appender 
{
public:
    ConsoleAppender(bool immediateFlush = false);

    ConsoleAppender(const Properties & properties);

    ~ConsoleAppender();

    virtual void close();

protected:
    virtual void append(const InternalLoggingEvent& loggingEvent);

    /**
        * Immediate flush means that the underlying output stream
        * will be flushed at the end of each append operation.
        */
    bool _immediateFlush;
};

} // namespace log4cplus

#endif // LOG4CPLUS_CONSOLE_APPENDER_HEADER_

