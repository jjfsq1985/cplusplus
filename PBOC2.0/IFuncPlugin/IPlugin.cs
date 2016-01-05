using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Windows.Forms;

namespace IFuncPlugin
{
    public class PublicFunc
    {
        //获取当前BCD码格式的系统时间
        public static byte[] GetBCDTime()
        {
            string strTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            int nByteSize = strTime.Length / 2;
            byte[] byteBCD = new byte[nByteSize];
            for (int i = 0; i < nByteSize; i++)
            {
                byteBCD[i] = Convert.ToByte(strTime.Substring(i * 2, 2), 16);
            }
            return byteBCD;
        }

        public static bool ByteDataEquals(byte[] byteL, byte[] byteR)
        {
            if (byteL.Length != byteR.Length)
                return false;
            for (int i = 0; i < byteL.Length; i++)
            {
                if (byteL[i] != byteR[i])
                    return false;
            }
            return true;
        }

        public static byte[] StringToBCD(string strData)
        {
            if (string.IsNullOrEmpty(strData) || strData.Length % 2 != 0)
                return null;
            try
            {
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
            catch
            {
                return null;
            }
        }

        public static string GetCardTypeString(byte CardType)
        {
            string strCardType = "";
            switch (CardType)
            {
                case 0x01:
                    strCardType = "个人卡";
                    break;
                case 0x02:
                    strCardType = "管理卡";
                    break;
                case 0x04:
                    strCardType = "员工卡";
                    break;
                case 0x06:
                    strCardType = "维修卡";
                    break;
                case 0x11:
                    strCardType = "单位子卡";
                    break;
                case 0x21:
                    strCardType = "单位母卡";
                    break;
            }
            return strCardType;
        }

        public static string GetPhysicalAddress()
        {
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                string strSplit = ":";
                string[] macVal = mac.Split(strSplit.ToCharArray(), 6);
                byte[] byteVal = new byte[16];
                int i = 0;
                foreach (string strMac in macVal)
                {
                    byteVal[i] = Convert.ToByte(strMac, 16);
                    i++;
                }                
                byteVal[6] = 0x55;
                byteVal[7] = 0xAA;
                for (i = 0; i < 8; i++)
                    byteVal[8 + i] = (byte)(byteVal[i] ^ 0xFF);                
                return BitConverter.ToString(byteVal).Replace("-", "");
            }
            catch
            {
                return "";
            }
        }

        public static string GetStringFromEncoding(byte[] SrcData, int nIndex, int nCount)
        {
            return Encoding.Unicode.GetString(SrcData, nIndex, nCount);
        }

        public static byte[] GetBytesFormEncoding(string strData)
        {
            return Encoding.Unicode.GetBytes(strData);
        }

    }

    public class GrobalVariable
    {
        public static readonly string[] strAuthority = new string[] { "账户管理", "单位管理", "站点管理", "充值记录", "制卡密钥管理", "卡片管理", "限制代码管理", "数据库管理" };

        public static readonly int Authority_Config_Count = 8;
        //权限值
        public static readonly int Account_Authority = 1;
        public static readonly int ClientInfo_Authority = 1 << 1;
        public static readonly int StationInfo_Authority = 1 << 2;
        public static readonly int RechargeList_Authority = 1 << 3;
        public static readonly int CardOp_KeyManage_Authority = 1 << 4;
        public static readonly int CardPublish_Authority = 1 << 5;        
        public static readonly int CodeTable_Authority = 1 << 6;
        public static readonly int DbManage_Authority = 1 << 7;
    }

    public class SqlConnectInfo
    {
        public bool m_bConfig = false;
        public string strServerName = "(local)";
        public string strDbName = "FunnettStation";
        public string strUser = "";
        public string strUserPwd = "";
    }

    public enum MenuType
    {
        eUnknown = 0,
        eSystemAccount, //账户管理
        eClientInfo,       //单位信息
        eStationInfo,       //站点信息
        eRechargeList,   //充值记录        
        eCardOperating,      //制卡操作
        eOneKeyMadeCard,  //一键制卡
        eCardPublish,      //卡信息读写
        eOrgKeyManage, //初始卡密钥
        ePsamKeyManage,  //PSAM卡密钥
        eUserKeysManage, //密钥管理
        eExportKeyXml,   //密钥导出成XML
        eProvinceCode,   //省代码表
        eCityCode,      //地市代码表
        eCompanyCode,   //公司代码表
        eImportKeyXml, //密钥XML文件配置
        eDbManage,       //数据库备份还原
        eToBlackCard     //卡挂失、补卡操作
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
        void SetAuthority(int nLoginUserId, int nAuthority);//该插件的权限
        void ShowPluginForm(Panel parent, SqlConnectInfo DbInfo); //显示插件界面  
    }
}
