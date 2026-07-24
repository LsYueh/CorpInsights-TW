using CorpInsightsTW.Core.Attributes;

namespace CorpInsightsTW.Core.Enums._ListingStatus;

/// <summary>
/// 上櫃狀態 Tpex
/// </summary>
public enum Tpex
{
    /// <summary> 全狀態 </summary>
    [Code("all"), Display("全櫃狀態")]
    All = 0,

    /// <summary> 上櫃公司 </summary>
    [Code("O"), Display("上櫃公司")]
    O = 1,

    /// <summary> 興櫃公司 </summary>
    [Code("U"), Display("興櫃公司")]
    U = 2,
}
