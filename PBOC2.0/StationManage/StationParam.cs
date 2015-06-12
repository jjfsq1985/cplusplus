using System;
using System.Collections.Generic;
using System.Text;

namespace StationManage
{
    public enum DbStateFlag
    {
        eDbOK,  //正常
        eDbDirty,  //db需更新
        eDbAdd,   //新增
        eDbDelete  //删除
    }

    class ClientParam
    {
        public int ClientId = 0;
        public string ClientName = "";
    }

    public class ProvinceCode
    {        
        public string strProvinceName = "";
        public byte ProvinceVal = 0;        
    }

    public class CityCode
    {        
        public string strCityName = "";
        public byte[] CityVal = new byte[2];
    }

    public class SuperiorCode
    {
        public string strSuperiorName = ""; //公司代码（Client的上级)
        public byte[] SuperiorVal = new byte[2];
    }

    public class StationParam
    {
        public int nDataGridViewRowIndex = 0;
        public string strStationName = "";  //气站名称
        public byte[] StationId = new byte[4]; //气站编号
        public int ClientID = 0; //所属单位ID
        
        public byte ProvCode = 0;              //省代码
        public byte[] CityCode = new byte[2];  //地市代码
        public byte[] SuperiorCode = new byte[2]; //上级单位代码    

        public DbStateFlag eDbState = DbStateFlag.eDbOK;
    }
}
