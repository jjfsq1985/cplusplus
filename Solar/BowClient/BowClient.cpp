// SolarSvr.cpp : 定义应用程序的入口点。
//

#include "stdafx.h"
#include "BowClient.h"

static LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    LONG_PTR DataPtr = GetWindowLongPtr(hWnd, GWLP_USERDATA);
    if (DataPtr != NULL)
    {
        BowClient *pClient = reinterpret_cast<BowClient *>(DataPtr);
        pClient->BowClientWndProc(hWnd, message, wParam, lParam);
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

BowClient::BowClient(HINSTANCE hInstance)
    :hInst(hInstance)
    , hWnd(NULL)
    , classAtom(0)
{
}

BowClient::~BowClient()
{
}

void BowClient::Initialize(int nCmdShow)
{
    LoadString(hInst, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
    LoadString(hInst, IDC_BOWCLIENT, szWindowClass, MAX_LOADSTRING);

    RegisterWindowClass();
    CreateWindowInstance(nCmdShow);
}

void BowClient::CreateWindowInstance(int nCmdShow)
{
    hWnd = CreateWindow(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW | WS_MAXIMIZE,
        CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, NULL, NULL, hInst, NULL);

    if (!hWnd)
        return;
    SetWindowLongPtr(hWnd, GWLP_USERDATA, (LONG_PTR)this);
    ShowWindow(hWnd, nCmdShow);
    UpdateWindow(hWnd);
    SetScrollPos(hWnd, SB_HORZ, 0, TRUE);
    SetScrollPos(hWnd, SB_VERT, 0, TRUE);
}

HWND BowClient::CreateImageWnd(HWND parent,RECT rcWnd)
{
    return CreateWindow(szWindowClass, szTitle, WS_CHILD|WS_VISIBLE,
        rcWnd.left, rcWnd.top, rcWnd.right - rcWnd.left, rcWnd.bottom - rcWnd.top, parent, NULL, hInst, NULL);
}

void BowClient::RegisterWindowClass()
{
    WNDCLASSEX wcex;

    wcex.cbSize = sizeof(WNDCLASSEX);

    wcex.style = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc = (WNDPROC)WndProc;
    wcex.cbClsExtra = 0;
    wcex.cbWndExtra = 0;
    wcex.hInstance = hInst;
    wcex.hIcon = LoadIcon(hInst, MAKEINTRESOURCE(IDI_BOWCLIENT));
    wcex.hCursor = LoadCursor(NULL, IDC_ARROW);
    wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);  // no background drawing, please
    wcex.lpszMenuName = MAKEINTRESOURCE(IDC_BOWCLIENT);
    wcex.lpszClassName = szWindowClass;
    wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

    classAtom = RegisterClassEx(&wcex);
}

int BowClient::Run()
{
    MSG msg;
    HACCEL hAccelTable = LoadAccelerators(hInst, MAKEINTRESOURCE(IDC_BOWCLIENT));
    InitBowClient();

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

LRESULT BowClient::BowClientWndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
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
        bRet = BowPaint();
        break;
    case WM_DESTROY:
        m_bCapture = false;
        WaitForSingleObject(m_hThread, INFINITE);
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

bool BowClient::BowPaint()
{
    PAINTSTRUCT ps;
    HDC hdc = BeginPaint(hWnd, &ps);
    //绘图
    EndPaint(hWnd, &ps);
    return true;;
}

void BowClient::InitBowClient()
{
    set_system("do_low_error", "false");
    DWORD threadid;
    m_bCapture = true;
    m_hThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)&CameraAction, this, 0, &threadid);
}

