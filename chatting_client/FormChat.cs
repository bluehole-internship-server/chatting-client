using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            /* TODO : Make packet and send to Chatting server */
        }

        private void txtChatMsg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                btnSend_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        public void receive()
        {
            while(true) {
                /* TODO : Receive packet and apply */
            }
        }
    }
}
