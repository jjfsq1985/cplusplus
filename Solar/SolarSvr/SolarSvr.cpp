// SolarSvr.cpp : 定义应用程序的入口点。
//

#include "stdafx.h"
#include "SolarSvr.h"
#include "OpcCtrl.h"
#include "opcda.h"

#define GET_X_LPARAM(LL) (MAKEPOINTS(LL).x)
#define GET_Y_LPARAM(LL) (MAKEPOINTS(LL).y)

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
    , m_pOpc(NULL)
    , hScrollActive_(FALSE)
    , vScrollActive_(FALSE)
    , mouseScrollActive_(FALSE)
    , xMouseDown_(0)
    , yMouseDown_(0)
{
    InitSvr();
}

SolarSvr::~SolarSvr()
{
    if (m_pOpc != NULL)
        delete m_pOpc;
    m_pOpc = NULL;
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
    updateScrollBarInfo(false);
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

bool SolarSvr::processCmd(int wmId, int wmEvent, LPARAM lParam)
{
    bool bProcessed = true;

    if (wmId >= IDM_OPC_START && wmId <= IDM_OPC_END)
    {
        processOpcMenu(wmId, wmEvent, lParam);
        return true;
    }

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
        bProcessed = false;
    }
    return bProcessed;
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
        bRet = processCmd(wmId, wmEvent, lParam);
        break;
    case WM_MENUSELECT:
        break;
    case WM_PAINT:
        bRet = SolarPaint();
        break;
    case WM_DESTROY:
        PostQuitMessage(0);
        bRet = true;
        break;
    case WM_LBUTTONDOWN:
        if (hScrollActive_ || vScrollActive_)
        {
            // embed the current scroll position in the "mouseDown" members
            getScrollPosition(xMouseDown_, yMouseDown_);
            // and offset by the clicked position
            xMouseDown_ += GET_X_LPARAM(lParam);
            yMouseDown_ += GET_Y_LPARAM(lParam);

            // set the activity flag, and capture the mouse so drag events can leave the client area
            mouseScrollActive_ = TRUE;
            SetCapture(hWnd);
        }
        break;
    case WM_LBUTTONUP:
        if (mouseScrollActive_)
        {
            // done dragging around
            xMouseDown_ = 0;
            yMouseDown_ = 0;
            mouseScrollActive_ = 0;
            ReleaseCapture();
        }
        break;
    case WM_MOUSEMOVE:
        if (mouseScrollActive_)
        {
            // subtract off the current mouse coordinate to
            // get back to the desired scroll offset
            int deltaX = xMouseDown_ - GET_X_LPARAM(lParam);
            int deltaY = yMouseDown_ - GET_Y_LPARAM(lParam);

            // push that offset to the scroll bars
            setScrollPosition(deltaX, deltaY);

            // and redraw
            RECT cr;
            GetClientRect(hWnd, &cr);
            InvalidateRect(hWnd, &cr, FALSE);
        }
        break;
    case WM_HSCROLL:
    case WM_VSCROLL:
    {
        int sbId = (message == WM_HSCROLL ? SB_HORZ : SB_VERT);
        SCROLLINFO si;
        si.cbSize = sizeof(SCROLLINFO);
        si.fMask = SIF_ALL;
        GetScrollInfo(hWnd, sbId, &si);
        switch (LOWORD(wParam))
        {
        case SB_THUMBPOSITION:
            si.nPos = si.nTrackPos;
            break;
        case SB_LEFT:
        case SB_LINELEFT:
            si.nPos -= 10;
            break;
        case SB_RIGHT:
        case SB_LINERIGHT:
            si.nPos += 10;
            break;
        case SB_PAGELEFT:
            si.nPos -= si.nPage;
            break;
        case SB_PAGERIGHT:
            si.nPos += si.nPage;
            break;
        }
        si.fMask = SIF_POS;
        int thePos = (LOWORD(wParam) == SB_THUMBTRACK ? si.nTrackPos : si.nPos);
        if (thePos < 0)
            thePos = 0;
        if (thePos >= si.nMax)
            thePos = si.nMax - 1;
        si.nPos = thePos;
        SetScrollInfo(hWnd, sbId, &si, TRUE);
        RECT cr;
        GetClientRect(hWnd, &cr);
        InvalidateRect(hWnd, &cr, FALSE);
    }
    break;
    case WM_SIZING:
    {
        // maintain a minimum size of 240x240
        // note: this functionality could also be embedded in a generic parent
        RECT *pRect = (RECT *)(lParam);
        if ((pRect->bottom - pRect->top) < 240)
        {
            if (wParam == WMSZ_TOP || wParam == WMSZ_TOPLEFT || wParam == WMSZ_TOPRIGHT)
            {
                pRect->top = pRect->bottom - 240;
            }
            else {
                pRect->bottom = pRect->top + 240;
            }
        }

        if ((pRect->right - pRect->left) < 240)
        {
            if (wParam == WMSZ_LEFT || wParam == WMSZ_TOPLEFT || wParam == WMSZ_BOTTOMLEFT)
            {
                pRect->left = pRect->right - 240;
            }
            else {
                pRect->right = pRect->left + 240;
            }
        }
        // once we're done wrestling size stuff, update scrollbar stuff
        updateScrollBarInfo(false);
        return TRUE;
    }
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
    m_pOpc = new OpcCtrl();
    m_pCfgParam = new class IniFile();
    m_pCfgParam->load("solar.ini");
    string strPort = "9999";
    m_pCfgParam->getValue("Messstation", "STEUERUNG", "PORT", strPort);
    int nPort = atoi(strPort.c_str());
    svr.Init(nPort);
}

