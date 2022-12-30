namespace XProtocol
{
    public enum XPacketType
    {
        Unknown,
        Handshake,
        NewPlayer,
        PlayerReady,
        GameStart,
        UpdateCardOnTable,
        AddCardToHand,
        ChangeCardsCount,
        SuccessfulMove,
        SkipMove,
        Uno,
        PlayerDidntSayUno,
        GameOver
    }
}
