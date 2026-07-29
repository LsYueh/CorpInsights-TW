namespace CorpInsightsTW.Core;

public class Utils()
{
    /// <summary>
    /// 根據指定日期（預設為今天）計算目前處於哪一個申報季度 (1~4)
    /// </summary>
    /// <param name="currentDate">要判斷的日期，若不傳則預設為今日 (DateTime.Today)</param>
    /// <returns>1: 第一季 (Q1), 2: 第二季 (Q2), 3: 第三季 (Q3), 4: 第四季/年度 (Q4)</returns>
    public static int GetCurrentFilingQuarter(DateTime? currentDate = null)
    {
        // 取得目標日期（不含時間 component）
        var today = (currentDate ?? DateTime.Today).Date;
        var year = today.Year;

        // 01/01 ~ 03/31：申報「前一年度 (Q4)」財報（截止日 03/31）
        if (today <= new DateTime(year, 3, 31)) return 4;

        // 04/01 ~ 05/15：申報「第一季 (Q1)」財報（截止日 05/15）
        if (today <= new DateTime(year, 5, 15)) return 1;

        // 05/16 ~ 08/14：申報「第二季 (Q2)」財報（截止日 08/14）
        if (today <= new DateTime(year, 8, 14)) return 2;

        // 08/15 ~ 11/14：申報「第三季 (Q3)」財報（截止日 11/14）
        if (today <= new DateTime(year, 11, 14)) return 3;
        
        // 11/15 ~ 12/31：進入「當年度 (Q4)」財報申報期（截止日為隔年 03/31）
        return 4;
    }
}
