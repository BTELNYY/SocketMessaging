// ALL THE COMMENTS ARE JUST NOT EVEN CLOSE, THERE MUCH MORE PROCESSES ON SERVER, ETC.
// USED JUST TO APPROXIMATELY EXPLAIN WHAT IS GOING ON
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        // at the form load, then load debuger
        private void Chat_Load(object sender, EventArgs e)
        {
#if DEBUG
            Debugger();
#endif
        }
        // debuger only for debugin fast testing used for fastLogin
        private void Debugger()
        {
            bool LoginSucces = Program.MyClient.ClientLogin("user", "pass");
            if (LoginSucces)
            {
                this.Close();
                Chat chat = new Chat();
                chat.ShowDialog();
            };
        }

        
        //if click on login button -> login
        private void button2_Click(object sender, EventArgs e)
        {
            loginl();
            
        }

        //exact same login just when we clicked enter on keyboard
        private void passwordtextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return)
                loginl();
        }

        //exact same login just when we clicked enter on keyboard
        private void usernametextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                loginl();
        }

        // the fucntion that made login (verifying data in textboxes and then make the login if successful then swap the forms, otherwise show the unseccess label + data)
        private void loginl()
        {
            if (usernametextbox.Text != string.Empty && passwordtextbox.Text != string.Empty)
            {
                bool LoginSucces = Program.MyClient.ClientLogin(usernametextbox.Text, passwordtextbox.Text);
                if (LoginSucces)
                {
                    this.Hide();
                    Chat chat = new Chat();
                    chat.ShowDialog();
                }
                else
                {
                    label1.Visible = false;
                    label2.Text = "Unsuccessful";
                }
            }
        }

        // if sign up label was clicked, then swap to the sign up form
        private void label6_Click(object sender, EventArgs e)
        {
            this.Hide();
            SignUp signUp = new SignUp();
            signUp.ShowDialog();
        }

        // Exit the app if the form closes.
        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
