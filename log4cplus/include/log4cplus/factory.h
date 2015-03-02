
// Module:  Log4CPLUS
// File:    factory.h

#ifndef LOG4CPLUS_SPI_FACTORY_HEADER_
#define LOG4CPLUS_SPI_FACTORY_HEADER_

#include "log4cplus/platform.h"
#include "log4cplus/appender.h"
#include "log4cplus/layout.h"
#include "log4cplus/filter.h"
#include "log4cplus/objectregistry.h"

#include <memory>
#include <vector>


namespace log4cplus {


void initializeFactoryRegistry();


/**
* This is the base class for all factories.
*/
class BaseFactory 
{
public:
	virtual ~BaseFactory() = 0;

	/**
	* Returns the typename of the objects this factory creates.
	*/
	virtual std::string const& getTypeName() const = 0;
};


/**
* This abstract class defines the "Factory" interface to create "Appender"
* objects.
*/
class AppenderFactory : public BaseFactory 
{
public:
	typedef Appender ProductType;
	typedef SharedAppenderPtr ProductPtr;

	AppenderFactory();
	virtual ~AppenderFactory() = 0;

	/**
	* Create an "Appender" object.
	*/
	virtual SharedAppenderPtr createObject(const Properties& props) = 0;
};



/**
* This abstract class defines the "Factory" interface to create "Layout"
* objects.
*/
class LayoutFactory : public BaseFactory 
{
public:
	typedef Layout ProductType;
	typedef std::auto_ptr<Layout> ProductPtr;

	LayoutFactory();
	virtual ~LayoutFactory() = 0;

	/**
	* Create a "Layout" object.
	*/
	virtual std::auto_ptr<Layout> createObject(const Properties& props) = 0;
};



/**
* This abstract class defines the "Factory" interface to create "Appender"
* objects.
*/
class FilterFactory : public BaseFactory 
{
public:
	typedef Filter ProductType;
	typedef FilterPtr ProductPtr;

	FilterFactory();
	virtual ~FilterFactory() = 0;

	/**
	* Create a "Filter" object.
	*/
	virtual FilterPtr createObject(const Properties& props) = 0;
};


template<class T>
class FactoryRegistry : public ObjectRegistryBase
{
public:

	virtual ~FactoryRegistry() 
	{
		clear();
	}

	// public methods
	/**
	* Used to enter an object into the registry. (The registry now
	*  owns <code>object</code>.)
	*/
	bool put(std::auto_ptr<T> object) 
	{
		bool putValResult = putVal(object->getTypeName(), object.get());
		object.release();
		return putValResult; 
	}

	/**
	* Used to retrieve an object from the registry. (The registry
	* owns the returned pointer.)
	*/
	T* get(const std::string& name) const 
	{
		return static_cast<T*>(getVal(name));
	}

protected:
	virtual void deleteObject(void *object) const 
	{
		delete static_cast<T*>(object);
	}
};


typedef FactoryRegistry<AppenderFactory> AppenderFactoryRegistry;
typedef FactoryRegistry<LayoutFactory> LayoutFactoryRegistry;
typedef FactoryRegistry<FilterFactory> FilterFactoryRegistry;


/**
* Returns the "singleton" <code>AppenderFactoryRegistry</code>.
*/
AppenderFactoryRegistry& getAppenderFactoryRegistry();

/**
* Returns the "singleton" <code>LayoutFactoryRegistry</code>.
*/
LayoutFactoryRegistry& getLayoutFactoryRegistry();

/**
* Returns the "singleton" <code>FilterFactoryRegistry</code>.
*/
FilterFactoryRegistry& getFilterFactoryRegistry();

template <typename ProductFactoryBase>
class LocalFactoryBase : public ProductFactoryBase
{
public:
	LocalFactoryBase(char const* name) : _typeName(name){}

	virtual std::string const& getTypeName() const
	{
		return _typeName;
	}

private:
	std::string _typeName;
};


template <typename LocalProduct, typename ProductFactoryBase>
class FactoryTempl : public LocalFactoryBase<ProductFactoryBase>
{
public:
	typedef typename ProductFactoryBase::ProductPtr ProductPtr;

	FactoryTempl(char const* name) : LocalFactoryBase<ProductFactoryBase>(name) {}

	virtual ProductPtr createObject(Properties const& props)
	{
		return ProductPtr(new LocalProduct(props));
	}
};


}	// namespace log4cplus


#endif // LOG4CPLUS_SPI_FACTORY_HEADER_
