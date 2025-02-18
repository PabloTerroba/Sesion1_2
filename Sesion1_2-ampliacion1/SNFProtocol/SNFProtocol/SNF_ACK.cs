using System;
using System.IO;

namespace SNFProtocol
{
    public class SNF_ACK : ICodecACK  // Debe ser pública para utilizarse en otro proyecto
    {
        public byte SequenceNumber { get; set; }
        private const int ACK_SIZE = 1;
        public byte[] Encode()
        {
            return new byte[] { SequenceNumber };
        }

        public void Decode(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Data cannot be null.");
            }

            if (data.Length != ACK_SIZE)
            {
                throw new InvalidDataException($"Invalid ACK size. Expected {ACK_SIZE} byte, but received {data.Length}.");
            }

            SequenceNumber = data[0];
        }
    }
}
