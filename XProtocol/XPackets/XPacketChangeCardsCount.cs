using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProtocol.Serializator;

namespace XProtocol.XPackets
{
    public class XPacketChangeCardsCount
    {
        [XField(1)]
        public int PlayerId;
        [XField(2)]
        public int CardsCount;
    }
}
