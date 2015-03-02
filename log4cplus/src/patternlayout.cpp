// Module:  Log4CPLUS
// File:    patternlayout.cpp

#include <sstream>
#include <cstdlib>

#include "log4cplus/layout.h"
#include "log4cplus/loglog.h"
#include "log4cplus/timehelper.h"
#include "log4cplus/stringhelper.h"
#include "log4cplus/property.h"
#include "log4cplus/loggingevent.h"
#include "log4cplus/environment.h"


using namespace std;
using namespace log4cplus;

namespace log4cplus
{
	/**
	* This is used by PatternConverter class to inform them how to format
	* their output.
	*/
	struct FormattingInfo 
	{
		int minLen;
		std::size_t maxLen;
		bool leftAlign;
		FormattingInfo() 
		{
			reset(); 
		}

		void reset()
		{
			minLen = -1;
			maxLen = 0x7FFFFFFF;
			leftAlign = false;
		}
	};

	/**
	* This is the base class of all "Converter" classes that format a
	* field of InternalLoggingEvent objects.  In fact, the PatternLayout
	* class simply uses an array of PatternConverter objects to format
	* and append a logging loggingEvent.
	*/
	class PatternConverter
	{
	public:
		explicit PatternConverter(const FormattingInfo& info);
		virtual ~PatternConverter() {}
		void formatAndAppend(ostream& output, 
			const InternalLoggingEvent& loggingEvent);

		virtual void convert(string& result,
			const InternalLoggingEvent& loggingEvent) = 0;

	private:
		int _minLen;
		std::size_t _maxLen;
		bool _leftAlign;
	};
}

typedef vector<PatternConverter*> PatternConverterList;


static char const ESCAPE_CHAR = '%';

extern void formatRelativeTimestamp(ostream & output, log4cplus::InternalLoggingEvent const& loggingEvent);



/**
* This PatternConverter returns a constant string.
*/
class LiteralPatternConverter : public PatternConverter
{
public:
	LiteralPatternConverter(const string& str);
	virtual void convert(string& result,
		const InternalLoggingEvent&)
	{
		result = str;
	}

private:
	string str;
};


/**
* This PatternConverter is used to format most of the "simple" fields
* found in the InternalLoggingEvent object.
*/
class BasicPatternConverter: public PatternConverter
{
public:
	enum PatternConverterType 
	{
		PROCESS_CONVERTER,
		LOGLEVEL_CONVERTER,
		MESSAGE_CONVERTER,
		NEWLINE_CONVERTER,
	};
	BasicPatternConverter(const FormattingInfo& info, PatternConverterType type);
	virtual void convert(string& result, const InternalLoggingEvent& loggingEvent);

private:
	// Disable copy
	BasicPatternConverter(const BasicPatternConverter&);
	BasicPatternConverter& operator= (BasicPatternConverter&);

	LogLevelManager& _llmCache;
	PatternConverterType _ConverterType;
};



/**
* This PatternConverter is used to format the Logger field found in
* the InternalLoggingEvent object.
*/
class LoggerPatternConverter : public PatternConverter 
{
public:
	LoggerPatternConverter(const FormattingInfo& info, int precision);
	virtual void convert(string& result, const InternalLoggingEvent& loggingEvent);

private:
	int precision;
};


/**
* This PatternConverter is used to format the timestamp field found in
* the InternalLoggingEvent object.  It will be formatted according to
* the specified "pattern".
*/
class DatePatternConverter : public PatternConverter 
{
public:
	DatePatternConverter(const FormattingInfo& info, const string& pattern);
	virtual void convert(string& result, const InternalLoggingEvent& loggingEvent);

private:
	string _stringFormat;
};


/**
* This PatternConverter is used to format an environment variable
*/
class EnvPatternConverter : public PatternConverter
{
public:
	EnvPatternConverter(const FormattingInfo& info, const string& env);
	virtual void convert(string& result, const InternalLoggingEvent& loggingEvent);

private:
	string _envKey;
};


/**
* This class parses a "pattern" string into an array of
* PatternConverter objects.
* <p>
* @see PatternLayout for the formatting of the "pattern" string.
*/
class PatternParser
{
public:
	PatternParser(const string& pattern);
	vector<PatternConverter*> parse();

private:
	// Types
	enum ParserState 
	{ 
		LITERAL_STATE, 
		CONVERTER_STATE,
		DOT_STATE,
		MIN_STATE,
		MAX_STATE 
	};

