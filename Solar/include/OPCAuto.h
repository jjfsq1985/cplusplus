

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.00.0603 */
/* at Thu Apr 14 15:22:34 2016
 */
/* Compiler settings for OPCAuto.idl:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 8.00.0603 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __OPCAuto_h__
#define __OPCAuto_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __IOPCServerEvent_FWD_DEFINED__
#define __IOPCServerEvent_FWD_DEFINED__
typedef interface IOPCServerEvent IOPCServerEvent;

#endif 	/* __IOPCServerEvent_FWD_DEFINED__ */


#ifndef __IOPCGroupsEvent_FWD_DEFINED__
#define __IOPCGroupsEvent_FWD_DEFINED__
typedef interface IOPCGroupsEvent IOPCGroupsEvent;

#endif 	/* __IOPCGroupsEvent_FWD_DEFINED__ */


#ifndef __IOPCGroupEvent_FWD_DEFINED__
#define __IOPCGroupEvent_FWD_DEFINED__
typedef interface IOPCGroupEvent IOPCGroupEvent;

#endif 	/* __IOPCGroupEvent_FWD_DEFINED__ */


#ifndef __IOPCAutoServer_FWD_DEFINED__
#define __IOPCAutoServer_FWD_DEFINED__
typedef interface IOPCAutoServer IOPCAutoServer;

#endif 	/* __IOPCAutoServer_FWD_DEFINED__ */


#ifndef __DIOPCServerEvent_FWD_DEFINED__
#define __DIOPCServerEvent_FWD_DEFINED__
typedef interface DIOPCServerEvent DIOPCServerEvent;

#endif 	/* __DIOPCServerEvent_FWD_DEFINED__ */


#ifndef __OPCBrowser_FWD_DEFINED__
#define __OPCBrowser_FWD_DEFINED__
typedef interface OPCBrowser OPCBrowser;

#endif 	/* __OPCBrowser_FWD_DEFINED__ */


#ifndef __IOPCGroups_FWD_DEFINED__
#define __IOPCGroups_FWD_DEFINED__
typedef interface IOPCGroups IOPCGroups;

#endif 	/* __IOPCGroups_FWD_DEFINED__ */


#ifndef __DIOPCGroupsEvent_FWD_DEFINED__
#define __DIOPCGroupsEvent_FWD_DEFINED__
typedef interface DIOPCGroupsEvent DIOPCGroupsEvent;

#endif 	/* __DIOPCGroupsEvent_FWD_DEFINED__ */


#ifndef __IOPCGroup_FWD_DEFINED__
#define __IOPCGroup_FWD_DEFINED__
typedef interface IOPCGroup IOPCGroup;

#endif 	/* __IOPCGroup_FWD_DEFINED__ */


#ifndef __DIOPCGroupEvent_FWD_DEFINED__
#define __DIOPCGroupEvent_FWD_DEFINED__
typedef interface DIOPCGroupEvent DIOPCGroupEvent;

#endif 	/* __DIOPCGroupEvent_FWD_DEFINED__ */


#ifndef __OPCItems_FWD_DEFINED__
#define __OPCItems_FWD_DEFINED__
typedef interface OPCItems OPCItems;

#endif 	/* __OPCItems_FWD_DEFINED__ */


#ifndef __OPCItem_FWD_DEFINED__
#define __OPCItem_FWD_DEFINED__
typedef interface OPCItem OPCItem;

#endif 	/* __OPCItem_FWD_DEFINED__ */


#ifndef __IOPCActivator_FWD_DEFINED__
#define __IOPCActivator_FWD_DEFINED__
typedef interface IOPCActivator IOPCActivator;

#endif 	/* __IOPCActivator_FWD_DEFINED__ */


#ifndef __OPCActivator_FWD_DEFINED__
#define __OPCActivator_FWD_DEFINED__

#ifdef __cplusplus
typedef class OPCActivator OPCActivator;
#else
typedef struct OPCActivator OPCActivator;
#endif /* __cplusplus */

#endif 	/* __OPCActivator_FWD_DEFINED__ */


#ifndef __OPCServer_FWD_DEFINED__
#define __OPCServer_FWD_DEFINED__

#ifdef __cplusplus
typedef class OPCServer OPCServer;
#else
typedef struct OPCServer OPCServer;
#endif /* __cplusplus */

#endif 	/* __OPCServer_FWD_DEFINED__ */


#ifndef __OPCGroups_FWD_DEFINED__
#define __OPCGroups_FWD_DEFINED__

#ifdef __cplusplus
typedef class OPCGroups OPCGroups;
#else
typedef struct OPCGroups OPCGroups;
#endif /* __cplusplus */

#endif 	/* __OPCGroups_FWD_DEFINED__ */


#ifndef __OPCGroup_FWD_DEFINED__
#define __OPCGroup_FWD_DEFINED__

#ifdef __cplusplus
typedef class OPCGroup OPCGroup;
#else
typedef struct OPCGroup OPCGroup;
#endif /* __cplusplus */

#endif 	/* __OPCGroup_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


/* interface __MIDL_itf_OPCAuto_0000_0000 */
/* [local] */ 








extern RPC_IF_HANDLE __MIDL_itf_OPCAuto_0000_0000_v0_0_c_ifspec;
extern RPC_IF_HANDLE __MIDL_itf_OPCAuto_0000_0000_v0_0_s_ifspec;

#ifndef __IOPCServerEvent_INTERFACE_DEFINED__
#define __IOPCServerEvent_INTERFACE_DEFINED__

/* interface IOPCServerEvent */
/* [oleautomation][unique][helpstring][uuid][dual][object] */ 


