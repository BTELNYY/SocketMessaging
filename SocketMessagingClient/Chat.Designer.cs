namespace SocketMessagingClient
{
    partial class Chat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Chat));
            this.MessageTextBox = new System.Windows.Forms.TextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.NoChannelsLabel = new System.Windows.Forms.Label();
            this.ChannelPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ChatPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // MessageTextBox
            // 
            this.MessageTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(40)))), ((int)(((byte)(41)))));
            this.MessageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MessageTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MessageTextBox.ForeColor = System.Drawing.SystemColors.Window;
            this.MessageTextBox.Location = new System.Drawing.Point(431, 591);
            this.MessageTextBox.Multiline = true;
            this.MessageTextBox.Name = "MessageTextBox";
            this.MessageTextBox.Size = new System.Drawing.Size(610, 45);
            this.MessageTextBox.TabIndex = 0;
            this.MessageTextBox.TextChanged += new System.EventHandler(this.MessageTextBox_TextChanged);
            this.MessageTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MessageTextBox_KeyDown);
            // 
            // SendButton
            // 
            this.SendButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(171)))), ((int)(((byte)(174)))));
            this.SendButton.FlatAppearance.BorderSize = 0;
            this.SendButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SendButton.Location = new System.Drawing.Point(1047, 591);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(45, 45);
            this.SendButton.TabIndex = 1;
            this.SendButton.Text = "➤";
            this.SendButton.UseVisualStyleBackColor = false;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // NoChannelsLabel
            // 
            this.NoChannelsLabel.AutoSize = true;
            this.NoChannelsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NoChannelsLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.NoChannelsLabel.Location = new System.Drawing.Point(428, 250);
            this.NoChannelsLabel.Name = "NoChannelsLabel";
            this.NoChannelsLabel.Size = new System.Drawing.Size(632, 33);
            this.NoChannelsLabel.TabIndex = 3;
            this.NoChannelsLabel.Text = "You have selected no channel, nothing to show.";
            // 
            // ChannelPanel
            // 
            this.ChannelPanel.AutoScroll = true;
            this.ChannelPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ChannelPanel.Location = new System.Drawing.Point(0, 0);
            this.ChannelPanel.Name = "ChannelPanel";
            this.ChannelPanel.Size = new System.Drawing.Size(308, 585);
            this.ChannelPanel.TabIndex = 5;
            this.ChannelPanel.WrapContents = false;
            this.ChannelPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.ChannelPanel_Paint);
            // 
            // ChatPanel
            // 
            this.ChatPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ChatPanel.Location = new System.Drawing.Point(314, 0);
            this.ChatPanel.Name = "ChatPanel";
            this.ChatPanel.Size = new System.Drawing.Size(812, 585);
            this.ChatPanel.TabIndex = 6;
            this.ChatPanel.WrapContents = false;
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(40)))), ((int)(((byte)(41)))));
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.ChannelPanel);
            this.Controls.Add(this.NoChannelsLabel);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.MessageTextBox);
            this.Controls.Add(this.ChatPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Chat";
            this.Text = "Chat";
            this.Load += new System.EventHandler(this.Chat_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Chat_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox MessageTextBox;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.Label NoChannelsLabel;
        private System.Windows.Forms.FlowLayoutPanel ChannelPanel;
        private System.Windows.Forms.FlowLayoutPanel ChatPanel;
    }
}