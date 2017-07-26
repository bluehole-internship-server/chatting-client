namespace chatting_client
{
    partial class FormChat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtChatMsg = new System.Windows.Forms.TextBox();
            this.rtfChatBox = new System.Windows.Forms.RichTextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtChatMsg
            // 
            this.txtChatMsg.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtChatMsg.BackColor = System.Drawing.Color.White;
            this.txtChatMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtChatMsg.Location = new System.Drawing.Point(20, 512);
            this.txtChatMsg.Margin = new System.Windows.Forms.Padding(0);
            this.txtChatMsg.MaxLength = 140;
            this.txtChatMsg.Multiline = true;
            this.txtChatMsg.Name = "txtChatMsg";
            this.txtChatMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChatMsg.Size = new System.Drawing.Size(300, 50);
            this.txtChatMsg.TabIndex = 0;
            this.txtChatMsg.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtChatMsg_KeyDown);
            // 
            // rtfChatBox
            // 
            this.rtfChatBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rtfChatBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.rtfChatBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtfChatBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.rtfChatBox.Location = new System.Drawing.Point(20, 12);
            this.rtfChatBox.Name = "rtfChatBox";
            this.rtfChatBox.ReadOnly = true;
            this.rtfChatBox.Size = new System.Drawing.Size(300, 490);
            this.rtfChatBox.TabIndex = 1;
            this.rtfChatBox.Text = "";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(275, 574);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(45, 30);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "채팅";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(18, 583);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 12);
            this.lblError.TabIndex = 3;
            // 
            // FormChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(340, 621);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.rtfChatBox);
            this.Controls.Add(this.txtChatMsg);
            this.Name = "FormChat";
            this.Text = "Chat";
            this.Shown += new System.EventHandler(this.FormChat_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtChatMsg;
        private System.Windows.Forms.RichTextBox rtfChatBox;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lblError;
    }
}