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
        public int CardOnTableType;
        [XField(2)]
        public int CardOnTableColor;
        [XField(3)]
        public int Card1Type;
        [XField(4)]
        public int Card1Color;
        [XField(5)]
        public int Card2Type;
        [XField(6)]
        public int Card2Color;
        [XField(7)]
        public int Card3Type;
        [XField(8)]
        public int Card3Color;
        [XField(9)]
        public int Card4Type;
        [XField(10)]
        public int Card4Color;
        [XField(11)]
        public int Card5Type;
        [XField(12)]
        public int Card5Color;
        [XField(13)]
        public int Card6Type;
        [XField(14)]
        public int Card6Color;
        [XField(15)]
        public int Card7Type;
        [XField(16)]
        public int Card7Color;
    }
}
