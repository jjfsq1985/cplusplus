// SolarSvr.cpp : 定义应用程序的入口点。
//

#include "stdafx.h"
#include "SolarSvr.h"

static LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    LONG_PTR DataPtr = GetWindowLongPtr(hWnd, GWLP_USERDATA);
    if (DataPtr != NULL)
    {
        SolarSvr *pSvr = reinterpret_cast<SolarSvr *>(DataPtr);
        pSvr->SolarSvrWndProc(hWnd, message, wParam, lParam);
    }
    return DefWindowProc(hWnd, message, wParam, lParam);
}

// “关于”框的消息处理程序。
static INT_PTR CALLBACK AboutProc(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
    UNREFERENCED_PARAMETER(lParam);
    switch (message)
    {
    case WM_INITDIALOG:
        return (INT_PTR)TRUE;

    case WM_COMMAND:
        if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
        {
            EndDialog(hDlg, LOWORD(wParam));
            return (INT_PTR)TRUE;
        }
        break;
    }
    return (INT_PTR)FALSE;
}

SolarSvr::SolarSvr(HINSTANCE hInstance)
    :hInst(hInstance)
    , hWnd(NULL)
    , classAtom(0)
    , m_pCfgParam(NULL)
{
    InitSvr();
}

SolarSvr::~SolarSvr()
{
    if (m_pCfgParam != NULL)
        delete m_pCfgParam;
    m_pCfgParam = NULL;
}

void SolarSvr::Initialize(int nCmdShow)
{
    LoadString(hInst, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
    LoadString(hInst, IDC_SOLARSVR, szWindowClass, MAX_LOADSTRING);

    RegisterWindowClass();
    CreateWindowInstance(nCmdShow);
}

void SolarSvr::CreateWindowInstance(int nCmdShow)
{
    hWnd = CreateWindow(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, NULL, NULL, hInst, NULL);

    if (!hWnd)
        return;
    SetWindowLongPtr(hWnd, GWLP_USERDATA, (LONG_PTR)this);
    ShowWindow(hWnd, nCmdShow);
    UpdateWindow(hWnd);
    SetScrollPos(hWnd, SB_HORZ, 0, TRUE);
    SetScrollPos(hWnd, SB_VERT, 0, TRUE);
}

void SolarSvr::RegisterWindowClass()
{
    WNDCLASSEX wcex;

    wcex.cbSize = sizeof(WNDCLASSEX);

    wcex.style = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc = (WNDPROC)WndProc;
    wcex.cbClsExtra = 0;
    wcex.cbWndExtra = 0;
    wcex.hInstance = hInst;
    wcex.hIcon = LoadIcon(hInst, MAKEINTRESOURCE(IDI_SOLARSVR));
    wcex.hCursor = LoadCursor(NULL, IDC_ARROW);
    wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);  // no background drawing, please
    wcex.lpszMenuName = MAKEINTRESOURCE(IDC_SOLARSVR);
    wcex.lpszClassName = szWindowClass;
    wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

    classAtom = RegisterClassEx(&wcex);
}

int SolarSvr::Run()
{
    MSG msg;
    HACCEL hAccelTable = LoadAccelerators(hInst, MAKEINTRESOURCE(IDC_SOLARSVR));

    // 主消息循环: 
    while (GetMessage(&msg, NULL, 0, 0))
    {
        if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
        {
            TranslateMessage(&msg);
            DispatchMessage(&msg);
        }
    }
    return (int)msg.wParam;
}

LRESULT SolarSvr::SolarSvrWndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    bool bRet = false;
    int wmId, wmEvent;

    switch (message)
    {
    case WM_COMMAND:
        wmId = LOWORD(wParam);
        wmEvent = HIWORD(wParam);
        // 分析菜单选择: 
        switch (wmId)
        {
        case IDM_ABOUT:
            DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, AboutProc);
            break;
        case IDM_EXIT:
            DestroyWindow(hWnd);
            break;
        default:
            return DefWindowProc(hWnd, message, wParam, lParam);
        }
        break;
    case WM_MENUSELECT:
        break;
    case WM_PAINT:
        bRet = SolarPaint();
        break;
    case WM_DESTROY:
        PostQuitMessage(0);
        break;
    case WM_LBUTTONDOWN:
        break;
    case WM_LBUTTONUP:
        break;
    case WM_MOUSEMOVE:
        break;
    case WM_HSCROLL:
    case WM_VSCROLL:
       break;
    case WM_SIZING:
        break;
    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }

    return bRet ? 0 : 1;
}

bool SolarSvr::SolarPaint()
{
    PAINTSTRUCT ps;
    HDC hdc = BeginPaint(hWnd, &ps);
    //绘图
    EndPaint(hWnd, &ps);
    return true;;
}

void SolarSvr::InitSvr()
{
    m_pCfgParam = new class IniFile();
    m_pCfgParam->load("solar.ini");
    string strPort = "9999";
    m_pCfgParam->getValue("Messstation", "STEUERUNG", "PORT", strPort);
    int nPort = atoi(strPort.c_str());
    svr.Init(nPort);
}