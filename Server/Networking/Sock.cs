using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

public class Sock
{
    #region Server

    public class Server
    {
        #region Delegates

        public delegate void ClientAcceptedEventHandler(Sock.Client client);
        public delegate void DataReceivedEventHandler(Sock.Client client, object[] data);
        public delegate void ClientDisconnectedEventHandler(Sock.Client client);

        #endregion

        #region Events

        public event ClientAcceptedEventHandler OnClientAccepted;
        public event DataReceivedEventHandler OnDataReceived;
        public event ClientDisconnectedEventHandler OnClientDisconnect;

        #endregion

        public Socket ServerSocket { get; set; }
        private byte[] DataBuffer { get; set; }

        public SocketEncryption EncryptionSettings { get; set; } = new SocketEncryption();
        public string EncryptionKey { get; set; } = "key";

        public bool UseEncryption { get; set; } = false;

        public int BufferSize
        {
            get
            {
                return DataBuffer.Length;
            }
            set
            {
                if (!ServerSocket.Connected)
                    DataBuffer = new byte[value];
            }
        }

        public Server()
        {
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            DataBuffer = new byte[1024];
        }

        public Server(Socket sock)
        {
            ServerSocket = sock;
            DataBuffer = new byte[1024];
        }

        public Server(Socket sock, int bufferSize)
        {
            ServerSocket = sock;
            DataBuffer = new byte[bufferSize];
        }

        #region Socket Functions

        public void Start(int port)
        {
            if (!ServerSocket.Connected)
            {
                ServerSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                ServerSocket.Listen(10);
                ServerSocket.BeginAccept(new AsyncCallback(OnAccept), ServerSocket);
            }
        }

        public void Start(int port, int backlog)
        {
            if (!ServerSocket.Connected)
            {
                ServerSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                ServerSocket.Listen(backlog);
                ServerSocket.BeginAccept(new AsyncCallback(OnAccept), ServerSocket);
            }
        }

        #endregion

        #region CallBacks

        private void OnAccept(IAsyncResult ar)
        {
            Socket sock = ServerSocket.EndAccept(ar);

            Sock.Client client = new Sock.Client(sock, DataBuffer.Length);
            client.EncryptionKey = EncryptionKey;
            client.EncryptionSettings = EncryptionSettings;
            client.UseEncryption = UseEncryption;

            if (OnClientAccepted != null)
                OnClientAccepted(client);

            client.ClientSocket.BeginReceive(DataBuffer, 0, DataBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), client);

            ServerSocket.BeginAccept(new AsyncCallback(OnAccept), sock);
        }

