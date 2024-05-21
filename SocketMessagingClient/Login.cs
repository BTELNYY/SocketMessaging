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
    }
}
