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
        private string m_strLoginName = "";

        public Main(int nLoginId, string strLoginName)
        {
            m_nLoginID = nLoginId;
            m_strLoginName = strLoginName;
            InitializeComponent();

            //搜索所有插件并显示菜单
            LoadPlugin();
            UserName.Text = m_strLoginName;
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
                    catch (System.Exception ex)
                    {
                        Trace.WriteLine(ex.Message);	
                    }
                }
            }
            
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
                    CardOperatingMenuItem.DropDownItems.Add(strMenu, null, new EventHandler(OnCardOperating_Click));
                    break;
                case MenuType.eCardPublish:
                    CardOperatingMenuItem.DropDownItems.Add(strMenu, null, new EventHandler(OnCardPublish_Click));
                    break;
                case MenuType.eOrgKeyManage:
                    KeyManageMenuItem.DropDownItems.Add(strMenu, null, new EventHandler(OnOrgKeyManage_Click));
                    break;
                case MenuType.eUserKeysManage:
                    KeyManageMenuItem.DropDownItems.Add(strMenu, null, new EventHandler(OnCpuKeyManage_Click));
                    break;
                case MenuType.ePsamKeyManage:
                    KeyManageMenuItem.DropDownItems.Add(strMenu, null, new EventHandler(OnPsamKeyManage_Click));
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
                default:
                    break;
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
            foreach (Form childrenForm in this.MdiChildren)
            {
                if (childrenForm.Name == strFormName)
                    return true;
            }
            return false;
        }

        private void OnAccountManage_Click(object sender, EventArgs e)
        {
            Guid account = new Guid("9A91172D-C36D-42f1-9320-78F3461FE0CD");
            if (FindChildForm(m_Plugins[account].strPluginName))
                return;
            object AccountObj = GetObject(account,m_Plugins[account].strPluginPath);
            if (AccountObj == null)
                return;
            Type t = AccountObj.GetType();            
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");            
            ShowPluginForm.Invoke(AccountObj, new object[] { this });            
        }

        private void OnClientInfo_Click(object sender, EventArgs e)
        {
            Guid clientinfo = new Guid("FFC0BC06-C24E-4067-A911-352673F74931");
            if (FindChildForm(m_Plugins[clientinfo].strPluginName))
                return;
            object clientObj = GetObject(clientinfo, m_Plugins[clientinfo].strPluginPath);
            if (clientObj == null)
                return;
            Type t = clientObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            ShowPluginForm.Invoke(clientObj, new object[] { this });          

        }

        private void OnStationInfo_Click(object sender, EventArgs e)
        {
            Guid stationinfo = new Guid("0E306A49-C0F3-4e6e-A986-BD27251D5196");
            if (FindChildForm(m_Plugins[stationinfo].strPluginName))
                return;
            object stationObj = GetObject(stationinfo, m_Plugins[stationinfo].strPluginPath);
            if (stationObj == null)
                return;
            Type t = stationObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            ShowPluginForm.Invoke(stationObj, new object[] { this });  
        }

        private void OnRechargeInfoManage_Click(object sender, EventArgs e)
        {
            Guid rechargeinfo = new Guid("5315D784-78EC-4bf7-AE8B-E639BE54B784");
            if (FindChildForm(m_Plugins[rechargeinfo].strPluginName))
                return;
            object RechargeInfoObj = GetObject(rechargeinfo, m_Plugins[rechargeinfo].strPluginPath);
            if (RechargeInfoObj == null)
                return;
            Type t = RechargeInfoObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            ShowPluginForm.Invoke(RechargeInfoObj, new object[] { this });    
        }

        private void OnCommunicationManage_Click(object sender, EventArgs e)
        {
            Guid communication = new Guid("EAF11A51-B785-4d78-A1B6-73AA3581DD1E");
            if (FindChildForm(m_Plugins[communication].strPluginName))
                return;
            object CommunicationObj = GetObject(communication, m_Plugins[communication].strPluginPath);
            if (CommunicationObj == null)
                return;             
            Type t = CommunicationObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            ShowPluginForm.Invoke(CommunicationObj, new object[] { this });    
        }

        private void OnCardOperating_Click(object sender, EventArgs e)
        {
            Guid CardOperating = new Guid("1AFEA8C6-5026-4bf7-9C77-573D8C10E4A8");
            if (FindChildForm(m_Plugins[CardOperating].strPluginName))
                return;
            object CardOperatingObj = GetObject(CardOperating, m_Plugins[CardOperating].strPluginPath);
            if (CardOperatingObj == null)
                return;
            Type t = CardOperatingObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            ShowPluginForm.Invoke(CardOperatingObj, new object[] { this });    
        }

        private void OnCardPublish_Click(object sender, EventArgs e)
        {
            Guid CardPublish = new Guid("4F0D50FF-AAE0-4504-9B20-701417840786");
            if (FindChildForm(m_Plugins[CardPublish].strPluginName))
                return;
            object CardPublishObj = GetObject(CardPublish, m_Plugins[CardPublish].strPluginPath);
            if (CardPublishObj == null)
                return;
            Type t = CardPublishObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            ShowPluginForm.Invoke(CardPublishObj, new object[] { this }); 
        }

        private void OnOrgKeyManage_Click(object sender, EventArgs e)
        {
            Guid CardOperating = new Guid("439DF630-0D7E-4cb8-B633-24CBCFB31499");
            if (FindChildForm(m_Plugins[CardOperating].strPluginName))
                return;
            object CardOperatingObj = GetObject(CardOperating, m_Plugins[CardOperating].strPluginPath);
            if (CardOperatingObj == null)
                return;
            Type t = CardOperatingObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            ShowPluginForm.Invoke(CardOperatingObj, new object[] { this });
         }

        private void OnCpuKeyManage_Click(object sender, EventArgs e)
        {
            Guid CardOperating = new Guid("A24CEFE8-E4ED-4808-891B-E3DBB203C600");
            if (FindChildForm(m_Plugins[CardOperating].strPluginName))
                return;
            object CardOperatingObj = GetObject(CardOperating, m_Plugins[CardOperating].strPluginPath);
            if (CardOperatingObj == null)
                return;
            Type t = CardOperatingObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            ShowPluginForm.Invoke(CardOperatingObj, new object[] { this });
        }

        private void OnPsamKeyManage_Click(object sender, EventArgs e)
        {
            Guid CardOperating = new Guid("C670EAFE-5966-4aef-944E-F10D5790F0F8");
            if (FindChildForm(m_Plugins[CardOperating].strPluginName))
                return;
            object CardOperatingObj = GetObject(CardOperating, m_Plugins[CardOperating].strPluginPath);
            if (CardOperatingObj == null)
                return;
            Type t = CardOperatingObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            ShowPluginForm.Invoke(CardOperatingObj, new object[] { this });
        }

        private void OnProvinceCode_Click(object sender, EventArgs e)
        {
            Guid ProvCode = new Guid("2F016FD9-8E92-4f30-989D-8687E22D76EB");
            if (FindChildForm(m_Plugins[ProvCode].strPluginName))
                return;
            object ProvCodeObj = GetObject(ProvCode, m_Plugins[ProvCode].strPluginPath);
            if (ProvCodeObj == null)
                return;
            Type t = ProvCodeObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            ShowPluginForm.Invoke(ProvCodeObj, new object[] { this });
        }


        private void OnCityCode_Click(object sender, EventArgs e)
        {
            Guid CityCode = new Guid("7094543C-D6FC-4453-84D7-0C9962FC7052");
            if (FindChildForm(m_Plugins[CityCode].strPluginName))
                return;
            object CityCodeObj = GetObject(CityCode, m_Plugins[CityCode].strPluginPath);
            if (CityCodeObj == null)
                return;
            Type t = CityCodeObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            ShowPluginForm.Invoke(CityCodeObj, new object[] { this });
        }


        private void OnCompanyCode_Click(object sender, EventArgs e)
        {
            Guid CompanyCode = new Guid("E37D8675-AB62-4804-A8BC-306ADEE68E58");
            if (FindChildForm(m_Plugins[CompanyCode].strPluginName))
                return;
            object CompanyCodeObj = GetObject(CompanyCode, m_Plugins[CompanyCode].strPluginPath);
            if (CompanyCodeObj == null)
                return;
            Type t = CompanyCodeObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            ShowPluginForm.Invoke(CompanyCodeObj, new object[] { this });
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            //AboutBox
            FntAboutBox box = new FntAboutBox();
            box.ShowDialog();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            //将数据库的用户改成未登录状态
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection("(local)", "FunnettStation", "sa", "sasoft"))
            {
                ObjSql = null;
                return;
            }

            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = ObjSql.MakeParam("UserID", SqlDbType.Int, 4, ParameterDirection.Input, m_nLoginID);
            ObjSql.ExecuteCommand("update UserDb set Status = 0 where UserId = @UserID", sqlparams);
            ObjSql.CloseConnection();
            ObjSql = null;
        }
    }
}