EXTERN_C const IID IID_IOPCServerEvent;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("28E68F90-8D75-11d1-8DC3-3C302A000000")
    IOPCServerEvent : public IDispatch
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE ServerShutDown( 
            /* [string][in] */ BSTR Reason) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IOPCServerEventVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IOPCServerEvent * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IOPCServerEvent * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IOPCServerEvent * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IOPCServerEvent * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IOPCServerEvent * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IOPCServerEvent * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IOPCServerEvent * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        HRESULT ( STDMETHODCALLTYPE *ServerShutDown )( 
            IOPCServerEvent * This,
            /* [string][in] */ BSTR Reason);
        
        END_INTERFACE
    } IOPCServerEventVtbl;

    interface IOPCServerEvent
    {
        CONST_VTBL struct IOPCServerEventVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IOPCServerEvent_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IOPCServerEvent_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IOPCServerEvent_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IOPCServerEvent_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IOPCServerEvent_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IOPCServerEvent_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IOPCServerEvent_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IOPCServerEvent_ServerShutDown(This,Reason)	\
    ( (This)->lpVtbl -> ServerShutDown(This,Reason) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IOPCServerEvent_INTERFACE_DEFINED__ */


#ifndef __IOPCGroupsEvent_INTERFACE_DEFINED__
#define __IOPCGroupsEvent_INTERFACE_DEFINED__

/* interface IOPCGroupsEvent */
/* [oleautomation][unique][helpstring][uuid][dual][object] */ 


EXTERN_C const IID IID_IOPCGroupsEvent;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("28E68F9C-8D75-11d1-8DC3-3C302A000000")
    IOPCGroupsEvent : public IDispatch
    {
    public:
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GlobalDataChange( 
            /* [in] */ LONG TransactionID,
            /* [in] */ LONG GroupHandle,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ClientHandles,
            /* [in] */ SAFEARRAY * *ItemValues,
            /* [in] */ SAFEARRAY * *Qualities,
            /* [in] */ SAFEARRAY * *TimeStamps) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IOPCGroupsEventVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IOPCGroupsEvent * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IOPCGroupsEvent * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IOPCGroupsEvent * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IOPCGroupsEvent * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IOPCGroupsEvent * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IOPCGroupsEvent * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IOPCGroupsEvent * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GlobalDataChange )( 
            IOPCGroupsEvent * This,
            /* [in] */ LONG TransactionID,
            /* [in] */ LONG GroupHandle,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ClientHandles,
            /* [in] */ SAFEARRAY * *ItemValues,
            /* [in] */ SAFEARRAY * *Qualities,
            /* [in] */ SAFEARRAY * *TimeStamps);
        
        END_INTERFACE
    } IOPCGroupsEventVtbl;

    interface IOPCGroupsEvent
    {
        CONST_VTBL struct IOPCGroupsEventVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IOPCGroupsEvent_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IOPCGroupsEvent_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IOPCGroupsEvent_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IOPCGroupsEvent_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IOPCGroupsEvent_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IOPCGroupsEvent_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IOPCGroupsEvent_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IOPCGroupsEvent_GlobalDataChange(This,TransactionID,GroupHandle,NumItems,ClientHandles,ItemValues,Qualities,TimeStamps)	\
    ( (This)->lpVtbl -> GlobalDataChange(This,TransactionID,GroupHandle,NumItems,ClientHandles,ItemValues,Qualities,TimeStamps) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IOPCGroupsEvent_INTERFACE_DEFINED__ */


#ifndef __IOPCGroupEvent_INTERFACE_DEFINED__
#define __IOPCGroupEvent_INTERFACE_DEFINED__

/* interface IOPCGroupEvent */
/* [oleautomation][unique][helpstring][uuid][dual][object] */ 


EXTERN_C const IID IID_IOPCGroupEvent;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("28E68F90-8D75-11d1-8DC3-3C302A000001")
    IOPCGroupEvent : public IDispatch
    {
    public:
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE DataChange( 
            /* [in] */ LONG TransactionID,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ClientHandles,
            /* [in] */ SAFEARRAY * *ItemValues,
            /* [in] */ SAFEARRAY * *Qualities,
            /* [in] */ SAFEARRAY * *TimeStamps) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE AsyncReadComplete( 
            /* [in] */ LONG TransactionID,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ClientHandles,
            /* [in] */ SAFEARRAY * *ItemValues,
            /* [in] */ SAFEARRAY * *Qualities,
            /* [in] */ SAFEARRAY * *TimeStamps,
            /* [in] */ SAFEARRAY * *Errors) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE AsyncWriteComplete( 
            /* [in] */ LONG TransactionID,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ClientHandles,
            /* [in] */ SAFEARRAY * *Errors) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE AsyncCancelComplete( 
            /* [in] */ LONG TransactionID) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IOPCGroupEventVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IOPCGroupEvent * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IOPCGroupEvent * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IOPCGroupEvent * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IOPCGroupEvent * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IOPCGroupEvent * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IOPCGroupEvent * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IOPCGroupEvent * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *DataChange )( 
            IOPCGroupEvent * This,
            /* [in] */ LONG TransactionID,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ClientHandles,
            /* [in] */ SAFEARRAY * *ItemValues,
            /* [in] */ SAFEARRAY * *Qualities,
            /* [in] */ SAFEARRAY * *TimeStamps);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *AsyncReadComplete )( 
            IOPCGroupEvent * This,
            /* [in] */ LONG TransactionID,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ClientHandles,
            /* [in] */ SAFEARRAY * *ItemValues,
            /* [in] */ SAFEARRAY * *Qualities,
            /* [in] */ SAFEARRAY * *TimeStamps,
            /* [in] */ SAFEARRAY * *Errors);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *AsyncWriteComplete )( 
            IOPCGroupEvent * This,
            /* [in] */ LONG TransactionID,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ClientHandles,
            /* [in] */ SAFEARRAY * *Errors);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *AsyncCancelComplete )( 
            IOPCGroupEvent * This,
            /* [in] */ LONG TransactionID);
        
        END_INTERFACE
    } IOPCGroupEventVtbl;

    interface IOPCGroupEvent
    {
        CONST_VTBL struct IOPCGroupEventVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IOPCGroupEvent_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IOPCGroupEvent_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IOPCGroupEvent_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IOPCGroupEvent_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IOPCGroupEvent_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IOPCGroupEvent_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IOPCGroupEvent_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IOPCGroupEvent_DataChange(This,TransactionID,NumItems,ClientHandles,ItemValues,Qualities,TimeStamps)	\
    ( (This)->lpVtbl -> DataChange(This,TransactionID,NumItems,ClientHandles,ItemValues,Qualities,TimeStamps) ) 

#define IOPCGroupEvent_AsyncReadComplete(This,TransactionID,NumItems,ClientHandles,ItemValues,Qualities,TimeStamps,Errors)	\
    ( (This)->lpVtbl -> AsyncReadComplete(This,TransactionID,NumItems,ClientHandles,ItemValues,Qualities,TimeStamps,Errors) ) 

#define IOPCGroupEvent_AsyncWriteComplete(This,TransactionID,NumItems,ClientHandles,Errors)	\
    ( (This)->lpVtbl -> AsyncWriteComplete(This,TransactionID,NumItems,ClientHandles,Errors) ) 

#define IOPCGroupEvent_AsyncCancelComplete(This,TransactionID)	\
    ( (This)->lpVtbl -> AsyncCancelComplete(This,TransactionID) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IOPCGroupEvent_INTERFACE_DEFINED__ */



#ifndef __OPCAutomation_LIBRARY_DEFINED__
#define __OPCAutomation_LIBRARY_DEFINED__

/* library OPCAutomation */
/* [helpstring][version][uuid] */ 


enum OPCNamespaceTypes
    {
        OPCHierarchical	= 1,
        OPCFlat	= ( OPCHierarchical + 1 ) 
    } ;

enum OPCDataSource
    {
        OPCCache	= 1,
        OPCDevice	= ( OPCCache + 1 ) 
    } ;

enum OPCAccessRights
    {
        OPCReadable	= 1,
        OPCWritable	= ( OPCReadable + 1 ) 
    } ;

enum OPCServerState
    {
        OPCRunning	= 1,
        OPCFailed	= ( OPCRunning + 1 ) ,
        OPCNoconfig	= ( OPCFailed + 1 ) ,
        OPCSuspended	= ( OPCNoconfig + 1 ) ,
        OPCTest	= ( OPCSuspended + 1 ) ,
        OPCDisconnected	= ( OPCTest + 1 ) 
    } ;

enum OPCErrors
    {
        OPCInvalidHandle	= 0xc0040001L,
        OPCBadType	= 0xc0040004L,
        OPCPublic	= 0xc0040005L,
        OPCBadRights	= 0xc0040006L,
        OPCUnknownItemID	= 0xc0040007L,
        OPCInvalidItemID	= 0xc0040008L,
        OPCInvalidFilter	= 0xc0040009L,
        OPCUnknownPath	= 0xc004000aL,
        OPCRange	= 0xc004000bL,
        OPCDuplicateName	= 0xc004000cL,
        OPCUnsupportedRate	= 0x4000dL,
        OPCClamp	= 0x4000eL,
        OPCInuse	= 0x4000fL,
        OPCInvalidConfig	= 0xc0040010L,
        OPCNotFound	= 0xc0040011L,
        OPCInvalidPID	= 0xc0040203L
    } ;

enum OPCQuality
    {
        OPCQualityMask	= 0xc0,
        OPCQualityBad	= 0,
        OPCQualityUncertain	= 0x40,
        OPCQualityGood	= 0xc0
    } ;

enum OPCQualityStatus
    {
        OPCStatusMask	= 0xfc,
        OPCStatusConfigError	= 0x4,
        OPCStatusNotConnected	= 0x8,
        OPCStatusDeviceFailure	= 0xc,
        OPCStatusSensorFailure	= 0x10,
        OPCStatusLastKnown	= 0x14,
        OPCStatusCommFailure	= 0x18,
        OPCStatusOutOfService	= 0x1c,
        OPCStatusLastUsable	= 0x44,
        OPCStatusSensorCal	= 0x50,
        OPCStatusEGUExceeded	= 0x54,
        OPCStatusSubNormal	= 0x58,
        OPCStatusLocalOverride	= 0xd8
    } ;

enum OPCQualityLimits
    {
        OPCLimitMask	= 0x3,
        OPCLimitOk	= 0,
        OPCLimitLow	= 0x1,
        OPCLimitHigh	= 0x2,
        OPCLimitConst	= 0x3
    } ;

EXTERN_C const IID LIBID_OPCAutomation;

#ifndef __IOPCAutoServer_INTERFACE_DEFINED__
#define __IOPCAutoServer_INTERFACE_DEFINED__

/* interface IOPCAutoServer */
/* [unique][helpstring][uuid][oleautomation][dual][object] */ 


EXTERN_C const IID IID_IOPCAutoServer;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("28E68F92-8D75-11d1-8DC3-3C302A000000")
    IOPCAutoServer : public IDispatch
    {
    public:
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_StartTime( 
            /* [retval][out] */ DATE *StartTime) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_CurrentTime( 
            /* [retval][out] */ DATE *CurrentTime) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_LastUpdateTime( 
            /* [retval][out] */ DATE *LastUpdateTime) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_MajorVersion( 
            /* [retval][out] */ short *MajorVersion) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_MinorVersion( 
            /* [retval][out] */ short *MinorVersion) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_BuildNumber( 
            /* [retval][out] */ short *BuildNumber) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_VendorInfo( 
            /* [retval][out] */ BSTR *VendorInfo) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_ServerState( 
            /* [retval][out] */ LONG *ServerState) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_ServerName( 
            /* [retval][out] */ BSTR *ServerName) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_ServerNode( 
            /* [retval][out] */ BSTR *ServerNode) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_ClientName( 
            /* [retval][out] */ BSTR *ClientName) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_ClientName( 
            /* [in] */ BSTR ClientName) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_LocaleID( 
            /* [retval][out] */ LONG *LocaleID) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_LocaleID( 
            /* [in] */ LONG LocaleID) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_Bandwidth( 
            /* [retval][out] */ LONG *Bandwidth) = 0;
        
        virtual /* [helpstring][propget][id] */ HRESULT STDMETHODCALLTYPE get_OPCGroups( 
            /* [retval][out] */ OPCGroups	**ppGroups) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_PublicGroupNames( 
            /* [retval][out] */ VARIANT *PublicGroups) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetOPCServers( 
            /* [optional][in] */ VARIANT Node,
            /* [retval][out] */ VARIANT *OPCServers) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Connect( 
            /* [string][in] */ BSTR ProgID,
            /* [optional][in] */ VARIANT Node) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Disconnect( void) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE CreateBrowser( 
            /* [retval][out] */ OPCBrowser **ppBrowser) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetErrorString( 
            /* [in] */ LONG ErrorCode,
            /* [retval][out] */ BSTR *ErrorString) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE QueryAvailableLocaleIDs( 
            /* [retval][out] */ VARIANT *LocaleIDs) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE QueryAvailableProperties( 
            /* [string][in] */ BSTR ItemID,
            /* [out] */ LONG *Count,
            /* [out] */ SAFEARRAY * *PropertyIDs,
            /* [out] */ SAFEARRAY * *Descriptions,
            /* [out] */ SAFEARRAY * *DataTypes) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetItemProperties( 
            /* [string][in] */ BSTR ItemID,
            /* [in] */ LONG Count,
            /* [in] */ SAFEARRAY * *PropertyIDs,
            /* [out] */ SAFEARRAY * *PropertyValues,
            /* [out] */ SAFEARRAY * *Errors) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE LookupItemIDs( 
            /* [string][in] */ BSTR ItemID,
            /* [in] */ LONG Count,
            /* [in] */ SAFEARRAY * *PropertyIDs,
            /* [out] */ SAFEARRAY * *NewItemIDs,
            /* [out] */ SAFEARRAY * *Errors) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IOPCAutoServerVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IOPCAutoServer * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IOPCAutoServer * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IOPCAutoServer * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IOPCAutoServer * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IOPCAutoServer * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IOPCAutoServer * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IOPCAutoServer * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_StartTime )( 
            IOPCAutoServer * This,
            /* [retval][out] */ DATE *StartTime);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CurrentTime )( 
            IOPCAutoServer * This,
            /* [retval][out] */ DATE *CurrentTime);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LastUpdateTime )( 
            IOPCAutoServer * This,
            /* [retval][out] */ DATE *LastUpdateTime);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MajorVersion )( 
            IOPCAutoServer * This,
            /* [retval][out] */ short *MajorVersion);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_MinorVersion )( 
            IOPCAutoServer * This,
            /* [retval][out] */ short *MinorVersion);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_BuildNumber )( 
            IOPCAutoServer * This,
            /* [retval][out] */ short *BuildNumber);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_VendorInfo )( 
            IOPCAutoServer * This,
            /* [retval][out] */ BSTR *VendorInfo);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ServerState )( 
            IOPCAutoServer * This,
            /* [retval][out] */ LONG *ServerState);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ServerName )( 
            IOPCAutoServer * This,
            /* [retval][out] */ BSTR *ServerName);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ServerNode )( 
            IOPCAutoServer * This,
            /* [retval][out] */ BSTR *ServerNode);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ClientName )( 
            IOPCAutoServer * This,
            /* [retval][out] */ BSTR *ClientName);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ClientName )( 
            IOPCAutoServer * This,
            /* [in] */ BSTR ClientName);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LocaleID )( 
            IOPCAutoServer * This,
            /* [retval][out] */ LONG *LocaleID);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_LocaleID )( 
            IOPCAutoServer * This,
            /* [in] */ LONG LocaleID);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Bandwidth )( 
            IOPCAutoServer * This,
            /* [retval][out] */ LONG *Bandwidth);
        
        /* [helpstring][propget][id] */ HRESULT ( STDMETHODCALLTYPE *get_OPCGroups )( 
            IOPCAutoServer * This,
            /* [retval][out] */ OPCGroups	**ppGroups);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_PublicGroupNames )( 
            IOPCAutoServer * This,
            /* [retval][out] */ VARIANT *PublicGroups);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetOPCServers )( 
            IOPCAutoServer * This,
            /* [optional][in] */ VARIANT Node,
            /* [retval][out] */ VARIANT *OPCServers);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Connect )( 
            IOPCAutoServer * This,
            /* [string][in] */ BSTR ProgID,
            /* [optional][in] */ VARIANT Node);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Disconnect )( 
            IOPCAutoServer * This);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *CreateBrowser )( 
            IOPCAutoServer * This,
            /* [retval][out] */ OPCBrowser **ppBrowser);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetErrorString )( 
            IOPCAutoServer * This,
            /* [in] */ LONG ErrorCode,
            /* [retval][out] */ BSTR *ErrorString);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *QueryAvailableLocaleIDs )( 
            IOPCAutoServer * This,
            /* [retval][out] */ VARIANT *LocaleIDs);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *QueryAvailableProperties )( 
            IOPCAutoServer * This,
            /* [string][in] */ BSTR ItemID,
            /* [out] */ LONG *Count,
            /* [out] */ SAFEARRAY * *PropertyIDs,
            /* [out] */ SAFEARRAY * *Descriptions,
            /* [out] */ SAFEARRAY * *DataTypes);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetItemProperties )( 
            IOPCAutoServer * This,
            /* [string][in] */ BSTR ItemID,
            /* [in] */ LONG Count,
            /* [in] */ SAFEARRAY * *PropertyIDs,
            /* [out] */ SAFEARRAY * *PropertyValues,
            /* [out] */ SAFEARRAY * *Errors);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *LookupItemIDs )( 
            IOPCAutoServer * This,
            /* [string][in] */ BSTR ItemID,
            /* [in] */ LONG Count,
            /* [in] */ SAFEARRAY * *PropertyIDs,
            /* [out] */ SAFEARRAY * *NewItemIDs,
            /* [out] */ SAFEARRAY * *Errors);
        
        END_INTERFACE
    } IOPCAutoServerVtbl;

    interface IOPCAutoServer
    {
        CONST_VTBL struct IOPCAutoServerVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IOPCAutoServer_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IOPCAutoServer_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IOPCAutoServer_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IOPCAutoServer_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IOPCAutoServer_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IOPCAutoServer_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IOPCAutoServer_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IOPCAutoServer_get_StartTime(This,StartTime)	\
    ( (This)->lpVtbl -> get_StartTime(This,StartTime) ) 

#define IOPCAutoServer_get_CurrentTime(This,CurrentTime)	\
    ( (This)->lpVtbl -> get_CurrentTime(This,CurrentTime) ) 

#define IOPCAutoServer_get_LastUpdateTime(This,LastUpdateTime)	\
    ( (This)->lpVtbl -> get_LastUpdateTime(This,LastUpdateTime) ) 

#define IOPCAutoServer_get_MajorVersion(This,MajorVersion)	\
    ( (This)->lpVtbl -> get_MajorVersion(This,MajorVersion) ) 

#define IOPCAutoServer_get_MinorVersion(This,MinorVersion)	\
    ( (This)->lpVtbl -> get_MinorVersion(This,MinorVersion) ) 

#define IOPCAutoServer_get_BuildNumber(This,BuildNumber)	\
    ( (This)->lpVtbl -> get_BuildNumber(This,BuildNumber) ) 

#define IOPCAutoServer_get_VendorInfo(This,VendorInfo)	\
    ( (This)->lpVtbl -> get_VendorInfo(This,VendorInfo) ) 

#define IOPCAutoServer_get_ServerState(This,ServerState)	\
    ( (This)->lpVtbl -> get_ServerState(This,ServerState) ) 

#define IOPCAutoServer_get_ServerName(This,ServerName)	\
    ( (This)->lpVtbl -> get_ServerName(This,ServerName) ) 

#define IOPCAutoServer_get_ServerNode(This,ServerNode)	\
    ( (This)->lpVtbl -> get_ServerNode(This,ServerNode) ) 

#define IOPCAutoServer_get_ClientName(This,ClientName)	\
    ( (This)->lpVtbl -> get_ClientName(This,ClientName) ) 

#define IOPCAutoServer_put_ClientName(This,ClientName)	\
    ( (This)->lpVtbl -> put_ClientName(This,ClientName) ) 

#define IOPCAutoServer_get_LocaleID(This,LocaleID)	\
    ( (This)->lpVtbl -> get_LocaleID(This,LocaleID) ) 

#define IOPCAutoServer_put_LocaleID(This,LocaleID)	\
    ( (This)->lpVtbl -> put_LocaleID(This,LocaleID) ) 

#define IOPCAutoServer_get_Bandwidth(This,Bandwidth)	\
    ( (This)->lpVtbl -> get_Bandwidth(This,Bandwidth) ) 

#define IOPCAutoServer_get_OPCGroups(This,ppGroups)	\
    ( (This)->lpVtbl -> get_OPCGroups(This,ppGroups) ) 

#define IOPCAutoServer_get_PublicGroupNames(This,PublicGroups)	\
    ( (This)->lpVtbl -> get_PublicGroupNames(This,PublicGroups) ) 

#define IOPCAutoServer_GetOPCServers(This,Node,OPCServers)	\
    ( (This)->lpVtbl -> GetOPCServers(This,Node,OPCServers) ) 

#define IOPCAutoServer_Connect(This,ProgID,Node)	\
    ( (This)->lpVtbl -> Connect(This,ProgID,Node) ) 

#define IOPCAutoServer_Disconnect(This)	\
    ( (This)->lpVtbl -> Disconnect(This) ) 

#define IOPCAutoServer_CreateBrowser(This,ppBrowser)	\
    ( (This)->lpVtbl -> CreateBrowser(This,ppBrowser) ) 

#define IOPCAutoServer_GetErrorString(This,ErrorCode,ErrorString)	\
    ( (This)->lpVtbl -> GetErrorString(This,ErrorCode,ErrorString) ) 

#define IOPCAutoServer_QueryAvailableLocaleIDs(This,LocaleIDs)	\
    ( (This)->lpVtbl -> QueryAvailableLocaleIDs(This,LocaleIDs) ) 

#define IOPCAutoServer_QueryAvailableProperties(This,ItemID,Count,PropertyIDs,Descriptions,DataTypes)	\
    ( (This)->lpVtbl -> QueryAvailableProperties(This,ItemID,Count,PropertyIDs,Descriptions,DataTypes) ) 

#define IOPCAutoServer_GetItemProperties(This,ItemID,Count,PropertyIDs,PropertyValues,Errors)	\
    ( (This)->lpVtbl -> GetItemProperties(This,ItemID,Count,PropertyIDs,PropertyValues,Errors) ) 

#define IOPCAutoServer_LookupItemIDs(This,ItemID,Count,PropertyIDs,NewItemIDs,Errors)	\
    ( (This)->lpVtbl -> LookupItemIDs(This,ItemID,Count,PropertyIDs,NewItemIDs,Errors) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IOPCAutoServer_INTERFACE_DEFINED__ */


#ifndef __DIOPCServerEvent_DISPINTERFACE_DEFINED__
#define __DIOPCServerEvent_DISPINTERFACE_DEFINED__

/* dispinterface DIOPCServerEvent */
/* [helpstring][nonextensible][uuid] */ 


EXTERN_C const IID DIID_DIOPCServerEvent;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("28E68F93-8D75-11d1-8DC3-3C302A000000")
    DIOPCServerEvent : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct DIOPCServerEventVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            DIOPCServerEvent * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            DIOPCServerEvent * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            DIOPCServerEvent * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            DIOPCServerEvent * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            DIOPCServerEvent * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            DIOPCServerEvent * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            DIOPCServerEvent * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        END_INTERFACE
    } DIOPCServerEventVtbl;

    interface DIOPCServerEvent
    {
        CONST_VTBL struct DIOPCServerEventVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define DIOPCServerEvent_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define DIOPCServerEvent_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define DIOPCServerEvent_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define DIOPCServerEvent_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define DIOPCServerEvent_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define DIOPCServerEvent_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define DIOPCServerEvent_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* __DIOPCServerEvent_DISPINTERFACE_DEFINED__ */


#ifndef __OPCBrowser_INTERFACE_DEFINED__
#define __OPCBrowser_INTERFACE_DEFINED__

/* interface OPCBrowser */
/* [unique][helpstring][uuid][oleautomation][dual][object] */ 


EXTERN_C const IID IID_OPCBrowser;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("28E68F94-8D75-11d1-8DC3-3C302A000000")
    OPCBrowser : public IDispatch
    {
    public:
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_Organization( 
            /* [retval][out] */ LONG *Organization) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_Filter( 
            /* [retval][out] */ BSTR *Filter) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_Filter( 
            /* [in] */ BSTR Filter) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_DataType( 
            /* [retval][out] */ SHORT *DataType) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_DataType( 
            /* [in] */ SHORT DataType) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_AccessRights( 
            /* [retval][out] */ LONG *AccessRights) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_AccessRights( 
            /* [in] */ LONG AccessRights) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_CurrentPosition( 
            /* [retval][out] */ BSTR *CurrentPosition) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_Count( 
            /* [retval][out] */ LONG *Count) = 0;
        
        virtual /* [id][restricted][propget] */ HRESULT STDMETHODCALLTYPE get__NewEnum( 
            /* [retval][out] */ IUnknown **ppUnk) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Item( 
            /* [in] */ VARIANT ItemSpecifier,
            /* [retval][out] */ BSTR *Item) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE ShowBranches( void) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE ShowLeafs( 
            /* [optional][in] */ VARIANT Flat) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE MoveUp( void) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE MoveToRoot( void) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE MoveDown( 
            /* [string][in] */ BSTR Branch) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE MoveTo( 
            /* [in] */ SAFEARRAY * *Branches) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetItemID( 
            /* [string][in] */ BSTR Leaf,
            /* [retval][out] */ BSTR *ItemID) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetAccessPaths( 
            /* [string][in] */ BSTR ItemID,
            /* [retval][out] */ VARIANT *AccessPaths) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct OPCBrowserVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            OPCBrowser * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            OPCBrowser * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            OPCBrowser * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            OPCBrowser * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            OPCBrowser * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            OPCBrowser * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            OPCBrowser * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Organization )( 
            OPCBrowser * This,
            /* [retval][out] */ LONG *Organization);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Filter )( 
            OPCBrowser * This,
            /* [retval][out] */ BSTR *Filter);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Filter )( 
            OPCBrowser * This,
            /* [in] */ BSTR Filter);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DataType )( 
            OPCBrowser * This,
            /* [retval][out] */ SHORT *DataType);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DataType )( 
            OPCBrowser * This,
            /* [in] */ SHORT DataType);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_AccessRights )( 
            OPCBrowser * This,
            /* [retval][out] */ LONG *AccessRights);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_AccessRights )( 
            OPCBrowser * This,
            /* [in] */ LONG AccessRights);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CurrentPosition )( 
            OPCBrowser * This,
            /* [retval][out] */ BSTR *CurrentPosition);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Count )( 
            OPCBrowser * This,
            /* [retval][out] */ LONG *Count);
        
        /* [id][restricted][propget] */ HRESULT ( STDMETHODCALLTYPE *get__NewEnum )( 
            OPCBrowser * This,
            /* [retval][out] */ IUnknown **ppUnk);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Item )( 
            OPCBrowser * This,
            /* [in] */ VARIANT ItemSpecifier,
            /* [retval][out] */ BSTR *Item);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *ShowBranches )( 
            OPCBrowser * This);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *ShowLeafs )( 
            OPCBrowser * This,
            /* [optional][in] */ VARIANT Flat);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *MoveUp )( 
            OPCBrowser * This);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *MoveToRoot )( 
            OPCBrowser * This);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *MoveDown )( 
            OPCBrowser * This,
            /* [string][in] */ BSTR Branch);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *MoveTo )( 
            OPCBrowser * This,
            /* [in] */ SAFEARRAY * *Branches);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetItemID )( 
            OPCBrowser * This,
            /* [string][in] */ BSTR Leaf,
            /* [retval][out] */ BSTR *ItemID);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetAccessPaths )( 
            OPCBrowser * This,
            /* [string][in] */ BSTR ItemID,
            /* [retval][out] */ VARIANT *AccessPaths);
        
        END_INTERFACE
    } OPCBrowserVtbl;

    interface OPCBrowser
    {
        CONST_VTBL struct OPCBrowserVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define OPCBrowser_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define OPCBrowser_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define OPCBrowser_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define OPCBrowser_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define OPCBrowser_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define OPCBrowser_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define OPCBrowser_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define OPCBrowser_get_Organization(This,Organization)	\
    ( (This)->lpVtbl -> get_Organization(This,Organization) ) 

#define OPCBrowser_get_Filter(This,Filter)	\
    ( (This)->lpVtbl -> get_Filter(This,Filter) ) 

#define OPCBrowser_put_Filter(This,Filter)	\
    ( (This)->lpVtbl -> put_Filter(This,Filter) ) 

#define OPCBrowser_get_DataType(This,DataType)	\
    ( (This)->lpVtbl -> get_DataType(This,DataType) ) 

#define OPCBrowser_put_DataType(This,DataType)	\
    ( (This)->lpVtbl -> put_DataType(This,DataType) ) 

#define OPCBrowser_get_AccessRights(This,AccessRights)	\
    ( (This)->lpVtbl -> get_AccessRights(This,AccessRights) ) 

#define OPCBrowser_put_AccessRights(This,AccessRights)	\
    ( (This)->lpVtbl -> put_AccessRights(This,AccessRights) ) 

#define OPCBrowser_get_CurrentPosition(This,CurrentPosition)	\
    ( (This)->lpVtbl -> get_CurrentPosition(This,CurrentPosition) ) 

#define OPCBrowser_get_Count(This,Count)	\
    ( (This)->lpVtbl -> get_Count(This,Count) ) 

#define OPCBrowser_get__NewEnum(This,ppUnk)	\
    ( (This)->lpVtbl -> get__NewEnum(This,ppUnk) ) 

#define OPCBrowser_Item(This,ItemSpecifier,Item)	\
    ( (This)->lpVtbl -> Item(This,ItemSpecifier,Item) ) 

#define OPCBrowser_ShowBranches(This)	\
    ( (This)->lpVtbl -> ShowBranches(This) ) 

#define OPCBrowser_ShowLeafs(This,Flat)	\
    ( (This)->lpVtbl -> ShowLeafs(This,Flat) ) 

#define OPCBrowser_MoveUp(This)	\
    ( (This)->lpVtbl -> MoveUp(This) ) 

#define OPCBrowser_MoveToRoot(This)	\
    ( (This)->lpVtbl -> MoveToRoot(This) ) 

#define OPCBrowser_MoveDown(This,Branch)	\
    ( (This)->lpVtbl -> MoveDown(This,Branch) ) 

#define OPCBrowser_MoveTo(This,Branches)	\
    ( (This)->lpVtbl -> MoveTo(This,Branches) ) 

#define OPCBrowser_GetItemID(This,Leaf,ItemID)	\
    ( (This)->lpVtbl -> GetItemID(This,Leaf,ItemID) ) 

#define OPCBrowser_GetAccessPaths(This,ItemID,AccessPaths)	\
    ( (This)->lpVtbl -> GetAccessPaths(This,ItemID,AccessPaths) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __OPCBrowser_INTERFACE_DEFINED__ */


#ifndef __IOPCGroups_INTERFACE_DEFINED__
#define __IOPCGroups_INTERFACE_DEFINED__

/* interface IOPCGroups */
/* [unique][helpstring][uuid][oleautomation][dual][object] */ 


EXTERN_C const IID IID_IOPCGroups;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("28E68F95-8D75-11d1-8DC3-3C302A000000")
    IOPCGroups : public IDispatch
    {
    public:
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_Parent( 
            /* [retval][out] */ IOPCAutoServer **ppParent) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_DefaultGroupIsActive( 
            /* [retval][out] */ VARIANT_BOOL *DefaultGroupIsActive) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_DefaultGroupIsActive( 
            /* [in] */ VARIANT_BOOL DefaultGroupIsActive) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_DefaultGroupUpdateRate( 
            /* [retval][out] */ LONG *DefaultGroupUpdateRate) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_DefaultGroupUpdateRate( 
            /* [in] */ LONG DefaultGroupUpdateRate) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_DefaultGroupDeadband( 
            /* [retval][out] */ float *DefaultGroupDeadband) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_DefaultGroupDeadband( 
            /* [in] */ float DefaultGroupDeadband) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_DefaultGroupLocaleID( 
            /* [retval][out] */ LONG *DefaultGroupLocaleID) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_DefaultGroupLocaleID( 
            /* [in] */ LONG DefaultGroupLocaleID) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_DefaultGroupTimeBias( 
            /* [retval][out] */ LONG *DefaultGroupTimeBias) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_DefaultGroupTimeBias( 
            /* [in] */ LONG DefaultGroupTimeBias) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_Count( 
            /* [retval][out] */ LONG *Count) = 0;
        
        virtual /* [id][restricted][propget] */ HRESULT STDMETHODCALLTYPE get__NewEnum( 
            /* [retval][out] */ IUnknown **ppUnk) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Item( 
            /* [in] */ VARIANT ItemSpecifier,
            /* [retval][out] */ OPCGroup	**ppGroup) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Add( 
            /* [optional][in] */ VARIANT Name,
            /* [retval][out] */ OPCGroup	**ppGroup) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetOPCGroup( 
            /* [in] */ VARIANT ItemSpecifier,
            /* [retval][out] */ OPCGroup	**ppGroup) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE RemoveAll( void) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Remove( 
            /* [in] */ VARIANT ItemSpecifier) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE ConnectPublicGroup( 
            /* [in] */ BSTR Name,
            /* [retval][out] */ OPCGroup	**ppGroup) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE RemovePublicGroup( 
            /* [in] */ VARIANT ItemSpecifier) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IOPCGroupsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IOPCGroups * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IOPCGroups * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IOPCGroups * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IOPCGroups * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IOPCGroups * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IOPCGroups * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IOPCGroups * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Parent )( 
            IOPCGroups * This,
            /* [retval][out] */ IOPCAutoServer **ppParent);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DefaultGroupIsActive )( 
            IOPCGroups * This,
            /* [retval][out] */ VARIANT_BOOL *DefaultGroupIsActive);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DefaultGroupIsActive )( 
            IOPCGroups * This,
            /* [in] */ VARIANT_BOOL DefaultGroupIsActive);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DefaultGroupUpdateRate )( 
            IOPCGroups * This,
            /* [retval][out] */ LONG *DefaultGroupUpdateRate);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DefaultGroupUpdateRate )( 
            IOPCGroups * This,
            /* [in] */ LONG DefaultGroupUpdateRate);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DefaultGroupDeadband )( 
            IOPCGroups * This,
            /* [retval][out] */ float *DefaultGroupDeadband);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DefaultGroupDeadband )( 
            IOPCGroups * This,
            /* [in] */ float DefaultGroupDeadband);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DefaultGroupLocaleID )( 
            IOPCGroups * This,
            /* [retval][out] */ LONG *DefaultGroupLocaleID);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DefaultGroupLocaleID )( 
            IOPCGroups * This,
            /* [in] */ LONG DefaultGroupLocaleID);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DefaultGroupTimeBias )( 
            IOPCGroups * This,
            /* [retval][out] */ LONG *DefaultGroupTimeBias);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DefaultGroupTimeBias )( 
            IOPCGroups * This,
            /* [in] */ LONG DefaultGroupTimeBias);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Count )( 
            IOPCGroups * This,
            /* [retval][out] */ LONG *Count);
        
        /* [id][restricted][propget] */ HRESULT ( STDMETHODCALLTYPE *get__NewEnum )( 
            IOPCGroups * This,
            /* [retval][out] */ IUnknown **ppUnk);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Item )( 
            IOPCGroups * This,
            /* [in] */ VARIANT ItemSpecifier,
            /* [retval][out] */ OPCGroup	**ppGroup);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Add )( 
            IOPCGroups * This,
            /* [optional][in] */ VARIANT Name,
            /* [retval][out] */ OPCGroup	**ppGroup);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetOPCGroup )( 
            IOPCGroups * This,
            /* [in] */ VARIANT ItemSpecifier,
            /* [retval][out] */ OPCGroup	**ppGroup);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *RemoveAll )( 
            IOPCGroups * This);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Remove )( 
            IOPCGroups * This,
            /* [in] */ VARIANT ItemSpecifier);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *ConnectPublicGroup )( 
            IOPCGroups * This,
            /* [in] */ BSTR Name,
            /* [retval][out] */ OPCGroup	**ppGroup);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *RemovePublicGroup )( 
            IOPCGroups * This,
            /* [in] */ VARIANT ItemSpecifier);
        
        END_INTERFACE
    } IOPCGroupsVtbl;

    interface IOPCGroups
    {
        CONST_VTBL struct IOPCGroupsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IOPCGroups_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IOPCGroups_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IOPCGroups_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IOPCGroups_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IOPCGroups_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IOPCGroups_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IOPCGroups_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IOPCGroups_get_Parent(This,ppParent)	\
    ( (This)->lpVtbl -> get_Parent(This,ppParent) ) 

#define IOPCGroups_get_DefaultGroupIsActive(This,DefaultGroupIsActive)	\
    ( (This)->lpVtbl -> get_DefaultGroupIsActive(This,DefaultGroupIsActive) ) 

#define IOPCGroups_put_DefaultGroupIsActive(This,DefaultGroupIsActive)	\
    ( (This)->lpVtbl -> put_DefaultGroupIsActive(This,DefaultGroupIsActive) ) 

#define IOPCGroups_get_DefaultGroupUpdateRate(This,DefaultGroupUpdateRate)	\
    ( (This)->lpVtbl -> get_DefaultGroupUpdateRate(This,DefaultGroupUpdateRate) ) 

#define IOPCGroups_put_DefaultGroupUpdateRate(This,DefaultGroupUpdateRate)	\
    ( (This)->lpVtbl -> put_DefaultGroupUpdateRate(This,DefaultGroupUpdateRate) ) 

#define IOPCGroups_get_DefaultGroupDeadband(This,DefaultGroupDeadband)	\
    ( (This)->lpVtbl -> get_DefaultGroupDeadband(This,DefaultGroupDeadband) ) 

#define IOPCGroups_put_DefaultGroupDeadband(This,DefaultGroupDeadband)	\
    ( (This)->lpVtbl -> put_DefaultGroupDeadband(This,DefaultGroupDeadband) ) 

#define IOPCGroups_get_DefaultGroupLocaleID(This,DefaultGroupLocaleID)	\
    ( (This)->lpVtbl -> get_DefaultGroupLocaleID(This,DefaultGroupLocaleID) ) 

#define IOPCGroups_put_DefaultGroupLocaleID(This,DefaultGroupLocaleID)	\
    ( (This)->lpVtbl -> put_DefaultGroupLocaleID(This,DefaultGroupLocaleID) ) 

#define IOPCGroups_get_DefaultGroupTimeBias(This,DefaultGroupTimeBias)	\
    ( (This)->lpVtbl -> get_DefaultGroupTimeBias(This,DefaultGroupTimeBias) ) 

#define IOPCGroups_put_DefaultGroupTimeBias(This,DefaultGroupTimeBias)	\
    ( (This)->lpVtbl -> put_DefaultGroupTimeBias(This,DefaultGroupTimeBias) ) 

#define IOPCGroups_get_Count(This,Count)	\
    ( (This)->lpVtbl -> get_Count(This,Count) ) 

#define IOPCGroups_get__NewEnum(This,ppUnk)	\
    ( (This)->lpVtbl -> get__NewEnum(This,ppUnk) ) 

#define IOPCGroups_Item(This,ItemSpecifier,ppGroup)	\
    ( (This)->lpVtbl -> Item(This,ItemSpecifier,ppGroup) ) 

#define IOPCGroups_Add(This,Name,ppGroup)	\
    ( (This)->lpVtbl -> Add(This,Name,ppGroup) ) 

#define IOPCGroups_GetOPCGroup(This,ItemSpecifier,ppGroup)	\
    ( (This)->lpVtbl -> GetOPCGroup(This,ItemSpecifier,ppGroup) ) 

#define IOPCGroups_RemoveAll(This)	\
    ( (This)->lpVtbl -> RemoveAll(This) ) 

#define IOPCGroups_Remove(This,ItemSpecifier)	\
    ( (This)->lpVtbl -> Remove(This,ItemSpecifier) ) 

#define IOPCGroups_ConnectPublicGroup(This,Name,ppGroup)	\
    ( (This)->lpVtbl -> ConnectPublicGroup(This,Name,ppGroup) ) 

#define IOPCGroups_RemovePublicGroup(This,ItemSpecifier)	\
    ( (This)->lpVtbl -> RemovePublicGroup(This,ItemSpecifier) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IOPCGroups_INTERFACE_DEFINED__ */


#ifndef __DIOPCGroupsEvent_DISPINTERFACE_DEFINED__
#define __DIOPCGroupsEvent_DISPINTERFACE_DEFINED__

/* dispinterface DIOPCGroupsEvent */
/* [helpstring][nonextensible][uuid] */ 


EXTERN_C const IID DIID_DIOPCGroupsEvent;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("28E68F9D-8D75-11d1-8DC3-3C302A000000")
    DIOPCGroupsEvent : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct DIOPCGroupsEventVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            DIOPCGroupsEvent * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            DIOPCGroupsEvent * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            DIOPCGroupsEvent * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            DIOPCGroupsEvent * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            DIOPCGroupsEvent * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            DIOPCGroupsEvent * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            DIOPCGroupsEvent * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        END_INTERFACE
    } DIOPCGroupsEventVtbl;

    interface DIOPCGroupsEvent
    {
        CONST_VTBL struct DIOPCGroupsEventVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define DIOPCGroupsEvent_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define DIOPCGroupsEvent_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define DIOPCGroupsEvent_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define DIOPCGroupsEvent_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define DIOPCGroupsEvent_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define DIOPCGroupsEvent_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define DIOPCGroupsEvent_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* __DIOPCGroupsEvent_DISPINTERFACE_DEFINED__ */


#ifndef __IOPCGroup_INTERFACE_DEFINED__
#define __IOPCGroup_INTERFACE_DEFINED__

/* interface IOPCGroup */
/* [unique][helpstring][uuid][oleautomation][dual][object] */ 


EXTERN_C const IID IID_IOPCGroup;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("28E68F96-8D75-11d1-8DC3-3C302A000000")
    IOPCGroup : public IDispatch
    {
    public:
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_Parent( 
            /* [retval][out] */ IOPCAutoServer **ppParent) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_Name( 
            /* [retval][out] */ BSTR *Name) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_Name( 
            /* [in] */ BSTR Name) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_IsPublic( 
            /* [retval][out] */ VARIANT_BOOL *IsPublic) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_IsActive( 
            /* [retval][out] */ VARIANT_BOOL *IsActive) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_IsActive( 
            /* [in] */ VARIANT_BOOL IsActive) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_IsSubscribed( 
            /* [retval][out] */ VARIANT_BOOL *IsSubscribed) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_IsSubscribed( 
            /* [in] */ VARIANT_BOOL IsSubscribed) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_ClientHandle( 
            /* [retval][out] */ LONG *ClientHandle) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_ClientHandle( 
            /* [in] */ LONG ClientHandle) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_ServerHandle( 
            /* [retval][out] */ LONG *ServerHandle) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_LocaleID( 
            /* [retval][out] */ LONG *LocaleID) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_LocaleID( 
            /* [in] */ LONG LocaleID) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_TimeBias( 
            /* [retval][out] */ LONG *TimeBias) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_TimeBias( 
            /* [in] */ LONG TimeBias) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_DeadBand( 
            /* [retval][out] */ FLOAT *DeadBand) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_DeadBand( 
            /* [in] */ FLOAT DeadBand) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_UpdateRate( 
            /* [retval][out] */ LONG *UpdateRate) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_UpdateRate( 
            /* [in] */ LONG UpdateRate) = 0;
        
        virtual /* [helpstring][propget][id] */ HRESULT STDMETHODCALLTYPE get_OPCItems( 
            /* [retval][out] */ OPCItems **ppItems) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SyncRead( 
            /* [in] */ SHORT Source,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ServerHandles,
            /* [out] */ SAFEARRAY * *Values,
            /* [out] */ SAFEARRAY * *Errors,
            /* [optional][out] */ VARIANT *Qualities,
            /* [optional][out] */ VARIANT *TimeStamps) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE SyncWrite( 
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ServerHandles,
            /* [in] */ SAFEARRAY * *Values,
            /* [out] */ SAFEARRAY * *Errors) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AsyncRead( 
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ServerHandles,
            /* [out] */ SAFEARRAY * *Errors,
            /* [in] */ LONG TransactionID,
            /* [out] */ LONG *CancelID) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AsyncWrite( 
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ServerHandles,
            /* [in] */ SAFEARRAY * *Values,
            /* [out] */ SAFEARRAY * *Errors,
            /* [in] */ LONG TransactionID,
            /* [out] */ LONG *CancelID) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AsyncRefresh( 
            /* [in] */ SHORT Source,
            /* [in] */ LONG TransactionID,
            /* [out] */ LONG *CancelID) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE AsyncCancel( 
            /* [in] */ LONG CancelID) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IOPCGroupVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IOPCGroup * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IOPCGroup * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IOPCGroup * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IOPCGroup * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IOPCGroup * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IOPCGroup * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IOPCGroup * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Parent )( 
            IOPCGroup * This,
            /* [retval][out] */ IOPCAutoServer **ppParent);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Name )( 
            IOPCGroup * This,
            /* [retval][out] */ BSTR *Name);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Name )( 
            IOPCGroup * This,
            /* [in] */ BSTR Name);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IsPublic )( 
            IOPCGroup * This,
            /* [retval][out] */ VARIANT_BOOL *IsPublic);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IsActive )( 
            IOPCGroup * This,
            /* [retval][out] */ VARIANT_BOOL *IsActive);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_IsActive )( 
            IOPCGroup * This,
            /* [in] */ VARIANT_BOOL IsActive);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IsSubscribed )( 
            IOPCGroup * This,
            /* [retval][out] */ VARIANT_BOOL *IsSubscribed);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_IsSubscribed )( 
            IOPCGroup * This,
            /* [in] */ VARIANT_BOOL IsSubscribed);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ClientHandle )( 
            IOPCGroup * This,
            /* [retval][out] */ LONG *ClientHandle);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ClientHandle )( 
            IOPCGroup * This,
            /* [in] */ LONG ClientHandle);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ServerHandle )( 
            IOPCGroup * This,
            /* [retval][out] */ LONG *ServerHandle);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_LocaleID )( 
            IOPCGroup * This,
            /* [retval][out] */ LONG *LocaleID);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_LocaleID )( 
            IOPCGroup * This,
            /* [in] */ LONG LocaleID);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_TimeBias )( 
            IOPCGroup * This,
            /* [retval][out] */ LONG *TimeBias);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_TimeBias )( 
            IOPCGroup * This,
            /* [in] */ LONG TimeBias);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DeadBand )( 
            IOPCGroup * This,
            /* [retval][out] */ FLOAT *DeadBand);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DeadBand )( 
            IOPCGroup * This,
            /* [in] */ FLOAT DeadBand);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_UpdateRate )( 
            IOPCGroup * This,
            /* [retval][out] */ LONG *UpdateRate);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_UpdateRate )( 
            IOPCGroup * This,
            /* [in] */ LONG UpdateRate);
        
        /* [helpstring][propget][id] */ HRESULT ( STDMETHODCALLTYPE *get_OPCItems )( 
            IOPCGroup * This,
            /* [retval][out] */ OPCItems **ppItems);
        
        HRESULT ( STDMETHODCALLTYPE *SyncRead )( 
            IOPCGroup * This,
            /* [in] */ SHORT Source,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ServerHandles,
            /* [out] */ SAFEARRAY * *Values,
            /* [out] */ SAFEARRAY * *Errors,
            /* [optional][out] */ VARIANT *Qualities,
            /* [optional][out] */ VARIANT *TimeStamps);
        
        HRESULT ( STDMETHODCALLTYPE *SyncWrite )( 
            IOPCGroup * This,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ServerHandles,
            /* [in] */ SAFEARRAY * *Values,
            /* [out] */ SAFEARRAY * *Errors);
        
        HRESULT ( STDMETHODCALLTYPE *AsyncRead )( 
            IOPCGroup * This,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ServerHandles,
            /* [out] */ SAFEARRAY * *Errors,
            /* [in] */ LONG TransactionID,
            /* [out] */ LONG *CancelID);
        
        HRESULT ( STDMETHODCALLTYPE *AsyncWrite )( 
            IOPCGroup * This,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ServerHandles,
            /* [in] */ SAFEARRAY * *Values,
            /* [out] */ SAFEARRAY * *Errors,
            /* [in] */ LONG TransactionID,
            /* [out] */ LONG *CancelID);
        
        HRESULT ( STDMETHODCALLTYPE *AsyncRefresh )( 
            IOPCGroup * This,
            /* [in] */ SHORT Source,
            /* [in] */ LONG TransactionID,
            /* [out] */ LONG *CancelID);
        
        HRESULT ( STDMETHODCALLTYPE *AsyncCancel )( 
            IOPCGroup * This,
            /* [in] */ LONG CancelID);
        
        END_INTERFACE
    } IOPCGroupVtbl;

    interface IOPCGroup
    {
        CONST_VTBL struct IOPCGroupVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IOPCGroup_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IOPCGroup_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IOPCGroup_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IOPCGroup_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IOPCGroup_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IOPCGroup_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IOPCGroup_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IOPCGroup_get_Parent(This,ppParent)	\
    ( (This)->lpVtbl -> get_Parent(This,ppParent) ) 

#define IOPCGroup_get_Name(This,Name)	\
    ( (This)->lpVtbl -> get_Name(This,Name) ) 

#define IOPCGroup_put_Name(This,Name)	\
    ( (This)->lpVtbl -> put_Name(This,Name) ) 

#define IOPCGroup_get_IsPublic(This,IsPublic)	\
    ( (This)->lpVtbl -> get_IsPublic(This,IsPublic) ) 

#define IOPCGroup_get_IsActive(This,IsActive)	\
    ( (This)->lpVtbl -> get_IsActive(This,IsActive) ) 

#define IOPCGroup_put_IsActive(This,IsActive)	\
    ( (This)->lpVtbl -> put_IsActive(This,IsActive) ) 

#define IOPCGroup_get_IsSubscribed(This,IsSubscribed)	\
    ( (This)->lpVtbl -> get_IsSubscribed(This,IsSubscribed) ) 

#define IOPCGroup_put_IsSubscribed(This,IsSubscribed)	\
    ( (This)->lpVtbl -> put_IsSubscribed(This,IsSubscribed) ) 

#define IOPCGroup_get_ClientHandle(This,ClientHandle)	\
    ( (This)->lpVtbl -> get_ClientHandle(This,ClientHandle) ) 

#define IOPCGroup_put_ClientHandle(This,ClientHandle)	\
    ( (This)->lpVtbl -> put_ClientHandle(This,ClientHandle) ) 

#define IOPCGroup_get_ServerHandle(This,ServerHandle)	\
    ( (This)->lpVtbl -> get_ServerHandle(This,ServerHandle) ) 

#define IOPCGroup_get_LocaleID(This,LocaleID)	\
    ( (This)->lpVtbl -> get_LocaleID(This,LocaleID) ) 

#define IOPCGroup_put_LocaleID(This,LocaleID)	\
    ( (This)->lpVtbl -> put_LocaleID(This,LocaleID) ) 

#define IOPCGroup_get_TimeBias(This,TimeBias)	\
    ( (This)->lpVtbl -> get_TimeBias(This,TimeBias) ) 

#define IOPCGroup_put_TimeBias(This,TimeBias)	\
    ( (This)->lpVtbl -> put_TimeBias(This,TimeBias) ) 

#define IOPCGroup_get_DeadBand(This,DeadBand)	\
    ( (This)->lpVtbl -> get_DeadBand(This,DeadBand) ) 

#define IOPCGroup_put_DeadBand(This,DeadBand)	\
    ( (This)->lpVtbl -> put_DeadBand(This,DeadBand) ) 

#define IOPCGroup_get_UpdateRate(This,UpdateRate)	\
    ( (This)->lpVtbl -> get_UpdateRate(This,UpdateRate) ) 

#define IOPCGroup_put_UpdateRate(This,UpdateRate)	\
    ( (This)->lpVtbl -> put_UpdateRate(This,UpdateRate) ) 

#define IOPCGroup_get_OPCItems(This,ppItems)	\
    ( (This)->lpVtbl -> get_OPCItems(This,ppItems) ) 

#define IOPCGroup_SyncRead(This,Source,NumItems,ServerHandles,Values,Errors,Qualities,TimeStamps)	\
    ( (This)->lpVtbl -> SyncRead(This,Source,NumItems,ServerHandles,Values,Errors,Qualities,TimeStamps) ) 

#define IOPCGroup_SyncWrite(This,NumItems,ServerHandles,Values,Errors)	\
    ( (This)->lpVtbl -> SyncWrite(This,NumItems,ServerHandles,Values,Errors) ) 

#define IOPCGroup_AsyncRead(This,NumItems,ServerHandles,Errors,TransactionID,CancelID)	\
    ( (This)->lpVtbl -> AsyncRead(This,NumItems,ServerHandles,Errors,TransactionID,CancelID) ) 

#define IOPCGroup_AsyncWrite(This,NumItems,ServerHandles,Values,Errors,TransactionID,CancelID)	\
    ( (This)->lpVtbl -> AsyncWrite(This,NumItems,ServerHandles,Values,Errors,TransactionID,CancelID) ) 

#define IOPCGroup_AsyncRefresh(This,Source,TransactionID,CancelID)	\
    ( (This)->lpVtbl -> AsyncRefresh(This,Source,TransactionID,CancelID) ) 

#define IOPCGroup_AsyncCancel(This,CancelID)	\
    ( (This)->lpVtbl -> AsyncCancel(This,CancelID) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IOPCGroup_INTERFACE_DEFINED__ */


#ifndef __DIOPCGroupEvent_DISPINTERFACE_DEFINED__
#define __DIOPCGroupEvent_DISPINTERFACE_DEFINED__

/* dispinterface DIOPCGroupEvent */
/* [helpstring][nonextensible][uuid] */ 


EXTERN_C const IID DIID_DIOPCGroupEvent;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("28E68F97-8D75-11d1-8DC3-3C302A000000")
    DIOPCGroupEvent : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct DIOPCGroupEventVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            DIOPCGroupEvent * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            DIOPCGroupEvent * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            DIOPCGroupEvent * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            DIOPCGroupEvent * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            DIOPCGroupEvent * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            DIOPCGroupEvent * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            DIOPCGroupEvent * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        END_INTERFACE
    } DIOPCGroupEventVtbl;

    interface DIOPCGroupEvent
    {
        CONST_VTBL struct DIOPCGroupEventVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define DIOPCGroupEvent_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define DIOPCGroupEvent_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define DIOPCGroupEvent_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define DIOPCGroupEvent_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define DIOPCGroupEvent_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define DIOPCGroupEvent_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define DIOPCGroupEvent_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* __DIOPCGroupEvent_DISPINTERFACE_DEFINED__ */


#ifndef __OPCItems_INTERFACE_DEFINED__
#define __OPCItems_INTERFACE_DEFINED__

/* interface OPCItems */
/* [unique][helpstring][uuid][oleautomation][dual][object] */ 


EXTERN_C const IID IID_OPCItems;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("28E68F98-8D75-11d1-8DC3-3C302A000000")
    OPCItems : public IDispatch
    {
    public:
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_Parent( 
            /* [retval][out] */ OPCGroup	**ppParent) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_DefaultRequestedDataType( 
            /* [retval][out] */ SHORT *DefaultRequestedDataType) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_DefaultRequestedDataType( 
            /* [in] */ SHORT DefaultRequestedDataType) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_DefaultAccessPath( 
            /* [retval][out] */ BSTR *DefaultAccessPath) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_DefaultAccessPath( 
            /* [string][in] */ BSTR DefaultAccessPath) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_DefaultIsActive( 
            /* [retval][out] */ VARIANT_BOOL *DefaultIsActive) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_DefaultIsActive( 
            /* [in] */ VARIANT_BOOL DefaultIsActive) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_Count( 
            /* [retval][out] */ LONG *Count) = 0;
        
        virtual /* [id][restricted][propget] */ HRESULT STDMETHODCALLTYPE get__NewEnum( 
            /* [retval][out] */ IUnknown **ppUnk) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE Item( 
            /* [in] */ VARIANT ItemSpecifier,
            /* [retval][out] */ OPCItem **ppItem) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetOPCItem( 
            /* [in] */ LONG ServerHandle,
            /* [retval][out] */ OPCItem **ppItem) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE AddItem( 
            /* [string][in] */ BSTR ItemID,
            /* [in] */ LONG ClientHandle,
            /* [retval][out] */ OPCItem **ppItem) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE AddItems( 
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ItemIDs,
            /* [in] */ SAFEARRAY * *ClientHandles,
            /* [out] */ SAFEARRAY * *ServerHandles,
            /* [out] */ SAFEARRAY * *Errors,
            /* [optional][in] */ VARIANT RequestedDataTypes,
            /* [optional][in] */ VARIANT AccessPaths) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Remove( 
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ServerHandles,
            /* [out] */ SAFEARRAY * *Errors) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Validate( 
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ItemIDs,
            /* [out] */ SAFEARRAY * *Errors,
            /* [optional][in] */ VARIANT RequestedDataTypes,
            /* [optional][in] */ VARIANT AccessPaths) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetActive( 
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ServerHandles,
            /* [in] */ VARIANT_BOOL ActiveState,
            /* [out] */ SAFEARRAY * *Errors) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetClientHandles( 
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ServerHandles,
            /* [in] */ SAFEARRAY * *ClientHandles,
            /* [out] */ SAFEARRAY * *Errors) = 0;
        
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetDataTypes( 
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ServerHandles,
            /* [in] */ SAFEARRAY * *RequestedDataTypes,
            /* [out] */ SAFEARRAY * *Errors) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct OPCItemsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            OPCItems * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            OPCItems * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            OPCItems * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            OPCItems * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            OPCItems * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            OPCItems * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            OPCItems * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Parent )( 
            OPCItems * This,
            /* [retval][out] */ OPCGroup	**ppParent);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DefaultRequestedDataType )( 
            OPCItems * This,
            /* [retval][out] */ SHORT *DefaultRequestedDataType);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DefaultRequestedDataType )( 
            OPCItems * This,
            /* [in] */ SHORT DefaultRequestedDataType);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DefaultAccessPath )( 
            OPCItems * This,
            /* [retval][out] */ BSTR *DefaultAccessPath);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DefaultAccessPath )( 
            OPCItems * This,
            /* [string][in] */ BSTR DefaultAccessPath);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DefaultIsActive )( 
            OPCItems * This,
            /* [retval][out] */ VARIANT_BOOL *DefaultIsActive);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DefaultIsActive )( 
            OPCItems * This,
            /* [in] */ VARIANT_BOOL DefaultIsActive);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Count )( 
            OPCItems * This,
            /* [retval][out] */ LONG *Count);
        
        /* [id][restricted][propget] */ HRESULT ( STDMETHODCALLTYPE *get__NewEnum )( 
            OPCItems * This,
            /* [retval][out] */ IUnknown **ppUnk);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *Item )( 
            OPCItems * This,
            /* [in] */ VARIANT ItemSpecifier,
            /* [retval][out] */ OPCItem **ppItem);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *GetOPCItem )( 
            OPCItems * This,
            /* [in] */ LONG ServerHandle,
            /* [retval][out] */ OPCItem **ppItem);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *AddItem )( 
            OPCItems * This,
            /* [string][in] */ BSTR ItemID,
            /* [in] */ LONG ClientHandle,
            /* [retval][out] */ OPCItem **ppItem);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *AddItems )( 
            OPCItems * This,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ItemIDs,
            /* [in] */ SAFEARRAY * *ClientHandles,
            /* [out] */ SAFEARRAY * *ServerHandles,
            /* [out] */ SAFEARRAY * *Errors,
            /* [optional][in] */ VARIANT RequestedDataTypes,
            /* [optional][in] */ VARIANT AccessPaths);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Remove )( 
            OPCItems * This,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ServerHandles,
            /* [out] */ SAFEARRAY * *Errors);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Validate )( 
            OPCItems * This,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ItemIDs,
            /* [out] */ SAFEARRAY * *Errors,
            /* [optional][in] */ VARIANT RequestedDataTypes,
            /* [optional][in] */ VARIANT AccessPaths);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetActive )( 
            OPCItems * This,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ServerHandles,
            /* [in] */ VARIANT_BOOL ActiveState,
            /* [out] */ SAFEARRAY * *Errors);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetClientHandles )( 
            OPCItems * This,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ServerHandles,
            /* [in] */ SAFEARRAY * *ClientHandles,
            /* [out] */ SAFEARRAY * *Errors);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *SetDataTypes )( 
            OPCItems * This,
            /* [in] */ LONG NumItems,
            /* [in] */ SAFEARRAY * *ServerHandles,
            /* [in] */ SAFEARRAY * *RequestedDataTypes,
            /* [out] */ SAFEARRAY * *Errors);
        
        END_INTERFACE
    } OPCItemsVtbl;

    interface OPCItems
    {
        CONST_VTBL struct OPCItemsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define OPCItems_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define OPCItems_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define OPCItems_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define OPCItems_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define OPCItems_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define OPCItems_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define OPCItems_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define OPCItems_get_Parent(This,ppParent)	\
    ( (This)->lpVtbl -> get_Parent(This,ppParent) ) 

#define OPCItems_get_DefaultRequestedDataType(This,DefaultRequestedDataType)	\
    ( (This)->lpVtbl -> get_DefaultRequestedDataType(This,DefaultRequestedDataType) ) 

#define OPCItems_put_DefaultRequestedDataType(This,DefaultRequestedDataType)	\
    ( (This)->lpVtbl -> put_DefaultRequestedDataType(This,DefaultRequestedDataType) ) 

#define OPCItems_get_DefaultAccessPath(This,DefaultAccessPath)	\
    ( (This)->lpVtbl -> get_DefaultAccessPath(This,DefaultAccessPath) ) 

#define OPCItems_put_DefaultAccessPath(This,DefaultAccessPath)	\
    ( (This)->lpVtbl -> put_DefaultAccessPath(This,DefaultAccessPath) ) 

#define OPCItems_get_DefaultIsActive(This,DefaultIsActive)	\
    ( (This)->lpVtbl -> get_DefaultIsActive(This,DefaultIsActive) ) 

#define OPCItems_put_DefaultIsActive(This,DefaultIsActive)	\
    ( (This)->lpVtbl -> put_DefaultIsActive(This,DefaultIsActive) ) 

#define OPCItems_get_Count(This,Count)	\
    ( (This)->lpVtbl -> get_Count(This,Count) ) 

#define OPCItems_get__NewEnum(This,ppUnk)	\
    ( (This)->lpVtbl -> get__NewEnum(This,ppUnk) ) 

#define OPCItems_Item(This,ItemSpecifier,ppItem)	\
    ( (This)->lpVtbl -> Item(This,ItemSpecifier,ppItem) ) 

#define OPCItems_GetOPCItem(This,ServerHandle,ppItem)	\
    ( (This)->lpVtbl -> GetOPCItem(This,ServerHandle,ppItem) ) 

#define OPCItems_AddItem(This,ItemID,ClientHandle,ppItem)	\
    ( (This)->lpVtbl -> AddItem(This,ItemID,ClientHandle,ppItem) ) 

#define OPCItems_AddItems(This,NumItems,ItemIDs,ClientHandles,ServerHandles,Errors,RequestedDataTypes,AccessPaths)	\
    ( (This)->lpVtbl -> AddItems(This,NumItems,ItemIDs,ClientHandles,ServerHandles,Errors,RequestedDataTypes,AccessPaths) ) 

#define OPCItems_Remove(This,NumItems,ServerHandles,Errors)	\
    ( (This)->lpVtbl -> Remove(This,NumItems,ServerHandles,Errors) ) 

#define OPCItems_Validate(This,NumItems,ItemIDs,Errors,RequestedDataTypes,AccessPaths)	\
    ( (This)->lpVtbl -> Validate(This,NumItems,ItemIDs,Errors,RequestedDataTypes,AccessPaths) ) 

#define OPCItems_SetActive(This,NumItems,ServerHandles,ActiveState,Errors)	\
    ( (This)->lpVtbl -> SetActive(This,NumItems,ServerHandles,ActiveState,Errors) ) 

#define OPCItems_SetClientHandles(This,NumItems,ServerHandles,ClientHandles,Errors)	\
    ( (This)->lpVtbl -> SetClientHandles(This,NumItems,ServerHandles,ClientHandles,Errors) ) 

#define OPCItems_SetDataTypes(This,NumItems,ServerHandles,RequestedDataTypes,Errors)	\
    ( (This)->lpVtbl -> SetDataTypes(This,NumItems,ServerHandles,RequestedDataTypes,Errors) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __OPCItems_INTERFACE_DEFINED__ */


#ifndef __OPCItem_INTERFACE_DEFINED__
#define __OPCItem_INTERFACE_DEFINED__

/* interface OPCItem */
/* [unique][helpstring][uuid][oleautomation][dual][object] */ 


EXTERN_C const IID IID_OPCItem;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("28E68F99-8D75-11d1-8DC3-3C302A000000")
    OPCItem : public IDispatch
    {
    public:
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_Parent( 
            /* [retval][out] */ OPCGroup	**Parent) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_ClientHandle( 
            /* [retval][out] */ LONG *ClientHandle) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_ClientHandle( 
            /* [in] */ LONG ClientHandle) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_ServerHandle( 
            /* [retval][out] */ LONG *ServerHandle) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_AccessPath( 
            /* [retval][out] */ BSTR *AccessPath) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_AccessRights( 
            /* [retval][out] */ LONG *AccessRights) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_ItemID( 
            /* [retval][out] */ BSTR *ItemID) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_IsActive( 
            /* [retval][out] */ VARIANT_BOOL *IsActive) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_IsActive( 
            /* [in] */ VARIANT_BOOL IsActive) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_RequestedDataType( 
            /* [retval][out] */ SHORT *RequestedDataType) = 0;
        
        virtual /* [helpstring][propput] */ HRESULT STDMETHODCALLTYPE put_RequestedDataType( 
            /* [in] */ SHORT RequestedDataType) = 0;
        
        virtual /* [helpstring][propget][id] */ HRESULT STDMETHODCALLTYPE get_Value( 
            /* [retval][out] */ VARIANT *CurrentValue) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_Quality( 
            /* [retval][out] */ LONG *Quality) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_TimeStamp( 
            /* [retval][out] */ DATE *TimeStamp) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_CanonicalDataType( 
            /* [retval][out] */ SHORT *CanonicalDataType) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_EUType( 
            /* [retval][out] */ SHORT *EUType) = 0;
        
        virtual /* [helpstring][propget] */ HRESULT STDMETHODCALLTYPE get_EUInfo( 
            /* [retval][out] */ VARIANT *EUInfo) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Read( 
            /* [in] */ SHORT Source,
            /* [optional][out] */ VARIANT *Value,
            /* [optional][out] */ VARIANT *Quality,
            /* [optional][out] */ VARIANT *TimeStamp) = 0;
        
        virtual HRESULT STDMETHODCALLTYPE Write( 
            /* [in] */ VARIANT Value) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct OPCItemVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            OPCItem * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            OPCItem * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            OPCItem * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            OPCItem * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            OPCItem * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            OPCItem * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            OPCItem * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Parent )( 
            OPCItem * This,
            /* [retval][out] */ OPCGroup	**Parent);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ClientHandle )( 
            OPCItem * This,
            /* [retval][out] */ LONG *ClientHandle);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_ClientHandle )( 
            OPCItem * This,
            /* [in] */ LONG ClientHandle);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ServerHandle )( 
            OPCItem * This,
            /* [retval][out] */ LONG *ServerHandle);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_AccessPath )( 
            OPCItem * This,
            /* [retval][out] */ BSTR *AccessPath);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_AccessRights )( 
            OPCItem * This,
            /* [retval][out] */ LONG *AccessRights);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_ItemID )( 
            OPCItem * This,
            /* [retval][out] */ BSTR *ItemID);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_IsActive )( 
            OPCItem * This,
            /* [retval][out] */ VARIANT_BOOL *IsActive);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_IsActive )( 
            OPCItem * This,
            /* [in] */ VARIANT_BOOL IsActive);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_RequestedDataType )( 
            OPCItem * This,
            /* [retval][out] */ SHORT *RequestedDataType);
        
        /* [helpstring][propput] */ HRESULT ( STDMETHODCALLTYPE *put_RequestedDataType )( 
            OPCItem * This,
            /* [in] */ SHORT RequestedDataType);
        
        /* [helpstring][propget][id] */ HRESULT ( STDMETHODCALLTYPE *get_Value )( 
            OPCItem * This,
            /* [retval][out] */ VARIANT *CurrentValue);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Quality )( 
            OPCItem * This,
            /* [retval][out] */ LONG *Quality);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_TimeStamp )( 
            OPCItem * This,
            /* [retval][out] */ DATE *TimeStamp);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CanonicalDataType )( 
            OPCItem * This,
            /* [retval][out] */ SHORT *CanonicalDataType);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_EUType )( 
            OPCItem * This,
            /* [retval][out] */ SHORT *EUType);
        
        /* [helpstring][propget] */ HRESULT ( STDMETHODCALLTYPE *get_EUInfo )( 
            OPCItem * This,
            /* [retval][out] */ VARIANT *EUInfo);
        
        HRESULT ( STDMETHODCALLTYPE *Read )( 
            OPCItem * This,
            /* [in] */ SHORT Source,
            /* [optional][out] */ VARIANT *Value,
            /* [optional][out] */ VARIANT *Quality,
            /* [optional][out] */ VARIANT *TimeStamp);
        
        HRESULT ( STDMETHODCALLTYPE *Write )( 
            OPCItem * This,
            /* [in] */ VARIANT Value);
        
        END_INTERFACE
    } OPCItemVtbl;

    interface OPCItem
    {
        CONST_VTBL struct OPCItemVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define OPCItem_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define OPCItem_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define OPCItem_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define OPCItem_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define OPCItem_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define OPCItem_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define OPCItem_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define OPCItem_get_Parent(This,Parent)	\
    ( (This)->lpVtbl -> get_Parent(This,Parent) ) 

#define OPCItem_get_ClientHandle(This,ClientHandle)	\
    ( (This)->lpVtbl -> get_ClientHandle(This,ClientHandle) ) 

#define OPCItem_put_ClientHandle(This,ClientHandle)	\
    ( (This)->lpVtbl -> put_ClientHandle(This,ClientHandle) ) 

#define OPCItem_get_ServerHandle(This,ServerHandle)	\
    ( (This)->lpVtbl -> get_ServerHandle(This,ServerHandle) ) 

#define OPCItem_get_AccessPath(This,AccessPath)	\
    ( (This)->lpVtbl -> get_AccessPath(This,AccessPath) ) 

#define OPCItem_get_AccessRights(This,AccessRights)	\
    ( (This)->lpVtbl -> get_AccessRights(This,AccessRights) ) 

#define OPCItem_get_ItemID(This,ItemID)	\
    ( (This)->lpVtbl -> get_ItemID(This,ItemID) ) 

#define OPCItem_get_IsActive(This,IsActive)	\
    ( (This)->lpVtbl -> get_IsActive(This,IsActive) ) 

#define OPCItem_put_IsActive(This,IsActive)	\
    ( (This)->lpVtbl -> put_IsActive(This,IsActive) ) 

#define OPCItem_get_RequestedDataType(This,RequestedDataType)	\
    ( (This)->lpVtbl -> get_RequestedDataType(This,RequestedDataType) ) 

#define OPCItem_put_RequestedDataType(This,RequestedDataType)	\
    ( (This)->lpVtbl -> put_RequestedDataType(This,RequestedDataType) ) 

#define OPCItem_get_Value(This,CurrentValue)	\
    ( (This)->lpVtbl -> get_Value(This,CurrentValue) ) 

#define OPCItem_get_Quality(This,Quality)	\
    ( (This)->lpVtbl -> get_Quality(This,Quality) ) 

#define OPCItem_get_TimeStamp(This,TimeStamp)	\
    ( (This)->lpVtbl -> get_TimeStamp(This,TimeStamp) ) 

#define OPCItem_get_CanonicalDataType(This,CanonicalDataType)	\
    ( (This)->lpVtbl -> get_CanonicalDataType(This,CanonicalDataType) ) 

#define OPCItem_get_EUType(This,EUType)	\
    ( (This)->lpVtbl -> get_EUType(This,EUType) ) 

#define OPCItem_get_EUInfo(This,EUInfo)	\
    ( (This)->lpVtbl -> get_EUInfo(This,EUInfo) ) 

#define OPCItem_Read(This,Source,Value,Quality,TimeStamp)	\
    ( (This)->lpVtbl -> Read(This,Source,Value,Quality,TimeStamp) ) 

#define OPCItem_Write(This,Value)	\
    ( (This)->lpVtbl -> Write(This,Value) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __OPCItem_INTERFACE_DEFINED__ */


#ifndef __IOPCActivator_INTERFACE_DEFINED__
#define __IOPCActivator_INTERFACE_DEFINED__

/* interface IOPCActivator */
/* [unique][helpstring][uuid][oleautomation][dual][object] */ 


EXTERN_C const IID IID_IOPCActivator;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("860A4800-46A4-478b-A776-7F3A019369E3")
    IOPCActivator : public IDispatch
    {
    public:
        virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE Attach( 
            /* [in] */ IUnknown *Server,
            /* [string][in] */ BSTR ProgID,
            /* [optional][in] */ VARIANT NodeName,
            /* [retval][out] */ IOPCAutoServer **ppWrapper) = 0;
        
    };
    
    
#else 	/* C style interface */

    typedef struct IOPCActivatorVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IOPCActivator * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IOPCActivator * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IOPCActivator * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IOPCActivator * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IOPCActivator * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IOPCActivator * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IOPCActivator * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        /* [helpstring] */ HRESULT ( STDMETHODCALLTYPE *Attach )( 
            IOPCActivator * This,
            /* [in] */ IUnknown *Server,
            /* [string][in] */ BSTR ProgID,
            /* [optional][in] */ VARIANT NodeName,
            /* [retval][out] */ IOPCAutoServer **ppWrapper);
        
        END_INTERFACE
    } IOPCActivatorVtbl;

    interface IOPCActivator
    {
        CONST_VTBL struct IOPCActivatorVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IOPCActivator_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IOPCActivator_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IOPCActivator_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IOPCActivator_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IOPCActivator_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IOPCActivator_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IOPCActivator_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IOPCActivator_Attach(This,Server,ProgID,NodeName,ppWrapper)	\
    ( (This)->lpVtbl -> Attach(This,Server,ProgID,NodeName,ppWrapper) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IOPCActivator_INTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_OPCActivator;

#ifdef __cplusplus

class DECLSPEC_UUID("860A4801-46A4-478b-A776-7F3A019369E3")
OPCActivator;
#endif

EXTERN_C const CLSID CLSID_OPCServer;

#ifdef __cplusplus

class DECLSPEC_UUID("28E68F9A-8D75-11d1-8DC3-3C302A000000")
OPCServer;
#endif

EXTERN_C const CLSID CLSID_OPCGroups;

#ifdef __cplusplus

class DECLSPEC_UUID("28E68F9E-8D75-11d1-8DC3-3C302A000000")
OPCGroups;
#endif

EXTERN_C const CLSID CLSID_OPCGroup;

#ifdef __cplusplus

class DECLSPEC_UUID("28E68F9B-8D75-11d1-8DC3-3C302A000000")
OPCGroup;
#endif
#endif /* __OPCAutomation_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

unsigned long             __RPC_USER  BSTR_UserSize(     unsigned long *, unsigned long            , BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserMarshal(  unsigned long *, unsigned char *, BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserUnmarshal(unsigned long *, unsigned char *, BSTR * ); 
void                      __RPC_USER  BSTR_UserFree(     unsigned long *, BSTR * ); 

unsigned long             __RPC_USER  LPSAFEARRAY_UserSize(     unsigned long *, unsigned long            , LPSAFEARRAY * ); 
unsigned char * __RPC_USER  LPSAFEARRAY_UserMarshal(  unsigned long *, unsigned char *, LPSAFEARRAY * ); 
unsigned char * __RPC_USER  LPSAFEARRAY_UserUnmarshal(unsigned long *, unsigned char *, LPSAFEARRAY * ); 
void                      __RPC_USER  LPSAFEARRAY_UserFree(     unsigned long *, LPSAFEARRAY * ); 

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


