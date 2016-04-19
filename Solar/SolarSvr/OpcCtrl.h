#pragma once
#include <atlbase.h>  //CComPtr
#include <comdef.h>//_bstr_t
#include "OPCAuto.h"
#include <vector>
using namespace std;

typedef struct tagOpcItem
{
    _bstr_t sItemId;
    _bstr_t sItem;
    _bstr_t sParent;
    bool bLeafs;
}OpcItem;

class OpcCtrl
{
public:
    OpcCtrl();
    ~OpcCtrl();

public:
    bool getListOfServers(_bstr_t sNode, vector<_bstr_t>& serverlist);
    bool connectServer(_bstr_t serverName, _bstr_t sNode = "");
    bool DisconnectServer();
    bool BrowserBranches(vector<OpcItem>& vecBranches);
    bool BrowserLeafs(_bstr_t sBranch, vector<OpcItem>& vecLeafs);
    bool QueryItemProperties(_bstr_t item, LONG& PropCount, vector<LONG>& vecPropID, vector<_bstr_t>& vecPropDesc, vector<LONG>& vecDataType);
    bool GetItemProperties(_bstr_t item, LONG PropCount, vector<LONG> vecPropID, vector<VARIANT>& vecPropValue, vector<LONG>& vecErrors);

public:
    bool AddGroup(_bstr_t groupName, LONG UpdateRate, bool bSubscribed);
    bool RemoveGroup(_bstr_t groupName);


private:
    IUnknown *m_pUnk;
    ATL::CComPtr<IOPCGroup> m_ActiveOPCGroup;
};

