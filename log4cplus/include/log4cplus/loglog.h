
// Module:  Log4CPLUS
// File:    loglog.h

#ifndef LOG4CPLUS_HELPERS_LOGLOG
#define LOG4CPLUS_HELPERS_LOGLOG

#include "log4cplus/platform.h"
#include "log4cplus/mutex.h"

namespace log4cplus { 

/**
*
* All log4cplus internal debug calls go to <code>cout</code>
* where as internal errormessages are sent to
* <code>cerr</code>. All internal messages are prepended with
* the string "log4clus: ".
*/
class LOG4CPLUS_EXPORT LogLog
{
public:
	//!Return type of LogLog::getLogLog()->

	/**
	* Returns a reference to the <code>LogLog</code> singleton.
	*/
	static LogLog* getLogLog();

	/**
	* Allows to enable/disable log4cplus internal logging.
	*/
	void setInternalDebugging(bool enabled);

	/**
	* This method is used to output log4cplus internal debug
	* statements. Output goes to <code>std::cout</code>.
	*/
	void debug(const std::string& msg) const;
	void debug(char const* msg) const;

	/**
	* This method is used to output log4cplus internal error
	* statements. There is no way to disable error
	* statements.  Output goes to
	* <code>std::cerr</code>. Optionally, this method can
	* throw std::runtime_error exception too.
	*/
	void error(const std::string& msg, bool throw_flag = false) const;
	void error(char const* msg, bool throw_flag = false) const;

	// Public ctor  to be used only by internal::DefaultContext.
	LogLog();
	virtual ~LogLog();

private:

	void loggingWorker(char const*, std::string const&, bool throw_flag = false) const;
	
	mutable bool _isDebugEnabled;
	Mutex _mutex;

	LogLog(const LogLog&);
	LogLog& operator =(LogLog const&);

	static LogLog* s_pLogLog;
};

}  // namespace log4cplus 


#endif // LOG4CPLUS_HELPERS_LOGLOG

