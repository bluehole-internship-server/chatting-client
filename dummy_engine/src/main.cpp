//
// main.cpp
//

#include "header.hpp"

int main()
{
    DummyEngine dummy_engine(XML_PATH);

    SOCKADDR_IN server_addr;
    server_addr.sin_family = AF_INET;
    server_addr.sin_port = htons(1100);
    InetPtonA(AF_INET, SERVER_IP, &server_addr.sin_addr);

    if (dummy_engine.Connect(&server_addr)) {
        dummy_engine.Run();
    }
    return 0;
}