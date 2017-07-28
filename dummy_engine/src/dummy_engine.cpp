//
// dummy_engine.cpp
//
#include <random>
#include <stdlib.h>

#include "dummy_engine.hpp"

#include "3rd/pugixml/src/pugixml.hpp"

DummyEngine::DummyEngine(std::string &&xml_path)
{
    // try catch
    pugi::xml_document doc;
    pugi::xml_parse_result parse_result = doc.load_file(xml_path.c_str());
    
    if (parse_result) {
        pugi::xml_node dummy_groups = doc.child("dummy_group_list");

        for (auto dummy_group = dummy_groups.child("dummy_group");
            dummy_group;
            dummy_group = dummy_group.next_sibling("dummy_group")) {
            
            DummyGroup *group = new DummyGroup();
           
            group->num =
                std::stoi(dummy_group.child("num").first_child().value());
            group->prefix = dummy_group.attribute("prefix").value();

            pugi::xml_node script_list = dummy_group.child("script_list");
            for (auto script = script_list.child("script"); script;
                script = script.next_sibling("script")) {
                group->scripts.push_back(script.first_child().value());
            }

            for (int i = 0; i < group->num; i++) {
                group->dummies.push_back(new Dummy(
                    group->prefix + "#" + std::to_string(i + 1),
                    socket(AF_INET, SOCK_STREAM, IPPROTO_TCP)
                ));
            }
            dummy_groups_.push_back(group);
        }
    }
}

bool DummyEngine::Connect(PSOCKADDR_IN server_addr)
{
    login_packet packet;
    bool tret = true;

    packet.header.type = 0;

    for (DummyGroup* group : dummy_groups_) {
        for (Dummy* dummy : group->dummies) {
            bool ret = connect(dummy->GetSocket(),
                reinterpret_cast<PSOCKADDR>(server_addr),
                sizeof(*server_addr)) == 0;

            packet.header.size = dummy->GetName().size();
            strcpy(packet.dummy_name, dummy->GetName().c_str());
            int len = sizeof(packet_header) + dummy->GetName().size();
            ret &= send(dummy->GetSocket(),
                reinterpret_cast<const char*>(&packet),
                len, 0) == len;

            tret = tret & ret;
            if (!tret) break;
        }
        if (!tret) break;
    }

    is_connected_ = tret;
    return is_connected_;
}

void DummyEngine::Run()
{
    if (!is_connected_) return;
    for (DummyGroup* group : dummy_groups_) {
        std::mt19937 mt_rand(time(0) + rand());
        std::thread
        { [group, &mt_rand]() {
            while (true) {
                Dummy* dummy = group->dummies[mt_rand() % group->dummies.size()];
                {
                    std::string &message =
                        group->scripts[mt_rand() % group->scripts.size()];
                    dummy->Send(message);
                    Sleep(mt_rand() %  3000);
                }
            }
        }}.detach();
    }

    // forget thread join
    while (1);
}

Dummy::Dummy(std::string &dummy_name, SOCKET socket)
    : socket_(socket)
    , dummy_name_(dummy_name)
{
}

bool Dummy::Send(std::string &msg)
{
    chat_packet packet;
    packet.header.type = 2; /* CHAT_SEND */
    packet.header.size = msg.size();
    strcpy(packet.chat_msg, msg.c_str());

    int len = sizeof(packet_header) + packet.header.size;

    return send(socket_,
        reinterpret_cast<const char*>(&packet),
        len, 0) == len;
}