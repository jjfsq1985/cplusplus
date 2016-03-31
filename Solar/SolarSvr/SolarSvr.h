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

private:
    void InitSvr();
    bool SolarPaint();
    void RegisterWindowClass();
    void CreateWindowInstance(int nCmdShow);

private:
    class IniFile *m_pCfgParam;
    TcpServer svr;
    // Windows interface stuff
    HINSTANCE hInst;
    HWND hWnd;
    ATOM classAtom;
    TCHAR szTitle[MAX_LOADSTRING];
    TCHAR szWindowClass[MAX_LOADSTRING];
};