using System;
using System.Collections.Generic;
using System.Text;

namespace ApduParam
{
    //数据库中灰卡的交易记录
    public class GrayCardInfo
    {
        public byte[] ASN = new byte[8];             //卡号
        public byte[] StationNo = new byte[4];       //站点编号
        public int GunNo;               //枪号
        public DateTime ConsumerTime;   //交易时间
        public double Price;            //灰卡时单价
        public double Gas;              //气量
        public double Money;            //交易金额
        public double ResidualAmount;    //卡余额
    }
}
