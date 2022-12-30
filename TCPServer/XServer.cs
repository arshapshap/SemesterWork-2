﻿using GameLogic;
using GameLogic.Cards;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using XProtocol;
using XProtocol.Serializator;
using XProtocol.XPackets;

namespace TCPServer
{
    internal class XServer
    {
        private readonly Socket _socket;
        internal readonly List<ConnectedClient> _clients;

        public Game Game { get; private set; }
        private bool _listening;
        private bool _stopListening;

        public XServer()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clients = new List<ConnectedClient>();
        }

        public void Start()
        {
            if (_listening)
            {
                throw new Exception("Server is already listening incoming requests.");
            }

            Console.WriteLine("[!] Server is listening incoming requests.");

            _socket.Bind(new IPEndPoint(IPAddress.Any, 4910));
            _socket.Listen(10);

            _listening = true;
        }

        public void Stop()
        {
            if (!_listening)
            {
                throw new Exception("Server is already not listening incoming requests.");
            }

            _stopListening = true;
            _socket.Shutdown(SocketShutdown.Both);
            _listening = false;
        }

        public void AcceptClients()
        {
            while (true)
            {
                if (_stopListening)
                {
                    return;
                }

                Socket client;

                try
                {
                    client = _socket.Accept();
                } catch { return; }

                Console.WriteLine($"[!] Accepted client from {(IPEndPoint) client.RemoteEndPoint}");

                var c = new ConnectedClient(client, this);
                _clients.Add(c);
            }
        }

        #region Методы для работы с игровой логикой

        internal void StartGameIfAllReady()
        {
            if (_clients.Count > 1 && _clients.All(c => c.Player.Ready) && Game == null)
            {
                Game = new Game(_clients.Select(c => c.Player).ToArray());
                Game.Start();
                foreach (var client in _clients)
                {
                    var gameStart = new XPacketGameStart()
                    {
                        CurrentPlayerId = Game.CurrentPlayerId,
                        CardOnTableType = (int)Game.CardOnTable.Type,
                        CardOnTableColor = (int)Game.CardOnTable.Color,
                    };

                    client.QueuePacketSend(XPacketType.GameStart, gameStart);

                    foreach (var card in client.Player.Cards)
                    {
                        var addCard = new XPacketAddCardToHand()
                        {
                            CardType = (int)card.Type,
                            CardColor = (int)card.Color,
                        };
                        client.QueuePacketSend(XPacketType.AddCardToHand, addCard);
                    }
                };
            }
        }

        internal void UnoPenalty(int playerId)
        {
            var newCards = Game.UnoPenalty(playerId);
            var changeCardsCount = new XPacketChangeCardsCount()
            {
                PlayerId = playerId,
                CardsCount = Game.Players[playerId - 1].Cards.Count
            };
            foreach (var client in _clients)
            {
                if (client.Player.Id == playerId)
                {
                    client.QueuePacketSend(XPacketType.PlayerDidntSayUno, new XPacketPlayerDidntSayUno());
                    foreach (var card in newCards)
                    {
                        var addCard = new XPacketAddCardToHand()
                        {
                            CardType = (int)card.Type,
                            CardColor = (int)card.Color,
                        };
                        client.QueuePacketSend(XPacketType.AddCardToHand, addCard);
                    }
                }
                client.QueuePacketSend(XPacketType.ChangeCardsCount, changeCardsCount);
            }
        }

        internal void UpdateCardOnTable(XPacketSuccessfulMove successfulMove, Dictionary<int, Card[]> newCards)
        {
            foreach (var client in _clients)
            {
                client.QueuePacketSend(XPacketType.SuccessfulMove, successfulMove);

                if (Game.IsOver == true)
                    client.QueuePacketSend(XPacketType.GameOver, new XPacketGameOver() { WinnerId = Game.WinnerId });

                if (newCards.ContainsKey(client.Player.Id))
                {
                    foreach (var newCard in newCards[client.Player.Id])
                    {
                        var addCard = new XPacketAddCardToHand()
                        {
                            CardType = (int)newCard.Type,
                            CardColor = (int)newCard.Color,
                        };
                        client.QueuePacketSend(XPacketType.AddCardToHand, addCard);
                    }
                }

                foreach (var playerId in newCards.Keys)
                {
                    var changeCardsCount = new XPacketChangeCardsCount()
                    {
                        PlayerId = playerId,
                        CardsCount = Game.GetPlayerCardsCount(playerId)
                    };
                    client.QueuePacketSend(XPacketType.ChangeCardsCount, changeCardsCount);
                }
            }
        }

        #endregion

        internal void SendToAllClients(XPacketType packetType, object packet)
        {
            foreach (var client in _clients)
                client.QueuePacketSend(packetType, packet);
        }
    }
}
