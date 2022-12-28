using GameLogic.Cards;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class Game
    {
        private static Random rng = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

        public readonly Player[] Players;
        public readonly Queue<Card> Deck;
        public readonly Queue<Card> DiscardPile;

        public int CurrentPlayerId { get; private set; }

        public Card CardOnTable { get; private set; }
        public CardColor? SelectedColor { get; private set; }
        public bool Reversed { get; private set; }
        public bool IsOver { get; private set; }
        public int WinnerId { get; private set; }

        public Game(Player[] players)
        {
            if (players.Length < 2)
                throw new Exception("There must be at least two players in the game");
            if (players.Length > 10)
                throw new Exception("There must be not more than ten players in the game");
            Players = players;
            Deck = FillDeck();
            DiscardPile = new Queue<Card>();
        }

        public void Start()
        {
            DealCards();

            var cardOnTable = PopFromDeck();
            while (!Card.OrdinalCardTypes.Contains(cardOnTable.Type))
            {
                Deck.Enqueue(cardOnTable);
                cardOnTable = PopFromDeck();
            }

            CardOnTable = cardOnTable;
            CurrentPlayerId = Players[rng.Next(Players.Length)].Id;
        }

        public bool TryMove(Player player, Card card, out Dictionary<int, Card[]> newCards, CardColor? selectedColor = null)
        {
            newCards = new Dictionary<int, Card[]>();
            if (!CheckMoveCorrect(card) && !CheckMoveInterception(card))
                return false;
            else if (CheckMoveInterception(card))
                CurrentPlayerId = player.Id;

            player.TakeCard(card);
            CardOnTable = card;
            SelectedColor = selectedColor;

            if (player.Cards.Count == 0)
            {
                IsOver = true;
                WinnerId = player.Id;
                return true;
            }

            if (card.Type == CardType.Skip)
                NextPlayer();
            else if (card.Type == CardType.Reverse)
                Reversed = !Reversed;
            else if (card.Type == CardType.PlusTwo)
            {
                NextPlayer();
                newCards[CurrentPlayerId] = AddCards(CurrentPlayerId, 2);
            }
            else if (card.Type == CardType.PlusFour)
            {
                NextPlayer();
                newCards[CurrentPlayerId] = AddCards(CurrentPlayerId, 4);
            }
            NextPlayer();
            return true;
        }

        public bool TryTakeCardFromDeck(Player player, out Card card)
        {
            card = new Card();
            if (player.Id != CurrentPlayerId || player.Cards.Any(c => CheckMoveCorrect(CardOnTable, c, SelectedColor)))
                return false;

            card = PopFromDeck();
            player.AddCard(card);

            return true;
        }

        public static bool CheckMoveCorrect(Card cardOnTable, Card card, CardColor? selectedColor = null)
            => card.Color == CardColor.Black
                || card.Color == cardOnTable.Color
                || (cardOnTable.Color == CardColor.Black && card.Color == selectedColor)
                || card.Type == cardOnTable.Type;

        public static bool CheckMoveInterception(Card cardOnTable, Card card, CardColor? selectedColor = null)
            => card.Color == cardOnTable.Color 
            && card.Type == cardOnTable.Type
            && !(card.Color == CardColor.Black && card.Type == CardType.PlusFour);

        public int NextPlayer()
        {
            CurrentPlayerId = Reversed ?
                        ((CurrentPlayerId + Players.Length - 2) % Players.Length) + 1
                        : (CurrentPlayerId % Players.Length) + 1;
            return CurrentPlayerId;
        }

        public bool CheckMoveCorrect(Card card)
            => CheckMoveCorrect(CardOnTable, card, SelectedColor);

        public bool CheckMoveInterception(Card card)
            => CheckMoveInterception(CardOnTable, card, SelectedColor);

        public Card[] UnoPenalty(int playerId)
            => AddCards(playerId, 2);

        private Card[] AddCards(int playerId, int count)
        {
            var newCardsList = new List<Card>();
            for (int i = 0; i < count; i++)
            {
                var newCard = PopFromDeck();
                newCardsList.Add(newCard);
                Players[playerId - 1].AddCard(newCard);
            }
            return newCardsList.ToArray();
        }

        private void DealCards()
        {
            foreach (var player in Players)
                for (int i = 0; i < 7; i++)
                {
                    player.AddCard(PopFromDeck());
                }
        }

        private void FillDeckFromDiscardPile()
        {
            var shuffledCards = Shuffle(DiscardPile);
            foreach (var card in shuffledCards)
                Deck.Enqueue(card);
            DiscardPile.Clear();
        }

        private static IEnumerable<Card> Shuffle(IEnumerable<Card> cards)
        {
            var n = cards.Count();
            var result = cards.ToArray();
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var card = result[k];
                result[k] = result[n];
                result[n] = card;
            }
            return result;
        }

        private static Queue<Card> FillDeck()
        {
            var cards = new Queue<Card>();
            foreach (var color in Card.OrdinalCardColors)
                foreach (var type in Card.ColoredCardTypes)
                {
                    cards.Enqueue(new Card(type, color));
                    if (type != CardType.Zero)
                        cards.Enqueue(new Card(type, color));
                }

            for (var i = 0; i < 4; i++)
            {
                cards.Enqueue(new Card(CardType.None));
                cards.Enqueue(new Card(CardType.PlusFour));
            }

            return new Queue<Card>(Shuffle(cards));
        }

        private Card PopFromDeck()
        {
            if (Deck.Count == 0)
                FillDeckFromDiscardPile();

            return Deck.Dequeue();
        }

        public int GetPlayerCardsCount(int playerId) => Players[playerId - 1].Cards.Count;
    }
}
