using System;
using System.Net;
using System.Net.Sockets;

class SNFServer
{
    static void Main(string[] args)
    {
        UdpClient udpServer = new UdpClient(11000);
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        byte expectedSequence = 0;

        Console.WriteLine("Server started. Waiting for data...");

        while (true)
        {
            byte[] receivedData = udpServer.Receive(ref remoteEP);
            SNFMessage message = SNFMessage.Decode(receivedData);

            if (message.SequenceNumber == expectedSequence)
            {
                Console.WriteLine($"Received: {message.Data}");
                SNFMessage ack = new SNFMessage { SequenceNumber = expectedSequence, IsAck = true };
                udpServer.Send(ack.Encode(), ack.Encode().Length, remoteEP);
                expectedSequence++;
            }
            else
            {
                Console.WriteLine($"Discarded out-of-sequence packet: {message.SequenceNumber}");
            }
        }
    }
}


