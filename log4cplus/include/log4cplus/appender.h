
// Module:  Log4CPLUS
// File:    appender.h

#ifndef LOG4CPLUS_APPENDER_HEADER_
#define LOG4CPLUS_APPENDER_HEADER_

#include "log4cplus/platform.h"
#include "log4cplus/layout.h"
#include "log4cplus/loglevel.h"
#include "log4cplus/sharedptr.h"
#include "log4cplus/filter.h"
#include "log4cplus/mutex.h"

#include <memory>


namespace log4cplus {


class Properties;


/**
    * This class is used to "handle" errors encountered in an {@link
    * Appender}.
    */
class LOG4CPLUS_EXPORT ErrorHandler
{
public:
    ErrorHandler ();
    virtual ~ErrorHandler() = 0;
    virtual void error(const std::string& err) = 0;
    virtual void reset() = 0;
};


class LOG4CPLUS_EXPORT OnlyOnceErrorHandler : public ErrorHandler
{
public:
    
    OnlyOnceErrorHandler();
    virtual ~OnlyOnceErrorHandler ();
    virtual void error(const std::string& err);
    virtual void reset();

private:
    bool firstTime;
};


class LOG4CPLUS_EXPORT Appender 
{
public:
    
    Appender();

    Appender(const Properties & properties);

    
    virtual ~Appender();

    /**
        * This function is for derived appenders to call from their
        * destructors. All classes derived from `Appender` class
        * _must_ call this function from their destructors. 
        * destroyed.
        */
    void destructorImpl();

    /**
        * Release any resources allocated within the appender such as file
        * handles, network connections, etc.
        * 
        * It is a programming errorto append to a closed appender.
        */
    virtual void close() = 0;

    /**
        * Check if this appender is in closed state.
        */
    bool isClosed() const;

    /**
        * This method performs threshold checks and invokes filters before
        * delegating actual logging to the subclasses specific {@link
        * #append} method.
        */
    void doAppend(const InternalLoggingEvent& loggingEvent);

    /**
        * Get the name of this appender. The name uniquely identifies the
        * appender.
        */
    virtual std::string getName();

    /**
        * Set the name of this appender. The name is used by other
        * components to identify this appender.
        */
    virtual void setName(const std::string& name);

    /**
        * Set the {@link ErrorHandler} for this Appender.
        */
    virtual void setErrorHandler(std::auto_ptr<ErrorHandler> eh);

    /**
        * Return the currently set {@link ErrorHandler} for this
        * Appender.
        */
    virtual ErrorHandler* getErrorHandler();

    /**
        * Set the layout for this appender. Note that some appenders have
        * their own (fixed) layouts or do not use one. For example, the
        * SocketAppender ignores the layout set here.
        */
    virtual void setLayout(std::auto_ptr<Layout> layout);

    /**
        * Returns the layout of this appender. The value may be NULL.
        * 
        * This class owns the returned pointer.
        */
    virtual Layout* getLayout();

    /**
        * Set the filter chain on this Appender.
        */
    void setFilter(FilterPtr f) { _filter = f; }

    /**
        * Get the filter chain on this Appender.
        */
    FilterPtr getFilter() const { return _filter; }

    /**
        * Returns this appenders threshold LogLevel. See the {@link
        * #setThreshold} method for the meaning of this option.
        */
    LogLevel getThreshold() const { return _threshold; }

    /**
        * Set the threshold LogLevel. All log events with lower LogLevel
        * than the threshold LogLevel are ignored by the appender.
        * 
        * In configuration files this option is specified by setting the
        * value of the <b>Threshold</b> option to a LogLevel
        * std::string, such as "DEBUG", "INFO" and so on.
        */
    void setThreshold(LogLevel th) { _threshold = th; }

    /**
        * Check whether the message LogLevel is below the appender's
        * threshold. If there is no threshold set, then the return value is
        * always <code>true</code>.
        */
    bool isAsSevereAsThreshold(LogLevel ll) const 
	{
        return ((ll != NOT_SET_LOG_LEVEL) && (ll >= _threshold));
    }

protected:
    /**
        * Subclasses of <code>Appender</code> should implement this
        * method to perform actual logging.
        * @see doAppend method.
        */
    virtual void append(const InternalLoggingEvent& loggingEvent) = 0;


    
    /** The layout variable does not need to be set if the appender
        *  implementation has its own layout. */
    std::auto_ptr<Layout> _layout;

    /** Appenders are named. */
    std::string _name;

    /** There is no LogLevel threshold filtering by default.  */
    LogLevel _threshold;

    /** The first filter in the filter chain. Set to <code>null</code>
        *  initially. */
    FilterPtr _filter;

    /** It is assumed and enforced that errorHandler is never null. */
    std::auto_ptr<ErrorHandler> _errorHandler;

    /** Is this appender closed? */
    bool _isClosed;
	Mutex _mutex;
};

/** This is a pointer to an Appender. */
typedef SharedPtr<Appender> SharedAppenderPtr;

} // namespace log4cplus

#endif // LOG4CPLUS_APPENDER_HEADER_

