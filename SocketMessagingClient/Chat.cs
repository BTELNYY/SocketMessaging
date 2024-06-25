// ALL THE COMMENTS ARE JUST NOT EVEN CLOSE, THERE MUCH MORE PROCESSES ON SERVER, ETC.
// USED JUST TO APPROXIMATELY EXPLAIN WHAT IS GOING ON
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
    // list buttons used for list of buttns aka channels
    public partial class Chat : Form
    {
        List<Button> buttons = new List<Button>();
        //string of UUID of selected channel (selected button)
        string _selectedChannelUUID = "";
        //integer of id of selected channel
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
        // public class NetworkChannel CurrentChannel that return Selected Channel Index
        public NetworkChannel CurrentChannel
        {
            get
            {
                if(SelectedChannelIndex == -1)
                {
                    return null;
                }
                return Program.MyClient.ClientChannelController.Channels[SelectedChannelIndex];
            }
        }
        // public for Chat, at the start Initialize Components (all the stuff that take place at the form), then set for channls all the channel that exist at the start
        public Chat()
        {
            InitializeComponent();
            _channels = Program.MyClient.ClientChannelController.Channels;
        }

        // now that doing nothing -_-
        private void DisplayPreviousMessage(NetworkChannel channel)
        {

        }

        // sending the messages (veryfying data and then create new NetworkMessage message, set for it needed properties, and at the end send it)
        private void SendMessage(string text)
        {
            if(string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                return;
            }
            if(CurrentChannel == null)
            {
                return;
            }
            NetworkMessage message = new NetworkMessage();
            message.Content = text;
            message.Timestamp = DateTime.UtcNow.ToUnixTimestamp();
            message.AuthorName = Program.MyClient.Username;
            message.AuthorUUID = Program.MyClient.UUID;
            Program.MyClient.ClientChannelController.ClientSendMessage(CurrentChannel, message);
        }
        //when form loaded add event such as new channl or messages was received
        private void Chat_Load(object sender, EventArgs e)
        {
            Program.MyClient.ClientChannelController.ClientReceiveChannels += ClientChannelController_ClientReceiveChannels;
            Program.MyClient.ClientChannelController.MessegeRecieved += ClientChannelController_MessegeRecieved;
            Program.MyClient.ClientChannelController.MessagesRecieved += ClientChannelController_MessagesRecieved;
        }

        private object _lock = new object();
        // list of channels that are at the time
        private List<NetworkChannel> _channels = new List<NetworkChannel>();

        //event on new channel was recieved from the server, if so add channel and validate it
        private void ClientChannelController_ClientReceiveChannels(List<NetworkChannel> obj)
        {
            lock(_lock)
            {
                _channels = Program.MyClient.ClientChannelController.Channels;
            }
            ChannelPanel.Invalidate();
            Invalidate();
        }
        //event on new messages was recieved, claryfying data and then validate it
        private void ClientChannelController_MessagesRecieved(NetworkChannel obj)
        {
            lock (_lock)
            {
                _channels = Program.MyClient.ClientChannelController.Channels;
            }
            //Do not invalidate if the channel thats being updated isnt the channel we are focusing.
            if (obj.UUID != _selectedChannelUUID)
            {
                return;
            }
            ChatPanel.Invalidate();
            Invalidate();
        }
        //event on new message was recieved, claryfying data and then validate it
        private void ClientChannelController_MessegeRecieved(NetworkChannel arg1, NetworkMessage arg2)
        {
            lock (_lock)
            {
                _channels = Program.MyClient.ClientChannelController.Channels;
            }
            //Do not invalidate if the channel thats being updated isnt the channel we are focusing.
            if (arg1.UUID != _selectedChannelUUID)
            {
                return;
            }
            ChatPanel.Invalidate();
            Invalidate();
        }
        //remove button(channel) (redraw buttons each time when new channel where added)
        private void RemoveButtons()
        {
            foreach (Button button in buttons) 
            {
                ChannelPanel.Controls.Remove(button);
                if(button.Tag == null)
                {
                    continue;
                }
                _buttonsToChannels.Remove(button.Name);
            }
        }

        Dictionary<string, NetworkChannel> _buttonsToChannels = new Dictionary<string, NetworkChannel>();
        //displaying the buttons (channels) in right order
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
                ChannelPanel.Controls.Add(button);
                buttons.Add(button);
                c +=30;
                _buttonsToChannels.Add(channel.UUID, channel);
            }
        }
        //event if button was clicked (in that case if any channel was selected)
        private void Button_Click(object sender, EventArgs e)
        {
            Button pressed = sender as Button;
            NetworkChannel channel = _buttonsToChannels[pressed.Name];
            if(_selectedChannelUUID != channel.UUID)
            {
                _selectedChannelUUID = channel.UUID;
                DisplayPreviousMessage(channel);
                _alreadyRenderedMessages.Clear();
                ChatPanel.Controls.Clear();
                Invalidate();
            }
        }
        //redraw of the channels and hide/show elements
        private void Chat_Paint(object sender, PaintEventArgs e)
        {
            Log.GlobalDebug("Redraw");
            if (SelectedChannelIndex == -1)
            {
                NoChannelsLabel.Show();
                MessageTextBox.Hide();
                SendButton.Hide();
            }
            else
            {
                NoChannelsLabel.Hide();
                MessageTextBox.Show();
                SendButton.Show();
            }
            RemoveButtons();
            _buttonsToChannels.Clear();
            ShowButtons();
        }
        //list of drawen messages
        List<NetworkMessage> _alreadyRenderedMessages = new List<NetworkMessage>();
        // ChannelPanel_Paint for displaying info when ChannelPanel_Paint(Panel) claryfiyng data and selected channel
        // then set list messages to the Curenetchannel NetworkMessages (all the messages in the channel)
        // then add yoffset for messages
        // then for each message in messages list fo through and add Log debug to console
        // then claryfying if already redered not equal to message
        // then diisplying lables in right order
        // then also check if messages height is much more than our size of form (720) then add vertical scrollbar
        private void ChannelPanel_Paint(object sender, PaintEventArgs e)
        {
            if (CurrentChannel == null) return;
            List<NetworkMessage> messages = CurrentChannel.NetworkMessages;
            int yOffset = 0;
            foreach (NetworkMessage message in messages)
            {
                Log.GlobalDebug($"Channel: {CurrentChannel.Name}, Message: {message.AuthorName}: {message.Content}");
                if (_alreadyRenderedMessages.Contains(message))
                {
                    continue;
                }
                Label label = new Label();
                int lengthOfMessage = message.ToString().Length;
                int sizeMultiplier = (lengthOfMessage / 20)/2;
                label.Text = message.ToString();
                label.Location = new Point(265, yOffset);
                label.Visible = true;
                label.AutoSize = false;
                label.Size = new Size(780, 30 * Math.Max(1, sizeMultiplier));
                label.ForeColor = Color.White;
                label.Font = new Font(label.Font.FontFamily.Name, 15);
                bool shouldScrollToBottom = false;
                if(ChatPanel.VerticalScroll.Value == ChatPanel.VerticalScroll.Maximum - ChatPanel.VerticalScroll.LargeChange + 1)
                {
                    shouldScrollToBottom = true;
                }
                ChatPanel.Controls.Add(label);
                if (shouldScrollToBottom)
                {
                    ChatPanel.ScrollControlIntoView(label);
                }
                yOffset += 50;
                _alreadyRenderedMessages.Add(message);
            }
        }

        //when send message button was clicked call SendMessage with MessageTextBox.Text and set the MessageTextBox to empty
        private void SendButton_Click(object sender, EventArgs e)
        {
            SendMessage(MessageTextBox.Text);
            MessageTextBox.Text = string.Empty;
        }
    }
}