void SolarSvr::updateScrollBarInfo(bool center)
{
    // scrollbars steal space when activated: cache these here for use below
    SCROLLBARINFO sbi;
    sbi.cbSize = sizeof(SCROLLBARINFO);
    hScrollActive_ = FALSE;
    vScrollActive_ = FALSE;
    unsigned long fbw = 0, fbh = 0;

    // get client rect to determine drawing area
    RECT cr;
    GetClientRect(hWnd, &cr);
    int cw = cr.right - cr.left;
    int ch = cr.bottom - cr.top;

    // adjust client rect to account for current scrollbar vis
    GetScrollBarInfo(hWnd, OBJID_HSCROLL, &sbi);
    if (!(sbi.rgstate[0] & STATE_SYSTEM_INVISIBLE))
    {
        // hscroll visible, underlying client rect is taller
        ch += sbi.dxyLineButton;
    }

    GetScrollBarInfo(hWnd, OBJID_VSCROLL, &sbi);
    if (!(sbi.rgstate[0] & STATE_SYSTEM_INVISIBLE))
    {
        // vscroll visible, underlying client rect is wider
        cw += sbi.dxyLineButton;
    }

    // now compute the margins as though the scrollbars weren't there
    int hDelta = cw - fbw;
    int vDelta = ch - fbh;

    // check width first
    if (hDelta < 0)
    {
        hScrollActive_ = TRUE;
        ShowScrollBar(hWnd, SB_HORZ, TRUE);

        // account for hscroll height in vDelta
        GetScrollBarInfo(hWnd, OBJID_HSCROLL, &sbi);
        vDelta -= sbi.dxyLineButton;
    }

    if (vDelta < 0)
    {
        vScrollActive_ = TRUE;
        ShowScrollBar(hWnd, SB_VERT, TRUE);

        // account for vScroll width in hDelta
        GetScrollBarInfo(hWnd, OBJID_VSCROLL, &sbi);
        hDelta -= sbi.dxyLineButton;

        // and double-check that we don't need to light up hScroll as well
        if (hDelta < 0 && hScrollActive_ == FALSE)
        {
            hScrollActive_ = TRUE;
            ShowScrollBar(hWnd, SB_HORZ, TRUE);

            // vDelta update again, we need it to set scroll info below
            GetScrollBarInfo(hWnd, OBJID_HSCROLL, &sbi);
            vDelta -= sbi.dxyLineButton;
        }
    }

    // these should be encapsulated in a mini-method
    if (hScrollActive_ == TRUE)
    {
        SCROLLINFO sih;
        sih.cbSize = sizeof(SCROLLINFO);
        sih.fMask = SIF_ALL;
        GetScrollInfo(hWnd, SB_VERT, &sih);
        sih.nMax = -hDelta;
        sih.nMin = 0;
        if (sih.nPos < sih.nMin)
        {
            sih.nPos = sih.nMin;
        }

        if (sih.nPos > sih.nMax)
        {
            sih.nPos = sih.nMax;
        }

        sih.nPage = 5;
        if (center)
        {
            sih.nPos = sih.nMax >> 1;
        }

        sih.nTrackPos = sih.nPos;
        SetScrollInfo(hWnd, SB_HORZ, &sih, ESB_ENABLE_BOTH);
    }
    else
    {
        ShowScrollBar(hWnd, SB_HORZ, FALSE);
    }

    if (vScrollActive_ == TRUE)
    {
        SCROLLINFO siv;
        siv.cbSize = sizeof(SCROLLINFO);
        siv.fMask = SIF_ALL;
        GetScrollInfo(hWnd, SB_VERT, &siv);
        siv.nMax = -vDelta;
        siv.nMin = 0;
        if (siv.nPos < siv.nMin)
        {
            siv.nPos = siv.nMin;
        }

        if (siv.nPos > siv.nMax)
        {
            siv.nPos = siv.nMax;
        }

        if (center)
        {
            siv.nPos = siv.nMax >> 1;
        }
        siv.nPage = 5;
        siv.nTrackPos = siv.nPos;
        SetScrollInfo(hWnd, SB_VERT, &siv, ESB_ENABLE_BOTH);
    }
    else
    {
        ShowScrollBar(hWnd, SB_VERT, FALSE);
    }
}

