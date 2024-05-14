using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketMessagingClient
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            this.MaximumSize = Size.Empty;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }
    }
}
