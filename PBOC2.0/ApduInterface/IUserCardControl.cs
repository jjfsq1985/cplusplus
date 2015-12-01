using System;
using System.Collections.Generic;
using System.Text;
using ApduParam;

namespace ApduInterface
{
    public interface IUserCardControl
    {
        event MessageOutput TextOutput;

        int InitCard(bool bMainKey);

        void GetCosVer();

        bool CreateEFInMF();        

        void CreateKey();

        bool HasLyKey();

        bool CreateADFApp(int nAppIndex);

        bool CreateApplication(byte[] byteASN, bool bDefaultPwd, string strCustomPwd);

        bool CreateLoyaltyApp(byte[] byteASN, bool bDefaultPwd, string strCustomPwd);

        bool UpdateApplicationFile(UserCardInfoParam UserCardInfoPar, byte[] AppTendingKey);

        bool UpdateLoyaltyApp(UserCardInfoParam UserCardInfoPar, byte[] AppTendingKey);

        bool DebitFoUnLoad(byte[] byteMAC2, byte[] TimeBcd);        

        bool SelectCardApp(int nAppIndex);        

        int VerifyUserPin(string strPIN);        
        
        //金额圈存
        bool UserCardLoad(byte[] ASN, byte[] TermId, int nMoneyValue, bool bReadKey);  
      
        //积分圈存
        bool LoyaltyLoad(byte[] ASN, byte[] TermId, int nLoyaltyValue, bool bReadKey);

        bool UserCardUnLoad(byte[] ASN, byte[] TermId, int nMoneyValue, bool bReadKey);

        bool UserCardBalance(ref int nBalance, BalanceType eType);        

        bool UserCardGray(ref int nStatus, byte[] PSAM_TID,byte[] GTAC);

        bool InitForGray(byte[] TermialID, byte[] outData);

        bool InitForPurchase(byte[] TerminalID, int nLyAmount, byte[] outData);
                
        bool GrayLock(byte[] Data, byte[] outGTAC, byte[] outMAC2);

        bool LyPurchase(byte[] Data, byte[] outTAC, byte[] outMAC2);
                
        bool InitForUnlockGreyCard(byte[] TermialID, byte[] outData);

        bool UnLockGrayCard(byte[] ASN, byte[] TermialID, int nUnlockMoney, bool bReadKey, int nAppIndex);

        bool DebitForUnlock(byte[] byteData);        

        bool ClearTACUF();

        int ReadKeyValueFromSource();        

        bool SaveCpuCardInfoToDb(UserCardInfoParam UserCardInfoPar, bool bUpdate);               

        void GetUserCardInfo(UserCardInfoParam CardInfo);        

        byte[] GetUserCardASN(bool bMessage, ref DateTime cardStart, ref DateTime cardEnd);        

        List<CardRecord> ReadRecord();        
                
        bool CheckPublishedCard(bool bMainKey, byte[] KeyInit);        

        bool UpdateCardInfo(UserCardInfoParam CardInfo);
        
        bool ChangePIN(string strOldPin, string strNewPin);

        bool PINReset(byte[] ASN, string strPin, int nAppIndex);

        bool PINUnLock(byte[] ASN, string strPIN, int nAppIndex);

    }
}
