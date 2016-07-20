// SolarSvr.cpp : 定义应用程序的入口点。
//

#include "stdafx.h"
#include "SawClient.h"
#include <math.h>
#include "kfft.h"


static LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    LONG_PTR DataPtr = GetWindowLongPtr(hWnd, GWLP_USERDATA);
    if (DataPtr != NULL)
    {
        SawClient *pClient = reinterpret_cast<SawClient *>(DataPtr);
        pClient->SawClientWndProc(hWnd, message, wParam, lParam);
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

SawClient::SawClient(HINSTANCE hInstance)
    :hInst(hInstance)
    , hWnd(NULL)
    , classAtom(0)
    , m_pCfgParam(NULL)
{
	m_nIndex = 0;
	m_pDataY = new float[2048];
	m_pDataX = new float[2048];
    InitSawClient();
}

SawClient::~SawClient()
{
	delete[] m_pDataY;
	delete[] m_pDataX;
}

void SawClient::Initialize(int nCmdShow)
{
    LoadString(hInst, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
    LoadString(hInst, IDC_SAWCLIENT, szWindowClass, MAX_LOADSTRING);

    RegisterWindowClass();
    CreateWindowInstance(nCmdShow);
}

void SawClient::CreateWindowInstance(int nCmdShow)
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
	SetTimer(hWnd, 1, 500, TimeRefresh);
}

void SawClient::RegisterWindowClass()
{
    WNDCLASSEX wcex;

    wcex.cbSize = sizeof(WNDCLASSEX);

    wcex.style = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc = (WNDPROC)WndProc;
    wcex.cbClsExtra = 0;
    wcex.cbWndExtra = 0;
    wcex.hInstance = hInst;
    wcex.hIcon = LoadIcon(hInst, MAKEINTRESOURCE(IDI_SAWCLIENT));
    wcex.hCursor = LoadCursor(NULL, IDC_ARROW);
    wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);  // no background drawing, please
    wcex.lpszMenuName = MAKEINTRESOURCE(IDC_SAWCLIENT);
    wcex.lpszClassName = szWindowClass;
    wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

    classAtom = RegisterClassEx(&wcex);
}

int SawClient::Run()
{
    MSG msg;
    HACCEL hAccelTable = LoadAccelerators(hInst, MAKEINTRESOURCE(IDC_SAWCLIENT));

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

LRESULT SawClient::SawClientWndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
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
        bRet = SawPaint();
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

bool SawClient::SawPaint()
{
    PAINTSTRUCT ps;
    HDC hdc = BeginPaint(hWnd, &ps);
	POINT a = POINT{ ps.rcPaint.left, ps.rcPaint.top };
	ClientToScreen(hWnd, &a);
    //绘图
	float fltScope = 100.0f;
	float nSampleFreq = 10240.0f;
	float nSignalFreq = 400.0f;	
	int yTime = ps.rcPaint.top + 6 + (ps.rcPaint.bottom - ps.rcPaint.top - 12) / 4;
	int ySpec = ps.rcPaint.bottom - 6;
	int x = ps.rcPaint.left + 5;
	Rectangle(hdc, ps.rcPaint.left + 5, ps.rcPaint.top + 5, ps.rcPaint.right - 5, ps.rcPaint.bottom - 5);
	double nXRadio = (ps.rcPaint.right - ps.rcPaint.left - 12)*1.0 / 2048;
	double nYTimeRadio = (ps.rcPaint.bottom - ps.rcPaint.top - 12)*1.0 / (fltScope+50) / 4;

	HPEN penLine = CreatePen(PS_SOLID, 1, RGB(0, 0, 255));
	HPEN oldpen = (HPEN)SelectObject(hdc, penLine);

	MoveToEx(hdc, x, yTime, NULL);
	for (int i = 0; i < 2048; i++)
	{
		int nIndex = i + m_nIndex;
		m_pDataY[i] = 100 * sin(2 * M_PI * 1.1f * nIndex * nSignalFreq / nSampleFreq);
		//m_pDataY[i] += 30 * cos(2 * M_PI * 1.1f * nIndex * 100.0f / nSampleFreq);//噪声
		LineTo(hdc, x + i*nXRadio, yTime - m_pDataY[i] * nYTimeRadio);
	}
	m_nIndex += 2048;

	float *pImage = new float[2048];
	float *pFftReal = new float[2048];
	float *pFftImage = new float[2048];
	for (int i = 0; i < 2048; i++)
	{
		pImage[i] = 0.0f;
		pFftReal[i] = 0.0f;
		pFftImage[i] = 0.0f;
	}
	FourierTransform::kfft(m_pDataY, pImage, 2048, 11, pFftReal, pFftImage, 0, 1);

	double dbMax = 0.0;
	int nPos = 0;
	for (int i = 0; i < 1024; i++)
	{
		if (m_pDataY[i] > dbMax)
		{
			dbMax = m_pDataY[i];
			nPos = i;
		}
	}
	int nCalcPos = 2048 * nSignalFreq / nSampleFreq;

	double nYSpecRadio = (ps.rcPaint.bottom - ps.rcPaint.top - 12)*1.0 / dbMax  / 2;
	char cText[32];
	sprintf_s(cText, 32, "POS: %d, Calc Pos: %d", nPos, nCalcPos);
	TextOutA(hdc, x, ySpec - (ps.rcPaint.bottom - ps.rcPaint.top - 12) / 2, cText, strlen(cText));
	MoveToEx(hdc, x, ySpec, NULL);
	for (int i = 0; i < 1024; i++)
	{
		LineTo(hdc, x + i*nXRadio*2, ySpec - m_pDataY[i] * nYSpecRadio);
	}

	SelectObject(hdc, oldpen);
	DeleteObject(penLine);
    
	EndPaint(hWnd, &ps);
    return true;;
}

void SawClient::InitSawClient()
{
    m_pCfgParam = new class IniFile();
    m_pCfgParam->load("SawClient.cfg");
    string strPort = "9999";
    string strHost = "127.0.0.1";
    m_pCfgParam->getValue("Messstation", "STEUERUNG", "HOST", strHost);
    m_pCfgParam->getValue("Messstation", "STEUERUNG", "PORT", strPort);
    int nPort = atoi(strPort.c_str());
    
    saw.Init(strHost.c_str(), nPort);
}

void CALLBACK SawClient::TimeRefresh(HWND hwnd, UINT message, UINT iTimerID, DWORD dwTime)
{
	InvalidateRect(hwnd, NULL, FALSE);
}