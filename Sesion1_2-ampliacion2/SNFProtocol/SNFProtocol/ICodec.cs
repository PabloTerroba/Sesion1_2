using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace SNFProtocol
{
    public interface ICodec
    {
        byte[] Encode();
        void Decode(byte[] data);
    }
}
