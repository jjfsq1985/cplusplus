// Module:  Log4CPLUS
// File:    customappender.cpp


#include "log4cplus/customappender.h"
#include "log4cplus/layout.h"
#include "log4cplus/loglog.h"
#include "log4cplus/stringhelper.h"
#include "log4cplus/loggingevent.h"

#include <strstream>


using namespace std;
using namespace log4cplus;


pCustomFuncCallBack CustomAppender:: _pCustomFunc = NULL;


void CustomAppender::setCustomFunc(pCustomFuncCallBack pCustomFunc)
{
	_pCustomFunc = pCustomFunc;
}


CustomAppender::CustomAppender()
{
}


CustomAppender::CustomAppender(const Properties& properties) : Appender(properties)
{
}


CustomAppender::~CustomAppender()
{
    destructorImpl();
}


void CustomAppender::close()
{
	_isClosed = true;
}


// This method does not need to be locked since it is called by
// doAppend() which performs the locking
void CustomAppender::append(const InternalLoggingEvent& loggingEvent)
{
	if (NULL == _pCustomFunc)
		return;

	ostrstream output;
	_layout->formatAndAppend(output, loggingEvent);

	string outstring = output.str();

	_pCustomFunc(outstring.c_str());
}

