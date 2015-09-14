using System;
using System.Collections.Generic;
using System.Text;

namespace ApduInterface
{
    public enum BalanceType
    {
        Balance_ED = 1, //电子存折，加气应用
        Balance_EP = 2 //电子钱包，积分应用
    }

    //卡片种类
    public enum CardCategory
    {
        CpuCard = 0,  //CPU卡
        PsamCard  //PSAM卡
    }

    public interface IApduBase
    {
        byte[] GetOutputCmd();

        bool createGetResponseCmd(int nResponseLen);

        bool createNewMFcmd(byte[] PSE);
        bool createClearDFcmd();

        bool createSelectCmd(byte[] byteName, byte[] prefixData);

        bool createGetChallengeCmd(int nRandLen);        

        bool createExternalAuthenticationCmd(byte[] byteRandom, byte[] KeyValue);

        bool createClearMFcmd(byte[] byteRandom, byte[] KeyValue);

        bool createGetEFFileCmd(byte fileFlag, byte ReponseLen);

        byte[] CalcMacVal_DES(byte[] srcData, byte[] keytoMac);        

        byte[] CalcPrivateProcessKey(byte[] srcData, byte[] tmpck);
    }
}
