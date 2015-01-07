
// Module:  Log4CPLUS
// File:    timehelper.h


#ifndef LOG4CPLUS_HELPERS_TIME_HELPER_HEADER_
#define LOG4CPLUS_HELPERS_TIME_HELPER_HEADER_


#include "log4cplus/platform.h"
#include "log4cplus/timehelper.h"

#include <ctime>


namespace log4cplus { 


/**
* This class represents a Epoch time with microsecond accuracy.
*/
class TimeHelper 
{
public:
	TimeHelper();

	TimeHelper(std::time_t tv_sec, long tv_usec);

	explicit TimeHelper(std::time_t time);

	static TimeHelper gettimeofday();

	time_t sec() const { return _tv_seconds; }

	long usec() const { return _tv_microseconds; }

	void sec(std::time_t s) { _tv_seconds = s; }

	void usec(long us) { _tv_microseconds = us; }

	time_t setTime(std::tm* t);
	time_t getTime() const;
	void localtime(std::tm* t) const;

	std::string getFormattedTime(const std::string& fmt) const;

	// Operators
	TimeHelper& operator+= (const TimeHelper& rhs);
	TimeHelper& operator-= (const TimeHelper& rhs);
	TimeHelper& operator/= (long rhs);
	TimeHelper& operator*= (long rhs);

private:
	time_t _tv_seconds;		// seconds 
	long _tv_microseconds;  // microseconds 
};


} // namespace log4cplus


const log4cplus::TimeHelper operator+(const log4cplus::TimeHelper& lhs, const log4cplus::TimeHelper& rhs);
const log4cplus::TimeHelper operator-(const log4cplus::TimeHelper& lhs, const log4cplus::TimeHelper& rhs);
const log4cplus::TimeHelper operator/(const log4cplus::TimeHelper& lhs, long rhs);
const log4cplus::TimeHelper operator*(const log4cplus::TimeHelper& lhs, long rhs);
bool operator < (const log4cplus::TimeHelper& lhs, const log4cplus::TimeHelper& rhs);
bool operator <= (const log4cplus::TimeHelper& lhs, const log4cplus::TimeHelper& rhs);
bool operator > (const log4cplus::TimeHelper& lhs, const log4cplus::TimeHelper& rhs);
bool operator >= (const log4cplus::TimeHelper& lhs, const log4cplus::TimeHelper& rhs);
bool operator == (const log4cplus::TimeHelper& lhs, const log4cplus::TimeHelper& rhs);
bool operator != (const log4cplus::TimeHelper& lhs, const log4cplus::TimeHelper& rhs);


#endif // LOG4CPLUS_HELPERS_TIME_HELPER_HEADER_

