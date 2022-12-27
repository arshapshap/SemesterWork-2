using GameLogic.Cards;

namespace Client;

internal class Player
{
    public Player(int id)
    {
        Id = id;
        Cards = new List<Card>();
    }

    public readonly int Id;

    public string Name;

    public readonly List<Card> Cards;
}
