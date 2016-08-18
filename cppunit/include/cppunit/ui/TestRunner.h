#ifndef CPPUNITUI_MFC_TESTRUNNER_H
#define CPPUNITUI_MFC_TESTRUNNER_H

#include <cppunit/ui/TextTestRunner.h>

CPPUNIT_NS_BEGIN

#if defined(CPPUNIT_HAVE_NAMESPACES)
namespace MfcUi
{
  /*! Mfc TestRunner (DEPRECATED).
   * \deprecated Use CppUnit::MfcTestRunner instead.
   */
  typedef CPPUNIT_NS::MfcTestRunner TestRunner;
}

namespace TextUi
{
    /*! Text TestRunner (DEPRECATED).
    * \deprecated Use TextTestRunner instead.
    */
    typedef CPPUNIT_NS::TextTestRunner TestRunner;

}

namespace QtUi
{
    /*! Qt TestRunner (DEPRECATED).
    * \deprecated Use CppUnit::QtTestRunner instead.
    */
    typedef CPPUNIT_NS::QtTestRunner TestRunner;
}

#endif // defined(CPPUNIT_HAVE_NAMESPACES)

CPPUNIT_NS_END


#endif  // CPPUNITUI_MFC_TESTRUNNER_H
