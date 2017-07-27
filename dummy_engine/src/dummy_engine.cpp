//
// dummy_engine.cpp
//

#include "dummy_engine.hpp"

//#include "3rd/pugixml/src/pugixml.hpp"

DummyEngine::DummyEngine(std::string &&xml_path)
{

}

bool DummyEngine::Connect(PSOCKADDR_IN server_addr)
{
    is_connected_ = true;
    return is_connected_;
}

void DummyEngine::Run()
{
    if (!is_connected_) return;
}

Dummy::Dummy(std::string &dummy_name, SOCKET socket)
{

}

void Dummy::Send(std::string &msg)
{

}