using GameLogic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class Game
    {
        private static Random rng = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

        public readonly Player[] Players;
        public readonly Stack<Card> Deck;
        public readonly Stack<Card> DiscardPile;

        public Card LastCard { get; private set; }

        public Game(Player[] players)
        {
            if (players.Length < 2)
                throw new Exception("There must be at least two players in the game");
            if (players.Length > 10)
                throw new Exception("There must be not more than ten players in the game");
            Players = players;
            Deck = FillDeck();
            DiscardPile = new Stack<Card>();
        }

        public void Start()
        {
            DealCards();

            var lastCard = Deck.Pop();
            while (!Card.OrdinalCardTypes.Contains(lastCard.Type))
            {
                Deck.Push(lastCard);
                lastCard = Deck.Pop();
            }

            LastCard = lastCard;


        }

        private void DealCards()
        {
            foreach (var player in Players)
                for (int i = 0; i < 7; i++)
                {
                    if (Deck.Count == 0)
                        FillDeckFromDiscardPile();

                    player.Cards.Add(Deck.Pop());
                }
        }

        private void FillDeckFromDiscardPile()
        {
            var shuffledCards = Shuffle(DiscardPile);
            foreach (var card in shuffledCards)
                Deck.Push(card);
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

        private static Stack<Card> FillDeck()
        {
            var cards = new Stack<Card>();
            foreach (var color in Card.OrdinalCardColors)
                foreach (var type in Card.ColoredCardTypes)
                {
                    cards.Push(new Card(type, color));
                    if (type != CardType.Zero)
                        cards.Push(new Card(type, color));
                }

            for (var i = 0; i < 4; i++)
            {
                cards.Push(new Card(CardType.None));
                cards.Push(new Card(CardType.PlusFour));
            }

            return new Stack<Card>(Shuffle(cards));
        }
    }
}
