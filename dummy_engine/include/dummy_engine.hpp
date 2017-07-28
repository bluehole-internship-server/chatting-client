//
// dummy_engine.hpp
//

#pragma once

#include <WinSock2.h>
#include <WS2tcpip.h>
#include <Windows.h>

#include <thread>
#include <string>
#include <vector>

#pragma comment(lib, "ws2_32.lib")

struct packet_header {
    short size;
    short type;
};

struct login_packet {
    packet_header header;
    char dummy_name[80];
};

struct chat_packet {
    packet_header header;
    char chat_msg[256];
};

class DummyEngine {
private:
    typedef class Dummy* DummyPtr;
    struct DummyGroup {
        int num;
        std::string prefix;
        std::vector<DummyPtr> dummies;
        std::vector<std::string> scripts;
    };

public:
    explicit DummyEngine(std::string &&xml_path);
    bool Connect(PSOCKADDR_IN server_addr);
    void Run();
    
private:
    std::vector<DummyGroup*> dummy_groups_;
    bool is_connected_;

};

class Dummy {
public:
    explicit Dummy(std::string &dummy_name, SOCKET socket);

    // for now, it's sync and blocking
    bool Send(std::string &msg);

    SOCKET GetSocket() { return socket_; }
    std::string &GetName() { return dummy_name_; }

private:
    std::string dummy_name_;
    SOCKET socket_;

};