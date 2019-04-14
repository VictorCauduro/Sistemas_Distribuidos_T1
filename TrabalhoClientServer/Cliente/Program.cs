using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace Cliente
{
    class Program
    {


        // Main Method 
        static void Main(string[] args)
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 22222);
            Socket sender = new Socket(ipAddr.AddressFamily,
                               SocketType.Stream, ProtocolType.Tcp);
            //Cria lista de arquivos
            string path = Directory.GetCurrentDirectory();
            var pathCortado = path.Split("bin");
            string pathGambiarra = pathCortado[0] + "Files";
            List<string> listaArquivos = new List<string>();
            try
            {
                foreach (string file in Directory.GetFiles(pathGambiarra))
                    listaArquivos.Add(Path.GetFileName(file));
            }
            catch (Exception e)
            {
                Console.WriteLine("Lista de arquivos inexistente ou vazia!");
            }
            string arquivos = "";
            if (listaArquivos.Count > 0)
            {
                foreach (var item in listaArquivos)
                {
                    if (listaArquivos[0] != item)
                    {
                        arquivos = arquivos + "!" + item;
                    }
                    else
                    {
                        arquivos = arquivos + item;
                    }

                }
            }

            try
            {


                // Creation TCP/IP Socket using  
                // Socket Class Costructor 


                try
                {

                    sender.Connect(localEndPoint);
                    Console.WriteLine("Socket connected to -> {0} ",
                                  sender.RemoteEndPoint.ToString());


                    byte[] messageSent = Encoding.ASCII.GetBytes("0;" + ipAddr + ";" + arquivos);
                    int byteSent = sender.Send(messageSent);

                    // Data buffer 
                    byte[] messageReceived = new byte[1024];

                    // We receive the messagge using  
                    // the method Receive(). This  
                    // method returns number of bytes 
                    // received, that we'll use to  
                    // convert them to string 
                    int byteRecv = sender.Receive(messageReceived);
                    Console.WriteLine("Message from Server -> {0}",
                          Encoding.ASCII.GetString(messageReceived,
                                                     0, byteRecv));

                    // Close Socket using  
                    // the method Close() 
                    //sender.Shutdown(SocketShutdown.Both);
                    //sender.Close();
                    
                    sender.Disconnect(true);
                   
                }

                // Manage of Socket's Exceptions 
                catch (ArgumentNullException ane)
                {

                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }

                catch (SocketException se)
                {

                    Console.WriteLine("SocketException : {0}", se.ToString());
                }

                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }

            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }

            bool connected = true;
            while (connected)
            {
                Console.WriteLine("Escolha uma opção:" + "\n" + "1 - Requisitar arquivos" + "\n" + "2 - Sair");
                var opc = Console.ReadLine();
                
                switch (opc)
                {
                    case "1":
                        sender.ConnectAsync(localEndPoint);
                        Console.WriteLine("Requisitado arquivos");
                        byte[] messageSent = Encoding.ASCII.GetBytes("1;");                        
                        int byteSent = sender.Send(messageSent);
                        sender.Disconnect(true);                        
                        break;
                    case "2":
                        connected = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida");
                        break;
                }
            }
        }
    }
}
