
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Cards
{
    public readonly struct Card
    {
        public static readonly CardColor[] OrdinalCardColors = new CardColor[]  {
            CardColor.Red, CardColor.Green, CardColor.Blue, CardColor.Yellow
        };
        public static readonly CardType[] OrdinalCardTypes = new CardType[]  { 
            CardType.Zero, CardType.One, CardType.Two, CardType.Three, CardType.Four, CardType.Five, CardType.Six, CardType.Seven, CardType.Eight, CardType.Nine 
        };
        public static readonly CardType[] SpecialCardTypes = new CardType[]  {
            CardType.Skip, CardType.Reverse, CardType.PlusTwo, CardType.None, CardType.PlusFour
        };
        public static readonly CardType[] ColoredCardTypes = new CardType[]  {
            CardType.Zero, CardType.One, CardType.Two, CardType.Three, CardType.Four, CardType.Five, CardType.Six, CardType.Seven, CardType.Eight, CardType.Nine, CardType.Skip, CardType.Reverse, CardType.PlusTwo
        };

        public static Card FromString(string strCard)
        {
            if (!Enum.TryParse(strCard.Split('_')[0], out CardColor cardColor) || !int.TryParse(strCard.Split('_')[1], out int cardType) || cardType < 0 || cardType >= Enum.GetValues(typeof(CardType)).Length)
                throw new Exception($"{strCard} is not valid card");
            return new Card((CardType)cardType, cardColor);
        }

        public static string ToString(Card card)
            => $"{card.Color}_{(int)card.Type}";

        public readonly CardColor Color;
        public readonly CardType Type;

        public Card(CardType type, CardColor color)
        {
            Color = color;
            Type = type;
        }

        public Card(CardType type) : this(type, CardColor.Black)
        {
            if (((int)type) <= 12)
                throw new Exception("A card with this type must have a color");
        }
    }
}
