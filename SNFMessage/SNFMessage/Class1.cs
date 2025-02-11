using System;

public class SNFMessage
{
    public byte SequenceNumber { get; set; }
    public byte? Data { get; set; }
    public bool IsAck { get; set; }

    public byte[] Encode()
    {
        if (IsAck)
        {
            return new byte[] { SequenceNumber, 0xFF };
        }
        return new byte[] { SequenceNumber, Data.Value };
    }

    public static SNFMessage Decode(byte[] data)
    {
        if (data.Length != 2)
        {
            throw new ArgumentException("Invalid message length");
        }
        return new SNFMessage
        {
            SequenceNumber = data[0],
            Data = data[1] == 0xFF ? (byte?)null : data[1],
            IsAck = data[1] == 0xFF
        };
    }
}

