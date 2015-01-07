

#include "log4cplus/version.h"


using namespace log4cplus;

#if !defined (LOG4CPLUS_VERSION_STR_SUFFIX)
#define LOG4CPLUS_VERSION_STR_SUFFIX ""
#endif

unsigned const version = LOG4CPLUS_VERSION;
char const versionStr[] = LOG4CPLUS_VERSION_STR LOG4CPLUS_VERSION_STR_SUFFIX;

