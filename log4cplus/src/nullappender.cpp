// Module:  Log4CPLUS
// File:    nullappender.cpp


#include "log4cplus/nullappender.h"


using namespace log4cplus;


NullAppender::NullAppender()
{
}


NullAppender::NullAppender(const Properties& properties)
: Appender(properties)
{
}


NullAppender::~NullAppender()
{
    destructorImpl();
}



void NullAppender::close()
{
}



// This method does not need to be locked since it is called by
// doAppend() which performs the locking
void NullAppender::append(const InternalLoggingEvent&)
{
}

