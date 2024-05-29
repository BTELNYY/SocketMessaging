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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

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
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty && textBox2.Text != string.Empty && textBox3.Text != string.Empty && textBox4.Text != string.Empty && textBox5.Text != string.Empty && textBox6.Text != string.Empty)
            {
                Program.MyClinet.Connect(textBox1.Text + "." + textBox4.Text + "." + textBox5.Text + "." + textBox6.Text, int.Parse(textBox3.Text), textBox2.Text);
            }
        }
    }
}
