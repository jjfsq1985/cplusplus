using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SqlServerHelper;
using System.Data.SqlClient;
using System.Data;
using IFuncPlugin;
using ApduParam;

namespace CardControl
{

    public class PsamKeyData
    {
        public string strDescribe = "";
        public byte[] OrgKeyVal = new byte[16];
        public byte[] MasterKeyVal = new byte[16];
        public byte[] MasterTendingKeyVal = new byte[16];

        public byte[] ApplicationMasterKey = new byte[16];
        public byte[] ApplicationTendingKey = new byte[16];
        public byte[] ConsumerMasterKey = new byte[16];
        public byte[] GrayCardKey = new byte[16];
        public byte[] MacEncryptKey = new byte[16];       
    }


    public class CpuKeyData
    {
        public int nAppIndex = 0;
        public string strDescribe = "";
        
        public byte[] OrgKeyVal = new byte[16];
        public byte[] MasterKeyVal = new byte[16];
        public byte[] MasterTendingKeyVal = new byte[16];

        public byte[] AppMasterKey = new byte[16];
        public byte[] AppTendingKey = new byte[16];
        public byte[] AppInternalAuthKey = new byte[16];
        public byte[] AppPinResetKey = new byte[16];
        public byte[] AppPinUnlockKey = new byte[16];
        public byte[] AppConsumerKey = new byte[16];
        public byte[] AppLoadKey = new byte[16];
        public byte[] AppTacKey = new byte[16];
        public byte[] AppUnGrayKey = new byte[16];
        public byte[] AppUnLoadKey = new byte[16];
        public byte[] AppOverdraftKey = new byte[16];
    }

    public class GlobalControl
    {
        public static string GetErrString(byte SW1, byte SW2, string strErrCode)
        {
            if (SW1 == 0x63 && (byte)(SW2 & 0xF0) == 0xC0)
            {
                int nRetry = (int)(SW2 & 0x0F);
                return string.Format("认证失败，剩余{0}次机会", nRetry);
            }
            else if (SW1 == 0x69 && SW2 == 0x83)
            {
                return "认证方法已锁";
            }
            else if (SW1 == 0x93 && SW2 == 0x03)
            {
                return "应用永久锁定";
            }
            return "其他错误：" + strErrCode;
        }


        //计算过程密钥
        public static byte[] GetProcessKey(byte[] ASN, byte[] MasterKey, byte[] RandVal, byte[] byteSn)
        {
            if (ASN.Length != 8)
                return null;
            //过程密钥
            byte[] SubKey = new byte[16];
            byte[] encryptAsn = DesCryptography.TripleEncryptData(ASN, MasterKey);
            byte[] XorASN = new byte[8];
            for (int i = 0; i < 8; i++)
                XorASN[i] = (byte)(ASN[i] ^ 0xFF);
            byte[] encryptXorAsn = DesCryptography.TripleEncryptData(XorASN, MasterKey);
            Buffer.BlockCopy(encryptAsn, 0, SubKey, 0, 8);
            Buffer.BlockCopy(encryptXorAsn, 0, SubKey, 8, 8);
            byte[] byteData = new byte[8];
            Buffer.BlockCopy(RandVal, 0, byteData, 0, 4);
            Buffer.BlockCopy(byteSn, 0, byteData, 4, 2);
            byteData[6] = 0x80;
            byteData[7] = 0x00;
            byte[] byteRetKey = DesCryptography.TripleEncryptData(byteData, SubKey);
            return byteRetKey;
        }

        public static void StrKeyToByte(string strKey, byte[] byteKey)
        {
            byte[] BcdKey = PublicFunc.StringToBCD(strKey);
            if (BcdKey.Length == 16)
                Buffer.BlockCopy(BcdKey, 0, byteKey, 0, 16);
        }

        //卡信息维护相关的密钥读取
        public static byte[] GetPrivateKeyFromXml(string strXmlPath, string strSectionName,string strKeyName)
        {
            byte[] KeyValue = new byte[16];             
            try
            {
                XmlNode node = null;
                XmlDocument xml = new XmlDocument();
                xml.Load(strXmlPath);//按路径读xml文件
                XmlNode root = xml.DocumentElement;//指向根节点
                node = root.SelectSingleNode("Seed");
                byte[] InitData = PublicFunc.StringToBCD(node.InnerText);
                node = root.SelectSingleNode("InitKey");
                byte[] InitKey = PublicFunc.StringToBCD(node.InnerText);

                byte[] Left = DesCryptography.TripleEncryptData(InitData, InitKey);
                byte[] Right = DesCryptography.TripleDecryptData(InitData, InitKey);
                byte[] EncryptKey = new byte[16];
                Buffer.BlockCopy(Left, 0, EncryptKey, 0, 8);
                Buffer.BlockCopy(Right, 0, EncryptKey, 8, 8);

                XmlNode UserKeyNode = root.SelectSingleNode(strSectionName);

                node = UserKeyNode.SelectSingleNode(strKeyName);
                byte[] byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyValue, 0, 16);
            }
            catch
            {
                return null;
            }