void SolarSvr::getScrollPosition(int &xx, int &yy)
{
    SCROLLINFO si;
    si.cbSize = sizeof(SCROLLINFO);
    si.fMask = SIF_ALL;

    if (hScrollActive_)
    {
        GetScrollInfo(hWnd, SB_HORZ, &si);
        xx = si.nPos;
    }
    else
    {
        xx = 0;
    }

    if (vScrollActive_)
    {
        GetScrollInfo(hWnd, SB_VERT, &si);
        yy = si.nPos;
    }
    else
    {
        yy = 0;
    }
}

void SolarSvr::setScrollPosition(int xx, int yy)
{
    if (hScrollActive_)
    {
        SCROLLINFO sih;
        sih.cbSize = sizeof(SCROLLINFO);
        sih.fMask = SIF_ALL;

        GetScrollInfo(hWnd, SB_HORZ, &sih);
        if (xx < sih.nMin)
            xx = sih.nMin;
        if (xx > sih.nMax)
            xx = sih.nMax;
        sih.nPos = xx;

        SetScrollInfo(hWnd, SB_HORZ, &sih, TRUE);
    }

    if (vScrollActive_)
    {
        SCROLLINFO siv;
        siv.cbSize = sizeof(SCROLLINFO);
        siv.fMask = SIF_ALL;

        GetScrollInfo(hWnd, SB_VERT, &siv);
        if (yy < siv.nMin)
            yy = siv.nMin;
        if (yy > siv.nMax)
            yy = siv.nMax;

        siv.nPos = yy;

        SetScrollInfo(hWnd, SB_VERT, &siv, TRUE);
    }
}

bool SolarSvr::processOpcMenu(int wmId, int wmEvent, LPARAM lParam)
{
    switch (wmId)
    {
    case ID_OPC_LIST:
    {
        vector<_bstr_t> vecAllServer;
        m_pOpc->getListOfServers("localhost", vecAllServer);
        if (vecAllServer.size() <= 0)
            break;
        m_pOpc->connectServer(vecAllServer[0], "localhost");
    }
    break;
    case ID_OPC_BROWSER:
    {
        vector<OpcItem > vecAllBranches;
        m_pOpc->BrowserBranches(vecAllBranches);


        vector<OpcItem > vecLeafs;
        m_pOpc->BrowserLeafs(vecAllBranches[1].sItem, vecLeafs);

        LONG nCount;
        vector<LONG> vecID;
        vector<LONG> vecType;
        vector<_bstr_t> vecDesc;
        m_pOpc->QueryItemProperties(vecLeafs[0].sItemId, nCount, vecID, vecDesc, vecID);
        vector<VARIANT> vecValue;
        vector<LONG> vecErr;
        m_pOpc->GetItemProperties(vecLeafs[0].sItemId, nCount, vecID, vecValue, vecErr);
    }
    break;
    case ID_OPC_ADDGROUP:
    {
        m_pOpc->AddGroup("MyGroup", 1000, true);
    }
    break;
    default:
        break;
    }
    return true;
}