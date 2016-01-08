using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IFuncPlugin;
using SqlServerHelper;
using System.Data.SqlClient;
using ApduParam;
using ApduCtrl;
using ApduInterface;

namespace CardOperating
{
    //合并制中石化卡和一键制卡的公共部分
    public abstract class BaseMadeCard : Form
    {
        protected ApduController m_DevControl = null;

        protected ISamCardControl m_IccCardCtrl = null;
        protected IUserCardControl m_UserCardCtrl = null;

        protected SqlConnectInfo m_DBInfo = new SqlConnectInfo();

        protected UserCardInfoParam m_CardInfoPar = new UserCardInfoParam();
        protected IccCardInfoParam m_IccCardInfoPar = new IccCardInfoParam();
        protected List<ClientInfo> m_ListClientInfo = new List<ClientInfo>();

        protected abstract void WriteMsg(int nErrColor, string strMsg);

        protected int GetClientIdIndex(int nClientID)
        {
            int nSel = -1;
            int nIndex = 0;
            foreach (ClientInfo info in m_ListClientInfo)
            {
                if (info.ClientId == nClientID)
                {
                    nSel = nIndex;
                    break;
                }
                nIndex++;
            }
            return nSel;
        }

        protected void ReadInfoFromDb()
        {
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }
            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select ClientId,ClientName from Base_Client", out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ClientInfo info = new ClientInfo();
                        info.ClientId = (int)dataReader["ClientId"];
                        info.strClientName = (string)dataReader["ClientName"];
                        m_ListClientInfo.Add(info);
                    }
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
        }

        protected bool IsExistPsamId(byte[] psamID, byte[] TermId)
        {
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return false;
            }
            bool bExist = false;
            string strPsamId = BitConverter.ToString(psamID).Replace("-", "");
            string strTermId = BitConverter.ToString(TermId).Replace("-", "");


            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = ObjSql.MakeParam("PsamId", SqlDbType.Char, 16, ParameterDirection.Input, strPsamId);
            sqlparams[1] = ObjSql.MakeParam("TermId", SqlDbType.VarChar, 12, ParameterDirection.Input, strTermId);

            SqlDataReader dataReader = null;
            ObjSql.ExecuteCommand("select * from Psam_Card where PsamId=@PsamId or TerminalId=@TermId", sqlparams, out dataReader);
            if (dataReader != null)
            {
                if (dataReader.HasRows && dataReader.Read())
                {
                    bExist = true;
                }
                dataReader.Close();
            }
            ObjSql.CloseConnection();
            ObjSql = null;
            return bExist;
        }

        protected bool InitCard()
        {
            bool bRet = false;
            if (!m_DevControl.IsDeviceOpen() || m_UserCardCtrl == null)
                return false;
            //动作
            int nResult = m_UserCardCtrl.InitCard(false);
            if (nResult != 0)
            {
                if (nResult == 1)
                    MessageBox.Show("初始化失败，请确认卡商。", "警告", MessageBoxButtons.OK);
                else if (nResult == 2)
                    MessageBox.Show("已存在卡号外部认证失败，请确认当前卡片的初始密钥。", "警告", MessageBoxButtons.OK);
                else if (nResult == 3)
                    MessageBox.Show("已存在卡号初始化失败，请确认当前卡片的初始密钥。", "警告", MessageBoxButtons.OK);
                else if (nResult == 4)
                    MessageBox.Show("外部认证失败，请确认制卡使用的初始密钥。", "警告", MessageBoxButtons.OK);
                else if (nResult == 5)
                    MessageBox.Show("初始化失败，请确认制卡使用的初始密钥。", "警告", MessageBoxButtons.OK);
            }
            else
            {
                bRet = true;
            }
            return bRet;
        }

        protected bool ResetCard()
        {
            bool bRet = false;
            if (!m_DevControl.IsDeviceOpen() || m_UserCardCtrl == null)
                return false;
            //动作
            int nResult = m_UserCardCtrl.InitCard(true);
            if (nResult != 0)
            {
                if (nResult == 1)
                    MessageBox.Show("重置失败，请确认卡商。", "警告", MessageBoxButtons.OK);
                else if (nResult == 2)
                    MessageBox.Show("已存在卡号外部认证失败，请确认当前卡片的主控密钥。", "警告", MessageBoxButtons.OK);
                else if (nResult == 3)
                    MessageBox.Show("已存在卡号初始化失败，请确认当前卡片的主控密钥。", "警告", MessageBoxButtons.OK);
                else if (nResult == 4)
                    MessageBox.Show("外部认证失败，请确认制卡使用的主控密钥。", "警告", MessageBoxButtons.OK);
                else if (nResult == 5)
                    MessageBox.Show("初始化失败，请确认制卡使用的主控密钥。", "警告", MessageBoxButtons.OK);
            }
            else
            {
                bRet = true;
            }
            return bRet;
        }

        protected void MadeCard()
        {
            if (!m_DevControl.IsDeviceOpen() || m_UserCardCtrl == null)
                return;
            if (!m_UserCardCtrl.CreateEFInMF())
                return;
            m_UserCardCtrl.CreateKey();
            byte[] UserCardId = m_CardInfoPar.GetUserCardID();
            if (UserCardId == null)
                return;
            WriteMsg(0, "用户卡号：" + "01-00-" + BitConverter.ToString(UserCardId));

            //建立应用目录
            if (!m_UserCardCtrl.CreateADFApp(1))
                return;
            //生成加气数据文件
            if (!m_UserCardCtrl.CreateApplication(UserCardId, m_CardInfoPar.DefaultPwdFlag, m_CardInfoPar.CustomPassword))
                return;
            if (!m_UserCardCtrl.UpdateApplicationFile(m_CardInfoPar, null))
                return;
            //保存至数据库            
            string strSuccess = m_UserCardCtrl.SaveCpuCardInfoToDb(m_CardInfoPar, false) ? "成功" : "失败";
            WriteMsg(0, "卡信息写入数据库，结果：" + strSuccess);
        }

        protected bool InitIccCard()
        {
            if (!m_DevControl.IsDeviceOpen() || m_IccCardCtrl == null)
                return false;
            if (!ReMakeIccCard())
                return false;
            //动作            
            if (m_IccCardCtrl.InitIccCard(false) != 0)
            {
                MessageBox.Show("当前SAM卡内主控密钥不匹配，初始化失败。\n请确认卡商然后重置。", "警告", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        protected bool ResetIccCard()
        {
            if (!m_DevControl.IsDeviceOpen() || m_IccCardCtrl == null)
                return false;
            if (!ReMakeIccCard())
                return false;
            //动作            
            if (m_IccCardCtrl.InitIccCard(true) != 0)
            {
                MessageBox.Show("当前SAM卡内主控密钥不匹配，重置失败。\n请确认卡商然后初始化。", "警告", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取卡片厂商
        /// </summary>
        /// <param name="strHexInfo">Atr</param>
        /// <param name="nDevType">设备类型 0 达华+ 明泰; 1 龙寰+明泰</param>
        /// <param name="nCardType">0 非接cpu;  1 psam卡</param>
        /// <returns></returns>
        private string GetCardDescrib(string strHexInfo, int nDevType, int nCardType)
        {
            string strInfo = "";
            if (nDevType != 0)
            {
                //龙寰
                switch (nCardType)
                {
                    case 0:
                        {
                            if (strHexInfo.Contains("4C4F48434F53"))//非接cpu卡"LOHCOS"
                                strInfo = "龙寰-非接触CPU卡:" + strHexInfo;
                            else
                                strInfo = "未找到卡片信息:" + strHexInfo;
                        }
                        break;
                    case 1:
                        {
                            if (strHexInfo.Contains("53554E434F53"))//psam卡"SUNCOS"
                                strInfo = "龙寰-PSAM卡:" + strHexInfo;
                            else
                                strInfo = "未找到卡片信息:" + strHexInfo;
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                //达华
                switch (nCardType)
                {
                    case 0:
                        {
                            if (strHexInfo.Contains("7A6A"))//非接cpu卡"zj"
                                strInfo = "达华-非接触CPU卡:" + strHexInfo;
                            else
                                strInfo = "未找到卡片信息:" + strHexInfo;
                        }
                        break;
                    case 1:
                        {
                            if (strHexInfo.Contains("52434F53"))//psam卡"RCOS"
                                strInfo = "达华-PSAM卡:" + strHexInfo;
                            else
                                strInfo = "未找到卡片信息:" + strHexInfo;
                        }
                        break;
                    default:
                        break;
                }
            }
            return strInfo;
        }


        protected bool OpenUserCard(int nCardFactory)
        {
            if (m_DevControl == null || !m_DevControl.IsDeviceOpen())
                return false;
            if (m_UserCardCtrl != null)
                return true;
            string cardInfo = "";
            bool bRet = m_DevControl.OpenCard(ref cardInfo);
            if (!bRet)
            {
                WriteMsg(Color.Red.ToArgb(), "用户卡打开失败");
                return false;
            }
            WriteMsg(0, "用户卡打开成功");
            string strDescribe = GetCardDescrib(cardInfo, nCardFactory, 0);
            if (string.IsNullOrEmpty(strDescribe))
                WriteMsg(0, "卡信息：" + cardInfo);
            else
                WriteMsg(0, strDescribe);

            m_UserCardCtrl = m_DevControl.UserCardConstructor(false, m_DBInfo);
            int nResult = m_UserCardCtrl.ReadKeyValueFromSource();
            if (nResult == 1)
                MessageBox.Show("从数据库读取用户卡密钥失败，请检查。");
            else if (nResult == 2)
                MessageBox.Show("从XML文件读取用户卡密钥失败，请检查。");
            m_UserCardCtrl.GetCosVer();
            if (nResult != 0)
                return false;
            else
                return true;
        }

        protected bool CloseUserCard()
        {
            if (m_DevControl == null)
                return false;
            m_DevControl.CloseCard();
            m_UserCardCtrl = null;
            return true;
        }

        protected bool OpenIccCard(int nCardFactory)
        {
            if (!m_DevControl.IsDeviceOpen())
                return false;
            string strCardInfo = "";
            bool bRet = m_DevControl.IccPowerOn(ref strCardInfo);
            if (!bRet)
            {
                WriteMsg(Color.Red.ToArgb(), "SAM卡复位失败");
                return false;
            }
            else
            {
                WriteMsg(0, "SAM卡复位成功");
                string strCardDescribe = GetCardDescrib(strCardInfo, nCardFactory, 1);
                if (string.IsNullOrEmpty(strCardDescribe))
                    WriteMsg(0, "复位信息：" + strCardInfo);
                else
                    WriteMsg(0, strCardDescribe);
            }

            m_IccCardCtrl = m_DevControl.SamCardConstructor(m_DBInfo);
            

            int nResult = m_IccCardCtrl.ReadKeyValueFromSource();
            if (nResult == 1)
                MessageBox.Show("从数据库读取PSAM卡密钥失败，请检查。");
            else if (nResult == 2)
                MessageBox.Show("从XML文件读取PSAM卡密钥失败，请检查。");
            m_IccCardCtrl.GetCosVer();
            if (nResult != 0)
                return false;
            else
                return true;
        }

        protected bool CloseIccCard()
        {
            if (!m_DevControl.IsDeviceOpen())
                return false;
            m_DevControl.IccPowerOff();
            WriteMsg(0, "卡片关闭成功");
            m_IccCardCtrl = null;
            return true;
        }

        protected bool ReMakeIccCard()
        {
            byte[] IccCardId = m_IccCardInfoPar.GetBytePsamId();
            byte[] TermialId = m_IccCardInfoPar.GetByteTermId();
            if (IsExistPsamId(IccCardId, TermialId))
            {
                if (MessageBox.Show("该卡的SAM序列号或终端机编号已存在，是否要重新制作？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                    return false;
            }
            return true;
        }

        protected void MadeIccCard()
        {
            byte[] IccCardId = m_IccCardInfoPar.GetBytePsamId();
            byte[] TermialId = m_IccCardInfoPar.GetByteTermId();
            WriteMsg(0, "SAM卡号：" + "01-00-" + BitConverter.ToString(IccCardId));
            WriteMsg(0, "终端机编号：" + BitConverter.ToString(TermialId));
            if (!m_IccCardCtrl.CreateIccInfo(IccCardId, TermialId))
                return;
            if (!m_IccCardCtrl.WriteApplicationInfo(m_IccCardInfoPar))
                return;
            //安装所有密钥
            if (!m_IccCardCtrl.SetupIccKey())
                return;
            if (!m_IccCardCtrl.SetupMainKey())
                return;
            //保存至数据库
            string strSuccess = m_IccCardCtrl.SavePsamCardInfoToDb(m_IccCardInfoPar) ? "成功" : "失败";
            WriteMsg(0, "卡信息写入数据库，结果：" + strSuccess);
        }

    }
}
