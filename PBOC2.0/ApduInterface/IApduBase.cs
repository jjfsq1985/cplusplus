using System;
using System.Collections.Generic;
using System.Text;

namespace ApduInterface
{
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
