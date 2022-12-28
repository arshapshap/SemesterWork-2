using XProtocol.Serializator;
using XProtocol;
using GameLogic;
using XProtocol.XPackets;
using GameLogic.Cards;
using System.Numerics;

namespace Client
{
    public partial class MainForm : Form
    {
        string imagesPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"\images";
        public Player Player { get; internal set; }
        XClient client;
        int currentPlayerId = -1;

        public MainForm()
        {
            InitializeComponent();
            try
            {
                client = XClient.ConnectClient(this);
            }
            catch
            {
                errorLabel.Text = "Не удалось подключиться к серверу";
                errorLabel.Visible = true;
            }

        }

        internal void UpdateCardOnTable(Card card)
        {
            var cardFileName = $"{card.Color}_{(int)card.Type}";
            lastCardPicture.Image = Image.FromFile(imagesPath + @$"\{cardFileName}.png");
        }

        internal void AddCard(Card card)
        {
            var cardFileName = $"{card.Color}_{(int)card.Type}";
            if (!cardsList.Images.ContainsKey(cardFileName))
                cardsList.Images.Add(cardFileName, Image.FromFile(imagesPath + @$"\{cardFileName}.png"));
            cardsListView.Items.Add("", cardFileName);
        }

        internal void GameStart(Card lastCard, Card[] cards)
        {
            readyGroup.Dispose();
            cardsListView.Visible = true;
            lastCardPicture.Visible = true;
            deckPicture.Visible = true;

            for (int id = 1; id <= playersList.Items.Count; id++)
                playersList.Items[id - 1] = $"Player{id} [7 карт]";

            foreach (var card in cards)
                AddCard(card);
            UpdateCardOnTable(lastCard);
        }

        internal void PlayerReady(int id)
        {
            playersList.Items[id - 1] = $"Player{id} [Готов]";
        }

        internal void Handshake(int id, bool alreadyStarted)
        {
            if (id > 10)
            {
                errorLabel.Text = "Игра уже заполнена (максимум 10 игроков)";
                errorLabel.Visible = true;
            }
            else if (alreadyStarted)
            {
                errorLabel.Text = "Игра уже началась";
                errorLabel.Visible = true;
            }
            else
            {
                Player = new Player(id);

                Text = $"Player{Player.Id}";
                startScreen.Visible = true;
                nameTextBox.Text = $"Player{Player.Id}";
            }
        }

        internal void NewPlayer(int id, bool ready)
        {
            playersList.Items.Add($"Player{id}{(ready ? " [Готов]" : "")}");
        }

        internal void ChangeCurrentPlayer(int id)
        {
            if (currentPlayerId != -1)
                playersList.Items[id - 1] = playersList.Items[id].ToString().Replace(">>> ", "");

            currentPlayerId = id;
            playersList.Items[id - 1] = ">>> " + playersList.Items[id - 1];
            yourMoveLabel.Visible = id == Player.Id;
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