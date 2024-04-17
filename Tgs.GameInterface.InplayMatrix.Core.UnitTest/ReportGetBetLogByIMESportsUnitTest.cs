using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.InplayMatrix.Common;
using Tgs.GameInterface.InplayMatrix.Core.Extensions;

namespace Tgs.GameInterface.InplayMatrix.Core.UnitTest;

public class ReportGetBetLogByIMESportsUnitTest
{
	private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

	[Fact]
	public async Task No_data_found()
	{
		var reportUrl = new Uri("http://operatorapi-report.staging.imaegisapi.com/");
		var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
		var startDate = DateTimeOffset.Parse("2023-04-27T10:17:00+08:00");
		var endDate = DateTimeOffset.Parse("2023-04-27T10:27:00+08:00");
		var page = 1;

		var request = new
		{
			MerchantCode = merchantCode,
			StartDate = startDate.DatetimeConvertToString(),
			EndDate = endDate.DatetimeConvertToString(),
			Page = page,
			ProductWallet = (int)InplayMatrixProductWallet.IM_eSports_Wallet,
			DateFilterType = DateFilterType.IMESportsLastUpdatedTime,
			Language = "EN"
		};

		var parameters = JsonSerializer.SerializeToNode(request, _jsonOptions)!;

		var response = new
		{
			Code = "558",
			Message = "No data found."
		};
		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/Report/GetBetLog")
				.WithContent(parameters.ToJsonString())
				.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeSystemClock = Substitute.For<ISystemClock>();
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeSystemClock,
			fakeHttpClient);

		var actual = await client.ReportGetBetLogByIMESportsAsync(
			reportUrl: reportUrl,
			merchantCode: merchantCode,
			startDate: startDate,
			endDate: endDate,
			page: page)
			.ToArrayAsync();

