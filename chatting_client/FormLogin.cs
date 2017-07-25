using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;

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

            /* TODO check valid. if valid .. */

            lblResultMessage.Text = "성공! 접속중..";
            lblResultMessage.Refresh();

            Thread.Sleep(1000);

            Program.state = Program.State.LoginSucc;
            Program.user_name = txtUserName.Text;

            this.Close();
        }
    }
}
