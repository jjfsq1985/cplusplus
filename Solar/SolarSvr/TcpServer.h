#pragma once
#include <vector>
using std::vector; 

#include "event2\util.h"

class TcpServer
{
public:
    TcpServer();
    ~TcpServer();

public:
    bool Init(int nPort);
    bool SendData(const char* cIP, int nPort, const char *pData, int nLen);


protected:
    bool DealWithData(struct bufferevent *bev, const char*pData, int nLen);
    void TcpServer::RemoveFromVec(struct bufferevent *bev);
    struct bufferevent* GetBufferEvent(const char* cIP, int nPort);

public:
    static vector<struct bufferevent*> m_VecBev;

private:
    int m_nListenPort;
    static void ListenThread(void *pParam);
    static void do_accept(evutil_socket_t listener, short eventVal, void *arg);
    static void read_cb(struct bufferevent *bev, void *arg);
    static void event_cb(struct bufferevent *bev, short event, void *arg);
    static void write_cb(struct bufferevent *bev, void *arg);
};

