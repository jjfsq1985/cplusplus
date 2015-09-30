using System;
using System.Collections.Generic;
using System.Text;
using ApduParam;

namespace ApduInterface
{
    public interface ISamApduProvider : IApduBase
    {
        bool createGenerateKeyCmd(ushort uFileId,ushort RecordCount, byte RecordLength);

        bool createStorageKeyCmd(byte[] keyVal, byte[] param1, byte[] param2);

        bool createStorageFCICmd(byte[] byteName, byte[] prefix);

        bool createStorageCardInfoCmd(byte[] byteASN);

        bool createGenerateEFCmd(byte GenerateFlag, ushort FileId, byte FileType, ushort FileSize);

        bool createSelectEFCmd(ushort FileId);

        bool createStorageTermInfoCmd(byte[] TermialId);

        bool createGenerateADFCmd(byte[] byteName, ushort FileId);

        bool createStoragePsamInfoCmd(IccCardInfoParam PsamInfo);

        bool createWriteMAC2Cmd(byte maxCount, byte remainCount);

        bool createSetStatusCmd(byte[] RandomVal, byte[] keyCalc);

        bool createStorageAppKeyCmd(byte[] RandomVal, byte[] StorageKey, byte Usage, byte Ver, byte[] EncryptKey);

        bool createInitSamGrayLockCmd(byte[] DataVal);

        bool createInitSamPurchaseCmd(byte[] DataVal);

        bool createVerifyMAC2Cmd(byte[] MAC2);

        bool createVerifyPurchaseMAC2Cmd(byte[] MAC2);
        
        bool createCalcGMACCmd(byte BusinessType,byte[] ASN, int nOfflineSn, int nAmount);
    }
}