            return KeyValue;
        }

        private static void GetDbOrgKeyVal(SqlHelper ObjSql, int OrgKeyType, byte[] byteKeyVal)
        {
            SqlDataReader dataReader = null;
            //OrgKeyType 0-CPU卡，1-PSAM卡
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = ObjSql.MakeParam("OrgKeyType", SqlDbType.Int, 4, ParameterDirection.Input, OrgKeyType);
            ObjSql.ExecuteProc("PROC_GetOrgKey", sqlparams, out dataReader);
            if (dataReader == null)
                return;
            if (!dataReader.HasRows)
            {
                dataReader.Close();
                dataReader = null;
                return;
            }
            if (dataReader.Read())
            {
                string strKey = (string)dataReader["OrgKey"];
                byte[] BcdKey = PublicFunc.StringToBCD(strKey);
                Buffer.BlockCopy(BcdKey, 0, byteKeyVal, 0, 16);
            }
            dataReader.Close();
            dataReader = null;
        }

        public static byte[] GetPublishedCardKeyFormDb(SqlConnectInfo dbInfo,byte[] CardId, string strKeyName, int nAppIndex)
        {
            if (CardId == null)
                return null;
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(dbInfo.strServerName, dbInfo.strDbName, dbInfo.strUser, dbInfo.strUserPwd))
            {
                ObjSql = null;
                return null;
            }
            string strDbAsn = BitConverter.ToString(CardId).Replace("-", "");

            SqlDataReader dataReader = null;
            SqlParameter[] sqlparam = new SqlParameter[2];
            sqlparam[0] = ObjSql.MakeParam("CardNum", SqlDbType.Char, 16, ParameterDirection.Input, strDbAsn);
            sqlparam[1] = ObjSql.MakeParam("ApplicationIndex", SqlDbType.Int, 4, ParameterDirection.Input, nAppIndex);
            ObjSql.ExecuteProc("PROC_GetPublishedCard", sqlparam, out dataReader);
            string strKeyUsed = "";
            byte[] KeyValue = null;
            if (dataReader != null)
            {
                if (!dataReader.HasRows)
                    dataReader.Close();
                else
                {
                    if (dataReader.Read())
                    {
                        strKeyUsed = (string)dataReader[strKeyName];
                        byte[] byteKey = PublicFunc.StringToBCD(strKeyUsed);
                        KeyValue = new byte[16];
                        Buffer.BlockCopy(byteKey, 0, KeyValue, 0, 16);
                    }
                    dataReader.Close();
                }
            }
            ObjSql.CloseConnection();
            ObjSql = null;
            return KeyValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbInfo">数据库参数</param>
        /// <param name="strProcName">存储过程名称</param>
        /// <param name="strKeyName">列名称</param>
        /// <param name="AppIndex">为0时读取PSAM卡消费密钥</param>
        /// <returns></returns>
        public static byte[] GetDbConsumerKey(SqlConnectInfo dbInfo, string strProcName, string strKeyName, int AppIndex)
        {
            SqlHelper ObjSql = new SqlHelper();
            if (!ObjSql.OpenSqlServerConnection(dbInfo.strServerName, dbInfo.strDbName, dbInfo.strUser, dbInfo.strUserPwd))
            {
                ObjSql = null;
                return null;
            }
            SqlDataReader dataReader = null;
            if (AppIndex == 0)
            {
                ObjSql.ExecuteProc(strProcName, out dataReader);
            }
            else
            {
                SqlParameter[] sqlparam = new SqlParameter[1];
                sqlparam[0] = ObjSql.MakeParam("ApplicationIndex", SqlDbType.Int, 4, ParameterDirection.Input, AppIndex);
                ObjSql.ExecuteProc(strProcName, sqlparam, out dataReader);
            }
            if (dataReader == null)
            {
                ObjSql.CloseConnection();
                ObjSql = null;
                return null;
            }
            if (!dataReader.HasRows || !dataReader.Read())
            {
                dataReader.Close();
                ObjSql.CloseConnection();
                ObjSql = null;
                return null;
            }

            byte[] ConsumerKey = new byte[16];
            string strKey = (string)dataReader[strKeyName];
            byte[] BcdKey = PublicFunc.StringToBCD(strKey);
            Buffer.BlockCopy(BcdKey, 0, ConsumerKey, 0, 16);
            dataReader.Close();
            dataReader = null;
            ObjSql.CloseConnection();
            ObjSql = null;
            return ConsumerKey;
        }        

