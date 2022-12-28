using GameLogic.Cards;

namespace GameLogic;

public class Player
{
    public Player(int id)
    {
        Id = id;
        Cards = new List<Card>();
    }

    public void AddCard(Card card)
    {
        Cards.Add(card);
        Sort();

        Uno = false;
    }

    public void TakeCard(Card card)
    {
        Cards.Remove(card);
        Sort();
    }

    private void Sort()
    {
        Cards = Cards.OrderBy(c => (int)c.Type).OrderBy(c => (int)c.Color).ToList();
    }

    public bool Ready;
    public bool Uno;

    public readonly int Id;

    public readonly string Name;

    public List<Card> Cards { get; private set; }

}
