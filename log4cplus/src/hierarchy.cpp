// Module:  Log4CPLUS
// File:    hierarchy.cpp


#include "log4cplus/hierarchy.h"
#include "log4cplus/loglog.h"
#include "log4cplus/loggerimpl.h"
#include "log4cplus/rootlogger.h"
#include <utility>
#include <limits>

using namespace std;
using namespace log4cplus;


Hierarchy::Hierarchy() : defaultFactory(new DefaultLoggerFactory()), root(NULL)
	// Don't disable any LogLevel level by default.
	, _nDisableValue(NOT_SET_LOG_LEVEL), _isEmittedNoAppenderWarning(false)
{
	root = Logger(new RootLogger(*this, DEBUG_LOG_LEVEL));
}

Hierarchy::~Hierarchy()
{
}



void Hierarchy::clear() 
{
	MutexLock lock(&_hashtable_mutex);

	provisionNodes.erase(provisionNodes.begin(), provisionNodes.end());
	loggerPtrs.erase(loggerPtrs.begin(), loggerPtrs.end());
}


bool Hierarchy::exists(const string& name)
{
	// Root logger always does exist.
	if (name.empty ())
		return true;

	MutexLock lock(&_hashtable_mutex);

	LoggerMap::iterator it = loggerPtrs.find(name);
	return it != loggerPtrs.end();
}


void Hierarchy::disableLevel(const string& loglevelStr)
{
	_nDisableValue = getLogLevelManager().fromString(loglevelStr);
}


void Hierarchy::disableLevel(LogLevel ll) 
{
	_nDisableValue = ll;
}


void Hierarchy::disableAllLevel() 
{ 
	disableLevel((std::numeric_limits<LogLevel>::max) ());
}


void Hierarchy::enableAllLevel() 
{ 
	_nDisableValue = NOT_SET_LOG_LEVEL; 
}


Logger Hierarchy::getInstance(const string& name) 
{ 
	return getInstance(name, *defaultFactory); 
}


Logger Hierarchy::getInstance(const string& name, LoggerFactory& factory)
{
	MutexLock lock(&_hashtable_mutex);

	return getInstanceImpl(name, factory);
}


LoggerList Hierarchy::getCurrentLoggers()
{
	LoggerList retList;

	MutexLock lock(&_hashtable_mutex);
	initializeLoggerList(retList);

	return retList;
}


bool Hierarchy::isDisabled(LogLevel level) 
{ 
	return _nDisableValue >= level; 
}

Logger Hierarchy::getRoot() const
{ 
	return root; 
}


void Hierarchy::setLoggerFactory(std::auto_ptr<LoggerFactory> factory) 
{ 
	defaultFactory = factory; 
}


LoggerFactory * Hierarchy::getLoggerFactory()
{
	return defaultFactory.get();
}


//////////////////////////////////////////////////////////////////////////////
// Hierarchy private methods
//////////////////////////////////////////////////////////////////////////////

Logger Hierarchy::getInstanceImpl(const string& name, LoggerFactory& factory)
{
	Logger logger;
	LoggerMap::iterator lm_it;

	if (name.empty())
		logger = root;
	else if ((lm_it = loggerPtrs.find(name)) != loggerPtrs.end())
		logger = lm_it->second;
	else
	{
		// Need to create a new logger
		logger = factory.makeNewLoggerInstance(name, *this);
		bool inserted = loggerPtrs.insert(std::make_pair(name, logger)).second;
		if (!inserted)
		{
			LogLog::getLogLog()->error("Hierarchy::getInstanceImpl()- Insert failed", true);
		}

		ProvisionNodeMap::iterator pnm_it = provisionNodes.find(name);
		if (pnm_it != provisionNodes.end())
		{
			updateChildren(pnm_it->second, logger);
			bool deleted = (provisionNodes.erase(name) > 0);
			if (!deleted)
			{
				LogLog::getLogLog()->error("Hierarchy::getInstanceImpl()- Delete failed", true);
			}
		}
		updateParents(logger);
	}

	return logger;
}


void Hierarchy::initializeLoggerList(LoggerList& list) const
{
	for(LoggerMap::const_iterator it=loggerPtrs.begin(); it!= loggerPtrs.end(); ++it) 
	{
		list.push_back((*it).second);
	}
}

void Hierarchy::updateParents(Logger const& logger)
{
	string const& sName = logger.getName();
	std::size_t const length = sName.length();
	bool parentFound = false;
	string substr;

	// if name = "w.x.y.z", loop thourgh "w.x.y", "w.x" and "w", but not "w.x.y.z"
	for(std::size_t i = sName.find_last_of('.', length-1); 
		i != string::npos && i > 0; i = sName.find_last_of('.', i-1)) 
	{
		substr.assign (sName, 0, i);

		LoggerMap::iterator it = loggerPtrs.find(substr);
		if(it != loggerPtrs.end()) 
		{
			parentFound = true;
			logger._pLoggerImpl->_parent = it->second._pLoggerImpl;
			break;  // no need to update the ancestors of the closest ancestor
		}
		else 
		{
			ProvisionNodeMap::iterator it2 = provisionNodes.find(substr);
			if(it2 != provisionNodes.end()) 
			{
				it2->second.push_back(logger);
			}
			else 
			{
				ProvisionNode node;
				node.push_back(logger);
				std::pair<ProvisionNodeMap::iterator, bool> tmp = 
					provisionNodes.insert(std::make_pair(substr, node));
				//bool inserted = provisionNodes.insert(std::make_pair(substr, node)).second;
				if(!tmp.second) 
				{
					LogLog::getLogLog()->error("Hierarchy::updateParents()- Insert failed", true);
				}
			}
		} // end if Logger found
	} // end for loop

	if(!parentFound) 
	{
		logger._pLoggerImpl->_parent = root._pLoggerImpl;
	}
}


static bool startsWith(string const& teststr, string const& substr)
{
	bool val = false;
	string::size_type const len = substr.length();
	if (teststr.length() > len)
		val = teststr.compare (0, len, substr) == 0;

	return val;
}

void Hierarchy::updateChildren(ProvisionNode& pn, Logger const& logger)
{
	for(ProvisionNode::iterator it=pn.begin(); it!=pn.end(); ++it) 
	{
		Logger& tmp = *it;
		// Unless this child already points to a correct (lower) parent,
		// make logger.parent point to c.parent and c.parent to logger.
		if(!startsWith(tmp._pLoggerImpl->_parent->getName(), logger.getName())) 
		{
			logger._pLoggerImpl->_parent = tmp._pLoggerImpl->_parent;
			tmp._pLoggerImpl->_parent = logger._pLoggerImpl;
		}
	}
}


