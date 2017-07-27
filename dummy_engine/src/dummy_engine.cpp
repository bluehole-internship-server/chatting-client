//
// dummy_engine.cpp
//

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
        }
    }
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