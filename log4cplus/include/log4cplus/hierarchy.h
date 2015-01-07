
// Module:  Log4CPLUS
// File:    hierarchy.h

#ifndef LOG4CPLUS_HIERARCHY_HEADER_
#define LOG4CPLUS_HIERARCHY_HEADER_

#include <map>
#include <memory>
#include <vector>

#include "log4cplus/platform.h"
#include "log4cplus/mutex.h"
#include "log4cplus/logger.h"


namespace log4cplus {
	

class HierarchyLocker;


/**
* This class is specialized in retrieving loggers by name and
* also maintaining the logger hierarchy.
*
* <em>The casual user should not have to deal with this class
* directly.</em>  However, if you are in an environment where
* multiple applications run in the same process, then read on.
*
* The structure of the logger hierarchy is maintained by the
* {@link #getInstance} method. The hierarchy is such that children
* link to their parent but parents do not have any pointers to their
* children. Moreover, loggers can be instantiated in any order, in
* particular descendant before ancestor.
*
* In case a descendant is created before a particular ancestor,
* then it creates a provision node for the ancestor and adds itself
* to the provision node. Other descendants of the same ancestor add
* themselves to the previously created provision node.
*/
class LOG4CPLUS_EXPORT Hierarchy
{
public:

		
	/**
	* Create a new Logger hierarchy.
	*/
	Hierarchy();

	
	virtual ~Hierarchy();

		
	/**
	* This call will clear all logger definitions from the internal
	* hashtable. Invoking this method will irrevocably mess up the
	* logger hierarchy.
	*                     
	* You should <em>really</em> know what you are doing before
	* invoking this method.
	*/
	virtual void clear();

	/**
	* Returns <code>true </code>if the named logger exists 
	* (in the default hierarchy).
	*                
	* @param name The name of the logger to search for.
	*/
	virtual bool exists(const std::string& name);

	/**
	* Similar to {@link #disable(LogLevel)} except that the LogLevel
	* argument is given as a std::string.  
	*/
	virtual void disableLevel(const std::string& loglevelStr);

	/**
	* Disable all logging requests of LogLevel <em>equal to or
	* below</em> the ll parameter <code>p</code>, for
	* <em>all</em> loggers in this hierarchy. Logging requests of
	* higher LogLevel then <code>p</code> remain unaffected.
	*/
	virtual void disableLevel(LogLevel ll);

	/**
	* Disable all logging requests regardless of logger and LogLevel.
	* This method is equivalent to calling {@link #disable} with the
	* argument FATAL_LOG_LEVEL, the highest possible LogLevel.
	*/
	virtual void disableAllLevel();

	/**
	* Undoes the effect of calling any of {@link #disable}, {@link
	* #disableAll}, {@link #disableDebug} and {@link #disableInfo}
	* methods. More precisely, invoking this method sets the Logger
	* class internal variable called <code>disable</code> to its
	* default "off" value.
	*/
	virtual void enableAllLevel();

	/**
	* Return a new logger instance named as the first parameter using
	* the default factory. 
	*                
	* If a logger of that name already exists, then it will be
	* returned.  Otherwise, a new logger will be instantiated and
	* then linked with its existing ancestors as well as children.
	*                                    
	* @param name The name of the logger to retrieve.
	*/
	virtual Logger getInstance(const std::string& name);

	/**
	* Return a new logger instance named as the first parameter using
	* <code>factory</code>.
	*                
	* If a logger of that name already exists, then it will be
	* returned.  Otherwise, a new logger will be instantiated by the
	* <code>factory</code> parameter and linked with its existing
	* ancestors as well as children.
	*                                         
	* @param name The name of the logger to retrieve.
	* @param factory The factory that will make the new logger instance.
	*/
	virtual Logger getInstance(const std::string& name, LoggerFactory& factory);

	/**
	* Returns all the currently defined loggers in this hierarchy.
	*
	* The root logger is <em>not</em> included in the returned list. 
	*/
	virtual LoggerList getCurrentLoggers();

	/** 
	* Is the LogLevel specified by <code>level</code> enabled? 
	*/
	virtual bool isDisabled(LogLevel level);

	/**
	* Get the root of this hierarchy.
	*/
	virtual Logger getRoot() const;

	/**
	* Set the default LoggerFactory instance.
	*/
	virtual void setLoggerFactory(std::auto_ptr<LoggerFactory> factory);

	/**
	* Returns the default LoggerFactory instance.
	*/
	virtual LoggerFactory* getLoggerFactory();

private:
	// Types
	typedef std::vector<Logger> ProvisionNode;
	typedef std::map<std::string, ProvisionNode> ProvisionNodeMap;
	typedef std::map<std::string, Logger> LoggerMap;

		
	/**
	* This is the implementation of the <code>getInstance()</code> method.
	* NOTE: This method does not lock the <code>hashtable_mutex</code>.
	*/

	virtual Logger getInstanceImpl(const std::string& name, 
		LoggerFactory& factory);

	/**
	* This is the implementation of the <code>getCurrentLoggers()</code>.
	* NOTE: This method does not lock the <code>hashtable_mutex</code>.
	*/

	virtual void initializeLoggerList(LoggerList& list) const;

	/**
	* This method loops through all the *potential* parents of
	* logger'. There 3 possible cases:
	*/
	void updateParents(Logger const& logger);

	/**
	* We update the links for all the children that placed themselves
	* in the provision node 'pn'. The second argument 'logger' is a
	* reference for the newly created Logger, parent of all the
	* children in 'pn'
	*/
	void updateChildren(ProvisionNode& pn,Logger const& logger);

	
	Mutex _hashtable_mutex;
	std::auto_ptr<LoggerFactory> defaultFactory;
	ProvisionNodeMap provisionNodes;
	LoggerMap loggerPtrs;
	Logger root;

	int _nDisableValue;
	bool _isEmittedNoAppenderWarning;

	// Disallow copying of instances of this class
	Hierarchy(const Hierarchy&);
	Hierarchy& operator= (const Hierarchy&);

	// Friends
	friend class log4cplus::LoggerImpl;
	friend class log4cplus::HierarchyLocker;
};


LOG4CPLUS_EXPORT Hierarchy& getDefaultHierarchy();


} // namespace log4cplus

#endif // LOG4CPLUS_HIERARCHY_HEADER_

