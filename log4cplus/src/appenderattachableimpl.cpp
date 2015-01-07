// Module:  Log4CPLUS
// File:    appenderattachableimpl.cpp

#include "log4cplus/appenderattachableimpl.h"
#include "log4cplus/appender.h"
#include "log4cplus/loglog.h"
#include "log4cplus/loggingevent.h"

#include <algorithm>


using namespace std;
using namespace log4cplus;


AppenderAttachable::~AppenderAttachable() {}


AppenderAttachableImpl::AppenderAttachableImpl() {}


AppenderAttachableImpl::~AppenderAttachableImpl() {}


void AppenderAttachableImpl::addAppender(SharedAppenderPtr newAppender)
{
	if(!newAppender) 
	{
		LogLog::getLogLog()->error("Tried to add NULL appender");
		return;
	}

	MutexLock lock(&appender_list_mutex);

	ListType::iterator it = std::find(_appenderList.begin(), _appenderList.end(), newAppender);
	if(it == _appenderList.end()) 
	{
		_appenderList.push_back(newAppender);
	}
}


AppenderAttachableImpl::ListType AppenderAttachableImpl::getAllAppenders()
{
	MutexLock lock(&appender_list_mutex);

	return _appenderList;
}


SharedAppenderPtr AppenderAttachableImpl::getAppender(const string& name)
{
	MutexLock lock(&appender_list_mutex);

	for(ListType::iterator it=_appenderList.begin(); it!=_appenderList.end(); ++it)
	{
		if((*it)->getName() == name) {
			return *it;
		}
	}

	return SharedAppenderPtr(NULL);
}



void AppenderAttachableImpl::removeAllAppenders()
{
	MutexLock lock(&appender_list_mutex);

	_appenderList.erase(_appenderList.begin(), _appenderList.end());
}



void AppenderAttachableImpl::removeAppender(SharedAppenderPtr appender)
{
	if(!appender) 
	{
		LogLog::getLogLog()->error("Tried to remove NULL appender");
		return;
	}

	MutexLock lock(&appender_list_mutex);

	ListType::iterator it =
		std::find(_appenderList.begin(), _appenderList.end(), appender);
	if(it != _appenderList.end()) 
	{
		_appenderList.erase(it);
	}
}


void AppenderAttachableImpl::removeAppender(const string& name)
{
	removeAppender(getAppender(name));
}


int AppenderAttachableImpl::appendLoopOnAppenders(const InternalLoggingEvent& loggingEvent) const
{
	int count = 0;

	MutexLock lock(&const_cast<Mutex&>(appender_list_mutex));

	for(ListType::const_iterator it=_appenderList.begin(); it!=_appenderList.end(); ++it)
	{
		++count;
		static_cast<SharedAppenderPtr>(*it)->doAppend(loggingEvent);
	}

	return count;
}

