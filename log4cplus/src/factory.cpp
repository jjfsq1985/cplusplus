// Module:  Log4CPLUS
// File:    factory.cpp


#include "log4cplus/factory.h"
#include "log4cplus/loggerfactory.h"
#include "log4cplus/loglog.h"
#include "log4cplus/property.h"
#include "log4cplus/consoleappender.h"
#include "log4cplus/fileappender.h"
#include "log4cplus/nullappender.h"
#include "log4cplus/customappender.h"


using namespace log4cplus;
    

BaseFactory::~BaseFactory() {}

AppenderFactory::AppenderFactory() {}

AppenderFactory::~AppenderFactory() {}

LayoutFactory::LayoutFactory() {}

LayoutFactory::~LayoutFactory() {}

FilterFactory::FilterFactory() {}

FilterFactory::~FilterFactory()	{}

LoggerFactory::~LoggerFactory()	{}



#define LOG4CPLUS_REG_PRODUCT(reg, name, Factory) \
	reg.put(																  \
	std::auto_ptr<Factory>(                                                   \
	new FactoryTempl<name, Factory>(#name)))

#define LOG4CPLUS_REG_APPENDER(reg, appendername)                             \
	LOG4CPLUS_REG_PRODUCT(reg, appendername, AppenderFactory) 

#define LOG4CPLUS_REG_LAYOUT(reg, layoutname)                                 \
	LOG4CPLUS_REG_PRODUCT(reg, layoutname, LayoutFactory)

#define LOG4CPLUS_REG_FILTER(reg, filtername)                                 \
	LOG4CPLUS_REG_PRODUCT(reg, filtername, FilterFactory)

void log4cplus::initializeFactoryRegistry()
{
    AppenderFactoryRegistry& reg = getAppenderFactoryRegistry();
    LOG4CPLUS_REG_APPENDER(reg, ConsoleAppender);
    LOG4CPLUS_REG_APPENDER(reg, NullAppender);
    LOG4CPLUS_REG_APPENDER(reg, FileAppender);
    LOG4CPLUS_REG_APPENDER(reg, RollingFileAppender);
    LOG4CPLUS_REG_APPENDER(reg, DailyRollingFileAppender);
	LOG4CPLUS_REG_APPENDER(reg, CustomAppender);


    LayoutFactoryRegistry& reg2 = getLayoutFactoryRegistry();
    LOG4CPLUS_REG_LAYOUT(reg2, SimpleLayout);
    LOG4CPLUS_REG_LAYOUT(reg2, PatternLayout);

    FilterFactoryRegistry& reg3 = getFilterFactoryRegistry();
    LOG4CPLUS_REG_FILTER(reg3, DenyAllFilter);
    LOG4CPLUS_REG_FILTER(reg3, LogLevelMatchFilter);
    LOG4CPLUS_REG_FILTER(reg3, LogLevelRangeFilter);
}

