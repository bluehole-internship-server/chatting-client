using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace chatting_client
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        /// 
        public enum State { LoginReady, LoginSucc, Chat };

        private const String addr = "127.0.0.1";
        private const int port = 55150;

        public static Socket client;
        public static State state;
        public static String user_name = "default";

        [STAThread]
        static void Main()
        {
            try {
                IPAddress ipAddress = IPAddress.Parse(addr);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                client = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);
                client.Connect(remoteEP); //it's lagging

            } catch (Exception e) {
                MessageBox.Show(e.ToString());
            }

            state = State.LoginReady;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new FormLogin());

            if (state == State.LoginSucc) 
            {
                Application.Run(new FormChat());
            }
            Environment.Exit(Environment.ExitCode);
        }
    }
}
