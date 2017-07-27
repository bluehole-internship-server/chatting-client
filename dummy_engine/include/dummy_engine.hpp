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
    //class Dummy;
    typedef class Dummy* DummyPtr;
    struct DummyGroup {
        std::string prefix_;
        std::vector<DummyPtr> dummies_;
        std::vector<std::string> scripts_;
    };

public:
    explicit DummyEngine(std::string &&xml_path);
    bool Connect(PSOCKADDR_IN server_addr);
    void Run();
    
private:
    std::vector<DummyGroup> dummy_groups_;
    bool is_connected_;

    // server_addr, dummy type, script for dummy, number of each type
    // prefixes for dummy
};

class Dummy {
public:
    explicit Dummy(std::string &dummy_name, SOCKET socket);

    void Send(std::string &msg);

private:
    std::string dummy_name_;
    SOCKET socket_;
};