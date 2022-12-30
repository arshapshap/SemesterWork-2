using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProtocol.Serializator;

namespace XProtocol.XPackets
{
    public class XPacketGameStart
    {
        [XField(1)]
        public int CurrentPlayerId;
        [XField(2)]
        public int CardOnTableType;
        [XField(3)]
        public int CardOnTableColor;
    }
}
