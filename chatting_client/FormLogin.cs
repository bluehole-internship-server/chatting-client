using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace chatting_client
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            String user_name = txtUserName.Text;
            if (user_name.Length < 2) {
                lblResultMessage.Text = "2글자 이상 입력하세요";
                return;
            }
            
            Protocol.PacketLoginAns.Type ret = AdmitRequest(user_name);

            switch (ret)
            {
                case Protocol.PacketLoginAns.Type.SUCCESS:
                    lblResultMessage.Text = "성공! 접속중..";
                    lblResultMessage.Refresh();

                    Thread.Sleep(500);

                    Program.state = Program.State.LoginSucc;
                    Program.user_name = txtUserName.Text;

                    this.Close();

                    break;

                case Protocol.PacketLoginAns.Type.FAIL_DUPLICATE:
                    lblResultMessage.Text = "이미 사용중인 이름";
                    break;

                case Protocol.PacketLoginAns.Type.FAIL_TOO_LONG:
                    lblResultMessage.Text = "너무 긴 이름";
                    break;

                case Protocol.PacketLoginAns.Type.FAIL_TOO_SHORT:
                    lblResultMessage.Text = "너무 짧은 이름";
                    break;

                case Protocol.PacketLoginAns.Type.FAIL_UNKNOWN:
                    lblResultMessage.Text = "기타 에러..";
                    break;
            }
        }

        private Protocol.PacketLoginAns.Type AdmitRequest(String user_name)
        {
            Protocol.PacketHeader header = new Protocol.PacketHeader();
            Protocol.PacketLoginReq request = new Protocol.PacketLoginReq();
            Protocol.PacketLoginAns answer = default(Protocol.PacketLoginAns);

            header.type = Protocol.PacketType.LOGIN_REQ;
            header.size = (short)Protocol.currEncoding.GetByteCount(user_name);
            request.user_name = user_name;

            byte[] byte_header = Protocol.PacketToByteArray(header);
            byte[] byte_request = Protocol.PacketToByteArray(request);

            try
            {
                Program.client.Send(byte_header, Marshal.SizeOf(header), System.Net.Sockets.SocketFlags.None);
                Program.client.Send(byte_request, header.size, System.Net.Sockets.SocketFlags.None);

                byte[] byte_answer = Protocol.RecvFromServer(Program.client, ref (header));

                if (header.type != Protocol.PacketType.LOGIN_ANS) {
                    throw new Exception();
                }

                answer = Protocol.ByteArrayToPacket<Protocol.PacketLoginAns>
                    (byte_answer, header.size);
            }
            catch (Exception) {
                return Protocol.PacketLoginAns.Type.FAIL_UNKNOWN;
            }

            return answer.type;
        }

        
    }
}