	// Methods
	string extractOption();
	int extractPrecisionOption();
	void finalizeConverter(char c);

	// Data
	string _patternString;
	FormattingInfo _formattingInfo;
	vector<PatternConverter*> _patternConverterlist;
	ParserState _parserState;
	string::size_type _pos;
	string _currentLiteral;
};


////////////////////////////////////////////////
// PatternConverter methods:
////////////////////////////////////////////////
PatternConverter::PatternConverter(const FormattingInfo& info)
{
	_minLen = info.minLen;
	_maxLen = info.maxLen;
	_leftAlign = info.leftAlign;
}


void PatternConverter::formatAndAppend(ostream& output, const InternalLoggingEvent& loggingEvent)
{
	string str;
	convert(str, loggingEvent);
	std::size_t len = str.length();

	if(len > _maxLen)
		output << str.substr(len - _maxLen);
	else if(static_cast<int>(len) < _minLen)
	{
		std::ios_base::fmtflags const original_flags = output.flags();
		char const fill = output.fill(' ');
		output.setf(_leftAlign ? std::ios_base::left : std::ios_base::right,
			std::ios_base::adjustfield);
		output.width(_minLen);
		output << str;
		output.fill(fill);
		output.flags(original_flags);
	}
	else
		output << str;
}



////////////////////////////////////////////////
// LiteralPatternConverter methods:
////////////////////////////////////////////////

LiteralPatternConverter::LiteralPatternConverter(const string& str_)
	: PatternConverter(FormattingInfo()), str(str_)
{
}


////////////////////////////////////////////////
// BasicPatternConverter methods:
////////////////////////////////////////////////

BasicPatternConverter::BasicPatternConverter(const FormattingInfo& info, PatternConverterType type_)
	: PatternConverter(info), _llmCache(getLogLevelManager()), _ConverterType(type_)
{
}



void BasicPatternConverter::convert(string& result,
	const InternalLoggingEvent& loggingEvent)
{
	switch(_ConverterType)
	{
	case LOGLEVEL_CONVERTER:
		result = _llmCache.toString(loggingEvent.getLogLevel());
		return;

	case PROCESS_CONVERTER:
		{		
			int pid = 0; 
#ifdef _MSC_VER 
			pid = (int)GetCurrentProcessId();
#else
			pid = (int)getpid();
#endif

			convertIntegerToString(result, pid); 
		}
		return;

	case MESSAGE_CONVERTER:
		result = loggingEvent.getMessage();
		return;

	case NEWLINE_CONVERTER:
		result = "\n";
		return; 
	}

	result = "INTERNAL LOG4CPLUS ERROR";
}


////////////////////////////////////////////////
// LoggerPatternConverter methods:
////////////////////////////////////////////////

LoggerPatternConverter::LoggerPatternConverter(const FormattingInfo& info, int prec)
	: PatternConverter(info), precision(prec)
{}


void LoggerPatternConverter::convert(string& result, const InternalLoggingEvent& loggingEvent)
{
	const string& name = loggingEvent.getLoggerName();
	if(precision <= 0) {
		result = name;
	}
	else 
	{
		std::size_t len = name.length();

		// We substract 1 from 'len' when assigning to 'end' to avoid out of
		// bounds exception in return r.substring(end+1, len). This can happen
		// if precision is 1 and the logger name ends with a dot. 
		string::size_type end = len - 1;
		for(int i = precision; i > 0; --i)
		{
			end = name.rfind('.', end - 1);
			if(end == string::npos) 
			{
				result = name;
				return;
			}
		}
		result = name.substr(end + 1);
	}
}



////////////////////////////////////////////////
// DatePatternConverter methods:
////////////////////////////////////////////////


DatePatternConverter::DatePatternConverter(const FormattingInfo& info, const string& pattern)
	: PatternConverter(info), _stringFormat(pattern)
{
}


void DatePatternConverter::convert(string& result, const InternalLoggingEvent& loggingEvent)
{
	result = loggingEvent.getTimestamp().getFormattedTime(_stringFormat);
}


////////////////////////////////////////////////
// EnvPatternConverter methods:
////////////////////////////////////////////////
EnvPatternConverter::EnvPatternConverter(const FormattingInfo& info, const string& env)
	: PatternConverter(info), _envKey(env)
{}