        public static bool GetDbPsamKeyVal(SqlHelper ObjSql, PsamKeyData PsamKey)
        {
            GetDbOrgKeyVal(ObjSql,1,PsamKey.OrgKeyVal);
            SqlDataReader dataReader = null;
            string strKey = "";
            //////////////////////////////////////////////////////////////////////////
            ObjSql.ExecuteProc("PROC_GetPsamKey", out dataReader);
            if (dataReader == null)
                return false;            
            if(!dataReader.HasRows)
            {
                dataReader.Close();
                dataReader = null;
                return false;
            }

            if (dataReader.Read())
            {
                strKey = (string)dataReader["MasterKey"];
                GlobalControl.StrKeyToByte(strKey, PsamKey.MasterKeyVal);
                strKey = (string)dataReader["MasterTendingKey"];
                GlobalControl.StrKeyToByte(strKey, PsamKey.MasterTendingKeyVal);
                strKey = (string)dataReader["ApplicationMasterKey"];
                GlobalControl.StrKeyToByte(strKey, PsamKey.ApplicationMasterKey);                
                strKey = (string)dataReader["ApplicationTendingKey"];
                GlobalControl.StrKeyToByte(strKey, PsamKey.ApplicationTendingKey);
                strKey = (string)dataReader["ConsumerMasterKey"];
                GlobalControl.StrKeyToByte(strKey, PsamKey.ConsumerMasterKey);
                strKey = (string)dataReader["GrayCardKey"];
                GlobalControl.StrKeyToByte(strKey, PsamKey.GrayCardKey);
                strKey = (string)dataReader["MacEncryptKey"];
                GlobalControl.StrKeyToByte(strKey, PsamKey.MacEncryptKey);
                PsamKey.strDescribe = (string)dataReader["InfoRemark"];
            }
            dataReader.Close();
            dataReader = null;
            return true;    
        }

        //还用不上的密钥没有读出
        public static bool GetDbCpuKeyVal(SqlHelper ObjSql,CpuKeyData CpuKey)
        {
            GetDbOrgKeyVal(ObjSql, 0, CpuKey.OrgKeyVal);
            SqlDataReader dataReader = null;
            string strKey = "";
            //读卡密钥和加气应用密钥
            SqlParameter[] sqlparam = new SqlParameter[1];
            sqlparam[0] = ObjSql.MakeParam("ApplicationIndex", SqlDbType.Int, 4, ParameterDirection.Input, CpuKey.nAppIndex);
            ObjSql.ExecuteProc("PROC_GetCpuKey", sqlparam, out dataReader);
            if (dataReader == null)
                return false;

            if (!dataReader.HasRows)
            {
                dataReader.Close();
                return false;
            }
            if (dataReader.Read())
            {
                strKey = (string)dataReader["MasterKey"];
                GlobalControl.StrKeyToByte(strKey, CpuKey.MasterKeyVal);
                strKey = (string)dataReader["MasterTendingKey"];
                GlobalControl.StrKeyToByte(strKey, CpuKey.MasterTendingKeyVal);
                strKey = (string)dataReader["AppMasterKey"];
                GlobalControl.StrKeyToByte(strKey, CpuKey.AppMasterKey);
                strKey = (string)dataReader["AppTendingKey"];
                GlobalControl.StrKeyToByte(strKey, CpuKey.AppTendingKey);
                strKey = (string)dataReader["AppAuthKey"];
                GlobalControl.StrKeyToByte(strKey, CpuKey.AppInternalAuthKey);
                strKey = (string)dataReader["AppPinResetKey"];
                GlobalControl.StrKeyToByte(strKey, CpuKey.AppPinResetKey);
                strKey = (string)dataReader["AppPinUnlockKey"];
                GlobalControl.StrKeyToByte(strKey, CpuKey.AppPinUnlockKey);
                strKey = (string)dataReader["AppConsumerKey"];
                GlobalControl.StrKeyToByte(strKey, CpuKey.AppConsumerKey);
                strKey = (string)dataReader["AppLoadKey"];
                GlobalControl.StrKeyToByte(strKey, CpuKey.AppLoadKey);
                strKey = (string)dataReader["AppTacKey"];
                GlobalControl.StrKeyToByte(strKey, CpuKey.AppTacKey);
                strKey = (string)dataReader["AppUnGrayKey"];
                GlobalControl.StrKeyToByte(strKey, CpuKey.AppUnGrayKey);
                strKey = (string)dataReader["AppUnLoadKey"];
                GlobalControl.StrKeyToByte(strKey, CpuKey.AppUnLoadKey);
                strKey = (string)dataReader["AppOverdraftKey"];
                GlobalControl.StrKeyToByte(strKey, CpuKey.AppOverdraftKey);
                CpuKey.strDescribe = (string)dataReader["InfoRemark"];
            }
            dataReader.Close();
            dataReader = null;
            return true;
        }

