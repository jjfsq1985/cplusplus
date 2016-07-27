
// GetIPDlg.h : 头文件
//

#pragma once

#include <string>
using namespace std;

// CGetIPDlg 对话框
class CGetIPDlg : public CDialogEx
{
// 构造
public:
	CGetIPDlg(CWnd* pParent = NULL);	// 标准构造函数

// 对话框数据
	enum { IDD = IDD_GETIP_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 支持


// 实现
protected:
	HICON m_hIcon;

	// 生成的消息映射函数
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()


private:
    bool GetLocalIP(string& strIP);
    bool GetInternetIP(string& strInternetIP);

    wstring str2wstr(const string& str);
    string wstr2str(const wstring& wstr);

};
