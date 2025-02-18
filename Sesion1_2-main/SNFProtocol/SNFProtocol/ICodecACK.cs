using System;
using System.Collections.Generic;
using System.Text;

namespace SNFProtocol
{
    internal interface ICodecACK : ICodec
    {
        byte SequenceNumber { get; set; }
    }
}
