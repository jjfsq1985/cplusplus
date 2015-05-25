using System;
using System.Collections.Generic;
using System.Text;

namespace AccountManage
{
    public enum DbStateFlag
    {
        eDbOK,  //正常
        eDbDirty,  //db需更新
        eDbAdd,   //新增
        eDbDelete  //删除
    }

    public class AccountInfo
    {
        public int nUserId = 0;
        public string strUserName = ""; //用户名
        public string strPassword = ""; //密码
        public int UserAuthority = 0;  //权限
        public int UserStatus = 0;     //登录状态
        public DbStateFlag eDbFlag = DbStateFlag.eDbOK;  //操作状态
    }
}
