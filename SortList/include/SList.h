#ifndef SLIST_H
#define SLIST_H

#include "SwapObject.h"

class SList
{
    public:
    SList();
    ~SList();
    public:
    class SList* FindData(SwapObject *pDest);
    void SetNext(SList *pNext);
    void SetData(SwapObject *pData);
    void InitSList(int nSize);
    void ReleaseSList();

    private:
    class SList *m_pNext;
    SwapObject *m_pData;

};

#endif // SLIST_H
