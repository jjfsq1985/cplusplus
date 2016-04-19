// Sink.cpp: implementation of the COPCGroupEventSink class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "OPCGroupEventSink.h"

COPCGroupEventSink::COPCGroupEventSink()
{
 m_dwRefCount =0;
}

COPCGroupEventSink::~COPCGroupEventSink()
{

}

STDMETHODIMP COPCGroupEventSink::DataChange(DWORD dwTransid, DWORD  hGroup, LONG    hrMasterquality, LONG    hrMastererror,
                                                                                       DWORD      dwCount, DWORD* phClientItems, VARIANT*   pvValues,
                                                                                       WORD*      pwQualities, FILETIME*  pftTimeStamps, LONG*   pErrors)
{
    return 0;
}