        private void OnReceive(IAsyncResult ar)
        {
            Sock.Client sock = ar.AsyncState as Sock.Client;

            int receivedLength = 0;

            try
            {
                receivedLength = sock.ClientSocket.EndReceive(ar);
            }
            catch (SocketException sockEx)
            {
                if (string.Equals(sockEx.Message, "An existing connection was forcibly closed by the remote host"))
                {
                    if (OnClientDisconnect != null)
                        OnClientDisconnect(sock);
                }
            }

            if (receivedLength != 0)
            {
                if (UseEncryption)
                {
                    byte[] dataPacket = new byte[receivedLength];
                    Buffer.BlockCopy(DataBuffer, 0, dataPacket, 0, receivedLength);

                    byte[] decrypted = Decompress(EncryptionSettings.Decrypt(dataPacket, EncryptionKey));

                    if (OnDataReceived != null)
                        OnDataReceived(sock, DataFormatter.ConvertToObject(decrypted));

                    sock.ClientSocket.BeginReceive(DataBuffer, 0, DataBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), sock);
                }
                else
                {
                    byte[] dataPacket = new byte[receivedLength];
                    Buffer.BlockCopy(DataBuffer, 0, dataPacket, 0, receivedLength);

                    if (OnDataReceived != null)
                        OnDataReceived(sock, DataFormatter.ConvertToObject(Decompress(dataPacket)));

                    sock.ClientSocket.BeginReceive(DataBuffer, 0, DataBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), sock);
                }
            }
        }

        private void OnSend(IAsyncResult ar)
        {
            Socket sock = ar.AsyncState as Socket;
            sock.EndSend(ar);
        }

        #endregion
    }

    #endregion

    #region Client

    public class Client
    {
        #region Delegates

        public delegate void ConnectedEventHandler(bool connected);
        public delegate void DataReceivedEventHandler(Sock.Server sender, object[] data);
        public delegate void ServerDisconnectedEventHanlder();

        #endregion

        #region Events

        public event ConnectedEventHandler OnConnect;
        public event DataReceivedEventHandler OnDataReceived;
        public event ServerDisconnectedEventHanlder OnServerDisconnect;

        #endregion

        public Socket ClientSocket { get; set; }
        private byte[] DataBuffer { get; set; }

        public SocketEncryption EncryptionSettings { get; set; } = new SocketEncryption();
        public string EncryptionKey { get; set; } = "key";

        public bool UseEncryption { get; set; } = false;

        public int BufferSize
        {
            get
            {
                return DataBuffer.Length;
            }
            set
            {
                if (!ClientSocket.Connected)
                    DataBuffer = new byte[value];
            }
        }

        public Client()
        {
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            DataBuffer = new byte[1024];
        }

        public Client(Socket sock)
        {
            ClientSocket = sock;
            DataBuffer = new byte[1024];
        }

        public Client(Socket sock, int bufferSize)
        {
            ClientSocket = sock;
            DataBuffer = new byte[bufferSize];
        }

        #region Socket Functions

        public void Connect(IPAddress ip, int port)
        {
            try
            {
                ClientSocket.Connect(new IPEndPoint(ip, 100));
            }
            catch (SocketException sockEx)
            {
                if (string.Equals(sockEx.Message, "No connection could be made because the target machine actively refused it"))
                {
                    if (OnConnect != null)
                        OnConnect(false);
                }
            }

            if (ClientSocket.Connected)
            {
                var sock = new Sock.Server(ClientSocket, DataBuffer.Length);
                sock.ServerSocket.BeginReceive(DataBuffer, 0, DataBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), sock);
            }

            if (OnConnect != null)
                OnConnect(ClientSocket.Connected);
        }

        public bool Send(object[] data)
        {
            if (ClientSocket.Connected)
            {
                if (UseEncryption)
                {
                    byte[] dataPacket = EncryptionSettings.Encrypt(Compress(DataFormatter.ConvertToByte(data)), EncryptionKey);
                    ClientSocket.BeginSend(dataPacket, 0, dataPacket.Length, SocketFlags.None, new AsyncCallback(OnSend), ClientSocket);
                }
                else
                {
                    byte[] dataPacket = Compress(DataFormatter.ConvertToByte(data));
                    ClientSocket.BeginSend(dataPacket, 0, dataPacket.Length, SocketFlags.None, new AsyncCallback(OnSend), ClientSocket);
                }

                return true;
            }

            return false;
        }

        #endregion

        #region CallBacks

        private void OnReceive(IAsyncResult ar)
        {
            Sock.Server sock = ar.AsyncState as Sock.Server;

            int receivedLength = 0;

            try
            {
                receivedLength = sock.ServerSocket.EndReceive(ar);
            }
            catch (Exception ex)
            {
                if (string.Equals(ex.Message, "Object reference not set to an instance of an object.") |
                    string.Equals(ex.Message, "An existing connection was forcibly closed by the remote host"))
                {
                    if (OnServerDisconnect != null)
                        OnServerDisconnect();
                }
            }

            if (receivedLength != 0)
            {
                if (UseEncryption)
                {
                    byte[] dataPacket = new byte[receivedLength];
                    Buffer.BlockCopy(DataBuffer, 0, dataPacket, 0, receivedLength);

                    byte[] decrypted = Decompress(EncryptionSettings.Decrypt(dataPacket, EncryptionKey));

                    if (OnDataReceived != null)
                        OnDataReceived(sock, DataFormatter.ConvertToObject(decrypted));

                    sock.ServerSocket.BeginReceive(DataBuffer, 0, DataBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), sock);
                }
                else
                {
                    byte[] dataPacket = new byte[receivedLength];
                    Buffer.BlockCopy(DataBuffer, 0, dataPacket, 0, receivedLength);

                    if (OnDataReceived != null)
                        OnDataReceived(sock, DataFormatter.ConvertToObject(Decompress(dataPacket)));

                    sock.ServerSocket.BeginReceive(DataBuffer, 0, DataBuffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), sock);
                }
            }
        }

        private void OnSend(IAsyncResult ar)
        {
            Socket sock = ar.AsyncState as Socket;
            sock.EndSend(ar);
        }

        #endregion
    }

    #endregion

    #region Data Formatting

    private class DataFormatter
    {
        public static byte[] ConvertToByte(object[] obj)
        {
            var bf = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static object[] ConvertToObject(byte[] arrBytes)
        {
            var bf = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                ms.Write(arrBytes, 0, arrBytes.Length);
                ms.Seek(0, SeekOrigin.Begin);

                return (object[])bf.Deserialize(ms);
            }
        }
    }

    #endregion

    #region Encryption

    public class SocketEncryption
    {
        public EncryptionMethod Method { get; set; }

        public SocketEncryption()
        {
            Method = new DefaultEncryption();
        }

        public SocketEncryption(EncryptionMethod method)
        {
            Method = method;
        }

        public string GenerateKey()
        {
            return Guid.NewGuid().ToString();
        }

        public byte[] Encrypt(byte[] input, string key)
        {
            return Method.Encrypt(input, key);
        }

        public byte[] Decrypt(byte[] input, string key)
        {
            return Method.Decrypt(input, key);
        }
    }

    public interface EncryptionMethod
    {
        byte[] Encrypt(byte[] input, string key);
        byte[] Decrypt(byte[] input, string key);
    }

    private class DefaultEncryption : EncryptionMethod
    {
        public byte[] Encrypt(byte[] input, string key)
        {
            using (var ms = new MemoryStream())
            {
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                    byte[] rijndaelKey = md5.ComputeHash(keyBytes, 0, keyBytes.Length);

                    using (var r = new RijndaelManaged())
                    {
                        r.Key = rijndaelKey;
                        r.IV = rijndaelKey;

                        r.Mode = CipherMode.CBC;
                        r.Padding = PaddingMode.PKCS7;

                        using (var cs = new CryptoStream(ms, r.CreateEncryptor(), CryptoStreamMode.Write))
                            cs.Write(input, 0, input.Length);

                        return ms.ToArray();
                    }
                }
            }
        }

        public byte[] Decrypt(byte[] input, string key)
        {
            using (var ms = new MemoryStream())
            {
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                    byte[] rijndaelKey = md5.ComputeHash(keyBytes, 0, keyBytes.Length);

                    using (var r = new RijndaelManaged())
                    {
                        r.Key = rijndaelKey;
                        r.IV = rijndaelKey;

                        r.Mode = CipherMode.CBC;
                        r.Padding = PaddingMode.PKCS7;

                        using (var cs = new CryptoStream(ms, r.CreateDecryptor(), CryptoStreamMode.Write))
                            cs.Write(input, 0, input.Length);

                        return ms.ToArray();
                    }
                }
            }
        }
    }

    #endregion

    #region Compression / Decompression

    public static byte[] Compress(byte[] input)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (GZipStream _gz = new GZipStream(ms, CompressionMode.Compress))
            {
                _gz.Write(input, 0, input.Length);
            }
            return ms.ToArray();
        }
    }

    public static byte[] Decompress(byte[] input)
    {
        using (var ms = new MemoryStream(input))
        {
            using (var ms2 = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    byte[] payload = new byte[1000];
                    int count = 0;

                    while ((count = gzip.Read(payload, 0, payload.Length)) > 0)
                    {
                        ms2.Write(payload, 0, count);
                    }

                    return ms2.ToArray();
                }
            }
        }
    }

    #endregion
}