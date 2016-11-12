using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace StudyPart.Windows.Network
{
    public static class Server
    {
        static TcpListener Listener { get; set; }

        static Thread Manager { get; set; }

        static Thread Cleaner { get; set; }

        public static List<Client> Clients { get; set; }

        public delegate void ClientAcceptedDelegate(Client client);

        public static event ClientAcceptedDelegate ClientAccepted;

        public static void Start()
        {
            Clients = new List<Client>();
            if (Manager == null || Manager.ThreadState == ThreadState.Stopped)
                (Manager = new Thread(() =>
                {
                    Listener = new TcpListener(IPAddress.Any, CommonVariables.ServerPortNumber);
                    Listener.Start();
                    while (true)
                        try
                        {
                            var client = new Client(Listener.AcceptTcpClient(), string.Empty);
                            client.Start();
                            lock (Clients)
                            {
                                Clients.Add(client);
                                ClientAccepted?.Invoke(client);
                            }
                        }
                        catch (Exception e)
                        {
                            if (e is ThreadAbortException)
                                throw;
                        }
                })).Start();
            if (Cleaner == null || Cleaner.ThreadState == ThreadState.Stopped)
                (Cleaner = new Thread(() =>
                {
                    while (true)
                        try
                        {
                            lock (Clients)
                            {
                                foreach (var current in Clients)
                                    if (!current.Connected || DateTime.Now.Subtract(current.LastReply).Minutes > 10)
                                        current.Stop();
                                Clients.RemoveAll(c => !c.Connected);
                            }
                            GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized);
                            Thread.Sleep(100);
                        }
                        catch (Exception e)
                        {
                            if (e is ThreadAbortException)
                                throw;
                        }
                })
                { Priority = ThreadPriority.Lowest, IsBackground = true }).Start();
        }

        public static string ReadToEndBlock(this StreamReader reader)
        {
            string result = "";
            string tmp;
            while (!reader.EndOfStream && (tmp = reader.ReadLine()) != "---END---")
                result += tmp;
            return result.Replace(@"\n", "\n");
        }

        public static void WriteWithEndBlock(this StreamWriter writer, string text)
        {
            writer.WriteLine("{0}\n---END---\n", text.Replace("\n", @"\n"));
        }


        public static void Stop()
        {
            Listener?.Stop();
            Listener = null;
            Cleaner?.Abort();
            Cleaner = null;
            Manager?.Abort();
            Manager = null;
        }
    }
}
