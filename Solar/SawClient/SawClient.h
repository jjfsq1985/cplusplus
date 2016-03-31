#pragma once

#include "resource.h"
#include "TcpSaw.h"
#include "IniFile.h"
using namespace inifile;

#define MAX_LOADSTRING 256

class SawClient
{
public:
    SawClient(HINSTANCE hInstance);
    ~SawClient();

public:
    LRESULT SawClientWndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
    void Initialize(int nCmdShow);
    int Run();

private:
    void InitSawClient();
    bool SawPaint();
    void RegisterWindowClass();
    void CreateWindowInstance(int nCmdShow);

private:
    class IniFile *m_pCfgParam;
    TcpSaw saw;
    // Windows interface stuff
    HINSTANCE hInst;
    HWND hWnd;
    ATOM classAtom;
    TCHAR szTitle[MAX_LOADSTRING];
    TCHAR szWindowClass[MAX_LOADSTRING];
};