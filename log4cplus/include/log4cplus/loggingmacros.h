
// Module:  Log4CPLUS
// File:    loggingmacros.h

#ifndef LOG4CPLUS_LOGGING_MACROS_HEADER_
#define LOG4CPLUS_LOGGING_MACROS_HEADER_

#include "log4cplus/platform.h"
#include "log4cplus/logger.h"

#include <sstream>
#include <utility>

#ifdef _MSC_VER
#define LOG4CPLUS_SUPPRESS_DOWHILE_WARNING()  \
    __pragma(warning(push))                 \
    __pragma(warning(disable:4127))           

#define LOG4CPLUS_RESTORE_DOWHILE_WARNING()   \
    __pragma(warning(pop))

#else
#define LOG4CPLUS_SUPPRESS_DOWHILE_WARNING() /* empty */
#define LOG4CPLUS_RESTORE_DOWHILE_WARNING() /* empty */

#endif

#define LOG4CPLUS_DOWHILE_NOTHING()                 \
    LOG4CPLUS_SUPPRESS_DOWHILE_WARNING()            \
    do {} while(0)                                \
    LOG4CPLUS_RESTORE_DOWHILE_WARNING()

#if defined(LOG4CPLUS_DISABLE_FATAL) && !defined(LOG4CPLUS_DISABLE_ERROR)
#define LOG4CPLUS_DISABLE_ERROR
#endif
#if defined(LOG4CPLUS_DISABLE_ERROR) && !defined(LOG4CPLUS_DISABLE_INFO)
#define LOG4CPLUS_DISABLE_INFO
#endif
#if defined(LOG4CPLUS_DISABLE_INFO) && !defined(LOG4CPLUS_DISABLE_DEBUG)
#define LOG4CPLUS_DISABLE_DEBUG
#endif


namespace log4cplus
{
	
	
inline Logger macros_getLogger(Logger const& logger)
{
    return logger;
}


inline Logger const& macros_getLogger(Logger & logger)
{
    return logger;
}


inline Logger macros_getLogger(std::string const& logger)
{
    return Logger::getInstance(logger);
}


inline Logger macros_getLogger(char const* logger)
{
    return Logger::getInstance(logger);
}


LOG4CPLUS_EXPORT void macro_forcedLog(Logger const&, LogLevel, std::string const&);


} // namespace log4cplus


#define LOG4CPLUS_MACRO_STR_BODY(logger, logEvent, logLevel)            \
    LOG4CPLUS_SUPPRESS_DOWHILE_WARNING()                                \
    do {                                                                \
        Logger const& constLogger = macros_getLogger(logger);			\
        if(constLogger.isEnabledFor(logLevel)) {						\
            macro_forcedLog(constLogger, logLevel, logEvent);			\
        }                                                               \
    } while(0)                                                          \
    LOG4CPLUS_RESTORE_DOWHILE_WARNING()


/**
 * @def LOG4CPLUS_DEBUG(logger, logEvent)  This macro is used to log a
 * DEBUG_LOG_LEVEL message to <code>logger</code>.
 * <code>logEvent</code> will be streamed into an <code>ostream</code>.
 */
#if !defined(LOG4CPLUS_DISABLE_DEBUG)
#define LOG4CPLUS_DEBUG(logger, logEvent)                               \
    LOG4CPLUS_MACRO_STR_BODY(logger, logEvent, DEBUG_LOG_LEVEL)

#else
#define LOG4CPLUS_DEBUG(logger, logEvent) LOG4CPLUS_DOWHILE_NOTHING()

#endif

/**
 * @def LOG4CPLUS_INFO(logger, logEvent)  This macro is used to log a
 * INFO_LOG_LEVEL message to <code>logger</code>.
 * <code>logEvent</code> will be streamed into an <code>ostream</code>.
 */
#if !defined(LOG4CPLUS_DISABLE_INFO)
#define LOG4CPLUS_INFO(logger, logEvent)                                \
    LOG4CPLUS_MACRO_STR_BODY(logger, logEvent, INFO_LOG_LEVEL)

#else
#define LOG4CPLUS_INFO(logger, logEvent) LOG4CPLUS_DOWHILE_NOTHING()

#endif


/**
 * @def LOG4CPLUS_ERROR(logger, logEvent)  This macro is used to log a
 * ERROR_LOG_LEVEL message to <code>logger</code>.
 * <code>logEvent</code> will be streamed into an <code>ostream</code>.
 */
#if !defined(LOG4CPLUS_DISABLE_ERROR)
#define LOG4CPLUS_ERROR(logger, logEvent)                               \
    LOG4CPLUS_MACRO_STR_BODY(logger, logEvent, ERROR_LOG_LEVEL)
#define LOG4CPLUS_ERROR_STR(logger, logEvent)                           \
    LOG4CPLUS_MACRO_STR_BODY(logger, logEvent, ERROR_LOG_LEVEL)

#else
#define LOG4CPLUS_ERROR(logger, logEvent) LOG4CPLUS_DOWHILE_NOTHING()
#define LOG4CPLUS_ERROR_STR(logger, logEvent) LOG4CPLUS_DOWHILE_NOTHING()

#endif

/**
 * @def LOG4CPLUS_FATAL(logger, logEvent)  This macro is used to log a
 * FATAL_LOG_LEVEL message to <code>logger</code>.
 * <code>logEvent</code> will be streamed into an <code>ostream</code>.
 */
#if !defined(LOG4CPLUS_DISABLE_FATAL)
#define LOG4CPLUS_FATAL(logger, logEvent)                               \
    LOG4CPLUS_MACRO_STR_BODY(logger, logEvent, FATAL_LOG_LEVEL)

#else
#define LOG4CPLUS_FATAL(logger, logEvent) LOG4CPLUS_DOWHILE_NOTHING()
#endif


//!If the condition given in second parameter evaluates false, this
//!macro logs it using FATAL log level, including the condition's
//!source text.
#define LOG4CPLUS_ASSERT(logger, condition)                             \
    LOG4CPLUS_SUPPRESS_DOWHILE_WARNING()                                \
    do {                                                                \
        if(!!(!(condition)))											\
            LOG4CPLUS_FATAL_STR((logger), "failed condition: "#condition)); \
    } while(0)                                                         \
    LOG4CPLUS_RESTORE_DOWHILE_WARNING()


#endif /* LOG4CPLUS_LOGGING_MACROS_HEADER_ */
