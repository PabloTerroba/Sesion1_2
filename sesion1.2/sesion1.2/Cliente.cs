using System;
using System.Net;
using System.Net.Sockets;

class SNFClient
{
    static void Main(string[] args)
    {
        UdpClient udpClient = new UdpClient();
        IPEndPoint serverEP = new IPEndPoint(IPAddress.Loopback, 11000);
        byte sequenceNumber = 0;
        byte[] dataToSend = new byte[] { 1, 2, 3, 4, 5 };

        foreach (byte data in dataToSend)
        {
            bool ackReceived = false;
            // Intentar enviar hasta recibir el ACK correcto
            while (!ackReceived){
                // Enviar el paquete
                SNFMessage message = new SNFMessage { SequenceNumber = sequenceNumber, Data = data };
                udpClient.Send(message.Encode(), message.Encode().Length, serverEP);
                Console.WriteLine($"Sent: {data}");

                udpClient.Client.ReceiveTimeout = 1000; // 1 second timeout

                try{
                    // Esperar por el ACK
                    byte[] receivedData = udpClient.Receive(ref serverEP);
                    SNFMessage ack = SNFMessage.Decode(receivedData);

                    // Verificar que el ACK es el correcto
                    if (ack.SequenceNumber == sequenceNumber)
                    {
                        ackReceived = true;  // El ACK recibido es correcto
                        sequenceNumber++;    // Ahora avanzamos al siguiente byte
                    }
                }catch (SocketException){
                    // Timeout, volver a enviar el mismo paquete
                    Console.WriteLine("Timeout, resending...");
                }
            }
        }
        Console.WriteLine("All data sent successfully.");
    } 
}