		Assert.Empty(actual);
	}

	[Fact]
	public async Task Normal_Process_Cancelled()
	{
		var reportUrl = new Uri("http://operatorapi-report.staging.imaegisapi.com/");
		var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
		var startDate = DateTimeOffset.Parse("2023-04-27T15:56:00+08:00");
		var endDate = DateTimeOffset.Parse("2023-04-27T16:00:00+08:00");
		var page = 1;

		var request = new
		{
			MerchantCode = merchantCode,
			StartDate = startDate.DatetimeConvertToString(),
			EndDate = endDate.DatetimeConvertToString(),
			Page = page,
			ProductWallet = (int)InplayMatrixProductWallet.IM_eSports_Wallet,
			DateFilterType = DateFilterType.IMESportsLastUpdatedTime,
			Language = "EN"
		};

		var parameters = JsonSerializer.SerializeToNode(request, _jsonOptions)!;

		var response = new
		{
			Code = "0",
			Message = "Successful.",
			Result = new[] {
				new{
					Provider="ESPORTSBULL_ESPORTS",
					GameId= "ESPORTSBULL",
					BetId= "432368343",
					WagerCreationDateTime= "2023-04-27 15:54:50 +08:00",
					LastUpdatedDate= "2023-04-27 15:59:47 +08:00",
					PlayerId= "IM1S3_4ucmqplxb029",
					ProviderPlayerId= "IM1S3IM1S3_4ucmqplxb029",
					Currency= "CNY",
					StakeAmount= "5.000000",
					WinLoss= "0.000000",
					OddsType="EURO",
					TotalOdds= "13.952",
					WagerType= "Single",
					Platform="Web",
					IsSettled="1",
					IsResettled= "0",
					IsCancelled= "1",
					SettlementDateTime= "2023-04-27 15:59:47 +08:00",
					Tolerance= "0",
					FlagCategory= "",
					DetailItems= new []{
						new {
							EventName= "eSuba VS Team GO",
							EventDateTime= "2023-04-29 00:30:00 +08:00",
							CompetitionName= "EMEA Masters Spring 2023",
							HomeTeamName="eSuba",
							AwayTeamName="Team GO",
							FavTeam="",
							Period= "",
							HomeTeamHTScore= "0",
							AwayTeamHTScore= "0",
							HomeTeamFTScore= "0",
							AwayTeamFTScore="0",
							WagerHomeTeamScore= "0",
							WagerAwayTeamScore= "0",
							MarketType= "",
							Handicap="0.00",
							CancelReason= "",
							SportsID= 45,
							SportsName= "LEAGUE OF LEGENDS",
							BetType= "Game 1 Win (Live Odds)",
							EventId="12929913",
							EventType= "BO7",
							GameOrder= "1",
							Selection= "eSuba",
							Odds="13.952",
							IsSettled="1",
							IsResettled="0",
							IsWagerItemCancelled= "1",
							WagerItemCancelType="2",
							ResultStatus="",
							BaseTierID="1",
							BaseTierCode="1",
							BaseTierName= "Tier1"
						}
					}
				}
			},
			Pagination = new
			{
				CurrentPage = 1,
				TotalPage = 1,
				ItemPerPage = 5000,
				TotalCount = 1
			}
		};
		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/Report/GetBetLog")
				.WithContent(parameters.ToJsonString())
				.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeSystemClock = Substitute.For<ISystemClock>();
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeSystemClock,
			fakeHttpClient);

		var actual = await client.ReportGetBetLogByIMESportsAsync(
			reportUrl: reportUrl,
			merchantCode: merchantCode,
			startDate: startDate,
			endDate: endDate,
			page: page)
			.ToArrayAsync();

		Assert.NotNull(actual);
		Assert.NotEmpty(actual);
		Assert.Equal("1", actual.Single().IsSettled);
		Assert.Equal("1", actual.Single().IsCancelled);
		Assert.Equal("0", actual.Single().IsResettled);
	}

	[Fact]
	public async Task Normal_Process_ParlayAll()
	{
		var reportUrl = new Uri("http://operatorapi-report.staging.imaegisapi.com/");
		var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
		var startDate = DateTimeOffset.Parse("2023-04-27T16:10:00+08:00");
		var endDate = DateTimeOffset.Parse("2023-04-27T16:20:00+08:00");
		var page = 1;

		var request = new
		{
			MerchantCode = merchantCode,
			StartDate = startDate.DatetimeConvertToString(),
			EndDate = endDate.DatetimeConvertToString(),
			Page = page,
			ProductWallet = (int)InplayMatrixProductWallet.IM_eSports_Wallet,
			DateFilterType = DateFilterType.IMESportsLastUpdatedTime,
			Language = "EN"
		};

		var parameters = JsonSerializer.SerializeToNode(request, _jsonOptions)!;

		var response = new
		{
			Code = "0",
			Message = "Successful.",
			Result = new[] {
				new{
					Provider="ESPORTSBULL_ESPORTS",
					GameId= "ESPORTSBULL",
					BetId= "432368345",
					WagerCreationDateTime= "2023-04-27 16:33:07 +08:00",
					LastUpdatedDate= "2023-04-27 16:33:07 +08:00",
					PlayerId= "IM1S3_4ucmqplxb029",
					ProviderPlayerId= "IM1S3IM1S3_4ucmqplxb029",
					Currency= "CNY",
					StakeAmount= "5.000000",
					WinLoss= "0.000000",
					OddsType="EURO",
					TotalOdds= "34.24",
					WagerType= "ParlayAll",
					Platform="Web",
					IsSettled="0",
					IsResettled= "0",
					IsCancelled= "0",
					SettlementDateTime=  "",
					Tolerance= "255",
					FlagCategory= "",
					DetailItems= new []{
						new {
							ParlaySign= "",
							EventName = "eSuba VS Team GO",
							EventDateTime= "2023-04-29 00:00:00 +08:00",
							CompetitionName= "EMEA Masters Spring 2023",
							ParlayWagerCreationDateTime="2023-04-27 16:33:07 +08:00",
							HomeTeamName="eSuba",
							AwayTeamName="Team GO",
							FavTeam="",
							Period= "",
							HomeTeamHTScore= "0",
							AwayTeamHTScore= "0",
							HomeTeamFTScore= "0",
							AwayTeamFTScore="0",
							WagerHomeTeamScore = "0",
							WagerAwayTeamScore = "0",
							MarketType = "",
							Handicap = "0.00",
							CancelReason = "",
							SportsID = 45,
							SportsName = "LEAGUE OF LEGENDS",
							BetType = "Game 1 Win (Live Odds)",
							EventId = "12929906",
							EventType = "BO7",
							GameOrder = "0",
							Selection = "eSuba",
							Odds = "20.0",
							IsSettled = "0",
							IsResettled = "0",
							IsWagerItemCancelled = "0",
							WagerItemCancelType = "0",
							ResultStatus = "",
							BaseTierID = "1",
							BaseTierCode = "1",
							BaseTierName = "Tier1"
						},
						new {
							ParlaySign= "",
							EventName =  "Astralis Talent VS Illuminar Gaming",
							EventDateTime= "2023-04-27 17:00:00 +08:00",
							CompetitionName= "CCT Central Europe Series #6",
							ParlayWagerCreationDateTime="2023-04-27 16:33:07 +08:00",
							HomeTeamName= "Astralis Talent",
							AwayTeamName= "Illuminar Gaming",
							FavTeam="",
							Period= "",
							HomeTeamHTScore= "0",
							AwayTeamHTScore= "0",
							HomeTeamFTScore= "0",
							AwayTeamFTScore="0",
							WagerHomeTeamScore = "0",
							WagerAwayTeamScore = "0",
							MarketType = "",
							Handicap = "0.00",
							CancelReason = "",
							SportsID = 47,
							SportsName = "CS:GO",
							BetType ="Game 3 Win",
							EventId =  "12957194",
							EventType = "BO3",
							GameOrder = "3",
							Selection = "Astralis Talent",
							Odds ="1.712",
							IsSettled =  "0",
							IsResettled =  "0",
							IsWagerItemCancelled = "0",
							WagerItemCancelType = "0",
							ResultStatus = "",
							BaseTierID = "0",
							BaseTierCode = "0",
							BaseTierName = "Other"
						}
					}
				}
			},
			Pagination = new
			{
				CurrentPage = 1,
				TotalPage = 1,
				ItemPerPage = 5000,
				TotalCount = 1
			}
		};
		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/Report/GetBetLog")
				.WithContent(parameters.ToJsonString())
				.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeSystemClock = Substitute.For<ISystemClock>();
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeSystemClock,
			fakeHttpClient);

		var actual = await client.ReportGetBetLogByIMESportsAsync(
			reportUrl: reportUrl,
			merchantCode: merchantCode,
			startDate: startDate,
			endDate: endDate,
			page: page)
			.ToArrayAsync();

		Assert.NotNull(actual);
		Assert.NotEmpty(actual);
		Assert.Equal("0", actual.Single().IsSettled);
		Assert.Equal("0", actual.Single().IsCancelled);
		Assert.Equal("0", actual.Single().IsResettled);
		Assert.Null(actual.Single().SettlementDateTime);
	}

	[Fact]
	public async Task Normal_Process_ParlayAll_12929906_Settlement_Win()
	{
		var reportUrl = new Uri("http://operatorapi-report.staging.imaegisapi.com/");
		var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
		var startDate = DateTimeOffset.Parse("2023-04-27T17:02:00+08:00");
		var endDate = DateTimeOffset.Parse("2023-04-27T17:03:00+08:00");
		var page = 1;

		var request = new
		{
			MerchantCode = merchantCode,
			StartDate = startDate.DatetimeConvertToString(),
			EndDate = endDate.DatetimeConvertToString(),
			Page = page,
			ProductWallet = (int)InplayMatrixProductWallet.IM_eSports_Wallet,
			DateFilterType = DateFilterType.IMESportsLastUpdatedTime,
			Language = "EN"
		};

		var parameters = JsonSerializer.SerializeToNode(request, _jsonOptions)!;

		var response = new
		{
			Code = "0",
			Message = "Successful.",
			Result = new[] {
				new{
					Provider="ESPORTSBULL_ESPORTS",
					GameId= "ESPORTSBULL",
					BetId= "432368345",
					WagerCreationDateTime= "2023-04-27 16:33:07 +08:00",
					LastUpdatedDate= "2023-04-27 17:02:03 +08:00",
					PlayerId= "IM1S3_4ucmqplxb029",
					ProviderPlayerId= "IM1S3IM1S3_4ucmqplxb029",
					Currency= "CNY",
					StakeAmount= "5.000000",
					WinLoss= "0.000000",
					OddsType="EURO",
					TotalOdds=  "34.24",
					WagerType= "ParlayAll",
					Platform="Web",
					IsSettled="0",
					IsResettled= "0",
					IsCancelled= "0",
					SettlementDateTime=  "",
					Tolerance= "255",
					FlagCategory= "",
					DetailItems= new []{
						new {
							ParlaySign= "",
							EventName = "eSuba VS Team GO",
							EventDateTime= "2023-04-29 00:00:00 +08:00",
							CompetitionName= "EMEA Masters Spring 2023",
							ParlayWagerCreationDateTime="2023-04-27 16:33:07 +08:00",
							HomeTeamName="eSuba",
							AwayTeamName="Team GO",
							FavTeam="",
							Period= "",
							HomeTeamHTScore= "0",
							AwayTeamHTScore= "0",
							HomeTeamFTScore= "0",
							AwayTeamFTScore="0",
							FTScore= new[] {
								new {
									Option= "eSuba",
									Outcome= 1
								},
								new   {
									Option= "Team GO",
									Outcome= 0
								}
							},
							WagerHomeTeamScore = "0",
							WagerAwayTeamScore = "0",
							MarketType = "",
							Handicap = "0.00",
							CancelReason = "",
							SportsID = 45,
							SportsName = "LEAGUE OF LEGENDS",
							BetType = "Game 1 Win (Live Odds)",
							EventId = "12929906",
							EventType = "BO7",
							GameOrder = "0",
							Selection = "eSuba",
							Odds = "20.0",
							IsSettled = "1",
							IsResettled = "0",
							IsWagerItemCancelled = "0",
							WagerItemCancelType = "0",
							ResultStatus = "1",
							BaseTierID = "1",
							BaseTierCode = "1",
							BaseTierName = "Tier1"
						},
						new {
							ParlaySign= "",
							EventName =  "Astralis Talent VS Illuminar Gaming",
							EventDateTime= "2023-04-27 17:00:00 +08:00",
							CompetitionName= "CCT Central Europe Series #6",
							ParlayWagerCreationDateTime="2023-04-27 16:33:07 +08:00",
							HomeTeamName= "Astralis Talent",
							AwayTeamName= "Illuminar Gaming",
							FavTeam="",
							Period= "",
							HomeTeamHTScore= "0",
							AwayTeamHTScore= "0",
							HomeTeamFTScore= "0",
							AwayTeamFTScore="0",
							FTScore= new[] {
								new {
									Option= "eSuba",
									Outcome= 1
								},
								new   {
									Option= "Team GO",
									Outcome= 0
								}
							},
							WagerHomeTeamScore = "0",
							WagerAwayTeamScore = "0",
							MarketType = "",
							Handicap = "0.00",
							CancelReason = "",
							SportsID = 47,
							SportsName = "CS:GO",
							BetType ="Game 3 Win",
							EventId =  "12957194",
							EventType = "BO3",
							GameOrder = "3",
							Selection = "Astralis Talent",
							Odds ="1.712",
							IsSettled =  "0",
							IsResettled =  "0",
							IsWagerItemCancelled = "0",
							WagerItemCancelType = "0",
							ResultStatus = "",
							BaseTierID = "0",
							BaseTierCode = "0",
							BaseTierName = "Other"
						}
					}
				}
			},
			Pagination = new
			{
				CurrentPage = 1,
				TotalPage = 1,
				ItemPerPage = 5000,
				TotalCount = 1
			}
		};
		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/Report/GetBetLog")
				.WithContent(parameters.ToJsonString())
				.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeSystemClock = Substitute.For<ISystemClock>();
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeSystemClock,
			fakeHttpClient);

		var actual = await client.ReportGetBetLogByIMESportsAsync(
			reportUrl: reportUrl,
			merchantCode: merchantCode,
			startDate: startDate,
			endDate: endDate,
			page: page)
			.ToArrayAsync();

		Assert.NotNull(actual);
		Assert.NotEmpty(actual);
		Assert.Equal("0", actual.Single().IsSettled);
		Assert.Equal("0", actual.Single().IsCancelled);
		Assert.Equal("0", actual.Single().IsResettled);
		Assert.Null(actual.Single().SettlementDateTime);
	}

	[Fact]
	public async Task Normal_Process_Settlement_Losee()
	{
		var reportUrl = new Uri("http://operatorapi-report.staging.imaegisapi.com/");
		var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
		var startDate = DateTimeOffset.Parse("2023-04-27T14:12:00+08:00");
		var endDate = DateTimeOffset.Parse("2023-04-27T14:13:00+08:00");
		var page = 1;

		var request = new
		{
			MerchantCode = merchantCode,
			StartDate = startDate.DatetimeConvertToString(),
			EndDate = endDate.DatetimeConvertToString(),
			Page = page,
			ProductWallet = (int)InplayMatrixProductWallet.IM_eSports_Wallet,
			DateFilterType = DateFilterType.IMESportsLastUpdatedTime,
			Language = "EN"
		};

		var parameters = JsonSerializer.SerializeToNode(request, _jsonOptions)!;

		var response = new
		{
			Code = "0",
			Message = "Successful.",
			Result = new[] {
				new{
					Provider="ESPORTSBULL_ESPORTS",
					GameId= "ESPORTSBULL",
					BetId= "432368314",
					WagerCreationDateTime= "2023-04-27 10:17:30 +08:00",
					LastUpdatedDate= "2023-04-27 10:17:30 +08:00",
					PlayerId= "IM1S3_4ucmqplxb029",
					ProviderPlayerId= "IM1S3IM1S3_4ucmqplxb029",
					Currency= "CNY",
					StakeAmount= "5.000000",
					WinLoss= "-5.000000",
					OddsType="EURO",
					TotalOdds= "1.093",
					WagerType= "Single",
					Platform="Web",
					IsSettled="1",
					IsResettled= "0",
					IsCancelled= "0",
					SettlementDateTime= "2023-04-27 14:12:34 +08:00",
					Tolerance= "255",
					FlagCategory= "",
					DetailItems= new []{
						new {
							EventName= "herO VS KeeN",
							EventDateTime= "2023-04-27 17:30:00 +08:00",
							CompetitionName= "Global StarCraft II League Season 1",
							HomeTeamName="herO",
							AwayTeamName="KeeN",
							FavTeam="",
							Period= "",
							HomeTeamHTScore= "0",
							AwayTeamHTScore= "0",
							HomeTeamFTScore= "0",
							AwayTeamFTScore="0",
							FTScore= new[] {
								new {
									Option= "herO",
									Outcome= 0
								},
								new   {
									Option= "KeeN",
									Outcome= 3
								}
							},
							WagerHomeTeamScore = "0",
							WagerAwayTeamScore = "0",
							MarketType = "",
							Handicap = "0.00",
							CancelReason = "",
							SportsID = 53,
							SportsName = "STARCRAFT2",
							BetType = "BO3 Series Win",
							EventId = "12933529",
							EventType = "BO3",
							GameOrder = "0",
							Selection = "herO",
							Odds = "1.093",
							IsSettled = "1",
							IsResettled = "0",
							IsWagerItemCancelled = "0",
							WagerItemCancelType = "0",
							ResultStatus = "2",
							BaseTierID = "0",
							BaseTierCode = "0",
							BaseTierName = "Other"
						}
					}
				}
			},
			Pagination = new
			{
				CurrentPage = 1,
				TotalPage = 1,
				ItemPerPage = 5000,
				TotalCount = 1
			}
		};
		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/Report/GetBetLog")
				.WithContent(parameters.ToJsonString())
				.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeSystemClock = Substitute.For<ISystemClock>();
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeSystemClock,
			fakeHttpClient);

		var actual = await client.ReportGetBetLogByIMESportsAsync(
			reportUrl: reportUrl,
			merchantCode: merchantCode,
			startDate: startDate,
			endDate: endDate,
			page: page)
			.ToArrayAsync();

		Assert.NotNull(actual);
		Assert.NotEmpty(actual);
		Assert.Equal("1", actual.Single().IsSettled);
		Assert.Equal("0", actual.Single().IsCancelled);
		Assert.Equal("0", actual.Single().IsResettled);
	}

	[Fact]
	public async Task Normal_Process_Settlement_Win()
	{
		var reportUrl = new Uri("http://operatorapi-report.staging.imaegisapi.com/");
		var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
		var startDate = DateTimeOffset.Parse("2023-04-27T14:12:00+08:00");
		var endDate = DateTimeOffset.Parse("2023-04-27T14:13:00+08:00");
		var page = 1;

		var request = new
		{
			MerchantCode = merchantCode,
			StartDate = startDate.DatetimeConvertToString(),
			EndDate = endDate.DatetimeConvertToString(),
			Page = page,
			ProductWallet = (int)InplayMatrixProductWallet.IM_eSports_Wallet,
			DateFilterType = DateFilterType.IMESportsLastUpdatedTime,
			Language = "EN"
		};

		var parameters = JsonSerializer.SerializeToNode(request, _jsonOptions)!;

		var response = new
		{
			Code = "0",
			Message = "Successful.",
			Result = new[] {
				new  {
					Provider = "ESPORTSBULL_ESPORTS",
					GameId = "ESPORTSBULL",
					BetId = "432368324",
					WagerCreationDateTime = "2023-04-27 13:40:17 +08:00",
					LastUpdatedDate = "2023-04-27 14:08:13 +08:00",
					PlayerId = "IM1S3_4ucmqplxb029",
					ProviderPlayerId = "IM1S3IM1S3_4ucmqplxb029",
					Currency = "CNY",
					StakeAmount = "5.000000",
					WinLoss = "3.005000",
					OddsType = "EURO",
					TotalOdds = "1.601",
					WagerType = "Single",
					Platform = "Web",
					IsSettled = "1",
					IsResettled = "0",
					IsCancelled = "0",
					SettlementDateTime = "2023-04-27 14:08:13 +08:00",
					Tolerance = "255",
					FlagCategory = "",
					DetailItems =new []{
						new  {
							EventName = "Team WE Academy VS Joy Dream",
							EventDateTime = "2023-04-27 14:00:00 +08:00",
							CompetitionName = "LDL 2023 Split 2",
							HomeTeamName = "Team WE Academy",
							AwayTeamName = "Joy Dream",
							FavTeam = "",
							Period = "",
							HomeTeamHTScore = "0",
							AwayTeamHTScore = "0",
							HomeTeamFTScore = "0",
							AwayTeamFTScore = "0",
							FTScore = new []{
								new  {
									Option = "Team WE Academy",
									Outcome = 3  },
								new   {
									Option = "Joy Dream",
									Outcome = 0  }
							},
							WagerHomeTeamScore = "0",
							WagerAwayTeamScore = "0",
							MarketType = "",
							Handicap = "0.00",
							CancelReason = "",
							SportsID = 45,
							SportsName = "LEAGUE OF LEGENDS",
							BetType = "BO3 Series Win",
							EventId = "12954756",
							EventType = "BO3",
							GameOrder = "0",
							Selection = "Team WE Academy",
							Odds = "1.601",
							IsSettled = "1",
							IsResettled = "0",
							IsWagerItemCancelled = "0",
							WagerItemCancelType = "0",
							ResultStatus = "1",
							BaseTierID = "2",
							BaseTierCode = "2",
							BaseTierName = "Tier2"
						}
					}
				}
			},
			Pagination = new
			{
				CurrentPage = 1,
				TotalPage = 1,
				ItemPerPage = 5000,
				TotalCount = 1
			}
		};
		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/Report/GetBetLog")
					.WithContent(parameters.ToJsonString())
					.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeSystemClock = Substitute.For<ISystemClock>();
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeSystemClock,
			fakeHttpClient);

		var actual = await client
			.ReportGetBetLogByIMESportsAsync(
				  reportUrl: reportUrl,
				  merchantCode: merchantCode,
				  startDate: startDate,
				  endDate: endDate,
				  page: page)
			  .ToArrayAsync();

		Assert.NotNull(actual);
		Assert.NotEmpty(actual);
		Assert.Equal("1", actual.Single().IsSettled);
		Assert.Equal("0", actual.Single().IsCancelled);
		Assert.Equal("0", actual.Single().IsResettled);
	}

	[Fact]
	public async Task Normal_Process_Unsettlement()
	{
		var reportUrl = new Uri("http://operatorapi-report.staging.imaegisapi.com/");
		var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
		var startDate = DateTimeOffset.Parse("2023-04-27T10:17:00+08:00");
		var endDate = DateTimeOffset.Parse("2023-04-27T10:27:00+08:00");
		var page = 1;

		var request = new
		{
			MerchantCode = merchantCode,
			StartDate = startDate.DatetimeConvertToString(),
			EndDate = endDate.DatetimeConvertToString(),
			Page = page,
			ProductWallet = (int)InplayMatrixProductWallet.IM_eSports_Wallet,
			DateFilterType = DateFilterType.IMESportsLastUpdatedTime,
			Language = "EN"
		};

		var parameters = JsonSerializer.SerializeToNode(request, _jsonOptions)!;

		var response = new
		{
			Code = "0",
			Message = "Successful.",
			Result = new[] {
				new{
					Provider="ESPORTSBULL_ESPORTS",
					GameId= "ESPORTSBULL",
					BetId= "432368314",
					WagerCreationDateTime= "2023-04-27 10:17:30 +08:00",
					LastUpdatedDate= "2023-04-27 10:17:30 +08:00",
					PlayerId= "IM1S3_4ucmqplxb029",
					ProviderPlayerId= "IM1S3IM1S3_4ucmqplxb029",
					Currency= "CNY",
					StakeAmount= "5.000000",
					WinLoss= "0.000000",
					OddsType="EURO",
					TotalOdds= "1.093",
					WagerType= "Single",
					Platform="Web",
					IsSettled="0",
					IsResettled= "0",
					IsCancelled= "0",
					SettlementDateTime= "",
					Tolerance= "255",
					FlagCategory= "",
					DetailItems= new []{
						new {
							EventName= "herO VS KeeN",
							EventDateTime= "2023-04-27 17:30:00 +08:00",
							CompetitionName= "Global StarCraft II League Season 1",
							HomeTeamName="herO",
							AwayTeamName="KeeN",
							FavTeam="",
							Period= "",
							HomeTeamHTScore= "0",
							AwayTeamHTScore= "0",
							HomeTeamFTScore= "0",
							AwayTeamFTScore="0",
							WagerHomeTeamScore= "0",
							WagerAwayTeamScore= "0",
							MarketType= "",
							Handicap="0.00",
							CancelReason= "",
							SportsID= 53,
							SportsName= "STARCRAFT2",
							BetType= "BO3 Series Win",
							EventId="12933529",
							EventType= "BO3",
							GameOrder= "0",
							Selection= "herO",
							Odds="1.093",
							IsSettled="0",
							IsResettled="0",
							IsWagerItemCancelled= "0",
							WagerItemCancelType="0",
							ResultStatus="",
							BaseTierID="0",
							BaseTierCode="0",
							BaseTierName= "Other"
						}
					}
				}
			},
			Pagination = new
			{
				CurrentPage = 1,
				TotalPage = 1,
				ItemPerPage = 5000,
				TotalCount = 1
			}
		};
		var content = JsonSerializer.Serialize(response);

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/Report/GetBetLog")
				.WithContent(parameters.ToJsonString())
				.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeSystemClock = Substitute.For<ISystemClock>();
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeSystemClock,
			fakeHttpClient);

		var actual = await client.ReportGetBetLogByIMESportsAsync(
			reportUrl: reportUrl,
			merchantCode: merchantCode,
			startDate: startDate,
			endDate: endDate,
			page: page)
			.ToArrayAsync();

		Assert.NotNull(actual);
		Assert.Equal("0", actual.Single().IsSettled);
		Assert.Equal("0", actual.Single().IsCancelled);
		Assert.Equal("0", actual.Single().IsResettled);
		Assert.Null(actual.Single().SettlementDateTime);
	}
}
