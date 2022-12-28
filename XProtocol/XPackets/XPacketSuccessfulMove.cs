using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProtocol.Serializator;

namespace XProtocol.XPackets
{
    public class XPacketSuccessfulMove
    {
        [XField(1)]
        public int PlayerId;
        [XField(2)]
        public int CardType;
        [XField(3)]
        public int CardColor;
        [XField(4)]
        public int SelectedColor; // if CardColor is Black
        [XField(5)]
        public int NextPlayerId;
    }
}
