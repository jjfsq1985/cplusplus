// Module:  Log4CPLUS
// File:    stringhelper.cpp

#include <iterator>
#include <algorithm>
#include <cstring>
#include <cctype>
#include <cassert>
#include <limits> 

#include "log4cplus/stringhelper.h"
#include "log4cplus/loggingevent.h"




using namespace std;
using namespace log4cplus;

char log4cplus::toUpper(char ch)
{
	return (char)std::toupper(ch);
}


string log4cplus::toUpper(const string& s)
{
	string ret;
	std::transform(s.begin(), s.end(), std::back_inserter(ret), (int(*)(int))std::toupper);
	return ret;
}


char log4cplus::toLower(char ch)
{
	return (char)std::tolower(ch);
}


string log4cplus::toLower(const string& s)
{
	string ret;
	std::transform(s.begin(), s.end(), std::back_inserter(ret), (int(*)(int))std::tolower);
	return ret;
}



void log4cplus::tokenize(const string& s, char c, OutputIter result, bool collapseTokens)
{
	size_t const slen = s.length();
	size_t first = 0;
	size_t i = 0;
	for(i=0; i < slen; ++i)
	{
		if(s[i] == c)
		{
			*result = string(s, first, i - first);
			++result;
			if(collapseTokens)
				while(i+1 < slen && s[i+1] == c)
					++i;
			first = i + 1;
		}
	}
	if(first != i)
		*result = string(s, first, i - first);
}

void log4cplus::convertIntegerToString(string & str, int intValue)
{
	typedef std::numeric_limits<int> intTypeLimits;

	char buffer[intTypeLimits::digits10 + 2];
	// We define buffer_size from buffer using sizeof operator
	// to apease HP aCC compiler.
	const std::size_t buffer_size = sizeof(buffer) / sizeof(char);

	char * it = &buffer[buffer_size];
	char const* const buf_end = &buffer[buffer_size];

	if(intValue == 0)
	{
		--it;
		*it = '0';
	}
	else
	{
		bool const isNegative = intValue < 0;

		for(; intValue != 0; --it)
		{
			int mod = intValue % 10;
			intValue = intValue / 10;
			*(it - 1) = static_cast<char>('0' + mod);
		}

		if(isNegative)
		{
			--it;
			*it = '-';
		}
	}

	str.assign(static_cast<char const*>(it), buf_end);
}

string log4cplus::convertIntegerToString(int intValue)
{
	string result;
	convertIntegerToString(result, intValue);
	return result;
}

void log4cplus::join(string& result, Iterator start, Iterator last, string const& sep)
{
	if(start != last)
		result = *start++;

	for(; start != last; ++start)
	{
		result += sep;
		result += *start;
	}
}