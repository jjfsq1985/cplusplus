#include "stdafx.h"
#include "TcpSaw.h"

#include<ws2tcpip.h>
#include <winsock2.h>
#pragma comment(lib,"wsock32.lib")
#pragma comment(lib, "ws2_32.lib")

#include "event2\bufferevent.h"
#include "event2\buffer.h"
#include "event2\listener.h"
#include "event2\util.h"
#include "event2\event.h"

struct bufferevent * TcpSaw::m_pBev = NULL;

TcpSaw::TcpSaw()
    :m_bConnecting(false)
    , m_nPort(0)
{
    strcpy_s(m_cIPAddr, 16,"127.0.0.1");
    WSAData wsaData;
    WSAStartup(MAKEWORD(2, 2), &wsaData);
}


TcpSaw::~TcpSaw()
{
    m_bConnecting = false;

    WSACleanup();
}


bool TcpSaw::Init(const char *cIPAddr, int nPort)
{
    strcpy_s(m_cIPAddr, 16, cIPAddr);
    m_nPort = nPort;
    m_bConnecting = true;
    DWORD dwID;
    CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)&ConnectThread, this, 0, &dwID);
    return true;
}

//调用bufferevent_write后会触发该事件，如果bufferevent_setcb中设置了bufferevent_data_cb的话
void TcpSaw::write_cb(struct bufferevent *bev, void *arg)
{
    //do nothing
}

void TcpSaw::ConnectThread(void *pParam)
{
    TcpSaw *pClient = (TcpSaw *)pParam;

    struct sockaddr_in sin;
    sin.sin_family = AF_INET;
    inet_pton(AF_INET, pClient->m_cIPAddr, (void*)&sin.sin_addr);
    sin.sin_port = htons(pClient->m_nPort);

    int nCycle = 0;
    while (pClient->m_bConnecting)
    {
        if (nCycle < 15)
        {
            nCycle++;
            Sleep(1000);
            continue;
        }
        Tprintf("Connecting\n");
        nCycle = 0;
        struct event_base *base = event_base_new();
        assert(base != NULL);
        struct bufferevent *bev = bufferevent_socket_new(base, -1, BEV_OPT_CLOSE_ON_FREE);
        bufferevent_setcb(bev, read_cb, NULL, event_cb, pClient);
        bufferevent_enable(bev, EV_READ | EV_WRITE | EV_PERSIST);
        //连接
        if (bufferevent_socket_connect(bev, (SOCKADDR*)&sin, sizeof(SOCKADDR)) < 0)
        {
            bufferevent_free(bev);
            return;
        }
        m_pBev = bev;

        event_base_dispatch(base);

        event_base_free(base);
    }
}

//从socket读
void TcpSaw::read_cb(struct bufferevent *bev, void *arg)
{
    TcpSaw *pClient = (TcpSaw *)arg;
    const int MAX_LENGHT = 1024;
    char cbData[MAX_LENGHT];
    int n;
    //读数据：bufferevent_read
    //写数据：bufferevent_write
    while (n = bufferevent_read(bev, cbData, MAX_LENGHT))
    {
        if (n <= 0)
            break;
        //处理接收到的数据
        pClient->DealWithData(cbData, n);
    }
}

void TcpSaw::event_cb(struct bufferevent *bev, short event, void *arg)
{
    evutil_socket_t fd = bufferevent_getfd(bev);
    Tprintf("fd = %u, ", fd);
    if (event & BEV_EVENT_TIMEOUT)
    {
        Tprintf("Timed out\n"); //if bufferevent_set_timeouts() called
    }
    else if (event & BEV_EVENT_CONNECTED)
    {
        Tprintf("Connect okay.\n");
    }
    else if (event & BEV_EVENT_EOF)
    {
        Tprintf("connection closed\n");
        bufferevent_free(bev);
    }
    else if (event & BEV_EVENT_ERROR)
    {
        Tprintf("some other error\n");
        bufferevent_free(bev);
    }
}

bool TcpSaw::SendData(const char *pData, int nLen)
{
    if (bufferevent_write(m_pBev, pData, nLen) < 0)
        return false;
    else
        return true;
}

//解析从Server发出的数据
bool TcpSaw::DealWithData(const char*pData, int nLen)
{
    return false;
}
