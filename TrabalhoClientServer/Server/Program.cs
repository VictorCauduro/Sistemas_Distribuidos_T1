using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        static IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
        static IPAddress ipAddr = ipHost.AddressList[0];
        static Dictionary<string, string[]> Users = new Dictionary<string, string[]>();
        static IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 22222);
        static Socket listener = new Socket(ipAddr.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);


        public static void ErroGenerico()
        {
            Console.WriteLine("Mensagem  vazia ou inválida. Descartada.");
        }
        static void Main(string[] args)
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);
            int count = 0;
            
            try
            {              
               
                while (true)
                {

                    Console.WriteLine("Aguardando conexão...");
                    Socket clientSocket = listener.Accept();

                      




                    // Suspend while waiting for 
                    // incoming connection Using  
                    // Accept() method the server  
                    // will accept connection of client 


                    // Data buffer 
                    byte[] bytes = new Byte[1024];
                    string data = null;
                    //ThreadStart aux = new ThreadStart()
                    //Thread newThread = new Thread(new ThreadStart(CadastrarCliente(data, bytes, clientSocket)));
                    while (true)
                    {

                        int numByte = clientSocket.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes,
                                               0, numByte);
                        if (data.Length > -1)
                        {
                            switch (data.Split(";")[0])
                            {
                                case "0":
                                    Console.WriteLine("Conexão Estabelecida com: " + data.Split(";")[1] + " Arquivos: " + data.Split(";")[2]);
                                    byte[] message = Encoding.ASCII.GetBytes("Conexão estabelecida com server: " + ipAddr);
                                    var ipUser = data.Split(";")[1];
                                    //Salvar na hash
                                    if (!data.Split(";")[2].Equals(""))
                                    {
                                        var files = data.Split(";")[2].Split("!");
                                        Users.Add(ipUser, files);
                                    }
                                    else
                                    {
                                        Users.Add(ipUser, null);
                                    }

                                    Console.WriteLine("Usuário cadastrado.");


                                    // Send a message to Client  
                                    // using Send() method 
                                    clientSocket.Send(message);

                                    // Close client Socket using the 
                                    // Close() method. After closing, 
                                    // we can use the closed Socket  
                                    // for a new Client Connection 
                                    //clientSocket.Shutdown(SocketShutdown.Both);
                                    //clientSocket.Close();       
                                    //clientSocket.Disconnect(true);
                                    //listener.Connect(localEndPoint);
                                    break;
                                case "1":
                                    Console.WriteLine("Requisição recebida");
                                    byte[] mensagemArquivo = Encoding.ASCII.GetBytes("Arquivos disponíveis:");                                    
                                    string aux = "";
                                    foreach (var x in Users)
                                    {
                                        foreach (var k in x.Value)
                                        {
                                            aux = aux + k;
                                        }
                                    }
                                    byte[] msgArquivos = Encoding.ASCII.GetBytes(aux);
                                    clientSocket.Send(msgArquivos);

                                    break;

                                default:
                                    ErroGenerico();
                                    break;
                            }
                            break;
                        }
                        else
                        {
                            ErroGenerico();
                            break;
                        }
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public static void CadastrarCliente(string data, byte[] bytes, Socket clientSocket)
        {

            int numByte = clientSocket.Receive(bytes);
            data += Encoding.ASCII.GetString(bytes,
                                   0, numByte);


        }




    }
}
