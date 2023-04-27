namespace Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;

public record GameRecordByIMESportsQuery(
    Uri ApiUrl,
    string MerchantCode,
    string MerchantPrefix,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int Page);
