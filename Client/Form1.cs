using XProtocol.Serializator;
using XProtocol;
using GameLogic;
using XProtocol.XPackets;
using GameLogic.Cards;

namespace Client
{
    public partial class Form1 : Form
    {
        string imagesPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\images";
        Player player;
        XClient client;

        public Form1()
        {
            InitializeComponent();
            try
            {
                ConnectClient();
            }
            catch
            {
                errorLabel.Text = "Не удалось подключиться к серверу";
                errorLabel.Visible = true;
            }

        }

        private void ConnectClient()
        {
            client = new XClient();
            client.OnPacketReceive += OnPacketRecieve;
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

        private void ProcessAddCardToHand(XPacket packet)
        {
            var cardPacket = XPacketConverter.Deserialize<XPacketAddCardToHand>(packet);
            var card = new Card((CardType)cardPacket.CardType, (CardColor)cardPacket.CardColor);

            BeginInvoke(new Action(() =>
            {
                AddCard(card);
            }));
        }

        private void ProcessUpdateCardOnTable(XPacket packet)
        {
            var cardPacket = XPacketConverter.Deserialize<XPacketUpdateCardOnTable>(packet);
            var card = new Card((CardType)cardPacket.CardType, (CardColor)cardPacket.CardColor);

            BeginInvoke(new Action(() =>
            {
                UpdateCardOnTable(card);
            }));
        }

        private void UpdateCardOnTable(Card card)
        {
            var cardFileName = $"{card.Color}_{(int)card.Type}";
            lastCardPicture.Image = Image.FromFile(imagesPath + @$"\{cardFileName}.png");
        }

        private void AddCard(Card card)
        {
            var cardFileName = $"{card.Color}_{(int)card.Type}";
            if (!cardsList.Images.ContainsKey(cardFileName))
                cardsList.Images.Add(cardFileName, Image.FromFile(imagesPath + @$"\{cardFileName}.png"));
            cardsListView.Items.Add("", cardFileName);
        }

        private void ProcessGameStart(XPacket packet)
        {
            BeginInvoke(new Action(() =>
            {
                readyGroup.Dispose();
                cardsListView.Visible = true;
                lastCardPicture.Visible = true;
                for (int i = 0; i < playersList.Items.Count; i++)
                    playersList.Items[i] = $"Player{i} [7 карт]";
            }));
        }

        private void ProcessPlayerReady(XPacket packet)
        {
            var player = XPacketConverter.Deserialize<XPacketPlayerReady>(packet);
            BeginInvoke(new Action(() =>
            {
                playersList.Items[player.Id - 1] = $"Player{player.Id} [Готов]";
            }));
        }

        private void ProcessHandshake(XPacket packet)
        {
            var handshake = XPacketConverter.Deserialize<XPacketHandshake>(packet);

            if (handshake.Id > 10)
                BeginInvoke(new Action(() =>
                {
                    errorLabel.Text = "Игра уже заполнена (максимум 10 игроков)";
                    errorLabel.Visible = true;
                }));
            else if (handshake.AlreadyStarted)
                BeginInvoke(new Action(() =>
                {
                    errorLabel.Text = "Игра уже началась";
                    errorLabel.Visible = true;
                }));
            else
            {
                player = new Player(handshake.Id);

                BeginInvoke(new Action(() =>
                {
                    Text = $"Player{player.Id}";
                    startScreen.Visible = true;
                    nameTextBox.Text = $"Player{player.Id}";
                }));

            }
        }

        private void ProcessNewPlayer(XPacket packet)
        {
            var newPlayer = XPacketConverter.Deserialize<XPacketNewPlayer>(packet);
            BeginInvoke(new Action(() =>
            {
                playersList.Items.Add($"Player{newPlayer.Id}{(newPlayer.Ready ? " [Готов]" : "")}");
            }));
        }

        private void readyButton_Click(object sender, EventArgs e)
        {
            client.QueuePacketSend(
                XPacketConverter.Serialize(
                    XPacketType.PlayerReady,
                    new XPacketPlayerReady())
                    .ToPacket());
            readyButton.Enabled = false;
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            //saveNameButton.Enabled = true;
            //saveNameButton.Visible = true;
        }

        private void saveNameButton_Click(object sender, EventArgs e)
        {
            //saveNameButton.Enabled = false;
            //saveNameButton.Visible = false;
        }
    }
}