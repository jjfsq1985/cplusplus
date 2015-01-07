
// Module:  Log4CPLUS
// File:    property.h

#ifndef LOG4CPLUS_HELPERS_PROPERTY_HEADER_
#define LOG4CPLUS_HELPERS_PROPERTY_HEADER_


#include "log4cplus/platform.h"

#include <map>
#include <vector>


namespace log4cplus { 


//!\sa log4cplus::PropertyConfigurator
class LOG4CPLUS_EXPORT Properties 
{
public:
	Properties();
	explicit Properties(std::istream& input);
	explicit Properties(const std::string& inputFile);
	virtual ~Properties();

	// constants
	static const char PROPERTIES_COMMENT_CHAR;

		
	/**
	* Tests to see if <code>key</code> can be found in this map.
	*/
	bool exists(const std::string& key) const;
	bool exists(char const* key) const;

	/**
	* Returns the number of entries in this map.
	*/
	std::size_t size() const
	{
		return _stringMap.size();
	}

	/**
	* Searches for the property with the specified key in this property
	* list. If the key is not found in this property list, the default
	* property list, and its defaults, recursively, are then checked. 
	* The method returns <code>null</code> if the property is not found.
	*/
	std::string const& getProperty(const std::string& key) const;
	std::string const& getProperty(char const* key) const;

	/**
	* Searches for the property with the specified key in this property
	* list. If the key is not found in this property list, the default
	* property list, and its defaults, recursively, are then checked. 
	* The method returns the default value argument if the property is 
	* not found.
	*/
	std::string getProperty(const std::string& key, const std::string& defaultVal) const;

	/**
	* Returns all the keys in this property list.
	*/
	std::vector<std::string> propertyNames() const;

	/**
	* Inserts <code>value</code> into this map indexed by <code>key</code>.
	*/
	void setProperty(const std::string& key, const std::string& value);

	/**
	* Removed the property index by <code>key</code> from this map.
	*/
	bool removeProperty(const std::string& key);

	/**
	* Returns a subset of the "properties" whose keys start with
	* "prefix".  The returned "properties" have "prefix" trimmed from
	* their keys.
	*/
	Properties getPropertySubset(const std::string& prefix) const;

	bool getInt(int& val, std::string const& key) const;
	bool getUInt(unsigned& val, std::string const& key) const;
	bool getLong(long& val, std::string const& key) const;
	bool getULong(unsigned long& val, std::string const& key) const;
	bool getBool(bool& val, std::string const& key) const;

protected:
	// Types
	typedef std::map<std::string, std::string> StringMap;

	void init(std::istream& input);
		
	StringMap _stringMap;

private:
	std::string const& getProperty2(std::string const& key) const;

	template <typename ValType>
	bool getValType(ValType& val, std::string const& key) const;
};

} // namespace log4cplus


#endif // LOG4CPLUS_HELPERS_PROPERTY_HEADER_

