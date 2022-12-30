using XProtocol.Serializator;
using XProtocol;
using GameLogic;
using XProtocol.XPackets;
using GameLogic.Cards;
using System.Numerics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Reflection;

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
        Hint currentHint = Hint.No;

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

        #region Методы для работы с игровой логикой

        private void ChangeCurrentPlayer(int id)
        {
            var previousPlayer = currentPlayerId;
            currentPlayerId = id;

            if (previousPlayer != -1)
                UpdatePlayerInfo(previousPlayer);
            UpdatePlayerInfo(id);
            yourMoveLabel.Visible = id == Player.Id;

            if (currentPlayerId == Player.Id && Player.Cards.Count > 0)
                CheckMoveAvailable();
        }

        private void RemoveCardFromHand(Card card)
        {
            Player.TakeCard(card);
            SortCardsInHand();
            DecreaseCardsNumber(Player.Id);

            unoButton.Visible = Player.Cards.Count == 1;
        }

        private void DecreaseCardsNumber(int playerId)
        {
            PlayersCardsCount[playerId - 1]--;
            UpdatePlayerInfo(playerId);
        }

        private void UpdatePlayerInfo(int playerId)
        {
            var playerInfo = $"{((currentPlayerId == playerId) ? ">>> " : "")}Player{playerId}";
            playerInfo += PlayersUno[playerId - 1] ? $" [УНО]" : $" [{PlayersCardsCount[playerId - 1]} карт]";
            playersList.Items[playerId - 1] = playerInfo;

            //if (playerId != Player.Id && PlayersCardsCount[playerId - 1] == 1 && !PlayersUno[playerId - 1])
            //    ShowHint($"Player{playerId} не сказал \"УНО!\"");
        }

        private void SortCardsInHand()
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

        private void CheckMoveAvailable()
        {
            IsMoveAvailable = Player.Cards.Any(c => Game.CheckMoveCorrect(cardOnTable, c, selectedCardOnTableColor));
            if (!IsMoveAvailable && (FirstMove && Player.Cards.Count >= 7 || !FirstMove))
            {
                ShowHint(Hint.NoCards);
                deckPicture.Cursor = Cursors.Hand;
            }
            else
                deckPicture.Cursor = Cursors.No;
        }

        #endregion

        #region Обработка событий на сервере

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

        internal void PlayerReady(int id)
        {
            playersList.Items[id - 1] = $"Player{id} [Готов]";
        }

        internal void GameStart(Card lastCard, int currentPlayerId)
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

            UpdateCardOnTable(lastCard);
            ChangeCurrentPlayer(currentPlayerId);
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

        internal void AddCardToHand(Card card)
        {
            Player.AddCard(card);
            SortCardsInHand();

            unoButton.Visible = !FirstMove && Player.Cards.Count == 1;
            if (FirstMove && currentHint != Hint.NoCards && currentPlayerId == Player.Id)
                CheckMoveAvailable();
        }

        internal void ChangeCardsCount(int playerId, int cardsCount)
        {
            PlayersCardsCount[playerId - 1] = cardsCount;
            PlayersUno[playerId - 1] = false;
            UpdatePlayerInfo(playerId);
        }

        internal void SuccessfulMove(int playerId, int skipPlayerId, int nextPlayerId, Card card, CardColor selectedColor)
        {
            HideHint();
            FirstMove = false;
            UpdateCardOnTable(card, (card.Color == CardColor.Black) ? (CardColor)selectedColor : null);
            if (playerId == Player.Id)
                RemoveCardFromHand(card);
            else
                DecreaseCardsNumber(playerId);
            ChangeCurrentPlayer(nextPlayerId);

            if (Player.Id == skipPlayerId)
            {
                switch (card.Type)
                {
                    case CardType.Skip:
                        ShowHint(Hint.Skip);
                        break;
                    case CardType.PlusTwo:
                        ShowHint(Hint.PlusTwo);
                        break;
                    case CardType.PlusFour:
                        ShowHint(Hint.PlusFour);
                        break;
                }
            }
        }

        internal void SkipMove(int skipPlayerId, int nextPlayerId)
        {
            if (skipPlayerId == Player.Id)
                ShowHint(Hint.NoCardsSkip);
            ChangeCurrentPlayer(nextPlayerId);
        }

        internal void Uno(int playerId)
        {
            PlayersUno[playerId - 1] = true;
            UpdatePlayerInfo(playerId);
        }

        internal void PlayerDidntSayUno()
        {
            ShowHint(Hint.DidntSayUno);
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

        #endregion

        #region Обработка событий формы

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
                if (playerId != Player.Id && PlayersCardsCount[playerId - 1] == 1)
                    client.QueuePacketSend(XPacketType.PlayerDidntSayUno, new XPacketPlayerDidntSayUno() { PlayerId = playerId });
                else if (playerId == Player.Id && Player.Cards.Count == 1)
                    unoButton_Click(sender, e);
            }
            playersList.ClearSelected();
        }

        private void cardsListView_DoubleClick(object sender, EventArgs e)
        {
            if (cardsListView.SelectedItems.Count >= 1)
            {
                if (currentHint == Hint.PlusFourUnavailable)
                    HideHint();

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
                    if (card.Type == CardType.PlusFour && Player.Cards
                        .Any(c
                        => (c.Color == cardOnTable.Color && cardOnTable.Color != CardColor.Black)
                        || (c.Color == selectedCardOnTableColor && cardOnTable.Color == CardColor.Black)))
                    {
                        ShowHint(Hint.PlusFourUnavailable);
                        return;
                    }

                    selectedCard = card;
                    ShowSelectColorBox();
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
                if (currentHint == Hint.NoCards)
                    HideHint();
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

        #endregion

        #region Вспомогательные методы для отображения элементов

        enum Hint
        {
            [Description("Карта \"Пропуск хода\":\nВы пропустили ход.")]
            Skip,
            [Description("Карта \"+2\":\nВы взяли 2 карты и пропустили ход.")]
            PlusTwo,
            [Description("Карта \"+4\":\nВы взяли 4 карты и пропустили ход.")]
            PlusFour,
            [Description("Вы не успели сказать \"УНО!\".\nВы взяли 2 карты.")]
            DidntSayUno,
            [Description("Вам нечем ходить.\nВозьмите карту из колоды.")]
            NoCards,
            [Description("Вам нечем ходить.\nВы пропустили ход.")]
            NoCardsSkip,
            [Description("Карту \"+4\" можно использовать,\nтолько если у Вас нет текущего цвета.")]
            PlusFourUnavailable,
            [Description("")]
            No
        }

        private string GetDescription(Hint hint)
        {
            Type type = typeof(Hint);
            string name = Enum.GetName(type, hint);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        private void ShowSelectColorBox()
        {
            mainGroupBox.Enabled = false;
            selectColorBox.Visible = true;
            selectColorBox.Enabled = true;
            selectColorBox.BringToFront();
        }

        private void HideSelectColorBox()
        {
            selectColorBox.Visible = false;
            selectColorBox.Enabled = false;
            mainGroupBox.Enabled = true;
        }

        private void ShowHint(Hint hint)
        {
            currentHint = hint;
            hintLabel.Text = GetDescription(hint);
            hintLabel.Visible = true;
        }

        private void HideHint()
        {
            currentHint = Hint.No;
            hintLabel.Visible = false;
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
        #endregion
    }
}