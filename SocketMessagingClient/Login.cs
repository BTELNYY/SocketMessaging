using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        private async void button2_Click(object sender, EventArgs e)
        {
            await Task.Run(() => 
            {
                if (usernametextbox.Text != string.Empty && passwordtextbox.Text != string.Empty)
                {
                    bool LoginSucces = Program.MyClinet.ClientLogin(usernametextbox.Text, passwordtextbox.Text);
                    if (LoginSucces)
                    {
                        Console.WriteLine(Program.MyClinet.ClientID);
                    }
                }
            });  
        }

        private async void label6_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }
    }
}
