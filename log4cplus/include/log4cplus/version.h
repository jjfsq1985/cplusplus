



#if !defined (LOG4CPLUS_VERSION_H)
#define LOG4CPLUS_VERSION_H

#include "log4cplus/platform.h"

#define LOG4CPLUS_MAKE_VERSION(major, minor, point) \
    (major * 1000 * 1000u + minor * 1000u + point)

#define LOG4CPLUS_MAKE_VERSION_STR(major, minor, point) \
    #major "." #minor "." #point

//!This is log4cplus version number as unsigned integer.  This must
//!be kept on a single line. It is used by Autotool and CMake build
//!systems to parse version number.
#define LOG4CPLUS_VERSION LOG4CPLUS_MAKE_VERSION(1, 0, 0)

//!This is log4cplus version number as a string.
#define LOG4CPLUS_VERSION_STR LOG4CPLUS_MAKE_VERSION_STR(1, 0, 0)


namespace log4cplus
{

extern LOG4CPLUS_EXPORT unsigned const version;
extern LOG4CPLUS_EXPORT char const versionStr[];

}

#endif