void EnvPatternConverter::convert(string& result, const InternalLoggingEvent&)
{
	if(!getEnvString(result, _envKey))
	{
		// Variable doesn't exist, return empty string.
		result.clear();
	}
}


////////////////////////////////////////////////
// PatternParser methods:
////////////////////////////////////////////////

PatternParser::PatternParser(const string& pattern_)
	: _patternString(pattern_), _parserState(LITERAL_STATE), _pos(0)
{
}

string PatternParser::extractOption() 
{
	if((_pos < _patternString.length()) &&(_patternString[_pos] == '{')) 
	{
		string::size_type end = _patternString.find_first_of('}', _pos);
		if(end != string::npos) 
		{
			string r = _patternString.substr(_pos + 1, end - _pos - 1);
			_pos = end + 1;
			return r;
		}
		else 
		{
			ostringstream buf;
			buf << "No matching '}' found in conversion pattern string " << _patternString;
			LogLog::getLogLog()->error(buf.str());
			_pos = _patternString.length();
		}
	}

	return "";
}


int PatternParser::extractPrecisionOption() 
{
	string opt = extractOption();
	int r = 0;
	if(!opt.empty())
		r = std::atoi(opt.c_str());

	return r;
}



PatternConverterList PatternParser::parse() 
{
	char c;
	_pos = 0;
	while(_pos < _patternString.length()) 
	{
		c = _patternString[_pos++];
		switch(_parserState) 
		{
		case LITERAL_STATE :
			// In literal state, the last char is always a literal.
			if(_pos == _patternString.length()) 
			{
				_currentLiteral += c;
				continue;
			}
			if(c == ESCAPE_CHAR) {
				// peek at the next char. 
				switch(_patternString[_pos])
				{
				case ESCAPE_CHAR:
					_currentLiteral += c;
					_pos++; // move pointer
					break;
				default:
					if(!_currentLiteral.empty())
					{
						_patternConverterlist.push_back(new LiteralPatternConverter(_currentLiteral));
					}
					_currentLiteral.resize(0);
					_currentLiteral += c; // append %
					_parserState = CONVERTER_STATE;
					_formattingInfo.reset();
				}
			}
			else 
			{
				_currentLiteral += c;
			}
			break;

		case CONVERTER_STATE:
			_currentLiteral += c;
			switch(c) 
			{
			case '-':
				_formattingInfo.leftAlign = true;
				break;
			case '.':
				_parserState = DOT_STATE;
				break;
			default:
				if(c >= '0' && c <= '9') 
				{
					_formattingInfo.minLen = c - '0';
					_parserState = MIN_STATE;
				}
				else
				{
					finalizeConverter(c);
				}
			} // switch
			break;

		case MIN_STATE:
			_currentLiteral += c;
			if(c >= '0' && c <= '9')
			{
				_formattingInfo.minLen = _formattingInfo.minLen * 10 +(c - '0');
			}
			else if(c == '.') 
			{
				_parserState = DOT_STATE;
			}
			else 
			{
				finalizeConverter(c);
			}
			break;

		case DOT_STATE:
			_currentLiteral += c;
			if(c >= '0' && c <= '9')
			{
				_formattingInfo.maxLen = c - '0';
				_parserState = MAX_STATE;
			}
			else 
			{
				ostringstream buf;
				buf << "Error occured in position "
					<< _pos
					<< ".\n Was expecting digit, instead got char "
					<< c;
				LogLog::getLogLog()->error(buf.str());
				_parserState = LITERAL_STATE;
			}
			break;

		case MAX_STATE:
			_currentLiteral += c;
			if(c >= '0' && c <= '9')
				_formattingInfo.maxLen = _formattingInfo.maxLen * 10 +(c - '0');
			else {
				finalizeConverter(c);
				_parserState = LITERAL_STATE;
			}
			break;
		} // end switch
	} // end while

	if(!_currentLiteral.empty()) {
		_patternConverterlist.push_back(new LiteralPatternConverter(_currentLiteral));
	}

	return _patternConverterlist;
}



