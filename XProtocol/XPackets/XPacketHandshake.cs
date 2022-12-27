using XProtocol.Serializator;

namespace XProtocol.XPackets
{
    public class XPacketHandshake
    {
        [XField(1)]
        public int Id;

        [XField(2)]
        public bool AlreadyStarted;
    }
}