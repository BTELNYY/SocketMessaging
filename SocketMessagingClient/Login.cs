using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SocketMessagingShared;

namespace SocketMessagingClient
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Chat_Load(object sender, EventArgs e)
        {
      
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (usernametextbox.Text != string.Empty && passwordtextbox.Text != string.Empty)
            {
                bool LoginSucces = Program.MyClinet.ClientLogin(usernametextbox.Text, passwordtextbox.Text);
                if (LoginSucces)
                {
                    this.Hide();
                    Chat chat = new Chat();
                    chat.ShowDialog();
                }
                else
                {
                    label1.Visible = false;
                    label2.Text = "Unsuccesful";
                }
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Hide();
            SignUp signUp = new SignUp();
            signUp.ShowDialog();
        }
    }
}
