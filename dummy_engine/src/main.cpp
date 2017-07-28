//
// main.cpp
//

#include "header.hpp"

int main()
{
    // init wsa
    WSADATA wsa_data;
    int ret = WSAStartup(MAKEWORD(2, 2), &wsa_data);
    if (ret != 0) return 1;

    DummyEngine dummy_engine(XML_PATH);

    SOCKADDR_IN server_addr;
    server_addr.sin_family = AF_INET;
    server_addr.sin_port = htons(SERVER_PORT);
    InetPtonA(AF_INET, SERVER_IP, &server_addr.sin_addr);

    if (dummy_engine.Connect(&server_addr)) {
        dummy_engine.Run();
    }

    WSACleanup();
    return 0;
}