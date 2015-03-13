using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IFuncPlugin
{
    public enum MenuType
    {
        eSystemAccount, //账户管理
        eGasDataList,   //加气数据库管理
        eCommunicationUDP,  //UDP通讯实时显示加气
        eCardOperating      //卡操作
    }

    //插件中各个控件的位置
    public struct ControlPos
    {
        public int x;
        public int y;
    }

    public interface IPlugin
    {
        MenuType GetMenuType();//插件的菜单位置
        string PluginName();  //插件名称
        Guid PluginGuid();  //插件唯一标识（GUID）
        string PluginMenu(); //插件对应菜单名称
        void ShowPluginForm(Form parent); //显示插件界面        
    }
}
