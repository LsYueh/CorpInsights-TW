using CorpInsightsTW.Core.Attributes;

namespace CorpInsightsTW.Core.Enums;

/// <summary>
/// 股票市場
/// </summary>
public enum StockMarket
{
    /// <summary> 全市場 </summary>
    [Code("all"), Display("全市場")]
    All = 0,

    /// <summary> 臺灣證券交易所 </summary>
    [Code("TWSE"), Display("臺灣證券交易所")]
    TWSE = 1,

    /// <summary> 證券櫃檯買賣中心 </summary>
    [Code("TPEX"), Display("證券櫃檯買賣中心")]
    TPEX = 2,
}