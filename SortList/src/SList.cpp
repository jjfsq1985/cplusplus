#include "SList.h"

SList::SList()
:m_pNext(NULL)
,m_pData(NULL)
{
}

SList::~SList()
{
    //dtor
    if(m_pData != NULL)
        delete m_pData;
    m_pData = NULL;
}

void SList::InitSList(int nSize)
{
    if(nSize <= 0)
        return;
    SList *pIt = this;
    while(--nSize > 0)
    {
        SList *pAlloc = new SList();
        pIt->SetNext(pAlloc);
        pIt = pAlloc;
    }
}

void SList::ReleaseSList()
{
    SList *pIt = m_pNext;
    SList *pNext = NULL;
    while(pIt != NULL)
    {
        pNext = pIt->m_pNext;
        delete pIt;
        pIt = pNext;
    }
}

void SList::SetNext(SList *pNext)
{
    if(pNext == this)
        return;
     m_pNext = pNext;
}

void SList::SetData(SwapObject *pData)
{
    if(pData == NULL)
        return;
    m_pData = pData;
}

SList* SList::FindData(SwapObject *pDest)
{
    SList *pIt = this;
    do
    {
        if(pIt->m_pData == pDest)
        {
            break;
        }
        else
        {
            pIt = m_pNext;
        }
    }
    while(pIt != NULL);

    return pIt;
}
