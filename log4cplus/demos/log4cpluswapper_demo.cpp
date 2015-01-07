

#include "log4cplusWapper.h"

#include <iostream>
#include <exception>

using namespace log4cplusWapper;


#ifdef _WIN32
#define PTESTINFO(fmt, ...)\
	log4cplusWapper::PrintInfo(moduleName1, fmt, __VA_ARGS__);
#else   //__linux__
#define PTESTINFO(fmt, args...)\
	log4cplusWapper::PrintInfo(moduleName1, fmt, ##args);
#endif


const char* moduleName1 = "logFile1";	//exists
const char* moduleName2 = "logFile2";	//exists
const char* moduleName3 = "logFile3";	//not exists


int main()
{
	try
	{
		StartLogSystem("urconfig.properties");
	}
	catch (std::exception& e)
	{
		std::cout << e.what() << std::endl;
		return 0;	// start failed
	}

	try
	{
		tryInitLoggerInstance(moduleName1);
		tryInitLoggerInstance(moduleName2);
		tryInitLoggerInstance(moduleName3);
	}
	catch (std::exception& e)
	{
		std::cout << e.what() << std::endl;
	}

	//use moduleName1 print log message
	for (int index = 0; index < 10; index++)
	{
		PrintDebug(moduleName1, "moduleName1 Debug[%d]", index);
		PrintInfo(moduleName1, "moduleName1 Info[%d]", index);
		PrintError(moduleName1, "moduleName1 Error[%d]", index);
		PrintFatal(moduleName1, "moduleName1 Fatal[%d]", index);
		std::cout<<"=========================="<<std::endl;
	
	}

	//use moduleName2 print log message
	for (int index = 0; index < 10; index++)
	{
		PrintDebug(moduleName2, "moduleName2 Debug[%d]", index);
		PrintInfo(moduleName2, "moduleName2 Info[%d]", index);
		PrintError(moduleName2, "moduleName2 Error[%d]", index);
		PrintFatal(moduleName2, "moduleName2 Fatal[%d]", index);
		std::cout<<"==========================="<<std::endl;
	}

	try
	{
		PrintInfo(moduleName3, "moduleName3 should throw");
	}
	catch (std::exception& e)
	{
		std::cout<<"Exception: "<<e.what()<<std::endl;
	}

	PTESTINFO("test111");
	PTESTINFO("test %s %d", "222", 333);
}