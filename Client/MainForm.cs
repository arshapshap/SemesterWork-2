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
        internal int[] PlayersCardsCount { get; private set; }
        internal bool[] PlayersUno { get; private set; }

        internal bool FirstMove { get; set; } = true;
        XClient client;
        Card cardOnTable;
        CardColor? selectedCardOnTableColor;
        int currentPlayerId = -1;
        Card selectedCard;
        bool IsMoveAvailable = true;

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

        internal void UpdateCardOnTable(Card card, CardColor? selectedColor = null)
        {
            selectedCardOnTableColor = selectedColor;

            cardOnTable = card;
            var cardFileName = Card.ToString(card);
            cardOnTablePicture.Image = Image.FromFile(imagesPath + @$"\{cardFileName}.png");

            ChangeSelectedColor(selectedColor);
            HideSelectColorBox();
        }

        private void CheckMoveAvailable()
        {
            IsMoveAvailable = Player.Cards.Any(c => Game.CheckMoveCorrect(cardOnTable, c, selectedCardOnTableColor));
            if (!IsMoveAvailable)
                deckPicture.Cursor = Cursors.Hand;
            else
                deckPicture.Cursor = Cursors.No;
        }

        private void ChangeSelectedColor(CardColor? selectedColor)
        {
            if (selectedColor == null)
                selectedColorPicture.Visible = false;
            else
            {
                selectedColorPicture.Visible = true;
                selectedColorPicture.BringToFront();
                switch (selectedColor)
                {
                    case CardColor.Red:
                        selectedColorPicture.BackColor = RedColor;
                        break;
                    case CardColor.Yellow:
                        selectedColorPicture.BackColor = YellowColor;
                        break;
                    case CardColor.Green:
                        selectedColorPicture.BackColor = GreenColor;
                        break;
                    case CardColor.Blue:
                        selectedColorPicture.BackColor = BlueColor;
                        break;
                }
            }
            
        }

        internal void RemoveCardFromHand(Card card)
        {
            Player.TakeCard(card);
            SortCardsInHand();
            DecreaseCardsNumber(Player.Id);

            unoButton.Visible = Player.Cards.Count == 1;
        }

        internal void DecreaseCardsNumber(int playerId)
        {
            PlayersCardsCount[playerId - 1]--;
            UpdatePlayerInfo(playerId);
        }

        internal void UpdatePlayerInfo(int playerId)
        {
            var playerInfo = $"{((currentPlayerId == playerId) ? ">>> " : "")}Player{playerId}";
            playerInfo += PlayersUno[playerId - 1] ? $" [УНО]" : $" [{PlayersCardsCount[playerId - 1]} карт]";
            playersList.Items[playerId - 1] = playerInfo;
        }

        internal void Uno(int playerId)
        {
            PlayersUno[playerId - 1] = true;
            UpdatePlayerInfo(playerId);
        }

        internal void ChangeCardsCount(int playerId, int cardsCount)
        {
            PlayersCardsCount[playerId - 1] = cardsCount;
            PlayersUno[playerId - 1] = false;
            UpdatePlayerInfo(playerId);
        }

        internal void AddCardToHand(Card card)
        {
            Player.AddCard(card);
            SortCardsInHand();

            unoButton.Visible = Player.Cards.Count == 1;
        }

        internal void SortCardsInHand()
        {
            cardsListView.Clear();
            foreach (var card in Player.Cards)
            {
                var cardFileName = Card.ToString(card);
                if (!cardsList.Images.ContainsKey(cardFileName))
                    cardsList.Images.Add(cardFileName, Image.FromFile(imagesPath + @$"\{cardFileName}.png"));
                cardsListView.Items.Add(cardFileName, "", cardFileName);
            }
        }

        internal void GameStart(Card lastCard, Card[] cards)
        {
            readyGroupBox.Dispose();
            rulesLabel.Dispose();
            cardsListView.Visible = true;
            cardOnTablePicture.Visible = true;
            deckPicture.Visible = true;

            PlayersCardsCount = new int[playersList.Items.Count];
            PlayersUno = new bool[playersList.Items.Count];
            for (int id = 1; id <= playersList.Items.Count; id++)
            {
                PlayersCardsCount[id - 1] = 7;
                UpdatePlayerInfo(id);
            }    

            foreach (var card in cards)
                AddCardToHand(card);
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
                mainGroupBox.Visible = true;
                nameTextBox.Text = $"Player{Player.Id}";
            }
        }

        internal void NewPlayer(int id, bool ready)
        {
            playersList.Items.Add($"Player{id}{(ready ? " [Готов]" : "")}");
        }

        internal void ChangeCurrentPlayer(int id)
        {
            var previousPlayer = currentPlayerId;
            currentPlayerId = id;

            if (previousPlayer != -1)
                UpdatePlayerInfo(previousPlayer);
            UpdatePlayerInfo(id);
            yourMoveLabel.Visible = id == Player.Id;

            if (currentPlayerId == Player.Id)
                CheckMoveAvailable();
        }

        private void readyButton_Click(object sender, EventArgs e)
        {
            client.QueuePacketSend(XPacketType.PlayerReady,
                    new XPacketPlayerReady());
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

        private void unoButton_Click(object sender, EventArgs e)
        {
            client.QueuePacketSend(XPacketType.Uno, new XPacketUno());
            unoButton.Visible = false;
        }

        private void playersList_DoubleClick(object sender, EventArgs e)
        {
            if (currentPlayerId >= 0 && playersList.SelectedItems.Count >= 1)
            {
                var playerId = playersList.SelectedIndex + 1;
                if (PlayersCardsCount[playerId - 1] == 1)
                    client.QueuePacketSend(XPacketType.PlayerDidntSayUno, new XPacketPlayerDidntSayUno() { PlayerId = playerId });
            }
        }

        private void cardsListView_DoubleClick(object sender, EventArgs e)
        {
            if (cardsListView.SelectedItems.Count >= 1)
            {
                ListViewItem item = cardsListView.SelectedItems[0];
                var card = Card.FromString(item.ImageKey);

                var isMoveAvailable = (currentPlayerId == Player.Id && Game.CheckMoveCorrect(cardOnTable, card, selectedCardOnTableColor)
                    || Game.CheckMoveInterception(cardOnTable, card, selectedCardOnTableColor) && !FirstMove);

                if (isMoveAvailable && card.Color != CardColor.Black)
                    client.QueuePacketSend(XPacketType.UpdateCardOnTable, new XPacketUpdateCardOnTable()
                    {
                        CardType = (int)card.Type,
                        CardColor = (int)card.Color
                    });
                else if (isMoveAvailable && card.Color == CardColor.Black)
                {
                    selectedCard = card;

                    mainGroupBox.Enabled = false;
                    selectColorBox.Visible = true;
                    selectColorBox.Enabled = true;
                    selectColorBox.BringToFront();
                }
            }
        }

        private void main_Click(object sender, EventArgs e)
        {
            HideSelectColorBox();
        }

        private void deckPicture_Click(object sender, EventArgs e)
        {
            if (currentPlayerId == Player.Id && !IsMoveAvailable)
            {
                deckPicture.Cursor = Cursors.No;
                client.QueuePacketSend(XPacketType.AddCardToHand, new XPacketAddCardToHand());
            }
        }

        private void redButton_Click(object sender, EventArgs e)
        {
            client.QueuePacketSend(XPacketType.UpdateCardOnTable, new XPacketUpdateCardOnTable()
            {
                CardType = (int)selectedCard.Type,
                CardColor = (int)selectedCard.Color,
                SelectedColor = (int)CardColor.Red
            });
            HideSelectColorBox();
        }

        private void yellowButton_Click(object sender, EventArgs e)
        {
            client.QueuePacketSend(XPacketType.UpdateCardOnTable, new XPacketUpdateCardOnTable()
            {
                CardType = (int)selectedCard.Type,
                CardColor = (int)selectedCard.Color,
                SelectedColor = (int)CardColor.Yellow
            });
            HideSelectColorBox();
        }

        private void greenButton_Click(object sender, EventArgs e)
        {
            client.QueuePacketSend(XPacketType.UpdateCardOnTable, new XPacketUpdateCardOnTable()
            {
                CardType = (int)selectedCard.Type,
                CardColor = (int)selectedCard.Color,
                SelectedColor = (int)CardColor.Green
            });
            HideSelectColorBox();
        }

        private void blueButton_Click(object sender, EventArgs e)
        {
            client.QueuePacketSend(XPacketType.UpdateCardOnTable, new XPacketUpdateCardOnTable()
            {
                CardType = (int)selectedCard.Type,
                CardColor = (int)selectedCard.Color,
                SelectedColor = (int)CardColor.Blue
            });
            HideSelectColorBox();
        }

        internal void GameOver(int winnerId)
        {
            HideSelectColorBox();

            winnerLabel.Text = $"Победитель: Player{winnerId}";
            mainGroupBox.Enabled = false;
            gameOverBox.Visible = true;
            gameOverBox.Enabled = true;
            gameOverBox.BringToFront();
            winnerLabel.BringToFront();
        }

        private void HideSelectColorBox()
        {
            selectColorBox.Visible = false;
            selectColorBox.Enabled = false;
            mainGroupBox.Enabled = true;
        }
    }
}