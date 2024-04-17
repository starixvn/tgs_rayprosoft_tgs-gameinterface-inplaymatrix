namespace Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;

public record GameRecordByIMESportsQuery(
	Uri ReportUrl,
	string MerchantCode,
	string MerchantPrefix,
	DateTimeOffset StartDate,
	DateTimeOffset EndDate,
	int Page);
