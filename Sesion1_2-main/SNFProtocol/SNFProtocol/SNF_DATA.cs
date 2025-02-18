using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SNFProtocol
{
    public class SNF_DATA : ICodecDATA  // Debe ser pública para utilizarse en otro proyecto
    {
        public byte SequenceNumber { get; set; }
        public byte Data { get; set; }
        private const int MESSAGE_SIZE = 2;
        public byte[] Encode()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(SequenceNumber);
                    bw.Write(Data);
                }
                return ms.ToArray();
            }
        }

        public void Decode(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Data cannot be null.");
            }

            if (data.Length != MESSAGE_SIZE)
            {
                throw new InvalidDataException($"Invalid message size. Expected {MESSAGE_SIZE} bytes, but received {data.Length}.");
            }

            SequenceNumber = data[0];
            Data = data[1];
        }

    }
}
