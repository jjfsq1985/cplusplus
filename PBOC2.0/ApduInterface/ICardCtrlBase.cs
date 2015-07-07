using System;
using System.Collections.Generic;
using System.Text;

namespace ApduInterface
{
    //卡片种类
    public enum CardCategory
    {
        CpuCard = 0,  //CPU卡
        ContactCard, //接触式CPU卡
        PsamCard  //PSAM卡
    }

    public interface ICardCtrlBase
    {
        void SetMainKeyValue(byte[] byteKey, CardCategory eCategory);

        void SetUserAppKeyValue(byte[] byteKey);

        byte[] CardKeyToDb(bool bOrg, CardCategory eCategory);

        byte[] GetKeyVal(bool bMainKey, CardCategory eCategory);

    }
}
