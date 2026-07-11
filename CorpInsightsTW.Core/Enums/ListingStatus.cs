using CorpInsightsTW.Core.Attributes;

namespace CorpInsightsTW.Core.Enums;

/// <summary>
/// 上市狀態
/// </summary>
public enum ListingStatus
{
    /// <summary> 全狀態 </summary>
    [Code("all"), Display("全上市狀態")]
    All = 0,

    /// <summary> 上市公司 </summary>
    [Code("L"), Display("上市公司")]
    Listed = 1,

    /// <summary> 上櫃公司 </summary>
    // [Code(""), Display("上櫃公司")]
    // OverTheCounter = 2,

    /// <summary> 興櫃公司 </summary>
    // [Code(""), Display("興櫃公司")]
    // Emerging = 3,

    /// <summary> 公開發行公司 </summary>
    [Code("X"), Display("公開發行公司")]
    PublicOffering = 4,
}
