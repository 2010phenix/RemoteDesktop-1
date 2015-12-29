using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;
        const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
        const uint MOUSEEVENTF_MOVE = 0x0001;
        const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        const uint MOUSEEVENTF_XDOWN = 0x0080;
        const uint MOUSEEVENTF_XUP = 0x0100;
        const uint MOUSEEVENTF_WHEEL = 0x0800;
        const uint MOUSEEVENTF_HWHEEL = 0x01000;

        Sock.Client _client = new Sock.Client();
        ClientInfo _clientInfo = new ClientInfo();

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            _client = new Sock.Client();
            _client.BufferSize = 1048576;

            _client.OnConnect += _client_OnConnect;
            _client.OnDataReceived += _client_OnDataReceived;
            _client.OnServerDisconnect += _client_OnServerDisconnect;

            IPAddress address;

            if (!IPAddress.TryParse(txtIPAddress.Text, out address))
            {
                MessageBox.Show("The entered IP Address is invalid!", "ERROR : Invalid IP Address",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _clientInfo = new ClientInfo(_client, Guid.NewGuid().ToString().ToUpper(), Environment.UserName);
                _client.Connect(address, 100);
            }
        }

        private void _client_OnConnect(bool connected)
        {
            if (connected)
            {
                Invoke((MethodInvoker)delegate { btnConnect.Enabled = false; });

                if (_client.Send(new object[] { (int)NetworkHeaders.Handshake, (int)NetworkCommands.Authenticate, _clientInfo.ClientID, _clientInfo.User }))
                {
                    MessageBox.Show("The connection to the Server was successful!", "Server Connection Status",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("The connection to the Server was unable to be made. Please try again!",
                    "Server Connection Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _client_OnDataReceived(Sock.Server sender, object[] data)
        {
            var header = (NetworkHeaders)data[0];
            var command = (NetworkCommands)data[1];

            if (header == NetworkHeaders.Image)
            {
                if (command == NetworkCommands.Request_Frame)
                {
                    var screenshot = ImageFunctions.TakeScreenshot((int)data[2]);
                    _client.Send(new object[] { (int)NetworkHeaders.Image, (int)NetworkCommands.Request_Frame, screenshot });
                }
                else if (command == NetworkCommands.Left_Click)
                {
                    var point = (Point)data[2];
                    Cursor.Position = point;

                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                }
                else if (command == NetworkCommands.Right_Click)
                {
                    var point = (Point)data[2];
                    Cursor.Position = point;

                    mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                    mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                }
                else if (command == NetworkCommands.Request_Monitors)
                {
                    var monitorList = Screen.AllScreens.Select(s => s.DeviceName).ToArray();
                    _client.Send(new object[] { (int)NetworkHeaders.Image, (int)NetworkCommands.Request_Monitors, monitorList });
                }
            }    
        }

        private void _client_OnServerDisconnect()
        {
            MessageBox.Show("The connection to the Server was lost! The program will now exit!",
                "ERROR : Connection Lost", MessageBoxButtons.OK, MessageBoxIcon.Error);

            Environment.Exit(0);
        }
    }
}
