using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SocketNetworking;

namespace SocketMessagingClient
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (usernametextbox.Text != string.Empty && passwordtextbox.Text == passwordtextbox2.Text)
            {
                await Task.Run(() =>
                {
                    bool AccCreated = Program.MyClinet.ClientCreateAccount(usernametextbox.Text, passwordtextbox.Text);
                    if (!AccCreated)
                    {
                        label1.Visible = false;
                        label2.Text = $"Account with username: {usernametextbox} already exist";
                    }
                    else
                    {
                        LoginSwitcher();
                    }
                });

            }
            else if (passwordtextbox.Text != passwordtextbox2.Text)
            {
                label1.Visible = false;
                label2.Text = "Password doesn't match";
            }
            else 
            {
                label1.Text = "Username is Inco";
            }
        }

        private void LoginSwitcher()
        {
            this.Close();
            Login login = new Login();
            login.Show();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            LoginSwitcher();
        }

        private void SignUp_Load(object sender, EventArgs e)
        {
            
        }
    }
}
