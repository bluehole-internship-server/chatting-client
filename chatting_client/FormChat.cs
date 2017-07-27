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

            Program.client.Send(Protocol.MergeTwoByteArr(byte_header, byte_chat_send),
                Marshal.SizeOf(header) + header.size, System.Net.Sockets.SocketFlags.None);

            //Program.client.Send(byte_header, Marshal.SizeOf(header), System.Net.Sockets.SocketFlags.None);
            //Program.client.Send(byte_chat_send, header.size, System.Net.Sockets.SocketFlags.None);
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

                    Protocol.PacketChatRecv.Type type = chat_recv.type;

                    byte[] byte_chat_contents = Protocol.currEncoding.GetBytes(chat_recv.chat_contents);
                    Color name_color = Color.FromArgb(byte_chat_contents[0], byte_chat_contents[1], byte_chat_contents[2]);

                    string user_name = Protocol.currEncoding.GetString(Protocol.SplitByteArr(byte_chat_contents,
                        0, chat_recv.len_user_name));
                    string chat_msg = Protocol.currEncoding.GetString(Protocol.SplitByteArr(byte_chat_contents,
                        chat_recv.len_user_name, header.size - 4 - chat_recv.len_user_name));
                    
                    AppendChatBox(type, user_name, name_color, chat_msg);
                }
                catch(Exception)
                {
                    this.Close();
                    break;
                }
            }
        }

        private void AppendChatBox(Protocol.PacketChatRecv.Type type, string user_name, Color name_color, string chat_msg)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<Protocol.PacketChatRecv.Type, string, Color, string>(AppendChatBox),
                    new object[] { type, user_name, name_color, chat_msg });
                return;
            }

            rtfChatBox.SelectionCharOffset = 5;
            rtfChatBox.SelectionColor = name_color;
            rtfChatBox.AppendText(user_name);
            rtfChatBox.SelectionColor = rtfChatBox.ForeColor;
            rtfChatBox.AppendText(": " + chat_msg + '\n');

            rtfChatBox.Select(rtfChatBox.Text.Length, 0);
            rtfChatBox.ScrollToCaret();
        }
    }
}
