using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManage
{
    public enum DbStateFlag
    {
        eDbOk,  //正常
        eDbUpdate,  //db需更新
        eDbAdd,   //新增
        eDbDel  //删除
    }

    class ClientInfo
    {
        public int    ClientId = 0;
        public string ClientName = "";
        public int    ParentId = 0;
        public string ParentName = "";
        public string LinkMan = ""; //联系人
        public string Telephone = "";  //联系电话
        public string FaxNum = "";   //传真
        public string Email = "";   //电子邮箱
        public string Zipcode = ""; //邮编
        public string Address = ""; //地址
        public string BankName = "";  //开户银行
        public string BankAccount = ""; //账号
        public string Remark = "";  //备注
    }
}
