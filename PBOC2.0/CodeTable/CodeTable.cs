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
        public string strProvinceName = "";
        public byte ProvinceCode = 0;
        public DbStateFlag eDbState = DbStateFlag.eDbOK;
    }

    public class CityCodeTable
    {
        public int nDataGridViewRowIndex = 0;
        public string strCityName = "";
        public byte[] CityCode = new byte[2];
        public DbStateFlag eDbState = DbStateFlag.eDbOK;
    }

    public class SuperiorCodeTable
    {
        public int nDataGridViewRowIndex = 0;
        public string strSuperiorName = ""; //公司代码（Client的上级)
        public byte[] SuperiorCode = new byte[2];
        public DbStateFlag eDbState = DbStateFlag.eDbOK;
    }

}
