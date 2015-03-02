// Module:  Log4CPLUS
// File:    timehelper.cpp


#include "log4cplus/timehelper.h"
#include "log4cplus/loglog.h"
#include "log4cplus/stringhelper.h"
#include "log4cplus/loggingevent.h"

#include <algorithm>
#include <vector>
#include <iomanip>
#include <cassert>
#include <cerrno>
#include <sys/types.h>
#ifdef __linux__
#include <sys/time.h>
#endif


using namespace log4cplus;
using namespace std;


const int ONE_SEC_IN_USEC = 1000000;


TimeHelper::TimeHelper() : _tv_seconds(0), _tv_microseconds(0){}


TimeHelper::TimeHelper(time_t tv_sec_, long tv_usec_) : _tv_seconds(tv_sec_), _tv_microseconds(tv_usec_)
{
	assert(_tv_microseconds < ONE_SEC_IN_USEC);
}


TimeHelper::TimeHelper(time_t time) : _tv_seconds(time), _tv_microseconds(0)
{
}


TimeHelper TimeHelper::gettimeofday()
{
#ifdef _MSC_VER
	FILETIME ft;

	GetSystemTimeAsFileTime(&ft);

	__int64 st100ns = __int64(ft.dwHighDateTime) << 32 | ft.dwLowDateTime;

	// Number of 100-ns intervals between UNIX epoch and Windows system time is 116444736000000000.
	__int64 const offset = __int64(116444736) * 1000 * 1000 * 1000;
	__int64 fixed_time = st100ns - offset;

	return TimeHelper(fixed_time /(10 * 1000 * 1000), fixed_time %(10 * 1000 * 1000) / 10);

#else // __linux__
	struct timeval tp;
	::gettimeofday(&tp, 0);

	return TimeHelper(tp.tv_sec, tp.tv_usec);
#endif
}


time_t TimeHelper::setTime(tm* t)
{
	time_t time = std::mktime(t);
	if(time != -1)
		_tv_seconds = time;

	return time;
}


time_t TimeHelper::getTime() const
{
	return _tv_seconds;
}


void TimeHelper::localtime(tm* t) const
{
	time_t clock = _tv_seconds;
#ifdef _MSC_VER 
	tm* tmp = std::localtime(&clock);
	*t = *tmp;
#else
	::localtime_r(&clock, t);
#endif
}

string TimeHelper::getFormattedTime(const string& string2Format) const
{
	string sFormatedArray;
	string sFormated;

	if(string2Format.empty() || string2Format[0] == 0)
		return string();

	std::tm time;

	localtime(&time);

	bool isText = true;
	// Walk the format string and process all occurences of %q, %Q and %s.

	for(string::const_iterator it = string2Format.begin(); it != string2Format.end(); ++it)
	{
		if (isText)
		{
			if(*it == '%')
				isText = false;
			else
				sFormatedArray.push_back(*it);
		}
		else
		{
			sFormatedArray.push_back('%');
			sFormatedArray.push_back(*it);
			isText = true;
		}
	}

	// Finally call strftime/wcsftime to format the rest of the string.

	sFormated.swap(sFormatedArray);
	std::size_t bufSize = sFormated.size() + 1;
	std::size_t len;

	// Limit how far can the buffer grow. This is necessary so that we
	// catch bad format string. Some implementations of strftime() signal
	// both too small buffer and invalid format string by returning 0
	// without changing errno. 
	std::size_t const maxBufferSize = (std::max)(static_cast<std::size_t>(1024), bufSize * 16);

	vector<char> szbuf;
	do
	{
		szbuf.resize(bufSize);
		errno = 0;
		len = std::strftime(&szbuf[0], bufSize, sFormated.c_str(), &time);
		if(len == 0)
		{
			int const eno = errno;
			bufSize *= 2;
			if(bufSize > maxBufferSize)
			{
				LogLog::getLogLog()->error("Error in strftime(): "+ convertIntegerToString(eno), true);
			}
		}
	} 
	while(len == 0);

	return string(szbuf.begin(), szbuf.begin() + len);
}


TimeHelper& TimeHelper::operator+=(const TimeHelper& rhs)
{
	_tv_seconds += rhs._tv_seconds;
	_tv_microseconds += rhs._tv_microseconds;

	if(_tv_microseconds > ONE_SEC_IN_USEC) 
	{
		++_tv_seconds;
		_tv_microseconds -= ONE_SEC_IN_USEC;
	}

	return *this;
}


TimeHelper& TimeHelper::operator-=(const TimeHelper& rhs)
{
	_tv_seconds -= rhs._tv_seconds;
	_tv_microseconds -= rhs._tv_microseconds;

	if(_tv_microseconds < 0) 
	{
		--_tv_seconds;
		_tv_microseconds += ONE_SEC_IN_USEC;
	}

	return *this;
}


TimeHelper& TimeHelper::operator/=(long rhs)
{
	long rem_secs = static_cast<long>(_tv_seconds % rhs);
	_tv_seconds /= rhs;

	_tv_microseconds /= rhs;
	_tv_microseconds += static_cast<long>((rem_secs * ONE_SEC_IN_USEC) / rhs);

	return *this;
}


TimeHelper& TimeHelper::operator*=(long rhs)
{
	long new_usec = _tv_microseconds * rhs;
	long overflow_sec = new_usec / ONE_SEC_IN_USEC;
	_tv_microseconds = new_usec % ONE_SEC_IN_USEC;

	_tv_seconds *= rhs;
	_tv_seconds += overflow_sec;

	return *this;
}


const TimeHelper operator+(const TimeHelper& lhs, const TimeHelper& rhs)
{
	return TimeHelper(lhs) += rhs;
}


const TimeHelper operator-(const TimeHelper& lhs, const TimeHelper& rhs)
{
	return TimeHelper(lhs) -= rhs;
}


const TimeHelper operator/(const TimeHelper& lhs, long rhs)
{
	return TimeHelper(lhs) /= rhs;
}


const TimeHelper operator*(const TimeHelper& lhs, long rhs)
{
	return TimeHelper(lhs) *= rhs;
}


bool operator<(const TimeHelper& lhs, const TimeHelper& rhs)
{
	return((lhs.sec() < rhs.sec())||((lhs.sec() == rhs.sec()) && (lhs.usec() < rhs.usec())));
}


bool operator<=(const TimeHelper& lhs, const TimeHelper& rhs)
{
	return((lhs < rhs) ||(lhs == rhs));
}


bool operator>(const TimeHelper& lhs, const TimeHelper& rhs)
{
	return((lhs.sec() > rhs.sec()) || ((lhs.sec() == rhs.sec())&&(lhs.usec() > rhs.usec())));
}


bool operator>=(const TimeHelper& lhs, const TimeHelper& rhs)
{
	return((lhs > rhs) ||(lhs == rhs));
}


bool operator==(const TimeHelper& lhs, const TimeHelper& rhs)
{
	return(   lhs.sec() == rhs.sec()
		&& lhs.usec() == rhs.usec());
}

bool operator!=(const TimeHelper& lhs, const TimeHelper& rhs)
{
	return !(lhs == rhs);
}


