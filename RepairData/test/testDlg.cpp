// testDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "test.h"
#include "testDlg.h"
#include <math.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// 用于应用程序“关于”菜单项的 CAboutDlg 对话框

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// 对话框数据
	enum { IDD = IDD_ABOUTBOX };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

// 实现
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
END_MESSAGE_MAP()


// CtestDlg 对话框




CtestDlg::CtestDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CtestDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	m_pData = new double[512];
}

CtestDlg::~CtestDlg()
{
	delete [] m_pData;
}

void CtestDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CtestDlg, CDialog)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
    ON_BN_CLICKED(IDC_BUTTON1, &CtestDlg::OnBnClickedButton1)
END_MESSAGE_MAP()


// CtestDlg 消息处理程序

BOOL CtestDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// 将“关于...”菜单项添加到系统菜单中。

	// IDM_ABOUTBOX 必须在系统命令范围内。
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// 设置此对话框的图标。当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	srand(time(NULL));
	int nScope = 100;
	double PI = 3.1415926;
	for(int i=0; i< 512; i++)
	{
		m_pData[i] = sin(2*PI*i/180)*nScope;
	}


	int nBlock = 128;
	int nCount = 512/nBlock;
	for(int i=0; i< nCount; i++)
	{
		double dbSum = 0.0;
		for(int j=i*nBlock; j<(i+1)*nBlock; j++)
		{
			dbSum += m_pData[j];
		}
		double dbAvg = dbSum/nBlock;

		for(int j=i*nBlock; j<(i+1)*nBlock; j++)
		{
			m_pData[j] -= dbAvg;
		}
	}


	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

void CtestDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。

void CtestDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // 用于绘制的设备上下文

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// 使图标在工作矩形中居中
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// 绘制图标
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CPaintDC dc(this); // 用于绘制的设备上下文

		CRect rcClient;
		GetClientRect(&rcClient);
		CPoint ptCenter = rcClient.CenterPoint();
		dc.MoveTo(rcClient.left,m_pData[0]+ptCenter.y);
		for(int i=1; i< 512; i++)
			dc.LineTo(rcClient.left+i,m_pData[i]+ptCenter.y);

		CDialog::OnPaint();
	}
}

//当用户拖动最小化窗口时系统调用此函数取得光标显示。
//
HCURSOR CtestDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CtestDlg::RepairData(double *pData, int nCount)
{
	int nPos = 0;
	int nEnd =  0;
	double dbDelta1 = 0;
	while(nPos < nCount)
	{
		int nFind = 0;//减少查找次数
		if(nEnd > 1)
			nFind = nEnd-2;
		int nStart = GetFixPos(nFind,pData,nCount,dbDelta1);
		if(nStart == 0)
			nStart = nEnd;
		double dbDelta2 = 0;
		nEnd = GetFixPos(nStart,pData,nCount,dbDelta2);
		if(nEnd == 0)
		{
			//应对最后一块
			dbDelta1 = dbDelta2;
			nEnd = nCount;
		}
		for(int n=nStart; n<nEnd; n++)
			pData[n] += dbDelta1;
		nPos = nEnd;
	}

}

int CtestDlg::GetFixPos(int nStart,double *pData, int nCount,double& delta)
{
	double dbPrevPrev = pData[nStart];
	double dbPrev = pData[nStart+1];
	double dbCur = 0;
	double dbNext = 0;
	int nPos = 0;
	delta = 0;
	for(int i=nStart+2; i< nCount; i++)
	{
		dbCur = pData[i];
		dbNext = pData[i+1];
		if(dbPrev > dbPrevPrev && dbCur < dbPrev/*突降*/&& dbNext > dbCur)//上升
		{
			delta = dbPrev - dbCur + dbNext - dbCur;
			nPos = i;
			break;
		}
		else if(dbPrev < dbPrevPrev && dbCur > dbPrev/*突升*/ && dbNext < dbCur)//下降
		{
			delta = dbPrev - dbCur + dbNext - dbCur;
			nPos = i;
			break;
		}
		dbPrevPrev = dbPrev;
		dbPrev = dbCur;
	}
	if(delta == 0 && nStart >= 2)//应对最后一块
		delta = pData[nStart-1] - pData[nStart] + pData[nStart+1] - pData[nStart];
	return nPos;
}

void CtestDlg::OnBnClickedButton1()
{
    //修正
    RepairData(m_pData,512);
    Invalidate(TRUE);
}
