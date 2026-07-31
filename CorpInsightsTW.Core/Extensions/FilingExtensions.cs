namespace CorpInsightsTW.Core.Extensions;

public static class FilingExtensions
{
    /// <summary>
    /// 根據台灣上市櫃公司財報申報截止日，取得目前最新已截止申報/可取得的 (財報年份, 財報季度)
    /// </summary>
    /// <param name="date">要判斷的日期</param>
    /// <returns>(Year: 財報西元年, Quarter: 1~4)</returns>
    /// <remarks>
    /// 申報截止時間對照表：<br/>
    /// - 01/01 ~ 03/31：Q4 申報中，最新確定可取得為「前一年 Q3」<br/>
    /// - 04/01 ~ 05/15：Q4 申報完畢，最新可取得為「前一年 Q4」<br/>
    /// - 05/16 ~ 08/14：Q1 申報完畢，最新可取得為「當年 Q1」<br/>
    /// - 08/15 ~ 11/14：Q2 申報完畢，最新可取得為「當年 Q2」<br/>
    /// - 11/15 ~ 12/31：Q3 申報完畢，最新可取得為「當年 Q3」
    /// </remarks>
    public static (int Year, int Quarter) GetFilingPeriod(this DateOnly date)
    {
        int year = date.Year;

        // 01/01 ~ 03/31：Q4 財報尚未全數申報完畢，最新確定有的資料是「前一年 Q3」
        if (date <= new DateOnly(year, 3, 31))
            return (year - 1, 3);

        // 04/01 ~ 05/15：Q4 財報已申報完畢 (截止日 03/31) -> 「前一年 Q4」
        if (date <= new DateOnly(year, 5, 15))
            return (year - 1, 4);

        // 05/16 ~ 08/14：Q1 財報已申報完畢 (截止日 05/15) -> 「當年 Q1」
        if (date <= new DateOnly(year, 8, 14))
            return (year, 1);

        // 08/15 ~ 11/14：Q2 財報已申報完畢 (截止日 08/14) -> 「當年 Q2」
        if (date <= new DateOnly(year, 11, 14))
            return (year, 2);

        // 11/15 ~ 12/31：Q3 財報已申報完畢 (截止日 11/14) -> 「當年 Q3」
        return (year, 3);
    }

    /// <summary>
    /// 取得指定日期目前對應的申報財報季度 (1~4)
    /// </summary>
    public static int GetFilingQuarter(this DateOnly date)
    {
        return date.GetFilingPeriod().Quarter;
    }

    /// <summary>
    /// 取得指定日期目前對應的申報財報西元年
    /// </summary>
    public static int GetFilingYear(this DateOnly date)
    {
        return date.GetFilingPeriod().Year;
    }
}