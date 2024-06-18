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
            this.writetextbox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.NoChannelsLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // writetextbox
            // 
            this.writetextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(40)))), ((int)(((byte)(41)))));
            this.writetextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.writetextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.writetextbox.ForeColor = System.Drawing.SystemColors.Window;
            this.writetextbox.Location = new System.Drawing.Point(431, 591);
            this.writetextbox.Multiline = true;
            this.writetextbox.Name = "writetextbox";
            this.writetextbox.Size = new System.Drawing.Size(610, 45);
            this.writetextbox.TabIndex = 0;
            this.writetextbox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.writetextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(171)))), ((int)(((byte)(174)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(1047, 591);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(45, 45);
            this.button1.TabIndex = 1;
            this.button1.Text = "➤";
            this.button1.UseVisualStyleBackColor = false;
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
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(314, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(799, 573);
            this.panel1.TabIndex = 4;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint_1);
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(40)))), ((int)(((byte)(41)))));
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.NoChannelsLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.writetextbox);
            this.Controls.Add(this.panel1);
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

        private System.Windows.Forms.TextBox writetextbox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label NoChannelsLabel;
        private System.Windows.Forms.Panel panel1;
    }
}