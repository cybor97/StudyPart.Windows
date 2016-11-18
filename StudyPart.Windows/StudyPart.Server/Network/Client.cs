using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using StudyPart.Core.Network;

namespace StudyPart.Server.Network
{
    public class Client
    {
        public static bool KeepConnection { get; set; } = true;

        TcpClient _Client { get; set; }

        NetworkStream ClientStream { get { return _Client.GetStream(); } }

        StreamReader Reader { get { return new StreamReader(ClientStream); } }

        StreamWriter Writer { get { return new StreamWriter(ClientStream) { AutoFlush = true }; } }

        Thread Manager { get; set; }

        public bool Connected { get { return _Client != null && ClientStream != null && Manager != null && !(Manager.ThreadState == ThreadState.Stopped || Manager.ThreadState == (ThreadState.Stopped | ThreadState.AbortRequested)) && _Client.Connected; } }

        public string Hash { get; set; }

        public DateTime LastReply = DateTime.Now;

        public delegate string RequestAcceptedDelegate(Client sender, Request request);

        public event RequestAcceptedDelegate RequestAccepted;

        public void Start()
        {
            if (Manager == null || Manager.ThreadState == ThreadState.Stopped)
                (Manager = new Thread(() =>
                {
                    _Client.LingerState = new LingerOption(false, 0);
                    _Client.NoDelay = true;
                    do
                        try
                        {
                            LastReply = DateTime.Now;
                            var data = Reader.ReadToEndBlock();
                            if (data.StartsWith("hash="))
                            {
                                Hash = data.Split('=')[1];
                                Writer.WriteWithEndBlock(string.Format("OK, hash={0}", Hash));
                            }
                            else if (RequestAccepted != null)
                                Writer.WriteWithEndBlock(RequestAccepted(this, Request.Parse(data.Replace(((char)0).ToString(), ""))));
                            else
                                Writer.WriteWithEndBlock("Sorry, I still not ready...");
                        }
                        catch (Exception e)
                        {
                            if (e is ThreadAbortException)
                                throw;
                            else if (e is SocketException || e is IOException)
                                Stop();
                            if (e.ToString().Contains("Connection reset by peer"))
                                Stop();
                            else if (e.Message.ToLower().Contains("timeout"))
                                Stop();
                            else
                                Thread.Sleep(100);
                            //Console.WriteLine("[{0}, CLIENT_ERROR]: {1}", DateTime.Now.ToString("G"), e.Message);
                        }
                    while (Connected && KeepConnection);
                })).Start();
        }

        public void Stop()
        {
            _Client?.Close();
            _Client = null;
            Manager?.Abort();
            Manager = null;
        }

        public Client(TcpClient client, string hash)
        {
            _Client = client;
            Hash = hash;
        }
    }
}
