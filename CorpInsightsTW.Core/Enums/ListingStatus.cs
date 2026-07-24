using CorpInsightsTW.Core.Attributes;

namespace CorpInsightsTW.Core.Enums;

/// <summary>
/// 上市狀態 TSE
/// </summary>
public enum ListingStatus
{
    /// <summary> 全狀態 </summary>
    [Code("all"), Display("全上市狀態")]
    All = 0,

    /// <summary> 上市公司 </summary>
    [Code("L"), Display("上市公司")]
    L = 1,

    /// <summary> 公開發行公司 </summary>
    [Code("X"), Display("公發公司")]
    X = 2,
}

/// <summary>
/// 上市狀態 OTC
/// </summary>
public enum ListingStatusOTC
{
    /// <summary> 全狀態 </summary>
    [Code("all"), Display("全上市狀態")]
    All = 0,

    /// <summary> 上櫃公司 </summary>
    [Code("O"), Display("上櫃公司")]
    O = 1,

    /// <summary> 興櫃公司 </summary>
    [Code("U"), Display("興櫃公司")]
    U = 2,
}

