// Module:  Log4CPLUS
// File:    logloguser.cpp


#include "log4cplus/logloguser.h"
#include "log4cplus/loglog.h"

using namespace log4cplus;


LogLogUser::LogLogUser() {}

LogLogUser::LogLogUser(const LogLogUser&) {}

LogLogUser::~LogLogUser() {}

LogLog& LogLogUser::getLogLog() const
{
    return *LogLog::getLogLog();
}

LogLogUser& LogLogUser::operator= (const LogLogUser&)
{
    return *this;
}


