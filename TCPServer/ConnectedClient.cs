using GameLogic;
using GameLogic.Cards;
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
                case XPacketType.UpdateCardOnTable:
                    ProcessUpdateCardOnTable(packet);
                    break;
                case XPacketType.AddCardToHand:
                    ProcessAddCardToHand(packet);
                    break;
                case XPacketType.Uno:
                    ProcessUno(packet);
                    break;
                case XPacketType.PlayerDidntSayUno:
                    ProcessPlayerDidntSayUno(packet);
                    break;
                case XPacketType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ProcessPlayerDidntSayUno(XPacket packet)
        {
            var playerPacket = XPacketConverter.Deserialize<XPacketPlayerDidntSayUno>(packet);
            var playerId = playerPacket.PlayerId;
            if (!_server.Game.Players[playerId - 1].Uno)
                _server.UnoPenalty(playerId);
        }

        private void ProcessUno(XPacket packet)
        {
            Player.Uno = true;
            _server.SendToAllClients(XPacketType.Uno, new XPacketUno() { PlayerId = Player.Id });
        }

        private void ProcessAddCardToHand(XPacket packet)
        {
            if (_server.Game != null && _server.Game.TryTakeCardFromDeck(Player, out Card card)) 
            {
                var addCard = new XPacketAddCardToHand()
                {
                    CardType = (int)card.Type,
                    CardColor = (int)card.Color,
                };
                QueuePacketSend(XPacketType.AddCardToHand, addCard);

                var changeCardsCount = new XPacketChangeCardsCount()
                {
                    PlayerId = Player.Id,
                    CardsCount = Player.Cards.Count
                };

                _server.SendToAllClients(XPacketType.ChangeCardsCount, changeCardsCount);

                if (!_server.Game.CheckMoveCorrect(card))
                {
                    var skipMove = new XPacketSkipMove()
                    {
                        NextPlayerId = _server.Game.NextPlayer()
                    };
                    _server.SendToAllClients(XPacketType.SkipMove, skipMove);
                }
            }
        }

        private void ProcessUpdateCardOnTable(XPacket packet)
        {
            var cardPacket = XPacketConverter.Deserialize<XPacketUpdateCardOnTable>(packet);
            var card = new Card((CardType)cardPacket.CardType, (CardColor)cardPacket.CardColor);
            if (_server.Game != null && _server.Game.TryMove(Player, card, out Dictionary<int, Card[]> newCards, (CardColor)cardPacket.SelectedColor))
            {
                var successfulMove = new XPacketSuccessfulMove()
                {
                    PlayerId = Player.Id,
                    CardType = cardPacket.CardType,
                    CardColor = cardPacket.CardColor,
                    NextPlayerId = _server.Game.CurrentPlayerId,
                    SelectedColor = cardPacket.SelectedColor,
                };

                _server.UpdateCardOnTable(successfulMove, newCards);
            }
        }

        private void ProcessPlayerReady(XPacket packet)
        {
            Player.Ready = true;

            var playerReady = new XPacketPlayerReady()
            {
                Id = Player.Id
            };

            _server.SendToAllClients(XPacketType.PlayerReady, playerReady);
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
