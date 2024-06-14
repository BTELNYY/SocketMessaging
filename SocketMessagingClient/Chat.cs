using SocketMessagingShared.CustomTypes;
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
using SocketNetworking;
using System.Runtime.Remoting.Channels;

namespace SocketMessagingClient
{
    public partial class Chat : Form
    {
        List<Button> buttons = new List<Button>();

        public Chat()
        {
            InitializeComponent();
            _channels = Program.MyClient.ClientChannelController.Channels;
        }

        private void Chat_Load(object sender, EventArgs e)
        {
            Program.MyClient.ClientChannelController.ClientReceiveChannels += ClientChannelController_ClientReceiveChannels;
        }

        private object _lock = new object();

        private List<NetworkChannel> _channels = new List<NetworkChannel>();

        private void ClientChannelController_ClientReceiveChannels(List<NetworkChannel> obj)
        {
            lock(_lock)
            {
                _channels = obj;
            }
            Invalidate();
        }

        private void RemoveButtons()
        {
            foreach (Button button in buttons) 
            {
                button.Controls.Remove(button);
            }
        }

        private void ChannelDisplays()
        {
            int c = 0;
            List<NetworkChannel> channels = _channels;
            foreach (NetworkChannel channel in channels)
            {
                Button button = new Button();
                button.Text = channel.Name;
                button.ForeColor = Color.White;
                button.Name = channel.Name;
                button.Location = new Point(50,c+10);
                button.Visible = true;
                button.Size = new Size(100,50);
                this.Controls.Add(button);
                buttons.Add(button);
                c +=50;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           
        }

        private void Chat_Paint(object sender, PaintEventArgs e)
        {
            lock (_lock)
            {
                RemoveButtons();
                ChannelDisplays();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) { writetextbox.Text = string.Empty; }
        }
    }
}
