using System;
using System.Collections.Generic;
using System.Text;

namespace CodeTable
{
    public enum DbStateFlag
    {
        eDbOK,  //正常
        eDbDirty,  //db需更新
        eDbAdd,   //新增
        eDbDelete  //删除
    }

    public class ProvinceCodeTable
    {
        public int nDataGridViewRowIndex = 0;
        public Guid guidCode = Guid.Empty;
        public string strProvinceName = "";
        public byte ProvinceCode = 0;
        public DbStateFlag eDbState = DbStateFlag.eDbOK;
    }
    public class CityCodeTable
    {
        public int nDataGridViewRowIndex = 0;
        public Guid guidCode = Guid.Empty;
        public string strCityName = "";
        public byte[] CityCode = new byte[2];
        public DbStateFlag eDbState = DbStateFlag.eDbOK;
    }

    public class SuperiorCodeTable
    {
        public int nDataGridViewRowIndex = 0;
        public Guid guidCode = Guid.Empty;
        public string strSuperiorName = ""; //公司代码（Client的上级)
        public byte[] SuperiorCode = new byte[2];
        public DbStateFlag eDbState = DbStateFlag.eDbOK;
    }

    public class CodeTable
    {
        public static bool IsCityListCompleted(List<CityCodeTable> list)
        {
            foreach (CityCodeTable value in list)
            {
                if ((value.strCityName == "") || (value.CityCode[0] == 0x00 && value.CityCode[1] == 0x00))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsSuperiorListCompleted(List<SuperiorCodeTable> list)
        {
            foreach (SuperiorCodeTable value in list)
            {
                if ((value.strSuperiorName == "") || (value.SuperiorCode[0] == 0x00 && value.SuperiorCode[1] == 0x00))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsProvListCompleted(List<ProvinceCodeTable> list)
        {
            foreach (ProvinceCodeTable value in list)
            {
                if ((value.strProvinceName == "") || (value.ProvinceCode == 0x00))
                {
                    return false;
                }
            }
            return true;
        }
    }

}
