using CorpInsightsTW.Core.Attributes;

namespace CorpInsightsTW.Core.Enums._ListingStatus;

/// <summary>
/// 上市狀態 Twse
/// </summary>
public enum Twse
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
