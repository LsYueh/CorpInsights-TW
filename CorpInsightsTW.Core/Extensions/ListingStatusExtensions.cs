using CorpInsightsTW.Core.Enums;

namespace CorpInsightsTW.Core.Extensions;

public static class ListingStatusExtensions
{
    /// <summary>
    /// 根據股票市場，將 ListingStatus 展開為實際包含的個別狀態集合
    /// </summary>
    public static IEnumerable<ListingStatus> ExpandForMarket(this ListingStatus status, StockMarket market)
    {
        if (status != ListingStatus.All)
        {
            // 如果指定了特定狀態 (如 L, X, O, U)，直接傳回該狀態
            yield return status;
            yield break;
        }

        // 當 status 為 All 時，根據市場展開該市場專屬的所有狀態
        switch (market)
        {
            case StockMarket.TWSE:
                yield return ListingStatus.L; // 上市
                yield return ListingStatus.X; // 公發
                break;

            case StockMarket.TPEX:
                yield return ListingStatus.O; // 上櫃
                yield return ListingStatus.U; // 興櫃
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(market), market, "未知的股票市場");
        }
    }
}