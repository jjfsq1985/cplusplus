
// Module:  Log4CPLUS
// File:    configurator.h

#ifndef LOG4CPLUS_CONFIGURATOR_HEADER_
#define LOG4CPLUS_CONFIGURATOR_HEADER_


#include "log4cplus/platform.h"
#include "log4cplus/appender.h"
#include "log4cplus/logger.h"
#include "log4cplus/sharedptr.h"
#include "log4cplus/property.h"

#include <map>
#include <vector>
#include <string>


namespace log4cplus
{


class Hierarchy;


/**
    * Provides configuration from an external file.  See configure() for
    * the expected format.
    */
class LOG4CPLUS_EXPORT PropertyConfigurator
{
public:     
    PropertyConfigurator(const std::string& propertyFile, Hierarchy& h = Logger::getDefaultHierarchy());
    PropertyConfigurator(const Properties& props, Hierarchy& h = Logger::getDefaultHierarchy());
    PropertyConfigurator(std::istream& propertyStream, Hierarchy& h = Logger::getDefaultHierarchy());
    virtual ~PropertyConfigurator();

    /**
        * This method eliminates the need to create a temporary
        * <code>PropertyConfigurator</code> to configure log4cplus.
        */
    static void doConfigure(const std::string& configFilename, Hierarchy& h = Logger::getDefaultHierarchy());

    /**
        * Read configuration from a file. <b>The existing configuration is
        * not cleared nor reset.</b> If you require a different behavior,
        * then call {@link Hierarchy::resetConfiguration
        * resetConfiguration} method before calling
        * <code>doConfigure</code>.
        */
    virtual void configure();
    Properties const& getProperties() const;
    std::string const& getPropertyFilename() const;
	static std::vector<std::string>& getLoggerNames() { return _loggerNames; }

protected:
    void init();  // called by the ctor
    void configureLoggers();
    void configureLogger(Logger logger, const std::string& config);
    void configureAppenders();
        
    virtual Logger getLogger(const std::string& name);
    virtual void addAppender(Logger &logger, SharedAppenderPtr& appender);

    // Types
    typedef std::map<std::string, SharedAppenderPtr> AppenderMap;

      
    Hierarchy& _hierarchy;
    std::string _propertyFilename;
    Properties _properties; 
    AppenderMap _appenders;
	static std::vector<std::string> _loggerNames;
        
private:
    // Disable copy
    PropertyConfigurator(const PropertyConfigurator&);
    PropertyConfigurator& operator=(PropertyConfigurator&);
};



/**
    * Use this class to quickly configure the package. For file based
    * configuration see PropertyConfigurator. BasicConfigurator
    * automatically attaches ConsoleAppender to
    * <code>rootLogger</code>, with output going to standard output,
    * using DEBUG LogLevel value. The additional parameter
    * logToStdErr may redirect the output to standard error.
    */
class LOG4CPLUS_EXPORT BasicConfigurator : public PropertyConfigurator 
{
public:
       
    BasicConfigurator(Hierarchy& h = Logger::getDefaultHierarchy(), bool logToStdErr = false);
    virtual ~BasicConfigurator();

    /**
        * This method eliminates the need to create a temporary
        * <code>BasicConfigurator</code> object to configure log4cplus.
        * It is equivalent to the following:<br>
        * <code><pre>
        * BasicConfigurator config;
        * config.configure();
        * </pre></code>
        */
    static void doConfigure(Hierarchy& h = Logger::getDefaultHierarchy(), bool logToStdErr = false);
        
private:
    // Disable copy
    BasicConfigurator(const BasicConfigurator&);
    BasicConfigurator& operator=(BasicConfigurator&);
};
   

} // namespace log4cplus


#endif // LOG4CPLUS_CONFIGURATOR_HEADER_

