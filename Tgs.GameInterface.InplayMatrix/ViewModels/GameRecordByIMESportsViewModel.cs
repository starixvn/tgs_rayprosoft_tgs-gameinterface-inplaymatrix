namespace Tgs.GameInterface.InplayMatrix.ViewModels;

public record GameRecordByIMESportsViewModel(
    Uri ReportUrl,
    string MerchantCode,
    string MerchantPrefix,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int Page);
