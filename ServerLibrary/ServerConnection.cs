﻿using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerLibrary
{

    class ServerConnection
    {
        public delegate void ParameterizedThreadStart(TcpClient client);
        public ClientProcesing menager {get;set;}
        public void RunServer()
        {
            // Create a TCP/IP (IPv4) socket and listen for incoming connections.
            TcpListener listener = new TcpListener(IPAddress.Any, 17777);
            listener.Start();
            while (true)
            {
                Console.WriteLine("Waiting for a client to connect...");
                TcpClient client = listener.AcceptTcpClient();
                Thread t = new Thread(ClientConnection);
                t.Start(client);

            }
        }

        public void ClientConnection(Object obj)
        {
            while (true)
            {
                TcpClient client = obj as TcpClient;
                NetworkStream stream = client.GetStream();

                int playerID = menager.AddPlayer(new Player());
                string sendMessage = "";
                byte[] buffer = new byte[2048];
                StringBuilder messageData = new StringBuilder();
                int bytes = -1;


                bytes = stream.Read(buffer, 0, buffer.Length);

                Decoder decoder = Encoding.ASCII.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);


                //sendMessage = menager.ProccesClient(messageData.ToString(), playerID);
                sendMessage = "Elo";
                byte[] message = Encoding.ASCII.GetBytes(sendMessage);
                stream.Write(message);
            }
        }



        public ServerConnection()
        {
            menager = new ClientProcesing();
        }


    }
}
