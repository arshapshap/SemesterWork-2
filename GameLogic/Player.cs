using GameLogic.Cards;

namespace GameLogic;

public class Player
{
    public Player(int id)
    {
        Id = id;
        Cards = new List<Card>();
    }

    public bool Ready;

    public readonly int Id;

    public readonly string Name;

    public readonly List<Card> Cards;
}
