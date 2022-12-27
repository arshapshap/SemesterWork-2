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
    }
}
