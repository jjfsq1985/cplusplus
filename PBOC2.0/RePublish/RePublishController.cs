using System;
using System.Collections.Generic;
using System.Text;
using IFuncPlugin;
using System.Windows.Forms;
using ApduParam;
using ApduInterface;
using System.Diagnostics;
using SqlServerHelper;
using System.Data.SqlClient;
using System.Data;
using CardControl;
using ApduCtrl;

namespace RePublish
{
    public class RePublishController
    {
        private ApduController m_DevControl = null;
        private IUserCardControl m_UserCardCtrl = null;
        private UserCardInfoParam m_CardInfoPar = new UserCardInfoParam();

        private SqlConnectInfo m_DBInfo = new SqlConnectInfo();
        private string m_strInvalidCardId = ""; //失效卡片卡号
        private bool m_bContact = false; //是否接触式CPU卡
        private ApduDomain m_DevType = ApduDomain.Unknown;

        private readonly string m_strPIN = "999999";
        private readonly byte[] m_FixedTermialId = new byte[] { 0x14, 0x32, 0x00, 0x00, 0x00, 0x01 };  //固定的终端机设备编号

        public RePublishController(string strInvalidCardId,bool bContact, int nDeviceType, SqlConnectInfo dbInfo)
        {
            m_strInvalidCardId = strInvalidCardId;
            m_bContact = bContact;
            if(nDeviceType == 0)
                m_DevType = ApduDomain.DaHua;
            else if(nDeviceType == 1)
                m_DevType = ApduDomain.LongHuan;
            else if (nDeviceType == 2)
                m_DevType = ApduDomain.LoH_at_MT;
            m_DBInfo = dbInfo;

            m_DevControl = new ApduController(m_DevType);
            m_DevControl.Open_Device();
        }

        public void ReleaseController()
        {
            if (m_DevControl == null)
                return;
            m_DevControl.Close_Device();
        }

        private bool OpenUserCard()
        {
            if (m_DevControl == null || !m_DevControl.IsDeviceOpen())
                return false;
            m_UserCardCtrl = m_DevControl.UserCardConstructor(m_bContact, m_DBInfo);

            string cardInfo = "";
            if (m_bContact)
                return m_DevControl.OpenContactCard(ref cardInfo);                
            else
                return m_DevControl.OpenCard(ref cardInfo);
        }

        private bool CloseUserCard()
        {
            if (m_DevControl == null || !m_DevControl.IsDeviceOpen())
                return false;
            if (m_bContact)
                m_DevControl.CloseContactCard();
            else
                m_DevControl.CloseCard();
            m_UserCardCtrl = null;
            return true;
        }

        /// <summary>
        /// 补卡
        /// </summary>
        /// <param name="strInvalidCardId">失效卡ID</param>
        /// <returns>新卡ID</returns>
        public string RePublishCard(string strInvalidCardId)
        {
            if (!OpenUserCard())
            {
                MessageBox.Show("打开卡片失败");
                return "";
            }

            byte[] byteRePublishCardId = GetRePublishCardId();
            if(byteRePublishCardId == null)
            {
                MessageBox.Show("未读到新卡的卡号，请确保新卡已制卡。");
                CloseUserCard(); 
                return "";
            }
            CardType CpuCardType = CardType.PersonalCard;
            double CardBalance = 0;
            if (!ReadInvalidCardFormDb(m_strInvalidCardId, m_CardInfoPar, ref CpuCardType, ref CardBalance))
            {
                MessageBox.Show("补卡失败");
                CloseUserCard(); 
                return "";
            }

            m_CardInfoPar.CardOrderNo = BitConverter.ToString(byteRePublishCardId, 5, 3).Replace("-", "");
            m_CardInfoPar.UserCardType = (CardType)byteRePublishCardId[3];
            if (CpuCardType != m_CardInfoPar.UserCardType)
            {
                MessageBox.Show("卡类型一致才能补卡，补卡失败");
                CloseUserCard(); 
                return "";
            }

            Trace.Assert(byteRePublishCardId[2] == 0x02);
            m_CardInfoPar.SetCardId(BitConverter.ToString(byteRePublishCardId, 0, 2).Replace("-", ""));

            if (!m_UserCardCtrl.UpdateCardInfo(m_CardInfoPar))
            {
                MessageBox.Show("补卡失败");
            }
            else
            {
                m_UserCardCtrl.SaveCpuCardInfoToDb(m_CardInfoPar,true);
                //圈存
                if (CpuCardType != CardType.ManagerCard && CpuCardType != CardType.ServiceCard && CardBalance > 0)
                {

                    if (CpuCardType == CardType.CompanyMotherCard)
                    {
                        RechargeMotherCard(byteRePublishCardId, CardBalance);
                    }
                    else
                    {
                        int nRet = m_UserCardCtrl.VerifyUserPin(m_strPIN);
                        if (nRet == 1)
                        {
                            LoadUserCard(m_FixedTermialId, byteRePublishCardId, CardBalance);
                        }
                        else if (nRet == 2)
                        {
                            MessageBox.Show("PIN码已锁,补卡成功但卡内余额未转入!");
                        }
                        else
                        {
                            MessageBox.Show("PIN码验证失败,补卡成功但卡内余额未转入!");
                        }
                    }                    
                }
            }
            CloseUserCard(); 
            return BitConverter.ToString(byteRePublishCardId).Replace("-","");
        }

