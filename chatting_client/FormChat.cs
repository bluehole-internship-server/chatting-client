using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace chatting_client
{
    public partial class FormChat : Form
    {
        public FormChat()
        {
            InitializeComponent();

            rtfChatBox.AppendText("Welcome to the chat room!\n");
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            String contents = txtChatMsg.Text;
            txtChatMsg.Clear();

            try
            {
                SendMessage(contents);
            }
            catch(Exception)
            {
                lblError.Text = "에러가 발생함";
            }
        }

        private void txtChatMsg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                btnSend_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        private void SendMessage(String message)
        {
            Protocol.PacketHeader header = new Protocol.PacketHeader();
            Protocol.PacketChatSend chat_send = new Protocol.PacketChatSend();

            header.type = Protocol.PacketType.CHAT_SEND;
            header.size = (short)Protocol.currEncoding.GetByteCount(message);
            chat_send.chat_msg = message;

            byte[] byte_header = Protocol.PacketToByteArray(header);
            byte[] byte_chat_send = Protocol.PacketToByteArray(chat_send);

            Program.client.Send(byte_header);
            Program.client.Send(byte_chat_send);
        }

        public void receive()
        {
            Protocol.PacketHeader header = new Protocol.PacketHeader();
            Protocol.PacketChatRecv chat_recv = new Protocol.PacketChatRecv();

            while (true)
            {
                byte[] byte_chat_recv = Protocol.RecvFromServer(Program.client, ref (header));

                if (header.type != Protocol.PacketType.CHAT_RECV) {
                    continue;
                }

                chat_recv = Protocol.ByteArrayToPacket<Protocol.PacketChatRecv>
                    (byte_chat_recv, header.size);
                rtfChatBox.AppendText(chat_recv.chat_msg + '\n');
            }
        }
    }
}
