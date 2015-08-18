using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using System.IO;
using IFuncPlugin;
using System.Diagnostics;
using SqlServerHelper;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace FNTMain
{
    public partial class Main : Form
    {
        private class PluginInfo
        {
            public string strPluginPath;
            public string strPluginName;

            public PluginInfo(string strPath, string strName)
            {
                strPluginPath = strPath;
                strPluginName = strName;
            }
        }
        //插件列表
        private Dictionary<Guid, PluginInfo> m_Plugins = new Dictionary<Guid, PluginInfo>();

        private int m_nLoginID = 0;
        private int m_nLoginAuthority = 0;
        private string m_strLoginName = "";
        private SqlConnectInfo m_dbConnectInfo = new SqlConnectInfo();

        private ToolStripMenuItem KeyManageMenuItem = new ToolStripMenuItem(); //动态添加密钥管理菜单

        public Main(int nLoginId,SqlConnectInfo DbInfo)
        {
            m_nLoginID = nLoginId;
            m_dbConnectInfo = DbInfo;
            InitializeComponent();

            KeyManageMenuItem.Name = "KeyManageMenuItem";
            KeyManageMenuItem.Size = new System.Drawing.Size(68, 21);
            KeyManageMenuItem.Text = "密钥管理";


            DbName.Text = "数据库：" + m_dbConnectInfo.strDbName;
        }


        public void LoadPlugin()
        {
            string[] pluginfiles = Directory.GetFiles(Application.StartupPath + @"\plugins");
            foreach (string strfile in pluginfiles)
            {
                if (strfile.ToLower().EndsWith(".dll"))
                {
                    try
                    {
                        //载入
                        Assembly dl = Assembly.LoadFrom(strfile);
                        Type[] types = dl.GetTypes();
                        foreach (Type t in types )
                        {
                            if (t.GetInterface("IPlugin") != null)
                            {
                                object plugobj = dl.CreateInstance(t.FullName);
                                InsertToMainForm(plugobj, t);                                
                            }
                        }

                    }                    
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.Message);	
                    }
                }
            }   
        }

        private bool HaveManagementCardAuthority()
        {
            if (m_nLoginID != 0 || m_strLoginName.ToLower() != "administrator")
                return false;
            if ((m_nLoginAuthority & GrobalVariable.CardOp_KeyManage_Authority) != GrobalVariable.CardOp_KeyManage_Authority)
                return false;
            return true;
        }

        void InsertToMainForm(object pluginObj,Type t)
        {
            MethodInfo PluginName = t.GetMethod("PluginName");
            MethodInfo PluginMenu = t.GetMethod("PluginMenu");
            MethodInfo GetMenuType = t.GetMethod("GetMenuType");            
            MenuType eMenu = (MenuType)GetMenuType.Invoke(pluginObj, null);
            string strMenu = (string)PluginMenu.Invoke(pluginObj, null);
            string strPluginName = (string)PluginName.Invoke(pluginObj, null);
            switch (eMenu)
            {
                case MenuType.eSystemAccount:
                    SystemMenuItem.DropDownItems.Add(strMenu,null, new EventHandler(OnAccountManage_Click));
                    break;
                case MenuType.eClientInfo:
                    SystemMenuItem.DropDownItems.Add(strMenu, null, new EventHandler(OnClientInfo_Click));
                    break;
                case MenuType.eStationInfo:
                    SystemMenuItem.DropDownItems.Add(strMenu, null, new EventHandler(OnStationInfo_Click));
                    break;
                case MenuType.eRechargeList:
                    RechargeMenuItem.DropDownItems.Add(strMenu, null, new EventHandler(OnRechargeInfoManage_Click));
                    break;
                case MenuType.eCardOperating:
                    if (HaveManagementCardAuthority())
                    {
                        CardOperatingMenuItem.DropDownItems.Add(strMenu, null, new EventHandler(OnCardOperating_Click));
                    }                    
                    break;
                case MenuType.eCardPublish:
                    CardOperatingMenuItem.DropDownItems.Add(strMenu, null, new EventHandler(OnCardPublish_Click));
                    break;
                case MenuType.eProvinceCode:
                    OptionMenuItem.DropDownItems.Add(strMenu, null, new EventHandler(OnProvinceCode_Click));
                    break;
                case MenuType.eCityCode:
                    OptionMenuItem.DropDownItems.Add(strMenu, null, new EventHandler(OnCityCode_Click));
                    break;
                case MenuType.eCompanyCode:
                    OptionMenuItem.DropDownItems.Add(strMenu, null, new EventHandler(OnCompanyCode_Click));
                    break;
                case MenuType.eDbManage:
                    OptionMenuItem.DropDownItems.Add(strMenu, null, new EventHandler(OnDbManage_Click));
                    break;
                case MenuType.eImportKeyXml:
                    OptionMenuItem.DropDownItems.Add(strMenu, null, new EventHandler(OnImportKey_Click));
                    break;
                default:
                    break;
            }

            if (HaveManagementCardAuthority())
            {
                 if(!MainMenu.Items.Contains(KeyManageMenuItem))
                 {
                     int nIndex = MainMenu.Items.IndexOf(OptionMenuItem);
                     MainMenu.Items.Insert(nIndex, KeyManageMenuItem); //密钥管理模块插在选项菜单前面                     
                 }
                 else
                 {
                     switch(eMenu)
                     {
                         case MenuType.eExportKeyXml:
                             KeyManageMenuItem.DropDownItems.Add(strMenu, null, new EventHandler(OnExportKey_Click));
                             break;
                          case MenuType.eUserKeysManage:
                             {
                                 ToolStripMenuItem item = new ToolStripMenuItem(strMenu, null, new EventHandler(OnCpuKeyManage_Click));
                                 KeyManageMenuItem.DropDownItems.Insert(0, item);     ////index 0
                             }
                             break;
                         case MenuType.eOrgKeyManage:
                             {
                                 ToolStripMenuItem item = new ToolStripMenuItem(strMenu, null, new EventHandler(OnOrgKeyManage_Click));
                                 KeyManageMenuItem.DropDownItems.Insert(1, item); ////index 2
                             }
                             break;
                        case MenuType.ePsamKeyManage:
                            {
                                ToolStripMenuItem item = new ToolStripMenuItem(strMenu, null, new EventHandler(OnPsamKeyManage_Click));
                                KeyManageMenuItem.DropDownItems.Insert(1, item);  ////index 1
                            }
                            break;
                        default:
                             break;
                     }

                 }
            }

            MethodInfo PluginGuid = t.GetMethod("PluginGuid");
            Guid plugGuid = (Guid)PluginGuid.Invoke(pluginObj, null);
            m_Plugins.Add(plugGuid, new PluginInfo(t.Assembly.Location, strPluginName));         
        }

        private object GetObject(Guid guidVal,string strPathName)
        {
            if (!m_Plugins.ContainsKey(guidVal))
                return null;
            object objPlugin = null;
            Assembly dl = Assembly.LoadFrom(strPathName);
            Type[] types = dl.GetTypes();
            foreach (Type t in types)
            {
                if (t.GetInterface("IPlugin") != null)
                {
                    object plugobj = dl.CreateInstance(t.FullName);
                    MethodInfo PluginGuid = t.GetMethod("PluginGuid");
                    Guid plugGuid = (Guid)PluginGuid.Invoke(plugobj, null);
                    if (plugGuid == guidVal)
                    {
                        objPlugin = plugobj;
                        break;
                    }
                }
            }
            return objPlugin;
        }

        //相同的窗体只打开一个，需要窗体Name与PluginName函数返回的一致
        private bool FindChildForm(string strFormName)
        {
            foreach (Control children in this.MainPanel.Controls)
            {
                if (children.Name == strFormName)
                    return true;
            }
            return false;
        }

        private void OnAccountManage_Click(object sender, EventArgs e)
        {
            Guid account = new Guid("9A91172D-C36D-42f1-9320-78F3461FE0CD");
            if (!m_Plugins.ContainsKey(account))
                return;
            if (FindChildForm(m_Plugins[account].strPluginName))
                return;
            object AccountObj = GetObject(account,m_Plugins[account].strPluginPath);
            if (AccountObj == null)
                return;
            Type t = AccountObj.GetType();            
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            MethodInfo SetAuthority = t.GetMethod("SetAuthority");
            SetAuthority.Invoke(AccountObj, new object[] { m_nLoginID, m_nLoginAuthority });
            ShowPluginForm.Invoke(AccountObj, new object[] { MainPanel, m_dbConnectInfo });            
        }

        private void OnClientInfo_Click(object sender, EventArgs e)
        {
            Guid clientinfo = new Guid("FFC0BC06-C24E-4067-A911-352673F74931");
            if (!m_Plugins.ContainsKey(clientinfo))
                return;
            if (FindChildForm(m_Plugins[clientinfo].strPluginName))
                return;
            object clientObj = GetObject(clientinfo, m_Plugins[clientinfo].strPluginPath);
            if (clientObj == null)
                return;
            Type t = clientObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            MethodInfo SetAuthority = t.GetMethod("SetAuthority");
            SetAuthority.Invoke(clientObj, new object[] { m_nLoginID, (m_nLoginAuthority & GrobalVariable.ClientInfo_Authority) });
            ShowPluginForm.Invoke(clientObj, new object[] { MainPanel, m_dbConnectInfo });          

        }

        private void OnStationInfo_Click(object sender, EventArgs e)
        {
            Guid stationinfo = new Guid("0E306A49-C0F3-4e6e-A986-BD27251D5196");
            if (!m_Plugins.ContainsKey(stationinfo))
                return;
            if (FindChildForm(m_Plugins[stationinfo].strPluginName))
                return;
            object stationObj = GetObject(stationinfo, m_Plugins[stationinfo].strPluginPath);
            if (stationObj == null)
                return;
            Type t = stationObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            MethodInfo SetAuthority = t.GetMethod("SetAuthority");
            SetAuthority.Invoke(stationObj, new object[] { m_nLoginID, (m_nLoginAuthority & GrobalVariable.StationInfo_Authority) });
            ShowPluginForm.Invoke(stationObj, new object[] { MainPanel, m_dbConnectInfo });  
        }

        private void OnRechargeInfoManage_Click(object sender, EventArgs e)
        {
            Guid rechargeinfo = new Guid("5315D784-78EC-4bf7-AE8B-E639BE54B784");
            if (!m_Plugins.ContainsKey(rechargeinfo))
                return;
            if (FindChildForm(m_Plugins[rechargeinfo].strPluginName))
                return;
            object RechargeInfoObj = GetObject(rechargeinfo, m_Plugins[rechargeinfo].strPluginPath);
            if (RechargeInfoObj == null)
                return;
            Type t = RechargeInfoObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            MethodInfo SetAuthority = t.GetMethod("SetAuthority");
            SetAuthority.Invoke(RechargeInfoObj, new object[] { m_nLoginID, (m_nLoginAuthority & GrobalVariable.RechargeList_Authority) });
            ShowPluginForm.Invoke(RechargeInfoObj, new object[] { MainPanel, m_dbConnectInfo });    
        }

        private void OnCardOperating_Click(object sender, EventArgs e)
        {
            if (!HaveManagementCardAuthority())
                return;

            Guid CardOperating = new Guid("1AFEA8C6-5026-4bf7-9C77-573D8C10E4A8");
            Guid CardPublish = new Guid("4F0D50FF-AAE0-4504-9B20-701417840786");
            if (!m_Plugins.ContainsKey(CardOperating))
                return;
            if (FindChildForm(m_Plugins[CardOperating].strPluginName))
                return;

            if (m_Plugins.ContainsKey(CardPublish) && FindChildForm(m_Plugins[CardPublish].strPluginName))
            {
                MessageBox.Show("请先关闭卡信息维护界面，然后才能打开制发卡界面","注意");
                return;
            }

            object CardOperatingObj = GetObject(CardOperating, m_Plugins[CardOperating].strPluginPath);
            if (CardOperatingObj == null)
                return;
            Type t = CardOperatingObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            MethodInfo SetAuthority = t.GetMethod("SetAuthority");
            SetAuthority.Invoke(CardOperatingObj, new object[] { m_nLoginID, (m_nLoginAuthority & GrobalVariable.CardOp_KeyManage_Authority) });
            ShowPluginForm.Invoke(CardOperatingObj, new object[] { MainPanel, m_dbConnectInfo });    
        }

        private void OnCardPublish_Click(object sender, EventArgs e)
        {
            Guid CardOperating = new Guid("1AFEA8C6-5026-4bf7-9C77-573D8C10E4A8");
            Guid CardPublish = new Guid("4F0D50FF-AAE0-4504-9B20-701417840786");
            if (!m_Plugins.ContainsKey(CardPublish))
                return;
            if (FindChildForm(m_Plugins[CardPublish].strPluginName))
                return;

            if (m_Plugins.ContainsKey(CardOperating) && FindChildForm(m_Plugins[CardOperating].strPluginName))
            {
                MessageBox.Show("请先关闭制发卡界面，然后才能打开卡信息维护界面", "注意");
                return;
            }

            object CardPublishObj = GetObject(CardPublish, m_Plugins[CardPublish].strPluginPath);
            if (CardPublishObj == null)
                return;
            Type t = CardPublishObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            MethodInfo SetAuthority = t.GetMethod("SetAuthority");
            SetAuthority.Invoke(CardPublishObj, new object[] { m_nLoginID, (m_nLoginAuthority & GrobalVariable.CardPublish_Authority) });
            ShowPluginForm.Invoke(CardPublishObj, new object[] { MainPanel, m_dbConnectInfo }); 
        }

        private void OnOrgKeyManage_Click(object sender, EventArgs e)
        {
            if (!HaveManagementCardAuthority())
                return;
            Guid CardOperating = new Guid("439DF630-0D7E-4cb8-B633-24CBCFB31499");
            if (!m_Plugins.ContainsKey(CardOperating))
                return;
            if (FindChildForm(m_Plugins[CardOperating].strPluginName))
                return;
            object CardOperatingObj = GetObject(CardOperating, m_Plugins[CardOperating].strPluginPath);
            if (CardOperatingObj == null)
                return;
            Type t = CardOperatingObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            MethodInfo SetAuthority = t.GetMethod("SetAuthority");
            SetAuthority.Invoke(CardOperatingObj, new object[] { m_nLoginID, (m_nLoginAuthority & GrobalVariable.CardOp_KeyManage_Authority) });
            ShowPluginForm.Invoke(CardOperatingObj, new object[] { MainPanel, m_dbConnectInfo });
         }

        private void OnCpuKeyManage_Click(object sender, EventArgs e)
        {
            if (!HaveManagementCardAuthority())
                return;
            Guid CardOperating = new Guid("A24CEFE8-E4ED-4808-891B-E3DBB203C600");
            if (!m_Plugins.ContainsKey(CardOperating))
                return;
            if (FindChildForm(m_Plugins[CardOperating].strPluginName))
                return;
            object CardOperatingObj = GetObject(CardOperating, m_Plugins[CardOperating].strPluginPath);
            if (CardOperatingObj == null)
                return;
            Type t = CardOperatingObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            MethodInfo SetAuthority = t.GetMethod("SetAuthority");
            SetAuthority.Invoke(CardOperatingObj, new object[] { m_nLoginID, (m_nLoginAuthority & GrobalVariable.CardOp_KeyManage_Authority) });
            ShowPluginForm.Invoke(CardOperatingObj, new object[] { MainPanel, m_dbConnectInfo });
        }

        private void OnPsamKeyManage_Click(object sender, EventArgs e)
        {
            if (!HaveManagementCardAuthority())
                return;
            Guid CardOperating = new Guid("C670EAFE-5966-4aef-944E-F10D5790F0F8");
            if (!m_Plugins.ContainsKey(CardOperating))
                return;
            if (FindChildForm(m_Plugins[CardOperating].strPluginName))
                return;
            object CardOperatingObj = GetObject(CardOperating, m_Plugins[CardOperating].strPluginPath);
            if (CardOperatingObj == null)
                return;
            Type t = CardOperatingObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            MethodInfo SetAuthority = t.GetMethod("SetAuthority");
            SetAuthority.Invoke(CardOperatingObj, new object[] { m_nLoginID, (m_nLoginAuthority & GrobalVariable.CardOp_KeyManage_Authority) });
            ShowPluginForm.Invoke(CardOperatingObj, new object[] { MainPanel, m_dbConnectInfo });
        }

        //指定制卡使用的XML文件
        private void OnImportKey_Click(object sender, EventArgs e)
        {
            Guid KeyImport = new Guid("04CD1292-9AC4-437f-BDD1-918E78846EFD");
            if (!m_Plugins.ContainsKey(KeyImport))
                return;
            if (FindChildForm(m_Plugins[KeyImport].strPluginName))
                return;
            object KeyImportObj = GetObject(KeyImport, m_Plugins[KeyImport].strPluginPath);
            if (KeyImportObj == null)
                return;
            Type t = KeyImportObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            MethodInfo SetAuthority = t.GetMethod("SetAuthority");
            SetAuthority.Invoke(KeyImportObj, new object[] { m_nLoginID, m_nLoginAuthority });
            ShowPluginForm.Invoke(KeyImportObj, new object[] { MainPanel, m_dbConnectInfo });
        }

        //将当前使用的密钥导出
        private void OnExportKey_Click(object sender, EventArgs e)
        {
            if (!HaveManagementCardAuthority())
                return;
            Guid KeyExport = new Guid("D122EE72-2338-456c-88BD-531F2D2415CD");
            if (!m_Plugins.ContainsKey(KeyExport))
                return;
            if (FindChildForm(m_Plugins[KeyExport].strPluginName))
                return;
            object KeyExportObj = GetObject(KeyExport, m_Plugins[KeyExport].strPluginPath);
            if (KeyExportObj == null)
                return;
            Type t = KeyExportObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            MethodInfo SetAuthority = t.GetMethod("SetAuthority");
            SetAuthority.Invoke(KeyExportObj, new object[] { m_nLoginID, (m_nLoginAuthority & GrobalVariable.CardOp_KeyManage_Authority) });
            ShowPluginForm.Invoke(KeyExportObj, new object[] { MainPanel, m_dbConnectInfo });
        }

        private void OnProvinceCode_Click(object sender, EventArgs e)
        {
            Guid ProvCode = new Guid("2F016FD9-8E92-4f30-989D-8687E22D76EB");
            if (!m_Plugins.ContainsKey(ProvCode))
                return;
            if (FindChildForm(m_Plugins[ProvCode].strPluginName))
                return;
            object ProvCodeObj = GetObject(ProvCode, m_Plugins[ProvCode].strPluginPath);
            if (ProvCodeObj == null)
                return;
            Type t = ProvCodeObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            MethodInfo SetAuthority = t.GetMethod("SetAuthority");
            SetAuthority.Invoke(ProvCodeObj, new object[] { m_nLoginID, (m_nLoginAuthority & GrobalVariable.CodeTable_Authority) });
            ShowPluginForm.Invoke(ProvCodeObj, new object[] { MainPanel, m_dbConnectInfo });
        }


        private void OnCityCode_Click(object sender, EventArgs e)
        {
            Guid CityCode = new Guid("7094543C-D6FC-4453-84D7-0C9962FC7052");
            if (!m_Plugins.ContainsKey(CityCode))
                return;
            if (FindChildForm(m_Plugins[CityCode].strPluginName))
                return;
            object CityCodeObj = GetObject(CityCode, m_Plugins[CityCode].strPluginPath);
            if (CityCodeObj == null)
                return;
            Type t = CityCodeObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            MethodInfo SetAuthority = t.GetMethod("SetAuthority");
            SetAuthority.Invoke(CityCodeObj, new object[] { m_nLoginID, (m_nLoginAuthority & GrobalVariable.CodeTable_Authority) });
            ShowPluginForm.Invoke(CityCodeObj, new object[] { MainPanel, m_dbConnectInfo });
        }


        private void OnCompanyCode_Click(object sender, EventArgs e)
        {
            Guid CompanyCode = new Guid("E37D8675-AB62-4804-A8BC-306ADEE68E58");
            if (!m_Plugins.ContainsKey(CompanyCode))
                return;
            if (FindChildForm(m_Plugins[CompanyCode].strPluginName))
                return;
            object CompanyCodeObj = GetObject(CompanyCode, m_Plugins[CompanyCode].strPluginPath);
            if (CompanyCodeObj == null)
                return;
            Type t = CompanyCodeObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            MethodInfo SetAuthority = t.GetMethod("SetAuthority");
            SetAuthority.Invoke(CompanyCodeObj, new object[] { m_nLoginID, (m_nLoginAuthority & GrobalVariable.CodeTable_Authority) });
            ShowPluginForm.Invoke(CompanyCodeObj, new object[] { MainPanel, m_dbConnectInfo });
        }

        private void OnDbManage_Click(object sender, EventArgs e)
        {
            Guid dbManage = new Guid("6A1B65FB-DA7D-40c4-AD11-B8B5ECB7411A");
            if (!m_Plugins.ContainsKey(dbManage))
                return;
            if (FindChildForm(m_Plugins[dbManage].strPluginName))
                return;
            object dbObj = GetObject(dbManage, m_Plugins[dbManage].strPluginPath);
            if (dbObj == null)
                return;
            Type t = dbObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            MethodInfo SetAuthority = t.GetMethod("SetAuthority");
            SetAuthority.Invoke(dbObj, new object[] { m_nLoginID, (m_nLoginAuthority & GrobalVariable.DbManage_Authority) });
            ShowPluginForm.Invoke(dbObj, new object[] { MainPanel, m_dbConnectInfo });            
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            //AboutBox
            FntAboutBox box = new FntAboutBox();
            box.ShowDialog();
        }

        private void GetUserAndAuthority(int nUserId)
        {
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_dbConnectInfo.strServerName, m_dbConnectInfo.strDbName, m_dbConnectInfo.strUser, m_dbConnectInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = ObjSql.MakeParam("UserID", SqlDbType.Int, 4, ParameterDirection.Input, nUserId);
            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select UserName,Authority from UserDb where UserId = @UserID", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    m_strLoginName = (string)dataReader["UserName"];
                    m_nLoginAuthority = (int)dataReader["Authority"];
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            SqlHelper ObjSql = new SqlHelper();
            if (ObjSql.OpenSqlServerConnection(m_dbConnectInfo.strServerName, m_dbConnectInfo.strDbName, m_dbConnectInfo.strUser, m_dbConnectInfo.strUserPwd))
            {
                //将数据库的用户改成未登录状态
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = ObjSql.MakeParam("UserID", SqlDbType.Int, 4, ParameterDirection.Input, m_nLoginID);
                ObjSql.ExecuteCommand("update UserDb set Status = 0 where UserId = @UserID", sqlparams);
                ObjSql.CloseConnection();
                ObjSql = null;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int nSel = cmbCondition.SelectedIndex;
            string strText = textSearchContent.Text;
            switch (nSel)
            {
                case 0:
                    {
                        Regex reg = new Regex(@"^[0-9]+$");
                        if (!reg.Match(strText).Success)
                        {
                            MessageBox.Show("卡号格式不正确,只能是数字");
                        }
                        else
                        {
                            SearchByCardNum(strText);
                        }
                    }
                    break;
                case 1:
                    {
                        Regex reg = new Regex(@"^[A-Za-z\u4e00-\u9fa5]+$");
                        if (!reg.Match(strText).Success)
                        {
                            MessageBox.Show("所有者姓名格式不正确,不能包含数字和特殊字符");
                        }
                        else
                        {
                            SearchByName(strText);
                        }
                    }                    
                    break;
                case 2:
                    {
                        Regex reg = new Regex(@"^[0-9|X]+$");
                        if (!reg.Match(strText).Success)
                        {
                            MessageBox.Show("身份证格式不正确");
                        }
                        else
                        {
                            SearchByPersionID(strText);
                        }
                    }
                    break;
                case 3:
                    {
                        int nClientId = 0;
                        int.TryParse(strText, out nClientId);
                        if (nClientId <= 0)
                        {
                            Regex reg = new Regex(@"^[A-Za-z\u4e00-\u9fa5]+$");
                            if (!reg.Match(strText).Success)
                            {
                                MessageBox.Show("单位名称不正确，不能包含数字和特殊字符");
                            }
                            else
                            {
                                SearchByClientName(strText,ChkSearchPsam.Checked);
                            }                            
                        }
                        else
                        {
                            SearchByClientID(nClientId);
                        }                        
                    }
                    break;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            GetUserAndAuthority(m_nLoginID);
            UserName.Text = "用户：" + m_strLoginName;

            cmbCondition.Items.Clear();
            cmbCondition.Items.Add("卡号");
            cmbCondition.Items.Add("卡所有者");
            cmbCondition.Items.Add("身份证号码");
            cmbCondition.Items.Add("所属单位");
            cmbCondition.SelectedIndex = 0;

            listSearchResult.Items.Clear();
            listSearchResult.Columns.Clear();
            listSearchResult.Columns.Add("卡号", 120);
            listSearchResult.Columns.Add("卡类型", 60);
            listSearchResult.Columns.Add("所属单位", 150);
            listSearchResult.Columns.Add("有效期", 150);
            listSearchResult.Columns.Add("身份证号", 120);
            listSearchResult.Columns.Add("用户姓名", 100);
            listSearchResult.Columns.Add("联系电话", 100);
            listSearchResult.Columns.Add("卡余额", 60);

            //搜索所有插件并显示菜单
            LoadPlugin();
        }

        private void cmbCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            listSearchResult.Items.Clear();
            listSearchResult.Columns.Clear();
            //清空textSearchContent内容，并限制输入
            textSearchContent.Text = "";

            int nSel = cmbCondition.SelectedIndex;
            switch (nSel)
            {
                case 0:
                    textSearchContent.MaxLength = 16;
                    ChkSearchPsam.Visible = true;
                    ChkSearchPsam.Enabled = true;
                    if (ChkSearchPsam.Checked)
                    {
                        listSearchResult.Columns.Add("卡号", 120);
                        listSearchResult.Columns.Add("终端机编号", 60);
                        listSearchResult.Columns.Add("所属单位", 150);
                        listSearchResult.Columns.Add("有效期", 150);
                        listSearchResult.Columns.Add("发行者标识", 150);
                        listSearchResult.Columns.Add("接收者标识", 150);
                    }
                    else
                    {
                        listSearchResult.Columns.Add("卡号", 120);
                        listSearchResult.Columns.Add("卡类型", 60);
                        listSearchResult.Columns.Add("所属单位", 150);
                        listSearchResult.Columns.Add("有效期", 150);
                        listSearchResult.Columns.Add("身份证号", 120);
                        listSearchResult.Columns.Add("用户姓名", 100);
                        listSearchResult.Columns.Add("联系电话", 100);
                        listSearchResult.Columns.Add("卡余额", 60);
                    }
                    break;
                case 1: 
                    textSearchContent.MaxLength = 16;
                    ChkSearchPsam.Visible = false;
                    ChkSearchPsam.Enabled = false;
                    listSearchResult.Columns.Add("卡号", 120);
                    listSearchResult.Columns.Add("卡类型", 60);
                    listSearchResult.Columns.Add("所属单位", 150);
                    listSearchResult.Columns.Add("有效期", 150);
                    listSearchResult.Columns.Add("身份证号", 120);
                    listSearchResult.Columns.Add("用户姓名", 100);
                    listSearchResult.Columns.Add("联系电话", 100);
                    listSearchResult.Columns.Add("卡余额", 60);
                    break;
                case 2:
                    textSearchContent.MaxLength = 18;
                    ChkSearchPsam.Visible = false;
                    ChkSearchPsam.Enabled = false;
                    listSearchResult.Columns.Add("卡号", 120);
                    listSearchResult.Columns.Add("卡类型", 60);
                    listSearchResult.Columns.Add("所属单位", 150);
                    listSearchResult.Columns.Add("有效期", 150);
                    listSearchResult.Columns.Add("身份证号", 120);
                    listSearchResult.Columns.Add("用户姓名", 100);
                    listSearchResult.Columns.Add("联系电话", 100);
                    listSearchResult.Columns.Add("卡余额", 60);
                    break;
                case 3:
                    textSearchContent.MaxLength = 50;
                    ChkSearchPsam.Visible = true;
                    ChkSearchPsam.Enabled = true;
                    if (ChkSearchPsam.Checked)
                    {
                        listSearchResult.Columns.Add("卡号", 120);
                        listSearchResult.Columns.Add("终端机编号", 60);
                        listSearchResult.Columns.Add("所属单位", 150);
                        listSearchResult.Columns.Add("有效期", 150);
                        listSearchResult.Columns.Add("发行者标识", 150);
                        listSearchResult.Columns.Add("接收者标识", 150);
                    }
                    else
                    {
                        listSearchResult.Columns.Add("卡号", 120);
                        listSearchResult.Columns.Add("卡类型", 60);
                        listSearchResult.Columns.Add("所属单位", 150);
                        listSearchResult.Columns.Add("有效期", 150);
                        listSearchResult.Columns.Add("身份证号", 120);
                        listSearchResult.Columns.Add("用户姓名", 100);
                        listSearchResult.Columns.Add("联系电话", 100);
                        listSearchResult.Columns.Add("卡余额", 60);
                    }
                    break;
            }
        }

        private string GetClientName(int nClientID)
        {
            string strRet = "";
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_dbConnectInfo.strServerName, m_dbConnectInfo.strDbName, m_dbConnectInfo.strUser, m_dbConnectInfo.strUserPwd))
            {
                ObjSql = null;
                return "";
            }
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = ObjSql.MakeParam("ClientId", SqlDbType.Int, 4, ParameterDirection.Input, nClientID);
            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select ClientName from Base_Client where ClientId = @ClientId", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    strRet = (string)dataReader["ClientName"];             
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
            return strRet;
        }

        private void SearchPsam(string strParamName, string strParamVal)
        {
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_dbConnectInfo.strServerName, m_dbConnectInfo.strDbName, m_dbConnectInfo.strUser, m_dbConnectInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = ObjSql.MakeParam("Search", SqlDbType.VarChar, 32, ParameterDirection.Input, "%" + strParamVal + "%");
            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select * from Psam_Card where " + strParamName + " like @Search", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ListViewItem ItemCard = new ListViewItem();
                        ItemCard.SubItems.Add((string)dataReader["TerminalId"]);
                        string strClientName = GetClientName((int)dataReader["ClientId"]);
                        ItemCard.SubItems.Add(strClientName);
                        DateTime DateStart = (DateTime)dataReader["UseValidateDate"];
                        DateTime DateEnd = (DateTime)dataReader["UseInvalidateDate"];
                        ItemCard.SubItems.Add(DateStart.ToString("yyyyMMdd") + "-" + DateEnd.ToString("yyyyMMdd"));
                        string strCompanyFrom = (string)dataReader["IssueCode"];
                        ItemCard.SubItems.Add(strCompanyFrom);
                        string strCompanyTo = (string)dataReader["RecvCode"];
                        ItemCard.SubItems.Add(strCompanyTo);
                        string strCardId = (string)dataReader["PsamId"];
                        ItemCard.Text = strCardId;
                        listSearchResult.Items.Add(ItemCard);
                    }
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
        }

        //模糊查询
        private void SearchCommon(string strParamName, string strParamVal)
        {
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_dbConnectInfo.strServerName, m_dbConnectInfo.strDbName, m_dbConnectInfo.strUser, m_dbConnectInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = ObjSql.MakeParam("Search", SqlDbType.VarChar, 32, ParameterDirection.Input, "%" + strParamVal + "%");
            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select * from Base_Card where " + strParamName + " like @Search", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ListViewItem ItemCard = new ListViewItem();
                        string strCardType = PublicFunc.GetCardTypeString(Convert.ToByte((string)dataReader["CardType"], 16));
                        ItemCard.SubItems.Add(strCardType);
                        string strClientName = GetClientName((int)dataReader["ClientId"]);
                        ItemCard.SubItems.Add(strClientName);
                        DateTime DateStart = (DateTime)dataReader["UseValidateDate"];
                        DateTime DateEnd = (DateTime)dataReader["UseInvalidateDate"];
                        ItemCard.SubItems.Add(DateStart.ToString("yyyyMMdd") + "-" + DateEnd.ToString("yyyyMMdd"));

                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("PersonalId")))
                        {
                            string strPersionId = (string)dataReader["PersonalId"];
                            ItemCard.SubItems.Add(strPersionId);
                        }
                        else
                        {
                            ItemCard.SubItems.Add("");
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("DriverName")))
                        {
                            string strDriverName = (string)dataReader["DriverName"];
                            ItemCard.SubItems.Add(strDriverName);
                        }
                        else
                        {
                            ItemCard.SubItems.Add("");
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("DriverTel")))
                        {
                            string strDriverTel = (string)dataReader["DriverTel"];
                            ItemCard.SubItems.Add(strDriverTel);
                        }
                        else
                        {
                            ItemCard.SubItems.Add("");
                        }
                        decimal balance = (decimal)dataReader["CardBalance"];
                        ItemCard.SubItems.Add(balance.ToString());

                        string strCardId = (string)dataReader["CardNum"];
                        ItemCard.Text = strCardId;
                        listSearchResult.Items.Add(ItemCard);
                    }
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
        }

        private void SearchPsamInfoByClient(int nClientID, string strClientName)
        {
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_dbConnectInfo.strServerName, m_dbConnectInfo.strDbName, m_dbConnectInfo.strUser, m_dbConnectInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = ObjSql.MakeParam("ClientId", SqlDbType.Int, 4, ParameterDirection.Input, nClientID);
            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select * from Psam_Card where ClientId = @ClientId", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ListViewItem ItemCard = new ListViewItem();
                        ItemCard.SubItems.Add((string)dataReader["TerminalId"]);                        
                        ItemCard.SubItems.Add(strClientName);//ID相同的名称一致
                        DateTime DateStart = (DateTime)dataReader["UseValidateDate"];
                        DateTime DateEnd = (DateTime)dataReader["UseInvalidateDate"];
                        ItemCard.SubItems.Add(DateStart.ToString("yyyyMMdd") + "-" + DateEnd.ToString("yyyyMMdd"));
                        string strCompanyFrom = (string)dataReader["IssueCode"];
                        ItemCard.SubItems.Add(strCompanyFrom);
                        string strCompanyTo = (string)dataReader["RecvCode"];
                        ItemCard.SubItems.Add(strCompanyTo);
                        string strCardId = (string)dataReader["PsamId"];
                        ItemCard.Text = strCardId;
                        listSearchResult.Items.Add(ItemCard);
                    }
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
        }

        private void SearchCardInfoByClient(int nClientID, string strClientName)
        {
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_dbConnectInfo.strServerName, m_dbConnectInfo.strDbName, m_dbConnectInfo.strUser, m_dbConnectInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = ObjSql.MakeParam("ClientId", SqlDbType.Int, 4, ParameterDirection.Input, nClientID);
            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select * from Base_Card where ClientId = @ClientId", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ListViewItem ItemCard = new ListViewItem();
                        string strCardType = PublicFunc.GetCardTypeString(Convert.ToByte((string)dataReader["CardType"], 16));
                        ItemCard.SubItems.Add(strCardType);
                        ItemCard.SubItems.Add(strClientName);//ID相同的名称一致
                        DateTime DateStart = (DateTime)dataReader["UseValidateDate"];
                        DateTime DateEnd = (DateTime)dataReader["UseInvalidateDate"];
                        ItemCard.SubItems.Add(DateStart.ToString("yyyyMMdd") + "-" + DateEnd.ToString("yyyyMMdd"));

                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("PersonalId")))
                        {
                            string strPersionId = (string)dataReader["PersonalId"];
                            ItemCard.SubItems.Add(strPersionId);
                        }
                        else
                        {
                            ItemCard.SubItems.Add("");
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("DriverName")))
                        {
                            string strDriverName = (string)dataReader["DriverName"];
                            ItemCard.SubItems.Add(strDriverName);
                        }
                        else
                        {
                            ItemCard.SubItems.Add("");
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("DriverTel")))
                        {
                            string strDriverTel = (string)dataReader["DriverTel"];
                            ItemCard.SubItems.Add(strDriverTel);
                        }
                        else
                        {
                            ItemCard.SubItems.Add("");
                        }
                        decimal balance = (decimal)dataReader["CardBalance"];
                        ItemCard.SubItems.Add(balance.ToString());

                        string strCardId = (string)dataReader["CardNum"];
                        ItemCard.Text = strCardId;
                        listSearchResult.Items.Add(ItemCard);
                    }
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
        }

        private void SearchByClientID(int nClientId)
        {
            listSearchResult.Items.Clear();
            if (ChkSearchPsam.Checked)
                SearchPsam("ClientId", nClientId.ToString());
            else
                SearchCommon("ClientId", nClientId.ToString());
        }

        private void SearchByClientName(string strClientName, bool bSearchPsam)
        {
            listSearchResult.Items.Clear();
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_dbConnectInfo.strServerName, m_dbConnectInfo.strDbName, m_dbConnectInfo.strUser, m_dbConnectInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }

            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = ObjSql.MakeParam("Search", SqlDbType.NVarChar, 50, ParameterDirection.Input, "%" + strClientName + "%");
            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select ClientId,ClientName from Base_Client where ClientName like @Search", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    int nClientID = (int)dataReader["ClientId"];
                    string strPreciseName = (string)dataReader["ClientName"];
                    if (bSearchPsam)
                        SearchPsamInfoByClient(nClientID, strPreciseName);
                    else
                        SearchCardInfoByClient(nClientID, strPreciseName);
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
        }

        private void SearchByPersionID(string strPersionID)
        {
            listSearchResult.Items.Clear();
            SearchCommon("PersonalId", strPersionID);
        }

        private void SearchByName(string strDriverName)
        {
            listSearchResult.Items.Clear();
            SearchCommon("DriverName", strDriverName);
        }

        private void SearchByCardNum(string strCardId)
        {
            listSearchResult.Items.Clear();
            if (ChkSearchPsam.Checked)
                SearchPsam("PsamId", strCardId);
            else
                SearchCommon("CardNum", strCardId);
        }

        private void ChkSearchPsam_CheckedChanged(object sender, EventArgs e)
        {
            listSearchResult.Items.Clear();
            listSearchResult.Columns.Clear();
            if (ChkSearchPsam.Checked)
            {
                listSearchResult.Columns.Add("卡号", 120);
                listSearchResult.Columns.Add("终端机编号", 60);
                listSearchResult.Columns.Add("所属单位", 150);
                listSearchResult.Columns.Add("有效期", 150);
                listSearchResult.Columns.Add("发行者标识", 150);
                listSearchResult.Columns.Add("接收者标识", 150);
            }
            else
            {
                listSearchResult.Columns.Add("卡号", 120);
                listSearchResult.Columns.Add("卡类型", 60);
                listSearchResult.Columns.Add("所属单位", 150);
                listSearchResult.Columns.Add("有效期", 150);
                listSearchResult.Columns.Add("身份证号", 120);
                listSearchResult.Columns.Add("用户姓名", 100);
                listSearchResult.Columns.Add("联系电话", 100);
                listSearchResult.Columns.Add("卡余额", 60);
            }
        }

    }
}