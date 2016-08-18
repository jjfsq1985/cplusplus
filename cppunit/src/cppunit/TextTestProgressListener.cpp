#include <cppunit/TestFailure.h>
#include <cppunit/listener/TextTestProgressListener.h>
#include <cppunit/tools/Stream.h>


CPPUNIT_NS_BEGIN


TextTestProgressListener::TextTestProgressListener()
{
}


TextTestProgressListener::~TextTestProgressListener()
{
}


void 
TextTestProgressListener::startTest( Test *test )
{
  stdCOut() << ".";
}


void 
TextTestProgressListener::addFailure( const TestFailure &failure )
{
  stdCOut() << ( failure.isError() ? "E" : "F" );
}


void 
TextTestProgressListener::endTestRun( Test *test, 
                                      TestResult *eventManager )
{
  stdCOut()  <<  "\n";
  stdCOut().flush();
}


CPPUNIT_NS_END

