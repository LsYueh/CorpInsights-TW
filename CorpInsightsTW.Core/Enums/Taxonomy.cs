using System.ComponentModel;

namespace CorpInsightsTW.Core.Enums;

/// <summary>
/// 財務報告分類 
/// (請參考：XBRL計畫暨IFRSs財務報告申報注意事項)
/// </summary>
public enum Taxonomy
{
    /// <summary> 全行業 </summary>
    All = 0,

    /// <summary> 一般行業 </summary>
    [Description("ci")]
    General = 1,

    /// <summary> 金融業 </summary>
    [Description("basi")]
    Banking = 2,

    /// <summary> 證券期貨業 </summary>
    [Description("bd")]
    Securities = 3,

    /// <summary> 金控業 </summary>
    [Description("fh")]
    Holding = 4,

    /// <summary> 保險業 </summary>
    [Description("ins")]
    Insurance = 5,

    /// <summary> 異業別合併 </summary>
    [Description("mim")]
    CrossIndustry = 6
}