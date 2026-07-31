using CorpInsightsTW.Core.Enums;

namespace CorpInsightsTW.Core.Extensions;

public static class DateOnlyExtensions
{
    /// <summary>
    /// 根據市場類別取得預期的報表日期
    /// </summary>
    /// <returns>推算後的預期日期</returns>
    public static DateOnly GetReportDate(this DateOnly contextDate, StockMarket market)
    {
        return market switch
        {
            StockMarket.TPEX => contextDate.AddDays(-1), // 證券櫃檯買賣中心 (T-1)
            StockMarket.TWSE => contextDate,             // 臺灣證券交易所 (T)
            _ => contextDate                             // 其他市場預設為當日 (T)
        };
    }

    /// <summary>
    /// 檢查傳入的日期是否等於預期的市場日期
    /// </summary>
    public static bool IsMatchMarketDate(this DateOnly rowDate, DateOnly contextDate, StockMarket market, out DateOnly expectedDate)
    {
        expectedDate = contextDate.GetReportDate(market);
        return rowDate == expectedDate;
    }
}