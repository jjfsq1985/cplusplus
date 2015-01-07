
#ifndef _LOG4CPLUS_WAPPER_INCLUDE_
#define _LOG4CPLUS_WAPPER_INCLUDE_

namespace log4cplusWapper
{
	void  StartLogSystem(const char* properties_filename);

	void  StopLogSystem();

	bool tryInitLoggerInstance(const char* szModuleName);

	void PrintDebug(const char* loggerName, const char* pszFormat, ...);

	void PrintInfo(const char* loggerName, const char* pszFormat, ...);

	void PrintError(const char* loggerName, const char* pszFormat, ...);

	void PrintFatal(const char* loggerName, const char* pszFormat, ...);
}

#endif	//end _LOG4CPLUS_WAPPER_INCLUDE_
