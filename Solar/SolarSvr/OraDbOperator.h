#pragma once
#include "ocilib.hpp"

#if defined(OCI_CHARSET_WIDE)
#pragma comment(lib, "ocilibw.lib")
#elif defined(OCI_CHARSET_ANSI)
#pragma comment(lib, "ociliba.lib")
#endif

using namespace ocilib;

enum DbDirection
{
    In = 1,
    Out,
    InOut
};

enum DbDataType
{
    TypeBoolean = 0,
    TypeNumberInt,
    TypeNumberInt64,
    TypeFloat,
    TypeDouble,
    TypeString,
    TypeDateTime
};

typedef struct _tagDbParam
{
    ostring Name;
    DbDirection Direction;
    DbDataType ParType;
    union 
    {
        bool bVal;
        int nValue;
        long long int64Val;
        float fltValue;
        double dbValue;
    }DbValue;
    ostring ValStr;
    Date     ValDate;
}DbParam;

class OraDbOperator
{
public:
    OraDbOperator();
    ~OraDbOperator();

public:
    bool OpenOraDb(const char* dbServerName, const char* cUser, const char* cPwd);
    bool CloseOraDb();

    int ExecuteCmd(const char* cSql);
    int ExecuteCmd(const char* cSql, Resultset& outResult);
    int ExecuteCmd(const char* cSql, DbParam* param, int nCount);
    int ExecuteCmd(const char* cSql, DbParam* param, int nCount, Resultset& outResult);
    int ExecuteProc(const char* cProcName);
    int ExecuteProc(const char* cProcName, Resultset& outResult);
    int ExecuteProc(const char* cProcName, DbParam* param, int nCount);
    int ExecuteProc(const char* cProcName, DbParam* param, int nCount, Resultset& outResult);

    template<class TObjectType>
    inline bool GetDataFromQuery(const Resultset &rs, int nIndex, TObjectType &outVal)
    {
        outVal = rs.Get<TObjectType>(nIndex);
        return true;
    }

    template <class TObjectType>
    inline DbParam MakeParam(const char* cParamName, DbDataType Type, DbDirection Direction, TObjectType& Value)
    {
        DbParam paramVal;
        paramVal.Name = cParamName;
        paramVal.Direction = Direction;
        paramVal.ParType = Type;
        switch (Type)
        {
        case TypeBoolean:
            paramVal.DbValue.bVal = Value;
            break;
        case TypeNumberInt:
            paramVal.DbValue.nValue = Value;
            break;
        case TypeNumberInt64:
            paramVal.DbValue.int64Val = Value;
            break;
        case TypeFloat:
            paramVal.DbValue.fltValue = Value;
            break;
        case TypeDouble:
            paramVal.DbValue.dbValue = Value;
            break;
        case TypeString:
            paramVal.ValStr = Value;
            break;
        case TypeDateTime:
            paramVal.ValDate = Value;
            break;
        default:
            break;
        }        
        return paramVal;
    }


protected:
    ostring OraDbOperator::GetParam(DbParam* param, int nCount);
    void BindData(Statement& st, DbParam& data);

private:
    static Connection m_con;
};

