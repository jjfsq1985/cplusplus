#include "SwapObject.h"

SwapObject::SwapObject()
:m_pData(NULL)
,m_dataSize(0)
{
    //ctor
}

SwapObject::~SwapObject()
{
    //dtor
    if(this->m_pData != NULL)
    {
        delete[] this->m_pData;
        this->m_pData = NULL;
    }

}

SwapObject::SwapObject(const SwapObject& other)
{
    if(this->m_pData != NULL)
    {
        delete[] this->m_pData;
        this->m_pData = NULL;
    }
    //copy ctor
    if(other.m_dataSize > 0)
    {
        this->m_dataSize = other.m_dataSize;
        this->m_pData = new int [this->m_dataSize];
        for(int i=0; i< this->m_dataSize; i++)
            this->m_pData[i] = other.m_pData[i];
    }
}

SwapObject& SwapObject::operator=(const SwapObject& rhs)
{
    if (this == &rhs)
     return *this; // handle self assignment

    if(this->m_pData != NULL)
    {
        delete[] this->m_pData;
        this->m_pData = NULL;
    }
    //copy ctor
    if(rhs.m_dataSize > 0)
    {
        this->m_dataSize = rhs.m_dataSize;
        this->m_pData = new int [this->m_dataSize];
        for(int i=0; i< this->m_dataSize; i++)
            this->m_pData[i] = rhs.m_pData[i];
    }

    return *this;
}

void SwapObject::SwapContent(SwapObject& lhs, SwapObject& rhs)
{
    int *pDataTemp = lhs.m_pData;
    lhs.m_pData = rhs.m_pData;
    rhs.m_pData = pDataTemp;
    int nSizeTemp = lhs.m_dataSize;
    lhs.m_dataSize = rhs.m_dataSize;
    rhs.m_dataSize = nSizeTemp;
}

void SwapObject::SetData(int nSize, int *pData)
{
    if(nSize  <= 0)
        return;
    if(this->m_pData != NULL)
    {
        delete[] this->m_pData;
        this->m_pData = NULL;
    }
    m_dataSize = nSize;
    m_pData = new int[m_dataSize];
    for(int i=0; i< nSize; i++)
    {
        m_pData[i] = pData[i];
    }
}

string SwapObject::Print()
{
    string strData = "";
    char temp[64];
    sprintf(temp,"Size:%d;\n",m_dataSize);
    strData += temp;
    for(int i=0; i< m_dataSize; i++)
    {
        sprintf(temp,"Index--%d:%d\n",i,m_pData[i]);
        strData += temp;
    }

    return strData;

}
