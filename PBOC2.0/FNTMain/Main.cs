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

namespace FNTMain
{
    public partial class Main : Form
    {
        //插件列表
        private Dictionary<Guid, string> m_Plugins = new Dictionary<Guid, string>();
        

        public Main()
        {
            InitializeComponent();

            //搜索所有插件并显示菜单
            LoadPlugin();
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
                    catch
                    {
                    	
                    }
                }
            }
            
        }

        void InsertToMainForm(object pluginObj,Type t)
        {
            MethodInfo PluginMenu = t.GetMethod("PluginMenu");
            MethodInfo GetMenuType = t.GetMethod("GetMenuType");            
            MenuType eMenu = (MenuType)GetMenuType.Invoke(pluginObj, null);
            string strMenu = (string)PluginMenu.Invoke(pluginObj, null);
            switch (eMenu)
            {
                case MenuType.eSystemAccount:
                    SystemMenuItem.DropDownItems.Add(strMenu,null, new EventHandler(OnAccountManage_Click));
                    break;
                case MenuType.eGasDataList:
                    GasMenuItem.DropDownItems.Add(strMenu,null,new EventHandler(OnGasInfoManage_Click));
                    break;
                case MenuType.eCommunicationUDP:
                    CommunicationMenuItem.DropDownItems.Add(strMenu, null, new EventHandler(OnCommunicationManage_Click));
                    break;
                case MenuType.eCardOperating:
                    CardOperatingMenuItem.DropDownItems.Add(strMenu, null, new EventHandler(OnCardOperating_Click));
                    break;
                default:
                    break;
            }
            MethodInfo PluginGuid = t.GetMethod("PluginGuid");
            Guid plugGuid = (Guid)PluginGuid.Invoke(pluginObj, null);
            m_Plugins.Add(plugGuid, t.Assembly.Location);         
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

        private void OnAccountManage_Click(object sender, EventArgs e)
        {            
            Guid account = new Guid("9A91172D-C36D-42f1-9320-78F3461FE0CD");
            object AccountObj = GetObject(account,m_Plugins[account]);
            if (AccountObj == null)
                return;
            Type t = AccountObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            ShowPluginForm.Invoke(AccountObj, new object[] { this });          
        }

        private void OnGasInfoManage_Click(object sender, EventArgs e)
        {
            Guid gasinfo = new Guid("5315D784-78EC-4bf7-AE8B-E639BE54B784");
            object GasInfoObj = GetObject(gasinfo, m_Plugins[gasinfo]);
            if (GasInfoObj == null)
                return;            
            Type t = GasInfoObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            ShowPluginForm.Invoke(GasInfoObj, new object[] { this });    
        }

        private void OnCommunicationManage_Click(object sender, EventArgs e)
        {
            Guid communication = new Guid("EAF11A51-B785-4d78-A1B6-73AA3581DD1E");
            object CommunicationObj = GetObject(communication, m_Plugins[communication]);
            if (CommunicationObj == null)
                return;             
            Type t = CommunicationObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            ShowPluginForm.Invoke(CommunicationObj, new object[] { this });    
        }

        private void OnCardOperating_Click(object sender, EventArgs e)
        {
            Guid CardOperating = new Guid("1AFEA8C6-5026-4bf7-9C77-573D8C10E4A8");
            object CardOperatingObj = GetObject(CardOperating, m_Plugins[CardOperating]);
            if (CardOperatingObj == null)
                return;
            Type t = CardOperatingObj.GetType();
            MethodInfo ShowPluginForm = t.GetMethod("ShowPluginForm");
            ShowPluginForm.Invoke(CardOperatingObj, new object[] { this });    
        }

    }
}