using System;
using System.Collections.Generic;
using System.Text;

namespace SNFProtocol
{
    internal interface ICodecDATA : ICodec
    {
        byte Data { get; set; }
        byte SequenceNumber { get; set; }

    }
}
