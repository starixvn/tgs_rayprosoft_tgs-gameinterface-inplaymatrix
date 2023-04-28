using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;
using Tgs.GameInterface.InplayMatrix.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.InplayMatrix.GrpcServices
{
    public class GameInterfaceInplayMatrixGrpcService : GameInterfaceInplayMatrix.GameInterfaceInplayMatrixBase
    {
        private readonly IGetService<FundInQuery, FundResult> _fundInService;
        private readonly IGetService<FundOutQuery, FundResult> _fundOutService;
        private readonly IGetService<FundStatusQuery, bool> _fundStatusService;
        private readonly IQueryService<GameRecordByIMESportsQuery, GameRecordByIMESportsResult> _gameRecordByIMESportsService;
        private readonly IGetService<GameUrlQuery, GameUrlResult> _gameUrlService;
        private readonly ILogger<GameInterfaceInplayMatrixGrpcService> _logger;
        private readonly IGetService<PlayerBalanceQuery, PlayerBalanceResult> _playerBalanceService;
        private readonly ICommandService<PlayerCreateCommand> _playerCreateService;

        public GameInterfaceInplayMatrixGrpcService(
            ILogger<GameInterfaceInplayMatrixGrpcService> logger,
            IGetService<FundInQuery, FundResult> fundInService,
            IGetService<FundOutQuery, FundResult> fundOutService,
            IGetService<FundStatusQuery, bool> fundStatusService,
            IQueryService<GameRecordByIMESportsQuery, GameRecordByIMESportsResult> gameRecordByIMESportsService,
            IGetService<GameUrlQuery, GameUrlResult> gameUrlService,
            IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
            ICommandService<PlayerCreateCommand> playerCreateService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fundInService = fundInService ?? throw new ArgumentNullException(nameof(fundInService));
            _fundOutService = fundOutService ?? throw new ArgumentNullException(nameof(fundOutService));
            _fundStatusService = fundStatusService ?? throw new ArgumentNullException(nameof(fundStatusService));
            _gameRecordByIMESportsService = gameRecordByIMESportsService ?? throw new ArgumentNullException(nameof(gameRecordByIMESportsService));
            _gameUrlService = gameUrlService ?? throw new ArgumentNullException(nameof(gameUrlService));
            _playerBalanceService = playerBalanceService ?? throw new ArgumentNullException(nameof(playerBalanceService));
            _playerCreateService = playerCreateService ?? throw new ArgumentNullException(nameof(playerCreateService));
        }

        public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
        {
            _logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

            return Task.FromResult(new CheckingServiceReply
            {
                MachineName = Environment.MachineName
            });
        }

        public override async Task<FundReply> FundIn(
            FundRequest request,
            ServerCallContext context)
        {
            var query = new FundInQuery(
                ApiUrl: new(request.ApiUrl),
                MerchantCode: request.MerchantCode,
                MerchantPrefix: request.MerchantPrefix,
                PlayerCode: request.PlayerCode,
                ProductWallet: request.ProductWallet,
                TransactionId: request.TransactionId,
                Amount: request.Amount);

            var result = await _fundInService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new FundReply
            {
                TransactionId = result.TransactionId,
                Res = result.Res
            };
        }

        public override async Task<FundReply> FundOut(
            FundRequest request,
            ServerCallContext context)
        {
            var query = new FundOutQuery(
                ApiUrl: new(request.ApiUrl),
                MerchantCode: request.MerchantCode,
                MerchantPrefix: request.MerchantPrefix,
                PlayerCode: request.PlayerCode,
                ProductWallet: request.ProductWallet,
                TransactionId: request.TransactionId,
                Amount: request.Amount);

            var result = await _fundOutService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new FundReply
            {
                TransactionId = result.TransactionId,
                Res = result.Res
            };
        }

        public override async Task<BaseReply> FundStatus(
            FundStatusRequest request,
            ServerCallContext context)
        {
            var query = new FundStatusQuery(
                ApiUrl: new(request.ApiUrl),
                MerchantCode: request.MerchantCode,
                MerchantPrefix: request.MerchantPrefix,
                PlayerCode: request.PlayerCode,
                ProductWallet: request.ProductWallet,
                TransactionId: request.TransactionId);

            var result = await _fundStatusService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new BaseReply
            {
                State = result
            };
        }

        public override async Task<GameRecordByIMESportsReply> GameRecordByIMESports(
            GameRecordByIMESportsRequest request,
            ServerCallContext context)
        {
            var query = new GameRecordByIMESportsQuery(
                ReportUrl: new(request.ReportUrl),
                MerchantCode: request.MerchantCode,
                MerchantPrefix: request.MerchantPrefix,
                StartDate: request.StartDate.ToDateTimeOffset(),
                EndDate: request.EndDate.ToDateTimeOffset(),
                Page: request.Page);

            var results = await _gameRecordByIMESportsService
                .QueryAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ToArrayAsync(
                    context.CancellationToken)
                .ConfigureAwait(false)
                ?? Enumerable.Empty<GameRecordByIMESportsResult>();

            var reply = new GameRecordByIMESportsReply();

            if (results.Any())
            {
                foreach (var result in results)
                {
                    reply.Items.Add(new GameRecordByIMESportsInfo
                    {
                        ExternalRoundId = result.ExternalRoundId,
                        ExternalGameId = result.ExternalGameId,
                        PlayerCode = result.PlayerCode,
                        IsSettled = result.IsSettled,
                        IsCancelled = result.IsCancelled,
                        IsResettled = result.IsResettled,
                        ExternalCreatedOn = result.ExternalCreatedOn.ToTimestamp(),
                        ExternalClosedOn = result.ExternalClosedOn.GetValueOrDefault(DateTimeOffset.UnixEpoch).ToTimestamp(),
                        Bet = result.Bet,
                        Settle = result.Settle,
                        WinLoss = result.WinLoss,
                        AdditionalData = result.AdditionalData
                    });
                }
            }

            return reply;
        }

        public override async Task<GameUrlReply> GameUrl(
            GameUrlRequest request,
            ServerCallContext context)
        {
            var query = new GameUrlQuery(
                ApiUrl: new(request.ApiUrl),
                MerchantCode: request.MerchantCode,
                MerchantPrefix: request.MerchantPrefix,
                PlayerCode: request.PlayerCode,
                ProductWallet: request.ProductWallet,
                GameCode: request.GameCode,
                IpAddress: request.IpAddress,
                IsMobile: request.IsMobile,
                Language: request.Language);

            var result = await _gameUrlService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new GameUrlReply
            {
                IsPlayerExist = result.IsPlayerExist,
                GameUrl = result.GameUrl
            };
        }

        public override async Task<PlayerBalanceReply> PlayerBalance(
            PlayerBalanceRequest request,
            ServerCallContext context)
        {
            var query = new PlayerBalanceQuery(
                ApiUrl: new(request.ApiUrl),
                MerchantCode: request.MerchantCode,
                MerchantPrefix: request.MerchantPrefix,
                PlayerCode: request.PlayerCode,
                ProductWallet: request.ProductWallet);

            var result = await _playerBalanceService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new PlayerBalanceReply
            {
                IsPlayerExist = result.IsPlayerExist,
                Balance = result.Balance
            };
        }

        public override async Task<BaseReply> PlayerCreate(
            PlayerCreateRequest request,
            ServerCallContext context)
        {
            var command = new PlayerCreateCommand(
                ApiUrl: new(request.ApiUrl),
                MerchantCode: request.MerchantCode,
                MerchantPrefix: request.MerchantPrefix,
                PlayerCode: request.PlayerCode,
                Currency: request.Currency,
                Password: request.Password);

            await _playerCreateService
                .ExecuteAsync(
                    command: command,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new BaseReply
            {
                State = true
            };
        }
    }
}
