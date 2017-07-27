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
    std::vector<DummyGroup> dummy_groups_;
    bool is_connected_;

};

class Dummy {
public:
    explicit Dummy(std::string &dummy_name, SOCKET socket);

    void Send(std::string &msg);

private:
    std::string dummy_name_;
    SOCKET socket_;

};