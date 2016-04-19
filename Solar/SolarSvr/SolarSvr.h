#pragma once

#include "resource.h"
#include "TcpServer.h"
#include "IniFile.h"
using namespace inifile;

#define MAX_LOADSTRING 256

class SolarSvr
{
public:
    SolarSvr(HINSTANCE hInstance);
    ~SolarSvr();

public:
    LRESULT SolarSvrWndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
    void Initialize(int nCmdShow);
    int Run();

protected:
    BOOL hScrollActive_;
    BOOL vScrollActive_;
    BOOL mouseScrollActive_;
    int xMouseDown_;
    int yMouseDown_;

    void updateScrollBarInfo(bool center = false);
    void getScrollPosition(int &xx, int &yy);
    void setScrollPosition(int xx, int yy);


private:
    void InitSvr();
    bool SolarPaint();
    void RegisterWindowClass();
    void CreateWindowInstance(int nCmdShow);
    bool processCmd(int wmId, int wmEvent, LPARAM lParam);
    bool processOpcMenu(int wmId, int wmEvent, LPARAM lParam);

private:
    class OpcCtrl *m_pOpc;
    class IniFile *m_pCfgParam;
    TcpServer svr;
    // Windows interface stuff
    HINSTANCE hInst;
    HWND hWnd;
    ATOM classAtom;
    TCHAR szTitle[MAX_LOADSTRING];
    TCHAR szWindowClass[MAX_LOADSTRING];
};