namespace XProtocol
{
    public enum XPacketType
    {
        Unknown,
        Handshake,
        NewPlayer,
        PlayerReady,
        GameStart,
        CurrentPlayer,
        UpdateCardOnTable,
        AddCardToHand,
        SuccessfulMove,
        ChangeCardsCount,
        SkipMove
    }
}
