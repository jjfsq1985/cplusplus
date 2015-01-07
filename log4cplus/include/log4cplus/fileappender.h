
// Module:  Log4CPLUS
// File:    fileappender.h

#ifndef LOG4CPLUS_FILE_APPENDER_HEADER_
#define LOG4CPLUS_FILE_APPENDER_HEADER_


#include "log4cplus/platform.h"
#include "log4cplus/appender.h"
#include "log4cplus/timeHelper.h"

#include <fstream>
#include <memory>


namespace log4cplus{


/**
* Appends log events to a file. 
* 
*/
class LOG4CPLUS_EXPORT FileAppender : public Appender 
{
public:
	FileAppender(const std::string& filename, 
		std::ios_base::openmode mode = std::ios_base::trunc,
		bool immediateFlush = true, bool createDirs = false);

	FileAppender(const Properties& properties,
		std::ios_base::openmode mode = std::ios_base::trunc);

	virtual ~FileAppender();

	virtual void close();

protected:
	virtual void append(const InternalLoggingEvent& loggingEvent);

	void open(std::ios_base::openmode mode);

	bool reopen();

	/**
	* Immediate flush means that the underlying writer or output stream
	* will be flushed at the end of each append operation. Immediate
	* flush is slower but ensures that each append request is actually
	* written. If <code>immediateFlush</code> is set to
	* <code>false</code>, then there is a good chance that the last few
	* logs events are not actually written to persistent media if and
	* when the application crashes.
	*  
	* The <code>immediateFlush</code> variable is set to
	* <code>true</code> by default.
	*/
	bool _immediateFlush;

	/**
	* When this variable is true, FileAppender will try to create
	* missing directories in path leading to log file.
	*
	* The `createDirs` variable is set to `false` by default.
	*/
	bool _isCreateDirs;

	/**
	* When any append operation fails, <code>reopenDelay</code> says 
	* for how many seconds the next attempt to re-open the log file and 
	* resume logging will be delayed. If <code>reopenDelay</code> is zero, 
	* each failed append operation will cause log file to be re-opened. 
	* By default, <code>reopenDelay</code> is 1 second.
	*/
	int _reopenDelay;
	unsigned long _ofstreamBufferSize;
	char* _ofstreamBuffer;
	std::ofstream _out;
	std::string _filename;
	TimeHelper _reopen_time;

private:
	void init(const std::string& filename, std::ios_base::openmode mode);

	FileAppender(const FileAppender&);

	FileAppender& operator= (const FileAppender&);
};


typedef SharedPtr<FileAppender> SharedFileAppenderPtr;



/**
* RollingFileAppender extends FileAppender to backup the log
* files when they reach a certain size.
*/
class LOG4CPLUS_EXPORT RollingFileAppender : public FileAppender {
public:
		
	RollingFileAppender(const std::string& filename,
		long maxFileSize = 10*1024*1024, // 10 MB
		int maxBackupIndex = 1,
		bool immediateFlush = true,
		bool createDirs = false);

	RollingFileAppender(const Properties& properties);

	virtual ~RollingFileAppender();

protected:
	virtual void append(const InternalLoggingEvent& loggingEvent);
	void rollover();

	long _maxFileSize;
	int _maxBackupIndex;

private:
	void init(long maxFileSize, int maxBackupIndex);
};


typedef SharedPtr<RollingFileAppender> SharedRollingFileAppenderPtr;


enum DailyRollingFileSchedule { MONTHLY, WEEKLY, DAILY, TWICE_DAILY, HOURLY, MINUTELY};

/**
* DailyRollingFileAppender extends {@link FileAppender} so that the
* underlying file is rolled over at a user chosen frequency.
*/
class LOG4CPLUS_EXPORT DailyRollingFileAppender : public FileAppender {
public:
		
	DailyRollingFileAppender(const std::string& filename,
		DailyRollingFileSchedule schedule = DAILY,
		bool immediateFlush = true,
		int maxBackupIndex = 10,
		bool createDirs = false);

	DailyRollingFileAppender(const Properties& properties);

		
	virtual ~DailyRollingFileAppender();

		
	virtual void close();

protected:
	virtual void append(const InternalLoggingEvent& loggingEvent);

	void rollover();

	TimeHelper calculateNextRolloverTime(const TimeHelper& t) const;

	std::string getFilename(const TimeHelper& t) const;

		
	DailyRollingFileSchedule _schedule;
	std::string _scheduledFilename;
	TimeHelper _nextRolloverTime;
	int _maxBackupIndex;

private:
	void init(DailyRollingFileSchedule schedule);
};


} // namespace log4cplus


#endif // LOG4CPLUS_FILE_APPENDER_HEADER_

