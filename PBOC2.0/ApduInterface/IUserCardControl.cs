using System;
using System.Collections.Generic;
using System.Text;
using ApduParam;

namespace ApduInterface
{
    public interface IUserCardControl : ICardCtrlBase
    {
        event MessageOutput TextOutput;

        int InitCard(bool bMainKey);
        
        bool CreateDIR();        

        void CreateKey();

        bool CreateADFApp();

        bool CreateApplication(byte[] byteASN, bool bDefaultPwd, string strCustomPwd);        

        bool UpdateApplicationFile(UserCardInfoParam UserCardInfoPar, byte[] AppTendingKey);        

        bool DebitFoUnLoad(byte[] byteMAC2, byte[] TimeBcd);        

        bool SelectCardApp();        

        int VerifyUserPin(string strPIN);        
        
        bool UserCardLoad(byte[] ASN, byte[] TermId, int nMoneyValue, bool bReadKeyFromDb);        

        bool UserCardUnLoad(byte[] ASN, byte[] TermId, int nMoneyValue, bool bReadKeyFromDb);

        bool UserCardBalance(ref double dbBalance);        

        bool UserCardGray(ref int nStatus, byte[] TerminalId);

        bool InitForGray(byte[] TermialID, byte[] outData);        
                
        bool GrayLock(byte[] Data, byte[] outGTAC, byte[] outMAC2);        
                
        bool InitForUnlockGreyCard(byte[] TermialID, byte[] outData);        
                
        bool UnLockGrayCard(byte[] ASN, byte[] TermialID, int nUnlockMoney, bool bReadKeyFromDb);
        
        bool DebitForUnlock(byte[] byteData);        

        bool ClearTACUF();        

        bool ReadKeyValueFormDb();        

        bool SaveCpuCardInfoToDb(UserCardInfoParam UserCardInfoPar);        

        bool UpdateCardInfoToDb(UserCardInfoParam UserCardInfoPar);        

        void GetUserCardInfo(UserCardInfoParam CardInfo);        

        byte[] GetUserCardASN(bool bMessage, ref DateTime cardStart, ref DateTime cardEnd);        

        List<CardRecord> ReadRecord();        
                
        bool CheckPublishedCard(bool bMainKey, byte[] KeyInit);        

        bool UpdateCardInfo(UserCardInfoParam CardInfo);
        
        bool ChangePIN(string strOldPin, string strNewPin);        

        bool PINReset(byte[] ASN, string strPin);

        bool PINUnLock(byte[] ASN, string strPIN);

    }
}
