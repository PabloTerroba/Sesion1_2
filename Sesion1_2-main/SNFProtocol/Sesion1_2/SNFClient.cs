using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SNFProtocol
{
    public class SNFClient
    {
        private const int PORT = 5000;
        private const string SERVER_IP = "127.0.0.1";
        private const double PACKET_LOSS_PROBABILITY = 0.3;


        public void Run()
{
            Console.WriteLine("SNF Client started...");

            UdpClient client = new UdpClient();
            client.Client.ReceiveTimeout = 5000;

            IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(SERVER_IP), PORT);
            Random random = new Random();

            try
            {
                for (byte i = 1; i <= 5; i++) // Enviar 5 mensajes de prueba
                {
                    SNF_DATA message = new SNF_DATA { SequenceNumber = i, Data = (byte)(i * 10) };
                    byte[] data = message.Encode();

                    bool ackReceived = false;
                    while (!ackReceived)
                    {
                        // Simulación de pérdida de paquetes con un 30% de probabilidad
                        if (i != 1 && random.NextDouble() < PACKET_LOSS_PROBABILITY)
                        {
                            Console.WriteLine($"[Simulation] Packet with Seq {i} lost.");

                        }
                        else
                        {
                            client.Send(data, data.Length, serverEP);
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
                                ackReceived = true;
                            }
                        }
                        catch (SocketException ex) when (ex.SocketErrorCode == SocketError.TimedOut)
                        {
                            Console.WriteLine($"Timeout waiting for ACK for Seq {i}. Retrying...");
                        }
                    }
                }

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