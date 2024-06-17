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
using System.Diagnostics;

namespace SocketMessagingClient
{
    public partial class Chat : Form
    {
        List<Button> buttons = new List<Button>();

        string _selectedChannelUUID = "";

        int SelectedChannelIndex
        {
            get
            {
                NetworkChannel channel = _channels.Where(x => x.UUID == _selectedChannelUUID).FirstOrDefault();
                if(channel == default(NetworkChannel))
                {
                    return -1;
                }
                return _channels.IndexOf(channel);
            }
        }

        public Chat()
        {
            InitializeComponent();
            _channels = Program.MyClient.ClientChannelController.Channels;
        }

        private void Chat_Load(object sender, EventArgs e)
        {
            Program.MyClient.ClientChannelController.ClientReceiveChannels += ClientChannelController_ClientReceiveChannels;
            Program.MyClient.ClientChannelController.MessegeRecieved += ClientChannelController_MessegeRecieved;
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
                if(button.Tag == null)
                {
                    continue;
                }
                _buttonsToChannels.Remove(button.Name);
            }
        }

        Dictionary<string, NetworkChannel> _buttonsToChannels = new Dictionary<string, NetworkChannel>();

        private void ShowButtons()
        {
            int c = 0;
            List<NetworkChannel> channels = _channels;
            foreach (NetworkChannel channel in channels)
            {
                Button button = new Button();
                button.Text = channel.Name;
                button.ForeColor = Color.White;
                button.Name = channel.Name;
                button.Location = new Point(0,c);
                button.Visible = true;
                button.Size = new Size(250,30);
                button.Click += Button_Click;
                button.Name = channel.UUID;
                this.Controls.Add(button);
                buttons.Add(button);
                c +=30;
                _buttonsToChannels.Add(channel.UUID, channel);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button pressed = sender as Button;
            NetworkChannel channel = _buttonsToChannels[pressed.Name];
            _selectedChannelUUID = channel.UUID;
            DisplayPreviousMessage(channel);
        }
        
        private void DisplayPreviousMessage(NetworkChannel channel)
        {
        
        }

        private void ClientChannelController_MessegeRecieved(NetworkChannel arg1, NetworkMessage arg2)
        {
            Invalidate();
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
                if(SelectedChannelIndex == -1)
                {
                    this.NoChannelsLabel.Show();
                    this.writetextbox.Hide();
                    this.button1.Hide();
                }
                else
                {
                    this.NoChannelsLabel.Hide();
                    this.writetextbox.Show();
                    this.button1.Show();
                }
                RemoveButtons();
                _buttonsToChannels.Clear();
                ShowButtons();
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
