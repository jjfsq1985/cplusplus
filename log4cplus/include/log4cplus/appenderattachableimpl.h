
// Module:  Log4CPLUS
// File:    appenderattachableimpl.h

#ifndef LOG4CPLUS_HELPERS_APPENDER_ATTACHABLE_IMPL_HEADER_
#define LOG4CPLUS_HELPERS_APPENDER_ATTACHABLE_IMPL_HEADER_

#include "log4cplus/platform.h"
#include "log4cplus/sharedptr.h"
#include "log4cplus/appenderattachable.h"
#include "log4cplus/mutex.h"

#include <memory>
#include <vector>


namespace log4cplus {


/**
* This Interface is for attaching Appenders to objects.
*/
class LOG4CPLUS_EXPORT AppenderAttachableImpl : public log4cplus::AppenderAttachable
{
public:
	
	Mutex appender_list_mutex;

	
	AppenderAttachableImpl();

	
	virtual ~AppenderAttachableImpl();

	/**
	* Add an appender.  If the appender is already in the list in
	* won't be added again.
	*/
	virtual void addAppender(SharedAppenderPtr newAppender);

	/**
	* Get all previously added appenders as an vectory.
	*/
	virtual SharedAppenderPtrList getAllAppenders();

	/**
	* Look for an attached appender named as <code>name</code>.
	*
	* Return the appender with that name if in the list. Return null
	* otherwise.  
	*/
	virtual SharedAppenderPtr getAppender(const std::string& name);

	/**
	* Remove all previously added appenders.
	*/
	virtual void removeAllAppenders();

	/**
	* Remove the appender passed as parameter from the list of appenders.
	*/
	virtual void removeAppender(SharedAppenderPtr appender);

	/**
	* Remove the appender with the name passed as parameter from the
	* list of appenders.  
	*/
	virtual void removeAppender(const std::string& name);

	/**
	* Call the <code>doAppend</code> method on all attached appenders.  
	*/
	int appendLoopOnAppenders(const InternalLoggingEvent& loggingEvent) const;

protected:
	// Types
	typedef std::vector<SharedAppenderPtr> ListType;

	
	/** Array of appenders. */
	ListType _appenderList;

private:
	AppenderAttachableImpl(AppenderAttachableImpl const&);
	AppenderAttachableImpl & operator = (AppenderAttachableImpl const&);
};  // end class AppenderAttachableImpl

}  // namespace log4cplus 

#endif // LOG4CPLUS_HELPERS_APPENDER_ATTACHABLE_IMPL_HEADER_

