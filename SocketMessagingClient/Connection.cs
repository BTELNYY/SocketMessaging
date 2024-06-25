// ALL THE COMMENTS ARE JUST NOT EVEN CLOSE, THERE MUCH MORE PROCESSES ON SERVER, ETC.
// USED JUST TO APPROXIMATELY EXPLAIN WHAT IS GOING ON
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SocketNetworking;

namespace SocketMessagingClient
{
    public partial class Connection : Form
    {
        public Connection()
        {
            InitializeComponent();
        }

        //some useless 2 borders made for fun
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.Location = new Point((ClientSize.Width - panel1.Size.Width)/2, ((ClientSize.Height - ClientSize.Height)/2)-3);
            panel1.Size = new Size((ClientSize.Width - 80 % ClientSize.Width), ClientSize.Height+6) ;
            panel1.BackColor = Color.FromArgb(39, 40, 41);
            int thickness = 3;
            int halfThickness = thickness / 2;
            using (Pen p = new Pen(Color.FromArgb(216, 217, 218), thickness))
            {
                e.Graphics.DrawRectangle(p, new Rectangle(halfThickness,
                                                          halfThickness,
                                                          panel1.ClientSize.Width - thickness,
                                                          panel1.ClientSize.Height - thickness));
            }
        }

        //If button clicked verifying info and then Client Connect to the server
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty && textBox3.Text != string.Empty && textBox4.Text != string.Empty && textBox5.Text != string.Empty && textBox6.Text != string.Empty)
            {
                bool result = Program.MyClient.Connect(textBox1.Text + "." + textBox4.Text + "." + textBox5.Text + "." + textBox6.Text, int.Parse(textBox3.Text), textBox2.Text);
                if (result)
                {
                    this.Hide();
                    tryConnect();
                }
            }
        }

        //at the start trying by default connect to the server and if so just call function that show other form
        private void Login_Load(object sender, EventArgs e)
        {
#if DEBUG
            bool connected = Program.MyClient.Connect("127.0.0.1", 7777, "");
            if (connected)
            {
                tryConnect();
            }
#endif
        }

        //function that swipes forms
        private void tryConnect()
        {
            Login login = new Login();
            login.ShowDialog();
            this.Close();
        }

        //Close app when the form is closed.
        private void Connection_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
