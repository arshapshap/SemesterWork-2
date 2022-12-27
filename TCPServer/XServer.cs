using GameLogic;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
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

        internal void StartGameIfAllReady()
        {
            if (_clients.Count > 1 && _clients.All(c => c.Player.Ready))
            {
                Game = new Game(_clients.Select(c => c.Player).ToArray());
                Game.Start();
                Thread.Sleep(100);
                _clients.ForEach(c =>
                {
                    var gameStart = new XPacketGameStart();
                    var lastCard = new XPacketUpdateCardOnTable()
                    {
                        CardType = (int)Game.LastCard.Type,
                        CardColor = (int)Game.LastCard.Color,
                    };

                    c.QueuePacketSend(XProtocol.XPacketType.GameStart, gameStart);
                    c.QueuePacketSend(XProtocol.XPacketType.UpdateCardOnTable, lastCard);


                    c.Player.Cards
                        .Select(card => new XPacketAddCardToHand() { CardType = (int)card.Type, CardColor = (int)card.Color })
                        .ToList()
                        .ForEach(packet =>
                        {
                            c.QueuePacketSend(XProtocol.XPacketType.AddCardToHand, packet);
                        });
                });
            }
        }
    }
}
