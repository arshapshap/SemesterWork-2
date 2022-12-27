using GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using XProtocol;
using XProtocol.Serializator;
using XProtocol.XPackets;

namespace TCPServer
{
    internal class ConnectedClient
    {
        private XServer _server;

        public Player Player { get; private set; }
        public Socket Client { get; }


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
            Console.WriteLine($"Recieved {type}Packet from {(IPEndPoint)Client.RemoteEndPoint}");

            switch (type)
            {
                case XPacketType.Handshake:
                    ProcessHandshake(packet);
                    break;
                case XPacketType.PlayerReady:
                    ProcessPlayerReady(packet);
                    break;
                case XPacketType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ProcessPlayerReady(XPacket packet)
        {
            Player.Ready = true;

            foreach (var client in _server._clients)
            {
                var playerReady = new XPacketPlayerReady()
                {
                    Id = Player.Id
                };
                client.QueuePacketSend(XPacketType.PlayerReady, playerReady);
            }

            _server.StartGameIfAllReady();
        }

        private void ProcessHandshake(XPacket packet)
        {
            Player = new Player(_server._clients.Count);

            var handshake = new XPacketHandshake()
            {
                Id = Player.Id,
                AlreadyStarted = _server.Game != null
            };

            QueuePacketSend(XPacketType.Handshake, handshake);


            if (Player.Id <= 10 && _server.Game == null)
            {
                var newPlayer = new XPacketNewPlayer()
                {
                    Id = Player.Id
                };
                foreach (var client in _server._clients)
                {
                    if (client.Player.Id != Player.Id)
                        QueuePacketSend(XPacketType.NewPlayer, new XPacketNewPlayer() { 
                            Id = client.Player.Id, 
                            Ready = client.Player.Ready 
                        });
                    client.QueuePacketSend(XPacketType.NewPlayer, newPlayer);
                }
            }
        }

        internal void QueuePacketSend(XPacketType packetType, object packet)
        {
            var bytes = XPacketConverter.Serialize(packetType, packet).ToPacket();
            Console.WriteLine($"Sent {packetType}Packet to {(IPEndPoint)Client.RemoteEndPoint}");

            if (bytes.Length > 256)
            {
                throw new Exception("Max packet size is 256 bytes.");
            }

            _packetSendingQueue.Enqueue(bytes);
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
