using System;
using System.Collections.Generic;
using System.Text;
using ApduParam;

namespace ApduInterface
{
    public interface ISamCardControl : ICardCtrlBase
    {
        event MessageOutput TextOutput;

        int InitIccCard(bool bMainKey);
        
        bool SelectPsamApp();

        bool CreateIccInfo(byte[] PsamId, byte[] TermialId);
       
        bool WriteApplicationInfo(IccCardInfoParam psamInfo);

        bool SetupIccKey();
        
        bool SetupMainKey();      

        bool InitSamGrayLock(byte[] TermialID, byte[] random, byte[] BusinessSn, byte[] byteBalance, byte BusinessType, byte[] ASN, byte[] outData);

        bool VerifyMAC2(byte[] MAC2);
        
        bool CalcGMAC(byte BusinessType, byte[] ASN, int nOffLineSn, int nMoney, byte[] outGMAC);
        
        bool ReadKeyValueFormDb();

        
        bool SavePsamCardInfoToDb(IccCardInfoParam PsamInfoPar);
        
        bool CheckPublishedCard(bool bMainKey, byte[] KeyInit);
        
        byte[] GetTerminalId();
        
        byte[] GetPsamASN(bool bMessage);
        
    }
}
