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
            if (_clients.Count > 1 && _clients.All(c => c.Player.Ready) && Game == null)
            {
                Game = new Game(_clients.Select(c => c.Player).ToArray());
                Game.Start();
                _clients.ForEach(c =>
                {
                    var gameStart = new XPacketGameStart()
                    {
                        CardOnTableType = (int)Game.LastCard.Type,
                        CardOnTableColor = (int)Game.LastCard.Color,
                        Card1Type = (int)c.Player.Cards[0].Type,
                        Card1Color = (int)c.Player.Cards[0].Color,
                        Card2Type = (int)c.Player.Cards[1].Type,
                        Card2Color = (int)c.Player.Cards[1].Color,
                        Card3Type = (int)c.Player.Cards[2].Type,
                        Card3Color = (int)c.Player.Cards[2].Color,
                        Card4Type = (int)c.Player.Cards[3].Type,
                        Card4Color = (int)c.Player.Cards[3].Color,
                        Card5Type = (int)c.Player.Cards[4].Type,
                        Card5Color = (int)c.Player.Cards[4].Color,
                        Card6Type = (int)c.Player.Cards[5].Type,
                        Card6Color = (int)c.Player.Cards[5].Color,
                        Card7Type = (int)c.Player.Cards[6].Type,
                        Card7Color = (int)c.Player.Cards[6].Color,
                    };
                    var currentPlayer = new XPacketCurrentPlayer()
                    {
                        Id = Game.CurrentPlayerId
                    };

                    c.QueuePacketSend(XProtocol.XPacketType.GameStart, gameStart);
                    c.QueuePacketSend(XProtocol.XPacketType.CurrentPlayer, currentPlayer);
                });
            }
        }
    }
}
