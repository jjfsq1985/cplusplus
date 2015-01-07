
// Module:  Log4CPLUS
// File:    stringhelper.h

#ifndef LOG4CPLUS_HELPERS_STRINGHELPER_HEADER_
#define LOG4CPLUS_HELPERS_STRINGHELPER_HEADER_

#include "log4cplus/platform.h"

#include <algorithm>
#include <vector>
#include <iterator>

namespace log4cplus { 
	/**
	* Returns <code>s</code> in upper case.
	*/
	std::string toUpper(const std::string& s);
	char toUpper(char);

	/**
	* Returns <code>s</code> in lower case.
	*/
	std::string toLower(const std::string& s);
	char toLower(char);

	
	typedef std::back_insert_iterator<std::vector<std::string> > OutputIter;

	void tokenize(const std::string& s, char c, OutputIter result, bool collapseTokens = true);
	
	void convertIntegerToString(std::string & str, int intValue);
	
	std::string convertIntegerToString(int intValue);

	//!Join a list of items into a string.
	typedef std::vector<std::string, std::allocator<std::string> >::iterator Iterator;

	void join(std::string& result, Iterator start, Iterator last, std::string const& sep);

} // namespace log4cplus 

#endif // LOG4CPLUS_HELPERS_STRINGHELPER_HEADER_
