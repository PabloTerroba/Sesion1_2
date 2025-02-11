using System;

public class SNFMessage
{
    public byte SequenceNumber { get; set; }
    public byte? Data { get; set; }  // Puede ser nulo si es un ACK

    // Codificación de un mensaje normal (incluye Data)
    public byte[] Encode(){
        if (Data.HasValue){
            return new byte[] { SequenceNumber, Data.Value };
        }
        else{
            throw new InvalidOperationException("No se puede codificar un mensaje sin datos. Usa EncodeACK para ACKs.");
        }
    }

    // Codificación de un ACK (solo incluye SequenceNumber)
    public byte[] EncodeACK(){
        return new byte[] { SequenceNumber };
    }

    // Decodificación de un mensaje normal (espera 2 bytes)
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

    // Decodificación de un ACK (espera 1 byte)
    public static SNFMessage DecodeACK(byte[] data){
        if (data == null || data.Length != 1) {
            throw new ArgumentException("El ACK debe tener exactamente 1 byte (SequenceNumber).");
        }
        return new SNFMessage{
            SequenceNumber = data[0],
            Data = null  // Los ACK no tienen datos asociados
        };
    }
}



