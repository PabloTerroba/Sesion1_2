using System;
using System.Collections.Generic;
using System.Text;

namespace SNFProtocol
{
    internal interface ICodecAck : ICodec
    {
        byte SequenceNumber { get; set; }
    }
}
