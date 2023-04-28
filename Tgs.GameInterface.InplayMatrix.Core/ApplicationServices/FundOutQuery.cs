namespace Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;

public record FundOutQuery(
    Uri ApiUrl,
    string MerchantCode,
    string MerchantPrefix,
    string PlayerCode,
    int ProductWallet,
    string TransactionId,
    decimal Amount);
