namespace CorpInsightsTW.Core.Extensions;

public static class FilingExtensions
{
    /// <summary>
    /// 根據台灣上市櫃公司財報申報截止日，計算指定日期目前處於哪一個申報季度 (1~4)
    /// </summary>
    /// <param name="date">要判斷的日期</param>
    /// <returns>1: 第一季 (Q1), 2: 第二季 (Q2), 3: 第三季 (Q3), 4: 第四季/年度 (Q4)</returns>
    /// <remarks>
    /// 申報截止時間區間：<br/>
    /// - 01/01 ~ 03/31：申報前一年 Q4 (截止日 03/31) -> 回傳 4<br/>
    /// - 04/01 ~ 05/15：申報當年 Q1 (截止日 05/15) -> 回傳 1<br/>
    /// - 05/16 ~ 08/14：申報當年 Q2 (截止日 08/14) -> 回傳 2<br/>
    /// - 08/15 ~ 11/14：申報當年 Q3 (截止日 11/14) -> 回傳 3<br/>
    /// - 11/15 ~ 12/31：進入當年 Q4/年度 申報期 (截止日隔年 03/31) -> 回傳 4
    /// </remarks>
    public static int GetFilingQuarter(this DateOnly date)
    {
        var year = date.Year;

        // 01/01 ~ 03/31：申報「前一年度 (Q4)」財報（截止日 03/31）
        if (date <= new DateOnly(year, 3, 31)) return 4;

        // 04/01 ~ 05/15：申報「第一季 (Q1)」財報（截止日 05/15）
        if (date <= new DateOnly(year, 5, 15)) return 1;

        // 05/16 ~ 08/14：申報「第二季 (Q2)」財報（截止日 08/14）
        if (date <= new DateOnly(year, 8, 14)) return 2;

        // 08/15 ~ 11/14：申報「第三季 (Q3)」財報（截止日 11/14）
        if (date <= new DateOnly(year, 11, 14)) return 3;

        // 11/15 ~ 12/31：進入「當年度 (Q4)」財報申報期（截止日為隔年 03/31）
        return 4;
    }
}