using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace SNFProtocol
{
    public class SNFServer
    {
        private const int PORT = 5000;
        private const string RECEIVED_FILENAME = "received_file.txt";

        public void Run()
        {
            Console.WriteLine("SNF Server started...");

            UdpClient server = new UdpClient(PORT);
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, PORT);

            FileStream fs = new FileStream(RECEIVED_FILENAME, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(fs);

            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for a message...");
                    byte[] data = server.Receive(ref remoteEP);

                    if (data.Length == 2) // Mensaje de 2 bytes: SequenceNumber + Data
                    {
                        SNF_DATA message = new SNF_DATA();
                        message.Decode(data);

                        if (message.IsEnd())
                        {
                            Console.WriteLine("[End] End message received. Shutting down server...");
                            break; // Salir del bucle y terminar la ejecución
                        }

                        writer.Write(data[1]);
                        Console.WriteLine($"Message received -> Seq: {message.SequenceNumber}, Data: {message.Data}");

                        // Enviar ACK con el mismo número de secuencia
                        SNF_ACK ack = new SNF_ACK { SequenceNumber = message.SequenceNumber };
                        byte[] ackData = ack.Encode();
                        server.Send(ackData, ackData.Length, remoteEP);
                        Console.WriteLine($"ACK sent -> Seq: {ack.SequenceNumber}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error in server: {ex.Message}");
            }
            finally
            {
                writer.Close();
                fs.Close();
                server.Close();
            }
        }
    }
}
