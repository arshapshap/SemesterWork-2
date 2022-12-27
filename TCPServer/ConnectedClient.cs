using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using XProtocol;
using XProtocol.Serializator;

namespace TCPServer
{
    internal class ConnectedClient
    {
        private XServer _server;
        public Socket Client { get; }

        public int Id { get; set; }

        private readonly Queue<byte[]> _packetSendingQueue = new Queue<byte[]>();

        public ConnectedClient(Socket client, XServer server)
        {
            Client = client;
            _server = server;

            Task.Run((Action) ProcessIncomingPackets);
            Task.Run((Action) SendPackets);
        }

        private void ProcessIncomingPackets()
        {
            while (true) // Слушаем пакеты, пока клиент не отключится.
            {
                var buff = new byte[256]; // Максимальный размер пакета - 256 байт.
                Client.Receive(buff);

                buff = buff.TakeWhile((b, i) =>
                {
                    if (b != 0xFF) return true;
                    return buff[i + 1] != 0;
                }).Concat(new byte[] {0xFF, 0}).ToArray();

                var parsed = XPacket.Parse(buff);

                if (parsed != null)
                {
                    ProcessIncomingPacket(parsed);
                }
            }
        }

        private void ProcessIncomingPacket(XPacket packet)
        {
            var type = XPacketTypeManager.GetTypeFromPacket(packet);

            switch (type)
            {
                case XPacketType.Handshake:
                    ProcessHandshake(packet);
                    break;
                case XPacketType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ProcessHandshake(XPacket packet)
        {
            Id = _server._clients.Count;

            var handshake = new XPacketHandshake()
            {
                Id = Id
            };

            QueuePacketSend(XPacketConverter
                .Serialize(XPacketType.Handshake, handshake).ToPacket());


            if (Id <= 10)
            {

                var newPlayer = new XPacketNewPlayer()
                {
                    Id = Id
                };
                foreach (var client in _server._clients)
                {
                    if (client.Id != Id)
                        QueuePacketSend(XPacketConverter.Serialize(XPacketType.NewPlayer, new XPacketNewPlayer() { Id = client.Id }).ToPacket());
                    client.QueuePacketSend(XPacketConverter.Serialize(XPacketType.NewPlayer, newPlayer).ToPacket());
                }
            }
        }

        public void QueuePacketSend(byte[] packet)
        {
            if (packet.Length > 256)
            {
                throw new Exception("Max packet size is 256 bytes.");
            }

            _packetSendingQueue.Enqueue(packet);
        }

        private void SendPackets()
        {
            while (true)
            {
                if (_packetSendingQueue.Count == 0)
                {
                    Thread.Sleep(100);
                    continue;
                }

                var packet = _packetSendingQueue.Dequeue();
                Client.Send(packet);

                Thread.Sleep(100);
            }
        }
    }
}
