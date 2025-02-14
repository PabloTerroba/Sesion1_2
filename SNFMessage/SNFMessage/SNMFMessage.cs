using SNFProtocol;
using System;
using System.IO;
using System.Linq.Expressions;

public class SNFMessage : ICodecMessage
{
    public byte SequenceNumber { get; set; }
    public byte Data { get; set; }  

    public byte[] Encode(){
        using (MemoryStream ms = new MemoryStream()) {
            using (BinaryWritter bw = new BinaryWritter)
            {

            }


        }
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



