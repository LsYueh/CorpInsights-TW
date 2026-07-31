using CorpInsightsTW.Core.Attributes;

namespace CorpInsightsTW.Core.Enums;

/// <summary>
/// 財務報表類型
/// </summary>
public enum StatementType
{
    /// <summary> 全財務報表 </summary>
    [Code("all"), Display("全財務報表")]
    All = 0,

    /// <summary> 綜合損益表 (OpenAPI: application/json)</summary>
    [Code("t187ap06"), Display("綜合損益表")]
    T187AP06 = 1,

    /// <summary> 資產負債表 (OpenAPI: application/json)</summary>
    [Code("t187ap07"), Display("資產負債表")]
    T187AP07 = 2,

    /// <summary> 現金流量表 (MOPS: text/html)</summary>
    [Code("t163sb20"), Display("現金流量表")]
    T163SB20,
}