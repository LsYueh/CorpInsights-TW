using CorpInsightsTW.Core.Attributes;

namespace CorpInsightsTW.Core.Enums;

public enum T187ApCode
{
    /// <summary> 全財務報表 </summary>
    [Code("all"), Display("全財務報表")]
    All = 0,

    /// <summary> 綜合損益表 </summary>
    [Code("t187ap06"), Display("綜合損益表")]
    T187AP06 = 1,

    /// <summary> 資產負債表 </summary>
    [Code("t187ap07"), Display("資產負債表")]
    T187AP07 = 2
}