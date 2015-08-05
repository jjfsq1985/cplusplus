using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Data;
using IFuncPlugin;
using System.Diagnostics;
using SqlServerHelper;

namespace PublishCardOperator
{
    public class  OrgKeyValue
    {
        public int nKeyId = 0;  //初始密钥ID        
        public byte[] OrgKey = new byte[16]; //密钥
        public int nKeyType = 0;  //密钥种类（0用于CPU卡，1用于PSAM卡 ）
        public string KeyDetail;  //密钥信息描述
        public bool bValid = false; //是否使用
        public DbStateFlag eDbFlag = DbStateFlag.eDbOK;  //是否已修改
    }

    public class PsamKeyValue
    {
        public int nKeyId = 0;  //密钥ID        
        public byte[] MasterKey = new byte[16]; //主控密钥
        public byte[] MasterTendingKey = new byte[16]; //卡片维护密钥
        public byte[] AppMasterKey = new byte[16]; //应用主控密钥
        public byte[] AppTendingKey = new byte[16]; //应用维护密钥
        public byte[] ConsumerMasterKey = new byte[16]; //消费主密钥
        public byte[] GrayCardKey = new byte[16]; //灰锁密钥
        public byte[] MacEncryptKey = new byte[16]; //MAC加密密钥
        public string KeyDetail;  //密钥信息描述
        public bool bValid = false; //是否使用
        public DbStateFlag eDbFlag = DbStateFlag.eDbOK;  //是否已修改

    }

    public class AppKeyValueGroup
    {
        public int AppIndex = 0;   //应用号
        public byte[] AppMasterKey = new byte[16]; //应用主控密钥
        public byte[] AppTendingKey = new byte[16]; //应用维护密钥
        public byte[] AppInternalAuthKey = new byte[16]; //应用内部认证
        public byte[] PINResetKey = new byte[16];  //PIN密码重装密钥
        public byte[] PINUnlockKey = new byte[16]; //PIN解锁密钥
        public byte[] ConsumerMasterKey = new byte[16]; //消费主密钥
        public byte[] LoadKey = new byte[16];//圈存密钥
        public byte[] TacMasterKey = new byte[16];//TAC密钥
        public byte[] UnGrayKey = new byte[16];//联机解扣密钥
        public byte[] UnLoadKey = new byte[16];//圈提密钥
        public byte[] OverdraftKey = new byte[16];//修改透支限额主密钥
        public DbStateFlag eDbFlag = DbStateFlag.eDbOK;  //是否已修改 
    }

    public class CpuKeyValue
    {
        public int nKeyId = 0;  //密钥ID
        public byte[] MasterKey = new byte[16]; //主控密钥
        public byte[] MasterTendingKey = new byte[16]; //卡片维护密钥
        public byte[] InternalAuthKey = new byte[16];  //内部认证密钥
        public string KeyDetail;  //密钥信息描述
        public bool bValid = false; //是否使用
        public DbStateFlag eDbFlag = DbStateFlag.eDbOK;  //是否已修改 
        public List<AppKeyValueGroup> LstAppKeyGroup = new List<AppKeyValueGroup>();
    }


    public enum DbStateFlag
    {
        eDbOK,  //正常
        eDbDirty,  //db需更新
        eDbAdd,   //新增
        eDbDelete  //删除
    } 

    public class RelatedKeyInDb
    {
        public static byte[] GetCpuConsumerKey(SqlHelper sqlHelp)
        {
            SqlDataReader dataReader = null;
                SqlParameter[] sqlparam = new SqlParameter[1];
                sqlparam[0] = sqlHelp.MakeParam("ApplicationIndex", SqlDbType.Int, 4, ParameterDirection.Input, 1);
                sqlHelp.ExecuteProc("PROC_GetCpuKey", sqlparam, out dataReader);
            if (dataReader == null)
                return null;
            if (!dataReader.HasRows)
            {
                dataReader.Close();
                return null;
            }
            else
            {
                byte[] ConsumerKey = new byte[16];
                if (dataReader.Read())
                {
                    string strKey = (string)dataReader["ConsumerMasterKey"];
                    byte[] BcdKey = PublicFunc.StringToBCD(strKey);
                    Trace.Assert(BcdKey.Length == 16);
                    Buffer.BlockCopy(BcdKey, 0, ConsumerKey, 0, 16);                    
                }
                dataReader.Close();
                return ConsumerKey;
            }
        }

        public static byte[] GetPsamConsumerKey(SqlHelper sqlHelp)
        {
            SqlDataReader dataReader = null;
            sqlHelp.ExecuteProc("PROC_GetPsamKey", out dataReader);
            if (dataReader == null)
                return null;
            if (!dataReader.HasRows)
            {
                dataReader.Close();
                return null;
            }
            else
            {
                byte[] ConsumerKey = new byte[16];
                if (dataReader.Read())
                {
                    string strKey = (string)dataReader["ConsumerMasterKey"];
                    byte[] BcdKey = PublicFunc.StringToBCD(strKey);
                    Trace.Assert(BcdKey.Length == 16);
                    Buffer.BlockCopy(BcdKey, 0, ConsumerKey, 0, 16);
                }
                dataReader.Close();
                return ConsumerKey;
            }
        }
    }
    
}
