using System;
using System.Collections.Generic;
using System.Text;
using ApduParam;

namespace ApduInterface
{
    public interface IUserApduProvider : IApduBase
    {
        bool createGenerateFCICmd();

        bool createStorageFCICmd(byte[] AidName, byte[] param, byte[] prefix);

        bool createUpdateEF01Cmd(byte nFileIndex, byte[] AidName);

        bool createGenerateKeyCmd();
        
        bool createStorageKeyCmd(byte[] StorageKey, byte[] param1, byte[] param2);

        bool createGenerateADFCmd(byte[] ADFName);

        bool createGenerateEFCmd(ushort fileID, byte fileType, ushort fileLen, byte keyIndex, byte RecordNum, byte RecordLen, ushort ACr, ushort ACw);

        bool createStorageApplicationCmd();

        bool createStoragePINFileCmd(bool bDefaultPwd, byte[] customPwd);

        bool createGenerateEFKeyCmd();

        bool createWriteUserKeyCmd(byte[] randVal, StorageKeyParam Param);

        bool createSetStatusCmd(byte[] RandomVal, byte[] keyCalc);

        bool createUpdateEF15FileCmd(byte[] key, byte[] RandomVal, byte[] ASN, DateTime dateBegin, DateTime dateEnd);

        bool createUpdateEF16FileCmd(byte[] key, byte[] RandomVal, UserCardInfoParam cardInfo);

        bool createVerifyPINCmd(bool bDefaultPwd, byte[] customPwd);

        bool createUpdateEF0BFileCmd(bool bDefaultPwd);

        bool createUpdateEF1CFileCmd(byte[] key, byte[] RandomVal, UserCardInfoParam cardInfo);

        bool createUpdateEF0DFileCmd(byte[] key, byte[] RandomVal, UserCardInfoParam cardInfo);
        
        bool createUpdateEF10FileCmd(byte[] key, byte[] RandomVal);

        bool createInitializeLoadCmd(int nMoney, byte[] TermialID);        

        bool createInitializeUnLoadCmd(int nMoney, byte[] TermialID);        

        bool createCreditLoadCmd(byte[] byteMAC2, byte[] TimeBcd);        

        bool createDebitUnLoadCmd(byte[] byteMAC2, byte[] TimeBcd);        

        bool createCardBalanceCmd();        

        bool createrCardGrayCmd(bool bClearTAC);        

        bool createrInitForGrayCmd(byte[] TermialID);        

        bool createrGrayLockCmd(byte[] DataVal);        

        bool createrInitForUnlockCardCmd(byte[] TermialID);

        bool createGreyCardUnLockCmd(int nMoney, byte[] byteMAC2, byte[] TimeBcd);
        
        bool createDebitForUnlockCmd(byte[] DebitData);

        bool createReadRecordCmd(byte ResponseLen);

        bool createPINResetCmd(byte[] key, byte[] bytePIN);
        
        bool createChangePINCmd(byte[] oldPwd, byte[] newPwd);

        bool createPINUnLockCmd(byte[] randval, byte[] key, byte[] bytePIN);        
    }
}
