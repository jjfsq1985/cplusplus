#pragma once
#include "ocilib.hpp"

#if defined(OCI_CHARSET_WIDE)
#pragma comment(lib, "ocilibw.lib")
#elif defined(OCI_CHARSET_ANSI)
#pragma comment(lib, "ociliba.lib")
#endif

using namespace ocilib;

class OraDbOperator
{
public:
    OraDbOperator();
    ~OraDbOperator();

public:
    bool OpenOraDb(const char* dbServerName, const char* cUser, const char* cPwd);
    bool CloseOraDb();



private:
    static Connection m_con;
};

