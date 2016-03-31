#include "stdafx.h"
#include "OraDbOperator.h"


Connection OraDbOperator::m_con;

OraDbOperator::OraDbOperator()
{
}


OraDbOperator::~OraDbOperator()
{
}

bool OraDbOperator::OpenOraDb(const char* dbServerName, const char* cUser, const char* cPwd)
{
    Environment::Initialize(Environment::Default | Environment::Threaded);

    Environment::EnableWarnings(true); 

    m_con.Open(dbServerName, cUser, cPwd, Environment::SessionDefault);
    return true;
}

bool OraDbOperator::CloseOraDb()
{
    m_con.Close();
    Environment::Cleanup();
    return true;
}