UINT BowClient::CameraAction(LPVOID pParam)
{
    BowClient *pWnd = (BowClient*)pParam;
    const long ImageHeight = 480;
    const long ImageWidth = 640;

    RECT rcClient;
    GetClientRect(pWnd->hWnd, &rcClient);

    Hlong nWidth = (rcClient.right - rcClient.left) /2;
    Hlong nClientCenter = nWidth;
    Hlong nHeight = rcClient.bottom - rcClient.top;
    nHeight = nHeight > 480 ? 480 : nHeight;

    Hlong x1, y1, x2, y2;
    x1 = nWidth > 640 ? 0 : (640 - nWidth) / 2;
    y1 = nHeight > 480 ? 0 : (480 - nHeight) / 2;
    x2 = nWidth > 640 ? 640 : (640 + nWidth) / 2;
    y2 = nHeight > 480 ? 480 : (480 + nHeight) / 2;

    RECT rcCaptureClient = { 0, 0, nWidth, nHeight };
    HWND hCaptureWnd = pWnd->CreateImageWnd(pWnd->hWnd, rcCaptureClient);
    HTuple hWindowHandle;
    open_window(0, 0, nWidth, nHeight, (Hlong)hCaptureWnd, "visible", "", &hWindowHandle);

    set_colored(hWindowHandle, 12);
    set_draw(hWindowHandle, "margin");
    set_line_width(hWindowHandle, 1);
    set_shape(hWindowHandle, "original");
    set_lut(hWindowHandle, "default");
    set_paint(hWindowHandle, "default");

    set_part(hWindowHandle, y1, x1, y2, x2);//很重要，否则显示图片不完整，Row为Y, column为X

    RECT rcScratchClient = { nClientCenter, 0, nClientCenter + nWidth, nHeight };
    HWND hScratchWnd = pWnd->CreateImageWnd(pWnd->hWnd, rcScratchClient);
    HTuple hDestWndHandle;
    open_window(0, 0, nWidth, nHeight, (Hlong)hScratchWnd, "visible", "", &hDestWndHandle);

    set_colored(hDestWndHandle, 12);
    set_draw(hDestWndHandle, "margin");
    set_line_width(hDestWndHandle, 4);
    set_shape(hDestWndHandle, "original");
    set_lut(hDestWndHandle, "default");
    set_paint(hDestWndHandle, "default");

    set_part(hDestWndHandle, y1, x1, y2, x2);//很重要，否则显示图片不完整，Row2为Height, column2为Width


    // Local iconic variables 
    Hobject  Image;

    // Local control variables 
    HTuple  AcqHandle;

    //Code generated by Image Acquisition 01
    //Attention: The initialization may fail in case parameters need to
    //be set in a specific order (e.g., image resolution vs. offset).
    open_framegrabber("DirectShow", 1, 1, 0, 0, 0, 0, "default", 8, "rgb", -1, "false",
        "default", "Lenovo EasyCamera", 0, -1, &AcqHandle);

    grab_image_start(AcqHandle, -1);
    while (pWnd->m_bCapture)
    {
        grab_image_async(&Image, AcqHandle, -1);//异步
        //mirror_image(Image, &MirrorImage, "column");//镜面图像
        //write_image(MirrorImage, "bmp", 16711680, "D:\\12.bmp"); //存文件
        //disp_image(MirrorImage, hWindowHandle);//黑白图像
        disp_color(Image, hWindowHandle);
        pWnd->DealImage(Image, hDestWndHandle);
        Sleep(50);
    }
    close_framegrabber(AcqHandle);
    return 0;
}

void BowClient::DealImage(const Hobject &Image, const HTuple& hWndHandle)
{
    //将处理后的图像在另一部分显示
    disp_image(Image, hWndHandle);
    Hobject  ImageMean;
    Hobject DarkPixels;
    mean_image(Image, &ImageMean, 31, 31);
    dyn_threshold(Image, ImageMean, &DarkPixels, 20, "dark");
    Hobject ConnectedRegions;
    Hobject SelectedRegions;
    connection(DarkPixels, &ConnectedRegions);
    select_shape(ConnectedRegions, &SelectedRegions, "area", "and", 20, 1000);
    Hobject RegionUnion;
    Hobject RegionDilation;
    union1(SelectedRegions, &RegionUnion);
    dilation_circle(RegionUnion, &RegionDilation, 3.5);
    Hobject Skeleton;
    Hobject Errors;
    skeleton(RegionDilation, &Skeleton);
    connection(Skeleton, &Errors);
    Hobject Scratches;   //划痕
    Hobject Dots;  //孔洞
    select_shape(Errors, &Scratches, "area", "and", 50, 10000);
    select_shape(Errors, &Dots, "area", "and", 10, 50);
    set_color(hWndHandle, "red");
    disp_region(Scratches, hWndHandle);
    set_color(hWndHandle, "blue");
    disp_region(Dots, hWndHandle);
}