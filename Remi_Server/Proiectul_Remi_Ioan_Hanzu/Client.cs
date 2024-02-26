using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiectul_Remi_Ioan_Hanzu
{
    internal class Client
    {
        static void Main()
        {
            TcpClient tcpClient = new TcpClient();

            try
            {
                tcpClient.Connect("127.0.0.1", 12345);
                Console.WriteLine("Connected to server");

                NetworkStream clientStream = tcpClient.GetStream();

                // Send a message to the server
                string message = "Hello from client!";
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                clientStream.Write(messageBytes, 0, messageBytes.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                tcpClient.Close();
            }

            Console.ReadLine(); // Keep the console application running
        }
    }
}
