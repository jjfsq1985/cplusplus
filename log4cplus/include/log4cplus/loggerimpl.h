
// Module:  Log4CPLUS
// File:    loggerimpl.h




#ifndef LOG4CPLUS_SPI_LOGGER_HEADER_
#define LOG4CPLUS_SPI_LOGGER_HEADER_

#include "log4cplus/platform.h"
#include "log4cplus/appenderattachableimpl.h"
#include "log4cplus/sharedptr.h"
#include "log4cplus/loggerfactory.h"

#include <memory>
#include <vector>

namespace log4cplus {


	class DefaultLoggerFactory;


/**
* This is the central class in the log4cplus package. One of the
* distintive features of log4cplus are hierarchical loggers and their
* evaluation.
*/
class LOG4CPLUS_EXPORT LoggerImpl : public AppenderAttachableImpl
{
public:
	typedef SharedPtr<LoggerImpl> SharedLoggerImplPtr;

	

	/**
	* Call the appenders in the hierrachy starting at
	* <code>this</code>.  If no appenders could be found, emit a
	* warning.
	* 
	* This method calls all the appenders inherited from the
	* hierarchy circumventing any evaluation of whether to log or not
	* to log the particular log request.
	*                                   
	* @param loggingEvent The loggingEvent to log. 
	*/
	virtual void callAppenders(const InternalLoggingEvent& loggingEvent);

	/**
	* Close all attached appenders implementing the AppenderAttachable
	* interface.  
	*/
	virtual void closeNestedAppenders();

	/**
	* Check whether this logger is enabled for a given LogLevel passed 
	* as parameter.
	*
	* @return boolean True if this logger is enabled for <code>ll</code>.
	*/
	virtual bool isEnabledFor(LogLevel ll) const;

	/**
	* This generic form is intended to be used by wrappers. 
	*/
	virtual void log(LogLevel ll, const std::string& message);

	virtual void log(InternalLoggingEvent const&);

	/**
	* Starting from this logger, search the logger hierarchy for a
	* "set" LogLevel and return it. Otherwise, return the LogLevel of the
	* root logger.
	*                     
	* The Logger class is designed so that this method executes as
	* quickly as possible.
	*/
	virtual LogLevel getChainedLogLevel() const;

	/**
	* Returns the assigned LogLevel, if any, for this Logger.  
	*           
	* @return LogLevel - the assigned LogLevel.
	*/
	LogLevel getLogLevel() const { return this->_ll; }

	/**
	* Set the LogLevel of this Logger.
	*/
	void setLogLevel(LogLevel _ll) { this->_ll = _ll; }

	/**
	* Return the the {@link Hierarchy} where this <code>Logger</code>
	* instance is attached.
	*/
	virtual Hierarchy& getHierarchy() const;

	/**
	* Return the logger name.  
	*/
	std::string const& getName() const { return _name; }

	virtual ~LoggerImpl();

protected:
	
	/**
	* This constructor created a new <code>Logger</code> instance and
	* sets its name.
	*
	* It is intended to be used by sub-classes only. You should not
	* create loggers directly.
	*
	* @param name The name of the logger.  
	* @param h Hierarchy
	*/
	LoggerImpl(const std::string& name, Hierarchy& h);


	
	/**
	* This method creates a new logging loggingEvent and logs the loggingEvent
	* without further checks.  
	*/
	virtual void forcedLog(LogLevel ll, const std::string& message);

	virtual void forcedLog(InternalLoggingEvent const& ev);


	
	/** The name of this logger */
	std::string _name;

	/**
	* The assigned LogLevel of this logger.
	*/
	LogLevel _ll;

	/**
	* The parent of this logger. All loggers have at least one
	* ancestor which is the root logger. 
	*/
	SharedLoggerImplPtr _parent;

private:
	
	/** Loggers need to know what Hierarchy they are in. */
	Hierarchy& _hierarchy;

	// Disallow copying of instances of this class
	LoggerImpl(const LoggerImpl&);
	LoggerImpl& operator= (const LoggerImpl&);

	// Friends
	friend class Logger;
	friend class DefaultLoggerFactory;
	friend class Hierarchy;
};

typedef LoggerImpl::SharedLoggerImplPtr SharedLoggerImplPtr;

} // namespace log4cplus

#endif // LOG4CPLUS_SPI_LOGGER_HEADER_


