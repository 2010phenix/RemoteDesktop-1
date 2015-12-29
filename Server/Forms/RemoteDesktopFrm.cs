using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class RemoteDesktopFrm : Form
    {
        Sock.Server _server = new Sock.Server();
        ClientInfo _clientInfo = new ClientInfo();

        bool _streaming = false;

        Point _resolution = new Point(0, 0);
        int _FPS = 0;

        public RemoteDesktopFrm(ClientInfo info, Sock.Server server)
        {
            InitializeComponent();

            _server = server;
            _clientInfo = info;
            Text += info.User;

            _server.OnDataReceived += _server_OnDataReceived;
            _server.OnClientDisconnect += _server_OnClientDisconnect;

            _clientInfo.ClientSocket.Send(new object[] { (int)NetworkHeaders.Image, (int)NetworkCommands.Request_Monitors });
        }

        private void _server_OnDataReceived(Sock.Client client, object[] data)
        {
            var header = (NetworkHeaders)data[0];
            var command = (NetworkCommands)data[1];

            if (header == NetworkHeaders.Image)
            {
                if (command == NetworkCommands.Request_Frame)
                {
                    Bitmap screenshot;

                    using (var ms = new MemoryStream(data[2] as byte[]))
                    {
                        screenshot = new Bitmap(ms);
                    }

                    _resolution = new Point(screenshot.Width, screenshot.Height);

                    Invoke((MethodInvoker)delegate { pbScreen.Image = screenshot; });
                    _FPS++;

                    if (_streaming)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            _clientInfo.ClientSocket.Send(new object[] { (int)NetworkHeaders.Image, (int)NetworkCommands.Request_Frame, toolStripComboBoxMonitors.SelectedIndex });
                        });
                    }
                }
                else if (command == NetworkCommands.Request_Monitors)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        toolStripComboBoxMonitors.Items.AddRange(data[2] as string[]);
                    });
                }
            }
        }

        private void _server_OnClientDisconnect(Sock.Client client)
        {
            if (client == _clientInfo.ClientSocket)
            {
                MessageBox.Show("The selected client has disconnected so you can no longer use this function!",
                    "ERROR : Client Disconnected", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Invoke((MethodInvoker)delegate { Close(); });
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (toolStripComboBoxMonitors.Text == "Select Monitor")
            {
                MessageBox.Show("There is no monitor selected! Please select a monitor!",
                    "ERROR : No Monitor Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                startToolStripMenuItem.Enabled = false;
                stopToolStripMenuItem.Enabled = true;

                _streaming = true;

                _clientInfo.ClientSocket.Send(new object[] { (int)NetworkHeaders.Image, (int)NetworkCommands.Request_Frame, toolStripComboBoxMonitors.SelectedIndex });

                tmrFPS.Start();
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startToolStripMenuItem.Enabled = true;
            stopToolStripMenuItem.Enabled = false;

            _streaming = false;
            tmrFPS.Stop();
        }

        private void pbScreen_MouseClick(object sender, MouseEventArgs e)
        {
            if (_streaming)
            {
                Point position = pbScreen.PointToClient(Cursor.Position);

                var multiplierX = (decimal)_resolution.X / (decimal)pbScreen.Width;
                var multiplierY = (decimal)_resolution.Y / (decimal)pbScreen.Height;

                var x = (int)Math.Round(position.X * multiplierX);
                var y = (int)Math.Round(position.Y * multiplierY);

                Point adjustedPoint = new Point(x, y);

                if (e.Button == MouseButtons.Left)
                {
                   _clientInfo.ClientSocket.Send(new object[] { (int)NetworkHeaders.Image, (int)NetworkCommands.Left_Click, adjustedPoint });
                }
                else if (e.Button == MouseButtons.Right)
                {
                    _clientInfo.ClientSocket.Send(new object[] { (int)NetworkHeaders.Image, (int)NetworkCommands.Right_Click, adjustedPoint });
                }
            }
        }

        private void RemoteDesktopFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _server.OnDataReceived -= _server_OnDataReceived;
            _server.OnClientDisconnect -= _server_OnClientDisconnect;
        }

        private void tmrFPS_Tick(object sender, EventArgs e)
        {
            toolStripTextBoxFPS.Text = "FPS: " + _FPS.ToString();
            _FPS = 0;
        }
    }
}
