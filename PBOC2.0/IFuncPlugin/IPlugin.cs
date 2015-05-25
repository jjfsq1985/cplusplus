using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IFuncPlugin
{
    public class ConvertBCD
    {
        public static byte[] StringToBCD(string strData)
        {
            if (string.IsNullOrEmpty(strData) || strData.Length % 2 != 0)
                return null;
            int nByteSize = strData.Length / 2;
            byte[] byteBCD = new byte[nByteSize];
            for (int i = 0; i < nByteSize; i++)
            {
                byte bcdbyte = 0;
                byte.TryParse(strData.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber, null, out bcdbyte);
                byteBCD[i] = bcdbyte;
            }
            return byteBCD;
        }
    }

    public class GrobalVariable
    {
        public static readonly string[] strAuthority = new string[] { "账户管理", "单位管理", "站点管理", "充值记录", "制卡操作", "卡信息维护", "密钥管理", "限制代码管理" };

        public static readonly int Authority_Config_Count = 8;
        //权限值
        public static readonly int Account_Authority = 1;
        public static readonly int ClientInfo_Authority = 1 << 1;
        public static readonly int StationInfo_Authority = 1 << 2;
        public static readonly int RechargeList_Authority = 1 << 3;
        public static readonly int CardOperating_Authority = 1 << 4;
        public static readonly int CardPublish_Authority = 1 << 5;
        public static readonly int KeyManage_Authority = 1 << 6;
        public static readonly int CodeTable_Authority = 1 << 7;
    }

    public enum MenuType
    {
        eSystemAccount, //账户管理
        eClientInfo,       //单位信息
        eStationInfo,       //站点信息
        eRechargeList,   //充值记录        
        eCardOperating,      //制卡操作
        eCardPublish,      //卡信息读写
        eOrgKeyManage, //初始卡密钥
        ePsamKeyManage,  //PSAM卡密钥
        eUserKeysManage, //密钥管理
        eProvinceCode,   //省代码表
        eCityCode,      //地市代码表
        eCompanyCode,   //公司代码表
    }

    //插件中各个控件的位置
    public struct ControlPos
    {
        public int x;
        public double dbRateH;//横向比例
        public int y;
        public double dbRateV; //纵向比例
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
