using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SNFProtocol
{
    public class SNFClient
    {
        private const int PORT = 5000;
        private const string SERVER_IP = "127.0.0.1";
        private const double PACKET_LOSS_PROBABILITY = 0;
        private const string FILENAME = "example.txt";

        public void Run()
        {
            Console.WriteLine("SNF Client started...");

            UdpClient client = new UdpClient();
            client.Client.ReceiveTimeout = 5000;

            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(SERVER_IP), PORT);
            Random random = new Random();

            try
            {
                // Leer archivo
                byte[] fileData = File.ReadAllBytes(FILENAME);
                Console.WriteLine($"File '{FILENAME}' loaded, {fileData.Length} bytes.");

                // Enviar el contenido del archivo
                for (byte i = 0; i < fileData.Length; i++)
                {
                    SNF_DATA message = new SNF_DATA { SequenceNumber= i, Data = fileData[i] };
                    byte[] dataPacket = message.Encode();

                    // Enviar paquete
                    bool ackReceived = false;
                    while (!ackReceived)
                    {
                        if (random.NextDouble() < PACKET_LOSS_PROBABILITY)
                        {
                            Console.WriteLine($"[Simulation] Packet with Seq {i} lost.");
                        }
                        else
                        {
                            client.Send(dataPacket, dataPacket.Length, serverEP);
                            Console.WriteLine($"Message sent -> Seq: {message.SequenceNumber}, Data: {message.Data}");
                        }

                        try
                        {
                            // Recibir ACK
                            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, PORT);
                            byte[] ackData = client.Receive(ref remoteEP);
                            SNF_ACK ack = new SNF_ACK();
                            ack.Decode(ackData);

                            if (ack.SequenceNumber == i)
                            {
                                Console.WriteLine($"ACK received -> Seq: {ack.SequenceNumber}");
                                ackReceived = true;                            }
                        }
                        catch (SocketException ex) when (ex.SocketErrorCode == SocketError.TimedOut)
                        {
                            Console.WriteLine($"Timeout waiting for ACK for Seq {i}. Retrying...");
                        }
                    }
                }

                // Enviar mensaje de fin de archivo
                SNF_DATA end = new SNF_DATA { SequenceNumber = 255, Data = 0 };
                client.Send(end.Encode(), 2, serverEP);
                Console.WriteLine("[End] End message sent.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error in client: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }
    }
}
