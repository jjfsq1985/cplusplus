#ifndef SWAPOBJECT_H
#define SWAPOBJECT_H
#include <string>
using namespace std;

#include <stddef.h>
#include <stdio.h>

class SwapObject
{
    public:
        SwapObject();
        virtual ~SwapObject();
        SwapObject(const SwapObject& other);
        SwapObject& operator=(const SwapObject& other);

    public:
    static void SwapContent(SwapObject& lhs, SwapObject& rhs);
    string Print();
    void SetData(int nSize, int *pData);

    protected:

    private:
        int *m_pData;
        int m_dataSize;
};

#endif // SWAPOBJECT_H
