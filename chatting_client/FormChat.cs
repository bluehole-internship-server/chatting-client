using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace chatting_client
{
    public partial class FormChat : Form
    {
        public FormChat()
        {
            InitializeComponent();

            rtfChatBox.SelectionCharOffset = 8;
            rtfChatBox.AppendText("Welcome to the chat room!\n");
        }

        private void FormChat_Shown(object sender, EventArgs e)
        {
            Thread recv_thread = new Thread(this.receive);
            recv_thread.Start();
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

            Program.client.Send(byte_header, Marshal.SizeOf(header), System.Net.Sockets.SocketFlags.None);
            Program.client.Send(byte_chat_send, header.size, System.Net.Sockets.SocketFlags.None);
        }

        public void receive()
        {
            Protocol.PacketHeader header = new Protocol.PacketHeader();
            Protocol.PacketChatRecv chat_recv = new Protocol.PacketChatRecv();
            
               
            while (true)
            {
                try
                {
                    byte[] byte_chat_recv = Protocol.RecvFromServer(Program.client, ref (header));

                    if (header.type != Protocol.PacketType.CHAT_RECV)
                    {
                        continue;
                    }

                    chat_recv = Protocol.ByteArrayToPacket<Protocol.PacketChatRecv>
                        (byte_chat_recv, header.size);

                    // post process

                    AppendChatBox(chat_recv.chat_msg);
                }
                catch(Exception)
                {
                    this.Close();
                    break;
                }
            }
        }

        private void AppendChatBox(string msg)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendChatBox), new object[] { msg });
                return;
            }
            rtfChatBox.SelectionCharOffset = 5;
            rtfChatBox.AppendText(msg + '\n');

            rtfChatBox.Select(rtfChatBox.Text.Length, 0);
            rtfChatBox.ScrollToCaret();
        }
    }
}
