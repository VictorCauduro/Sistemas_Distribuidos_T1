using System;
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
        static IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);
        static Socket listener = new Socket(ipAddr.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);

        private static Mutex mut = new Mutex();
        private const int numIterations = 1;
        private const int numThreads = 3;

        static void Main(string[] args)
        {
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {

                    Console.WriteLine("Aguardando conexão...");

                    // Suspend while waiting for 
                    // incoming connection Using  
                    // Accept() method the server  
                    // will accept connection of client 
                    Socket clientSocket = listener.Accept();

                    // Data buffer 
                    byte[] bytes = new Byte[1024];
                    string data = null;
                    //ThreadStart aux = new ThreadStart()
                    //Thread newThread = new Thread(new ThreadStart(CadastrarCliente(data, bytes, clientSocket)));
                    while (true)
                    {
                        mut.WaitOne();
                        int numByte = clientSocket.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes,
                                               0, numByte);
                        if (data.Length > -1)
                        {
                            break;
                        }
                        else
                        {
                            mut.ReleaseMutex();

                        }                            
                    }

                    Console.WriteLine("Text received -> {0} ", data);
                    byte[] message = Encoding.ASCII.GetBytes("Test Server");

                    // Send a message to Client  
                    // using Send() method 
                    clientSocket.Send(message);

                    // Close client Socket using the 
                    // Close() method. After closing, 
                    // we can use the closed Socket  
                    // for a new Client Connection 
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    mut.ReleaseMutex();
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
