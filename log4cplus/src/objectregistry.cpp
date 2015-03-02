// Module:  Log4CPLUS
// File:    objectregistry.cpp


#include "log4cplus/objectregistry.h"

using namespace std;
using namespace log4cplus;


ObjectRegistryBase::ObjectRegistryBase() {}

ObjectRegistryBase::~ObjectRegistryBase() {}


bool ObjectRegistryBase::exists(const string& name) const
{
	MutexLock lock(&const_cast<Mutex&>(_mutex));

	return _objectMap.find(name) != _objectMap.end();
}


vector<string> ObjectRegistryBase::getAllNames() const
{
	vector<string> tmp;

	MutexLock lock(&const_cast<Mutex&>(_mutex));
	for(ObjectMap::const_iterator it=_objectMap.begin(); it!=_objectMap.end(); ++it)
		tmp.push_back( (*it).first );

	return tmp;
}


///////////////////////////////////////////////////////////////////////////////
// ObjectRegistryBase protected methods
///////////////////////////////////////////////////////////////////////////////

bool ObjectRegistryBase::putVal(const string& name, void* object)
{
	ObjectMap::value_type v(name, object);
	std::pair<ObjectMap::iterator, bool> ret;

	MutexLock lock(&_mutex);
	ret = _objectMap.insert(v);

	if (!ret.second)
		deleteObject(v.second);
	return ret.second;
}


void* ObjectRegistryBase::getVal(const string& name) const
{
	MutexLock lock(&const_cast<Mutex&>(_mutex));

	ObjectMap::const_iterator it (_objectMap.find (name));
	if (it != _objectMap.end ())
		return it->second;
	else
		return 0;
}


void ObjectRegistryBase::clear()
{
	MutexLock lock(&_mutex);

	for(ObjectMap::iterator it=_objectMap.begin(); it!=_objectMap.end(); ++it)
		deleteObject( it->second );
}


