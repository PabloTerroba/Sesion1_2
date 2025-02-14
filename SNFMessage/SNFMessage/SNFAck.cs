using System;
using System.Collections.Generic;
using System.Text;

namespace SNFProtocol
{
    internal class SNFAck : ICodecAck
    {
        public byte SequenceNumber { get; set; }

        public byte[] Encode()
        {
            return new byte[] { SequenceNumber };
        }
        public void Decode(byte[] data) {
            if (data == null || data.Length != 1)
            {
                throw new ArgumentException("El ack debe tener exactamente 1 byte. ");
            }
            else
            {
                SequenceNumber = data[0];
            }
        }
    }
}