        public static bool GetXmlCpuKeyVal(string strXmlPath, CpuKeyData KeyVal)
        {
            byte[] byteKey = null;
            try
            {
                XmlNode node = null;
                XmlDocument xml = new XmlDocument();
                xml.Load(strXmlPath);//按路径读xml文件
                XmlNode root = xml.DocumentElement;//指向根节点
                node = root.SelectSingleNode("Seed");
                byte[] InitData = PublicFunc.StringToBCD(node.InnerText);
                node = root.SelectSingleNode("InitKey");
                byte[] InitKey = PublicFunc.StringToBCD(node.InnerText);

                byte[] Left = DesCryptography.TripleEncryptData(InitData, InitKey);
                byte[] Right = DesCryptography.TripleDecryptData(InitData, InitKey);
                byte[] EncryptKey = new byte[16];
                Buffer.BlockCopy(Left, 0, EncryptKey, 0, 8);
                Buffer.BlockCopy(Right, 0, EncryptKey, 8, 8);

                node = root.SelectSingleNode("UserOrgKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.OrgKeyVal, 0, 16);

                string strName = string.Format("UserKeyValue_App{0}", KeyVal.nAppIndex);
                XmlNode CpuKeyNode = root.SelectSingleNode(strName);

                node = CpuKeyNode.SelectSingleNode("Describe");
                KeyVal.strDescribe = node.InnerText;

                node = CpuKeyNode.SelectSingleNode("MasterKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.MasterKeyVal, 0, 16);

                node = CpuKeyNode.SelectSingleNode("MasterTendingKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.MasterTendingKeyVal, 0, 16);

                node = CpuKeyNode.SelectSingleNode("AppMasterKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.AppMasterKey, 0, 16);

                node = CpuKeyNode.SelectSingleNode("AppTendingKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.AppTendingKey, 0, 16);

                node = CpuKeyNode.SelectSingleNode("AppAuthKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.AppInternalAuthKey, 0, 16);

                node = CpuKeyNode.SelectSingleNode("AppPinResetKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.AppPinResetKey, 0, 16);

                node = CpuKeyNode.SelectSingleNode("AppPinUnlockKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.AppPinUnlockKey, 0, 16);

                node = CpuKeyNode.SelectSingleNode("AppConsumerKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.AppConsumerKey, 0, 16);

                node = CpuKeyNode.SelectSingleNode("AppLoadKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.AppLoadKey, 0, 16);

                node = CpuKeyNode.SelectSingleNode("AppTacKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.AppTacKey, 0, 16);

                node = CpuKeyNode.SelectSingleNode("AppUnGrayKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.AppUnGrayKey, 0, 16);

                node = CpuKeyNode.SelectSingleNode("AppUnLoadKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.AppUnLoadKey, 0, 16);

                node = CpuKeyNode.SelectSingleNode("AppOverdraftKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.AppOverdraftKey, 0, 16);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool GetXmlPsamKeyVal(string strXmlPath, PsamKeyData KeyVal)
        {
            byte[] byteKey = null;
            try
            {
                XmlNode node = null;
                XmlDocument xml = new XmlDocument();
                xml.Load(strXmlPath);//按路径读xml文件
                XmlNode root = xml.DocumentElement;//指向根节点
                node = root.SelectSingleNode("Seed");
                byte[] InitData = PublicFunc.StringToBCD(node.InnerText);
                node = root.SelectSingleNode("InitKey");
                byte[] InitKey = PublicFunc.StringToBCD(node.InnerText);

                byte[] Left = DesCryptography.TripleEncryptData(InitData, InitKey);
                byte[] Right = DesCryptography.TripleDecryptData(InitData, InitKey);
                byte[] EncryptKey = new byte[16];
                Buffer.BlockCopy(Left, 0, EncryptKey, 0, 8);
                Buffer.BlockCopy(Right, 0, EncryptKey, 8, 8);

                node = root.SelectSingleNode("PsamOrgKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.OrgKeyVal, 0, 16);

                XmlNode PsamKeyNode = root.SelectSingleNode("PsamKeyValue");

                node = PsamKeyNode.SelectSingleNode("Describe");
                KeyVal.strDescribe = node.InnerText;

                node = PsamKeyNode.SelectSingleNode("MasterKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.MasterKeyVal, 0, 16);

                node = PsamKeyNode.SelectSingleNode("MasterTendingKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.MasterTendingKeyVal, 0, 16);

                node = PsamKeyNode.SelectSingleNode("ApplicationMasterKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.ApplicationMasterKey, 0, 16);

                node = PsamKeyNode.SelectSingleNode("ApplicationTendingKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.ApplicationTendingKey, 0, 16);

                node = PsamKeyNode.SelectSingleNode("ConsumerMasterKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.ConsumerMasterKey, 0, 16);

                node = PsamKeyNode.SelectSingleNode("GrayCardKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.GrayCardKey, 0, 16);

                node = PsamKeyNode.SelectSingleNode("MacEncryptKey");
                byteKey = DesCryptography.TripleDecryptData(PublicFunc.StringToBCD(node.InnerText), EncryptKey);
                Buffer.BlockCopy(byteKey, 0, KeyVal.MacEncryptKey, 0, 16);
            }
            catch
            {
                return false;
            }
            return true;
        }

        //从XML获取密钥写入数据库Base_Card_Key表
        public static void InsertCardKeyFromXml(SqlHelper ObjSql, Guid KeyGuid, byte[] ASN, string strXmlFile)
        {
            try
            {
                CpuKeyData KeyData = new CpuKeyData();
                KeyData.nAppIndex = 1;
                if (!GetXmlCpuKeyVal(strXmlFile, KeyData))
                    return;
                string strCardId = BitConverter.ToString(ASN).Replace("-", "");
                SqlParameter[] sqlparams = new SqlParameter[12];
                sqlparams[0] = ObjSql.MakeParam("CardId", SqlDbType.Char, 16, ParameterDirection.Input, strCardId);
                sqlparams[1] = ObjSql.MakeParam("UserKeyGuid", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, KeyGuid);

                string strOrgKey = BitConverter.ToString(KeyData.OrgKeyVal).Replace("-", "");
                sqlparams[2] = ObjSql.MakeParam("OrgKey", SqlDbType.Char, 32, ParameterDirection.Input, strOrgKey);
 
                string strMasterKey = BitConverter.ToString(KeyData.MasterKeyVal).Replace("-", "");
                sqlparams[3] = ObjSql.MakeParam("MasterKey", SqlDbType.Char, 32, ParameterDirection.Input, strMasterKey);
                sqlparams[4] = ObjSql.MakeParam("ApplicationIndex", SqlDbType.Int, 4, ParameterDirection.Input, KeyData.nAppIndex);                

                string strAppTendingKey = BitConverter.ToString(KeyData.AppTendingKey).Replace("-", "");
                sqlparams[5] = ObjSql.MakeParam("AppTendingKey", SqlDbType.Char, 32, ParameterDirection.Input, strAppTendingKey);

                string strAppConsumerKey = BitConverter.ToString(KeyData.AppConsumerKey).Replace("-", "");
                sqlparams[6] = ObjSql.MakeParam("AppTendingKey", SqlDbType.Char, 32, ParameterDirection.Input, strAppConsumerKey);

                string strAppLoadKey = BitConverter.ToString(KeyData.AppLoadKey).Replace("-", "");
                sqlparams[7] = ObjSql.MakeParam("AppLoadKey", SqlDbType.Char, 32, ParameterDirection.Input, strAppLoadKey);

                string strAppUnloadKey = BitConverter.ToString(KeyData.AppUnLoadKey).Replace("-", "");
                sqlparams[8] = ObjSql.MakeParam("AppUnLoadKey", SqlDbType.Char, 32, ParameterDirection.Input, strAppUnloadKey);

                string strAppUnGrayKey = BitConverter.ToString(KeyData.AppUnGrayKey).Replace("-", "");
                sqlparams[9] = ObjSql.MakeParam("AppUnGrayKey", SqlDbType.Char, 32, ParameterDirection.Input, strAppUnGrayKey);
     
                string strAppPinUnlockKey = BitConverter.ToString(KeyData.AppPinUnlockKey).Replace("-", "");
                sqlparams[10] = ObjSql.MakeParam("AppPinUnlockKey", SqlDbType.Char, 32, ParameterDirection.Input, strAppPinUnlockKey);
          
                string strAppPinResetKey = BitConverter.ToString(KeyData.AppPinResetKey).Replace("-", "");
                sqlparams[11] = ObjSql.MakeParam("AppPinResetKey", SqlDbType.Char, 32, ParameterDirection.Input, strAppPinResetKey);
                
                ObjSql.ExecuteProc("PROC_PublishCardKey", sqlparams);
            }
            catch
            {

            }
        }

        //从Key_CARD_ADF表获取密钥写入数据库Base_Card_Key表
        public static void InsertCardKeyFromDb(SqlHelper ObjSql, Guid KeyGuid, byte[] ASN)
        {
            try
            {
                CpuKeyData KeyData = new CpuKeyData();
                KeyData.nAppIndex = 1;
                if (!GetDbCpuKeyVal(ObjSql, KeyData))
                    return;
                string strCardId = BitConverter.ToString(ASN).Replace("-", "");
                SqlParameter[] sqlparams = new SqlParameter[12];
                sqlparams[0] = ObjSql.MakeParam("CardId", SqlDbType.Char, 16, ParameterDirection.Input, strCardId);
                sqlparams[1] = ObjSql.MakeParam("UserKeyGuid", SqlDbType.UniqueIdentifier, 16, ParameterDirection.Input, KeyGuid);

                string strOrgKey = BitConverter.ToString(KeyData.OrgKeyVal).Replace("-", "");
                sqlparams[2] = ObjSql.MakeParam("OrgKey", SqlDbType.Char, 32, ParameterDirection.Input, strOrgKey);

                string strMasterKey = BitConverter.ToString(KeyData.MasterKeyVal).Replace("-", "");
                sqlparams[3] = ObjSql.MakeParam("MasterKey", SqlDbType.Char, 32, ParameterDirection.Input, strMasterKey);
                sqlparams[4] = ObjSql.MakeParam("ApplicationIndex", SqlDbType.Int, 4, ParameterDirection.Input, KeyData.nAppIndex);

                string strAppTendingKey = BitConverter.ToString(KeyData.AppTendingKey).Replace("-", "");
                sqlparams[5] = ObjSql.MakeParam("AppTendingKey", SqlDbType.Char, 32, ParameterDirection.Input, strAppTendingKey);

                string strAppConsumerKey = BitConverter.ToString(KeyData.AppConsumerKey).Replace("-", "");
                sqlparams[6] = ObjSql.MakeParam("AppTendingKey", SqlDbType.Char, 32, ParameterDirection.Input, strAppConsumerKey);

                string strAppLoadKey = BitConverter.ToString(KeyData.AppLoadKey).Replace("-", "");
                sqlparams[7] = ObjSql.MakeParam("AppLoadKey", SqlDbType.Char, 32, ParameterDirection.Input, strAppLoadKey);

                string strAppUnloadKey = BitConverter.ToString(KeyData.AppUnLoadKey).Replace("-", "");
                sqlparams[8] = ObjSql.MakeParam("AppUnLoadKey", SqlDbType.Char, 32, ParameterDirection.Input, strAppUnloadKey);

                string strAppUnGrayKey = BitConverter.ToString(KeyData.AppUnGrayKey).Replace("-", "");
                sqlparams[9] = ObjSql.MakeParam("AppUnGrayKey", SqlDbType.Char, 32, ParameterDirection.Input, strAppUnGrayKey);

                string strAppPinUnlockKey = BitConverter.ToString(KeyData.AppPinUnlockKey).Replace("-", "");
                sqlparams[10] = ObjSql.MakeParam("AppPinUnlockKey", SqlDbType.Char, 32, ParameterDirection.Input, strAppPinUnlockKey);

                string strAppPinResetKey = BitConverter.ToString(KeyData.AppPinResetKey).Replace("-", "");
                sqlparams[11] = ObjSql.MakeParam("AppPinResetKey", SqlDbType.Char, 32, ParameterDirection.Input, strAppPinResetKey);

                ObjSql.ExecuteProc("PROC_PublishCardKey", sqlparams);
            }
            catch
            {

            }
        }
    }
}
