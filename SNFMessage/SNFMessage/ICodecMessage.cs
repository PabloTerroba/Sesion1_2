using System;
using System.Collections.Generic;
using System.Text;

namespace SNFProtocol
{
    internal interface ICodecMessage
    {
        byte Data{ get; set; }
        byte SequenceNumber { get; set; }

    }
}
