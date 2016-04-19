// Sink.h: interface for the COPCGroupEventSink class.
//
#pragma once

#include "OPCAuto.h"

class COPCGroupEventSink : public DIOPCGroupEvent
{
private:
    DWORD       m_dwRefCount;
public:
    COPCGroupEventSink();
    virtual ~COPCGroupEventSink();
    STDMETHODIMP DataChange(DWORD dwTransid, DWORD  hGroup,LONG    hrMasterquality, LONG    hrMastererror,
                                                            DWORD dwCount, DWORD* phClientItems, VARIANT*   pvValues,
                                                            WORD*      pwQualities, FILETIME*  pftTimeStamps, LONG*   pErrors);

	HRESULT STDMETHODCALLTYPE QueryInterface(REFIID iid, void **ppvObject)
    {
        if (iid == IID_IUnknown)
        {
            m_dwRefCount++;
            *ppvObject = (void *)this;
            return S_OK;
        }
        else if (iid == DIID_DIOPCGroupEvent)
        {
            m_dwRefCount++;
            *ppvObject = (void *)this;
            return S_OK;
        }



        return E_NOINTERFACE;
    }
	ULONG STDMETHODCALLTYPE AddRef()
    {
        m_dwRefCount++;
        return m_dwRefCount;
    }
    
	ULONG STDMETHODCALLTYPE Release()
    {
        ULONG l;
        
        l  = m_dwRefCount--;

        if ( 0 == m_dwRefCount)
        {
            delete this;
        }
        
        return l;
    }

};