        private void LoadUserCard(byte[] TerminalId, byte[] ASN, double dbMoney)
        {
            int nBalance = 0;
            bool bRead = m_UserCardCtrl.UserCardBalance(ref nBalance, BalanceType.Balance_ED);//圈存前读余额
            int nCardStatus = 0;
            byte[] TermialId = new byte[6];
            byte[] GTAC = new byte[4];
            bool bReadGray = m_UserCardCtrl.UserCardGray(ref nCardStatus, TermialId, GTAC);
            if(!bRead || !bReadGray)
                return;
            if (nBalance != 0 || nCardStatus != 0)
                return;
            if (m_UserCardCtrl.UserCardLoad(ASN, TerminalId, (int)(dbMoney * 100.0), true))
            {
                //写圈存数据库记录
                SaveLoadRecord(ASN, dbMoney, "CardBalance");
                string strInfo = string.Format("成功对卡号{0}圈存{1}元.", BitConverter.ToString(ASN).Replace("-", ""), dbMoney.ToString("F2"));
                MessageBox.Show(strInfo);
            }
            else
            {
                MessageBox.Show("圈存失败");
            }            
        }

        private void RechargeMotherCard(byte[] ASN, double dbMoney)
        {
            SaveLoadRecord(ASN, dbMoney, "AccountBalance");//单位母卡充值后更新字段不一样
            string strInfo = string.Format("成功对单位母卡{0}充值{1}元.", BitConverter.ToString(ASN).Replace("-", ""), dbMoney.ToString("F2"));
            MessageBox.Show(strInfo);
        }

        private void SaveLoadRecord(byte[] ASN, double dbMoney, string strUpdateField)
        {
            string strCardId = BitConverter.ToString(ASN).Replace("-", "");

            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return;
            }
            
            UpdateBaseCardMoneyValue(ObjSql, strCardId, strUpdateField, dbMoney);

            ObjSql.CloseConnection();
            ObjSql = null;
        }


        private void UpdateBaseCardMoneyValue(SqlHelper ObjSql, string strCardId, string strFieldName, double dbValue)
        {
            SqlParameter[] sqlparams = new SqlParameter[3];
            sqlparams[0] = ObjSql.MakeParam("RechargeTotal", SqlDbType.Decimal, 16, ParameterDirection.Input, Convert.ToDecimal(dbValue));
            sqlparams[1] = ObjSql.MakeParam(strFieldName, SqlDbType.Decimal, 16, ParameterDirection.Input, Convert.ToDecimal(dbValue));
            sqlparams[2] = ObjSql.MakeParam("CardId", SqlDbType.Char, 16, ParameterDirection.Input, strCardId);
            ObjSql.ExecuteCommand("update Base_Card set RechargeTotal=@RechargeTotal," + strFieldName + "=@" + strFieldName + " where CardNum=@CardId", sqlparams);
        }

        private byte[] GetRePublishCardId()
        {
            if (!m_UserCardCtrl.SelectCardApp(1))
                return null;  
            DateTime cardStart = DateTime.MinValue;
            DateTime cardEnd = DateTime.MinValue;
            return m_UserCardCtrl.GetUserCardASN(false, ref cardStart, ref cardEnd);
        }

        //从数据库读取失效的卡号的相关信息
        private bool ReadInvalidCardFormDb(string strCardId, UserCardInfoParam CardInfoPar,ref CardType CpuCardType, ref double CardBalance)
        {
            if (strCardId == null)
                return false;
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(m_DBInfo.strServerName, m_DBInfo.strDbName, m_DBInfo.strUser, m_DBInfo.strUserPwd))
            {
                ObjSql = null;
                return false;
            }            

