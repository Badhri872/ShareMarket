﻿namespace Services.AngelWebSocket
{
    public record AngelToken(string jwtToken, string refreshToken, string feedToken);

    public record AngelTokenResponse(
        AngelToken data, bool status, string message, string errorcode);

    public record SymbolDetailsAngel(string symbol, string token);
    public record LTPRequest(
        string correlationID,
        int action,
        LTPRequestParameters @params);

    public record LTPRequestParameters(int mode, List<ExchangeSubscriptionDetailsAngel> tokenList);

    public record ExchangeSubscriptionDetailsAngel(int exchangeType, List<string> tokens);

    public record TokenData(string Token, string Symbol, string Strike, string Exch_Seg);
}