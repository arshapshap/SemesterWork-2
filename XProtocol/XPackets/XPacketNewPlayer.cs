﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProtocol.Serializator;

namespace XProtocol.XPackets
{
    public class XPacketNewPlayer
    {
        [XField(1)]
        public int Id;

        [XField(2)]
        public bool Ready;
    }
}
