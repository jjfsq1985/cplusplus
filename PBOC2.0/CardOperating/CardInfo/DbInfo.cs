using System;
using System.Collections.Generic;
using System.Text;

namespace CardOperating
{
    //单位信息
    public class ClientInfo
    {
        public int ClientId = 0;
        public string strClientName = "";
    }

    public class ProvinceInfo
    {
        public byte[] ProvinceCode = new byte[1];
        public string strProvinceName = "";
    }

    public class CityInfo
    {
        public byte[] CityCode = new byte[2];
        public string strCityName = "";
    }

    //公司信息
    public class SuperiorInfo
    {
        public byte[] SuperiorCode = new byte[2];
        public string strSuperiorName = "";
    }

    public class StationInfo
    {
        public byte[] StationCode = new byte[2];
        public string strStationName = "";
    }
}
