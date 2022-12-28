using GameLogic.Cards;
using GameLogic;
using System.Net;
using System.Net.Sockets;
using static System.Net.Mime.MediaTypeNames;
using XProtocol.Serializator;
using XProtocol.XPackets;
using XProtocol;
using System.Numerics;

namespace Client;

public class XClient
{
    public Action<byte[]> OnPacketReceive { get; set; }

    private readonly Queue<byte[]> _packetSendingQueue = new Queue<byte[]>();
    public MainForm Form { get; private set; }
    private Socket _socket;
    private IPEndPoint _serverEndPoint;
    
    public void Connect(string ip, int port)
    {
        Connect(new IPEndPoint(IPAddress.Parse(ip), port));
    }

    public void Connect(IPEndPoint server)
    {
        _serverEndPoint = server;

        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _socket.Connect(_serverEndPoint);

        Task.Run((Action) RecievePackets);
        Task.Run((Action) SendPackets);
    }

    public void QueuePacketSend(byte[] packet)
    {
        if (packet.Length > 256)
        {
            throw new Exception("Max packet size is 256 bytes.");
        }

        _packetSendingQueue.Enqueue(packet);
    }

    private void RecievePackets()
    {
        while (true)
        {
            var buff = new byte[256];
            _socket.Receive(buff);

            buff = buff.TakeWhile((b, i) =>
            {
                if (b != 0xFF) return true;
                return buff[i + 1] != 0;
            }).Concat(new byte[] {0xFF, 0}).ToArray();

            OnPacketReceive?.Invoke(buff);
        }
    }

    internal static XClient ConnectClient(MainForm form)
    {
        var client = new XClient();
        client.OnPacketReceive += client.OnPacketRecieve;
        client.Form = form;
        client.Connect("127.0.0.1", 4910);

        Console.WriteLine("Sending handshake packet..");

        client.QueuePacketSend(
            XPacketConverter.Serialize(
                XPacketType.Handshake,
                new XPacketHandshake
                {
                    Id = -1
                })
                .ToPacket());

        return client;
    }

    private void OnPacketRecieve(byte[] packet)
    {
        var parsed = XPacket.Parse(packet);

        if (parsed != null)
        {
            ProcessIncomingPacket(parsed);
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
            case XPacketType.NewPlayer:
                ProcessNewPlayer(packet);
                break;
            case XPacketType.PlayerReady:
                ProcessPlayerReady(packet);
                break;
            case XPacketType.GameStart:
                ProcessGameStart(packet);
                break;
            case XPacketType.CurrentPlayer:
                ProcessCurrentPlayer(packet);
                break;
            case XPacketType.UpdateCardOnTable:
                ProcessUpdateCardOnTable(packet);
                break;
            case XPacketType.AddCardToHand:
                ProcessAddCardToHand(packet);
                break;
            case XPacketType.Unknown:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ProcessCurrentPlayer(XPacket packet)
    {
        var currentPlayer = XPacketConverter.Deserialize<XPacketCurrentPlayer>(packet);

        Form.BeginInvoke(new Action(() =>
        {
            Form.ChangeCurrentPlayer(currentPlayer.Id);
        }));
    }

    private void ProcessAddCardToHand(XPacket packet)
    {
        var cardPacket = XPacketConverter.Deserialize<XPacketAddCardToHand>(packet);
        var card = new Card((CardType)cardPacket.CardType, (CardColor)cardPacket.CardColor);

        Form.BeginInvoke(new Action(() =>
        {
            Form.AddCard(card);
        }));
    }

    private void ProcessUpdateCardOnTable(XPacket packet)
    {
        var cardPacket = XPacketConverter.Deserialize<XPacketUpdateCardOnTable>(packet);
        var card = new Card((CardType)cardPacket.CardType, (CardColor)cardPacket.CardColor);

        Form.BeginInvoke(new Action(() =>
        {
            Form.UpdateCardOnTable(card);
        }));
    }

    private void ProcessGameStart(XPacket packet)
    {
        var gameStart = XPacketConverter.Deserialize<XPacketGameStart>(packet);
        var lastCard = new Card((CardType)gameStart.CardOnTableType, (CardColor)gameStart.CardOnTableColor);
        var cards = new[]
        {
                new Card((CardType)gameStart.Card1Type, (CardColor)gameStart.Card1Color),
                new Card((CardType)gameStart.Card2Type, (CardColor)gameStart.Card2Color),
                new Card((CardType)gameStart.Card3Type, (CardColor)gameStart.Card3Color),
                new Card((CardType)gameStart.Card4Type, (CardColor)gameStart.Card4Color),
                new Card((CardType)gameStart.Card5Type, (CardColor)gameStart.Card5Color),
                new Card((CardType)gameStart.Card6Type, (CardColor)gameStart.Card6Color),
                new Card((CardType)gameStart.Card7Type, (CardColor)gameStart.Card7Color),
            };


        Form.BeginInvoke(new Action(() =>
        {
            Form.GameStart(lastCard, cards);
        }));
    }

    private void ProcessPlayerReady(XPacket packet)
    {
        var player = XPacketConverter.Deserialize<XPacketPlayerReady>(packet);
        Form.BeginInvoke(new Action(() =>
        {
            Form.PlayerReady(player.Id);
        }));
    }

    private void ProcessHandshake(XPacket packet)
    {
        var handshake = XPacketConverter.Deserialize<XPacketHandshake>(packet);

        Form.BeginInvoke(new Action(() =>
        {
            Form.Handshake(handshake.Id, handshake.AlreadyStarted);
        }));
    }

    private void ProcessNewPlayer(XPacket packet)
    {
        var newPlayer = XPacketConverter.Deserialize<XPacketNewPlayer>(packet);
        Form.BeginInvoke(new Action(() =>
        {
            Form.NewPlayer(newPlayer.Id, newPlayer.Ready);
        }));
    }

    private async void SendPackets()
    {
        while (true)
        {
            if (_packetSendingQueue.Count == 0)
            {
                Thread.Sleep(100);
                continue;
            }

            var packet = _packetSendingQueue.Dequeue();
            _socket.Send(packet);

            Thread.Sleep(100);
        }
    }
}
