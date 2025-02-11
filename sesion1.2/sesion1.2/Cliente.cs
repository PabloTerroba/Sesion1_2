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
            while (!ackReceived)
            {
                SNFMessage message = new SNFMessage { SequenceNumber = sequenceNumber, Data = data };
                udpClient.Send(message.Encode(), message.Encode().Length, serverEP);
                Console.WriteLine($"Sent: {data}");

                udpClient.Client.ReceiveTimeout = 1000; // 1 second timeout
                try
                {
                    byte[] receivedData = udpClient.Receive(ref serverEP);
                    SNFMessage ack = SNFMessage.Decode(receivedData);
                    if (ack.IsAck && ack.SequenceNumber == sequenceNumber)
                    {
                        ackReceived = true;
                        sequenceNumber++;
                    }
                }
                catch (SocketException)
                {
                    Console.WriteLine("Timeout, resending...");
                }
            }
        }

        Console.WriteLine("All data sent successfully.");
    }
}


