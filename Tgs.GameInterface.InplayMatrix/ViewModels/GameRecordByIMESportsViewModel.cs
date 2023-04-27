namespace Tgs.GameInterface.InplayMatrix.ViewModels;

public record GameRecordByIMESportsViewModel(
    Uri ApiUrl,
    string MerchantCode,
    string MerchantPrefix,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int Page);
