using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientInfo
    {
        public Sock.Client ClientSocket { get; set; }

        public string ClientID { get; set; }
        public string User { get; set; }

        public ClientInfo() { }

        public ClientInfo(Sock.Client client, string ID, string user)
        {
            ClientSocket = client;
            ClientID = ID;
            User = user;
        }
    }
}