void PatternParser::finalizeConverter(char c) 
{
	PatternConverter* pc = 0;
	switch(c) 
	{
// 	case 'b':
// 		pc = new BasicPatternConverter(_formattingInfo, BasicPatternConverter::BASENAME_CONVERTER);      
// 		break;

	case 'c':
		pc = new LoggerPatternConverter(_formattingInfo, extractPrecisionOption());   
		break;

	case 'd':
	case 'D':
		{
			string dOpt = extractOption();
			if(dOpt.empty()) 
			{
				dOpt = "%Y-%m-%d %H:%M:%S";
			}
			pc = new DatePatternConverter(_formattingInfo, dOpt);      
		}
		break;

	case 'E':
		pc = new EnvPatternConverter(_formattingInfo, extractOption());     
		break;

// 	case 'F':
// 		pc = new BasicPatternConverter(_formattingInfo, BasicPatternConverter::FILE_CONVERTER);     
// 		break;

	case 'i':
		pc = new BasicPatternConverter(_formattingInfo, BasicPatternConverter::PROCESS_CONVERTER);   
		break;

// 	case 'l':
// 		pc = new BasicPatternConverter(_formattingInfo, BasicPatternConverter::FULL_LOCATION_CONVERTER);  
// 		break;
// 
// 	case 'L':
// 		pc = new BasicPatternConverter(_formattingInfo, BasicPatternConverter::LINE_CONVERTER);   
// 		break;

	case 'm':
		pc = new BasicPatternConverter(_formattingInfo, BasicPatternConverter::MESSAGE_CONVERTER);    
		break;

// 	case 'M':
// 		pc = new BasicPatternConverter(_formattingInfo, BasicPatternConverter::FUNCTION_CONVERTER);  
// 		break;

	case 'n':
		pc = new BasicPatternConverter(_formattingInfo, BasicPatternConverter::NEWLINE_CONVERTER);     
		break;

	case 'p':
		pc = new BasicPatternConverter(_formattingInfo, BasicPatternConverter::LOGLEVEL_CONVERTER);
		break;

	default:
		ostringstream buf;
		buf << "Unexpected char [" << c
			<< "] at position " << _pos
			<< " in conversion patterrn.";
		LogLog::getLogLog()->error(buf.str());
		pc = new LiteralPatternConverter(_currentLiteral);
	}

	_patternConverterlist.push_back(pc);
	_currentLiteral.resize(0);
	_parserState = LITERAL_STATE;
	_formattingInfo.reset();
}

typedef PatternConverterList PatternConverterList;


////////////////////////////////////////////////
// PatternLayout methods:
////////////////////////////////////////////////

PatternLayout::PatternLayout(const string& pattern_)
{
	init(pattern_);
}

PatternLayout::PatternLayout(const Properties& properties)
{
	bool isHasPattern = properties.exists("Pattern");
	bool isHasConversionPattern = properties.exists("ConversionPattern");

	if(isHasPattern) 
	{
		LogLog::getLogLog()->error("PatternLayout- the Pattern property has been"
			" deprecated.  Use ConversionPattern instead.");
	}

	if(isHasConversionPattern) 
	{
		init(properties.getProperty("ConversionPattern"));
	}
	else if(isHasPattern) 
	{
		init(properties.getProperty("Pattern"));
	}
	else
	{
		LogLog::getLogLog()->error("ConversionPattern not specified in properties", true);
	}

}

void PatternLayout::init(const string& pattern_)
{
	_pattern = pattern_;
	_parsedPattern = PatternParser(_pattern).parse();

	// Let's validate that our parser didn't give us any NULLs.  If it did,
	// we will convert them to a valid PatternConverter that does nothing so
	// at least we don't core.
	for(PatternConverterList::iterator it=_parsedPattern.begin(); it!=_parsedPattern.end(); ++it)
	{
		if((*it) == 0 ) 
		{
			LogLog::getLogLog()->error("Parsed Pattern created a NULL PatternConverter");
			(*it) = new LiteralPatternConverter("");
		}
	}
	if(_parsedPattern.empty()) 
	{
		LogLog::getLogLog()->error("PatternLayout pattern is empty.  Using default...");
		_parsedPattern.push_back(new BasicPatternConverter(FormattingInfo(), BasicPatternConverter::MESSAGE_CONVERTER));
	}
}


PatternLayout::~PatternLayout()
{
	for(PatternConverterList::iterator it=_parsedPattern.begin(); it!=_parsedPattern.end(); ++it)
	{
		delete(*it);
	}
}

void PatternLayout::formatAndAppend(ostream& output, const InternalLoggingEvent& loggingEvent)
{
	for(PatternConverterList::iterator it=_parsedPattern.begin(); it!=_parsedPattern.end(); ++it)
	{
		(*it)->formatAndAppend(output, loggingEvent);
	}
}

