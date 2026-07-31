
using CorpInsightsTW.Core.Attributes;

namespace CorpInsightsTW.Core.Enums;

/// <summary>
/// XBRL 的財務報告分類 
/// (請參考：XBRL計畫暨IFRSs財務報告申報注意事項)
/// </summary>
public enum XbrlTaxonomy
{
    /// <summary> 全部財報分類 </summary>
    [Code("all"), Display("全部財報分類")]
    All = 0,

    /// <summary> 金融業 </summary>
    [Code("basi"), Display("金融業")]
    BASI,

    /// <summary> 證券期貨業 </summary>
    [Code("bd"), Display("證券期貨業")]
    BD,

    /// <summary> 一般行業 </summary>
    [Code("ci"), Display("一般行業")]
    CI,

    /// <summary> 金控業 </summary>
    [Code("fh"), Display("金控業")]
    FH,

    /// <summary> 保險業 </summary>
    [Code("ins"), Display("保險業")]
    INS,

    /// <summary> 異業別合併 </summary>
    [Code("mim"), Display("異業別合併")]
    MIM,
}