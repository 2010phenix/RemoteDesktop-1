using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class MainForm : Form
    {
        Sock.Server _server = new Sock.Server();

        public MainForm()
        {
            InitializeComponent();
            SetupConnection();
        }

        public void SetupConnection()
        {
            _server = new Sock.Server();
            _server.BufferSize = 1048576;

            _server.OnDataReceived += _server_OnDataReceived;
            _server.OnClientDisconnect += _server_OnClientDisconnect;

            _server.Start(100);
        }

        private void _server_OnDataReceived(Sock.Client client, object[] data)
        {
            var header = (NetworkHeaders)data[0];
            var command = (NetworkCommands)data[1];

            if (header == NetworkHeaders.Handshake &&
                command == NetworkCommands.Authenticate)
            {
                var info = new ClientInfo(client, data[2] as string, data[3] as string);
                Invoke((MethodInvoker)delegate { lvClients.Items.Add(info.ToListView()); });
            }
        }

        private void _server_OnClientDisconnect(Sock.Client client)
        {
            Invoke((MethodInvoker)delegate
            {
                foreach (ListViewItem lvi in lvClients.Items)
                {
                    var tag = (ClientInfo)lvi.Tag;

                    if (tag.ClientSocket == client)
                        lvClients.Items.Remove(lvi);
                }
            });
        }

        private void remoteDesktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvClients.SelectedItems.Count < 1)
            {
                MessageBox.Show("You have to select a client in order to access this function!",
                    "ERROR : Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var selectedItem = lvClients.SelectedItems[0];
                var info = (ClientInfo)selectedItem.Tag;

                var frm = new RemoteDesktopFrm(info, _server);
                frm.ShowDialog();
            }
        }
    }
}