            SqlDataReader dataReader = null;
            SqlParameter[] sqlparam = new SqlParameter[2];
            sqlparam[0] = ObjSql.MakeParam("CardNum", SqlDbType.Char, 16, ParameterDirection.Input, strCardId);
            sqlparam[1] = ObjSql.MakeParam("ApplicationIndex", SqlDbType.Int, 4, ParameterDirection.Input, 1);
            ObjSql.ExecuteProc("PROC_GetPublishedCard", sqlparam, out dataReader);
            bool bRet = false;
            if (dataReader != null)
            {
                if (!dataReader.HasRows)
                    dataReader.Close();
                else
                {
                    if (dataReader.Read())
                    {
                        bRet = true;
                        string strCardType = (string)dataReader["CardType"];
                        CpuCardType = (CardType)Convert.ToByte(strCardType, 16);                         
                        CardInfoPar.ClientID = (int)dataReader["ClientId"];
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("RelatedMotherCard")))
                        {
                            CardInfoPar.SetMotherCard((string)dataReader["RelatedMotherCard"]);
                        }
                        CardInfoPar.ValidCardBegin = (DateTime)dataReader["UseValidateDate"];
                        CardInfoPar.ValidCardEnd   = (DateTime)dataReader["UseInvalidateDate"];
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("Plate")))
                        {
                            CardInfoPar.CarNo = (string)dataReader["Plate"];
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("SelfId")))
                        {
                            CardInfoPar.SelfId = (string)dataReader["SelfId"];
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("CertificatesType")))
                        {
                            string strCertType = (string)dataReader["CertificatesType"];
                            CardInfoPar.IdType = (UserCardInfoParam.IdentityType)Convert.ToByte(strCertType, 16);
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("PersonalId")))
                        {
                            CardInfoPar.UserIdentity = (string)dataReader["PersonalId"];                            
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("DriverName")))
                        {
                            CardInfoPar.UserName = (string)dataReader["DriverName"];
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("DriverTel")))
                        {
                            CardInfoPar.TelePhone = (string)dataReader["DriverTel"];
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("VechileCategory")))
                        {
                            CardInfoPar.CarType = (string)dataReader["VechileCategory"];
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("SteelCylinderId")))
                        {
                            CardInfoPar.BoalId = (string)dataReader["SteelCylinderId"];
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("CylinderTestDate")))
                        {
                            CardInfoPar.BoalExprie = (DateTime)dataReader["CylinderTestDate"];
                        }
                        if(CpuCardType == CardType.CompanyMotherCard)
                        {
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("AccountBalance")))
                            {
                                decimal balance = (decimal)dataReader["AccountBalance"];
                                CardBalance = decimal.ToDouble(balance);
                            }
                        }
                        else
                        {
                            if (!dataReader.IsDBNull(dataReader.GetOrdinal("CardBalance")))
                            {
                                decimal balance = (decimal)dataReader["CardBalance"];
                                CardBalance = decimal.ToDouble(balance);
                            }
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("R_OilTimesADay")))
                        {
                            CardInfoPar.LimitGasFillCount = (byte)dataReader["R_OilTimesADay"];
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("R_OilVolTotal")))
                        {
                            CardInfoPar.LimitGasFillAmount = (uint)dataReader["R_OilVolTotal"];
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("R_OilEndDate")))
                        {
                            CardInfoPar.ValidCardEnd = (DateTime)dataReader["R_OilEndDate"];
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("R_Plate")))
                        {
                            CardInfoPar.LimitCarNo = (bool)dataReader["R_Plate"];
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("R_Oil")))
                        {
                            string strR_Oil = (string)dataReader["R_Oil"];
                            CardInfoPar.LimitGasType = Convert.ToUInt16(strR_Oil, 16);
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("CylinderNum")))
                        {
                            CardInfoPar.CylinderNum = (int)dataReader["CylinderNum"];                            
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("FactoryNum")))
                        {
                            CardInfoPar.BoalFactoryID = (string)dataReader["FactoryNum"];
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("CylinderVolume")))
                        {
                            CardInfoPar.CylinderVolume = (ushort)dataReader["CylinderVolume"];
                        }
                        if (!dataReader.IsDBNull(dataReader.GetOrdinal("BusDistance")))
                        {
                            CardInfoPar.BusDistance = (string)dataReader["BusDistance"];
                        }
                    }
                    dataReader.Close();
                }
            }
            ObjSql.CloseConnection();
            ObjSql = null;

            return bRet;
        }

    }
}
