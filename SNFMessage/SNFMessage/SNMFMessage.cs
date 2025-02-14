using SNFProtocol;
using System;

public class SNFMessage : ICodecMessage
{
    public byte SequenceNumber { get; set; }
    public byte Data { get; set; }  

    public byte[] Encode(){
        return new byte[] { SequenceNumber, Data }; 
    }
    public static SNFMessage Decode(byte[] data){
        if (data == null || data.Length != 2)
        {
            throw new ArgumentException("El mensaje debe tener exactamente 2 bytes (SequenceNumber + Data).");
        }
        return new SNFMessage{
            SequenceNumber = data[0],
            Data = data[1]
        };
    }

    
}



