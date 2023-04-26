namespace Tgs.GameInterface.InplayMatrix.Core;

public enum DateFilterType
{
    /// <summary>
    /// 下注时间 (Bet Date 注单的下注时间)
    /// </summary>
    BetTime = 1,

    /// <summary>
    /// 比赛时间 (Event Date 所投注比赛的开赛时间)
    /// </summary>
    GameTime = 2,

    /// <summary>
    /// The IM ESports last updated time
    /// </summary>
    IMESportsLastUpdatedTime = 3,

    /// <summary>
    /// The IM Sportsbook last updated time
    /// </summary>
    IMSportsbookLastUpdatedTime = 4,
}
