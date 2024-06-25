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
        //if sign up button clicked -> call signupcheck function
        private void button2_Click(object sender, EventArgs e)
        {
            signupcheck();
        }
        //signupcheck fucntion that get response from server if Account was created we switch the forms, otherwise show lable + data
        private void signupcheck()
        {
            if (usernametextbox.Text != string.Empty && passwordtextbox.Text == passwordtextboxcheck.Text)
            {
                bool AccCreated = Program.MyClient.ClientCreateAccount(usernametextbox.Text, passwordtextbox.Text);
                if (!AccCreated)
                {
                    label1.Visible = false;
                    label2.Text = $"Account with username: {usernametextbox.Text} already exist {AccCreated}";
                }
                else
                {
                    LoginSwitcher();
                }
            }
            else if (passwordtextbox.Text != passwordtextboxcheck.Text)
            {
                label1.Visible = false;
                label2.Text = "Password doesn't match";
            }
            else
            {
                label1.Text = "Username is Incorrect";
            }
        }
        //simple form switcher used for switching the form -_-
        private void LoginSwitcher()
        {

                this.Hide();
                Login login = new Login();
                login.ShowDialog();
            
        }

        //if lable for login was cliked switch the form
        private void label6_Click(object sender, EventArgs e)
        {
            LoginSwitcher();
        }
        //SignUp_Load event used for nothing now -_-
        private void SignUp_Load(object sender, EventArgs e)
        {
            
        }
        //also createAccount just when enter on keyboard pressed event
        private void passwordtextboxcheck_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                signupcheck();
        }
        //also createAccount just when enter on keyboard pressed event
        private void passwordtextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                signupcheck();
        }
        //also createAccount just when enter on keyboard pressed event
        private void usernametextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                signupcheck();
        }
    }
}
