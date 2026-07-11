using System.ComponentModel;

namespace CorpInsightsTW.Core.Enums;

/// <summary>
/// 上市狀態
/// </summary>
public enum ListingStatus
{
    /// <summary> 全狀態 </summary>
    All = 0,

    /// <summary> 上市公司 </summary>
    [Description("L")]
    Listed = 1,

    /// <summary> 上櫃公司 </summary>
    // [Description("")]
    // OverTheCounter = 2,

    /// <summary> 興櫃公司 </summary>
    // [Description("")]
    // Emerging = 3,

    /// <summary> 公開發行公司 </summary>
    [Description("X")]
    PublicOffering = 4
}
