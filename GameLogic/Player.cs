using GameLogic.Cards;

namespace GameLogic;

internal class Player
{
    public Player(int id, string name)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Cards = new List<Card>();
    }

    public readonly int Id;

    public readonly string Name;

    public readonly List<Card> Cards;
}
