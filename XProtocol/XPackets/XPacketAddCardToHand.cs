﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProtocol.Serializator;

namespace XProtocol.XPackets
{
    public class XPacketAddCardToHand
    {
        [XField(1)]
        public int CardType;
        [XField(2)]
        public int CardColor;
        [XField(3)]
        public int Reason;
    }
}
