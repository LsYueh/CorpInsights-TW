namespace CorpInsightsTW.Core.Extensions;

/// <summary>
/// 提供 DateOnly 的工作日 (Working Day) 與平日 (Weekday) 計算擴充方法
/// </summary>
public static class WorkingDayExtensions
{
    /// <summary>
    /// 判斷是否為工作日 (週一至週五)
    /// </summary>
    public static bool IsWeekday(this DateOnly date)
    {
        return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
    }
    
    /// <summary>
    /// [避開週末] 若目前日期為六、日，則「往前 (過去)」退到最近的週五；週一至週五則保持原日期。
    /// </summary>
    /// <remarks>
    /// 適用情境：計算「截至最近一個平日」的資料、報表產出或交易日。<br/>
    /// 範例：<br/>
    /// - 週六 (2026/08/01) -> 退回 週五 (2026/07/31)<br/>
    /// - 週日 (2026/08/02) -> 退回 週五 (2026/07/31)<br/>
    /// - 週一 (2026/08/03) -> 保持 週一 (2026/08/03)
    /// </remarks>
    public static DateOnly ToLastWeekday(this DateOnly date) => date.DayOfWeek switch
    {
        DayOfWeek.Saturday => date.AddDays(-1),
        DayOfWeek.Sunday   => date.AddDays(-2),
        _                  => date
    };

    /// <summary>
    /// [避開週末] 若目前日期為六、日，則「往後 (未來)」進到最近的週一；週一至週五則保持原日期。
    /// </summary>
    /// <remarks>
    /// 適用情境：排程執行、順延撥款日或順延履約日。<br/>
    /// 範例：<br/>
    /// - 週六 (2026/08/01) -> 順延 週一 (2026/08/03)<br/>
    /// - 週日 (2026/08/02) -> 順延 週一 (2026/08/03)<br/>
    /// - 週一 (2026/08/03) -> 保持 週一 (2026/08/03)
    /// </remarks>
    public static DateOnly ToNextWeekday(this DateOnly date) => date.DayOfWeek switch
    {
        DayOfWeek.Saturday => date.AddDays(2),
        DayOfWeek.Sunday   => date.AddDays(1),
        _                  => date
    };

    /// <summary>
    /// [避開週末與假日] 尋找目前或「往前 (過去)」最近的有效工作日。
    /// </summary>
    /// <param name="date">起始判斷日期。</param>
    /// <param name="holidays">國定假日/不交易日集合 (可選)。</param>
    /// <returns>若目前為工作日且非假日則回傳原日期；否則持續往前扣一天，直到找到有效工作日。</returns>
    /// <remarks>
    /// 適用情境：計算截至最近一個實際開盤/營業日的資料 (如台股歷史開盤日)。<br/>
    /// 範例 (假設 08/03 週一為國定假日)：<br/>
    /// - 週六 (2026/08/01) -> 退回 週五 (2026/07/31)<br/>
    /// - 週日 (2026/08/02) -> 退回 週五 (2026/07/31)<br/>
    /// - 週一 (2026/08/03，國定假日) -> 退回 週五 (2026/07/31)
    /// </remarks>
    public static DateOnly ToLastWorkingDay(this DateOnly date, IReadOnlySet<DateOnly>? holidays = null)
    {
        var current = date;

        // 若為週末或指定國定假日，就持續往前扣一天
        while (!current.IsWeekday() || (holidays != null && holidays.Contains(current)))
        {
            current = current.AddDays(-1);
        }

        return current;
    }

    /// <summary>
    /// [避開週末與假日] 尋找目前或「往後 (未來)」最近的有效工作日。
    /// </summary>
    /// <param name="date">起始判斷日期。</param>
    /// <param name="holidays">國定假日/不交易日集合 (可選)。</param>
    /// <returns>若目前為工作日且非假日則回傳原日期；否則持續往後加一天，直到找到有效工作日。</returns>
    /// <remarks>
    /// 適用情境：自動順延撥款日、履約日或任務觸發日。<br/>
    /// 範例 (假設 08/03 週一為國定假日)：<br/>
    /// - 週六 (2026/08/01) -> 順延至 週二 (2026/08/04)<br/>
    /// - 週日 (2026/08/02) -> 順延至 週二 (2026/08/04)<br/>
    /// - 週一 (2026/08/03，國定假日) -> 順延至 週二 (2026/08/04)
    /// </remarks>
    public static DateOnly ToNextWorkingDay(this DateOnly date, IReadOnlySet<DateOnly>? holidays = null)
    {
        var current = date;

        // 若為週末或指定國定假日，就持續往後加一天
        while (!current.IsWeekday() || (holidays != null && holidays.Contains(current)))
        {
            current = current.AddDays(1);
        }

        return current;
    }

    /// <summary>
    /// 檢查是否落在基準日許可落後的工作日之內
    /// </summary>
    /// <param name="date">JSON 解析出來的資料日期</param>
    /// <param name="contextDate">Context 基準日期</param>
    /// <param name="expectedDate">Out: 推算出的標準預期基準日 (Context 最新工作日)</param>
    /// <param name="minAllowedDate">Out: 最小容許的歷史工作日 (往過去退 N 個工作日)</param>
    /// <param name="maxDaysLag">允許資料最多滯後 (落後) 幾天，預設 2 天 (應付 TPEX 下午才更新資料)</param>
    /// <param name="holidays">國定假日/不交易日集合 (可選)</param>
    public static bool IsWorkingDayAcceptable(
        this DateOnly date,
        DateOnly contextDate,
        out DateOnly expectedDate,
        out DateOnly minAllowedDate,
        int maxDaysLag = 2,
        IReadOnlySet<DateOnly>? holidays = null)
    {
        return IsAcceptableCore(date, contextDate, out expectedDate, out minAllowedDate, maxDaysLag, 
            d => d.ToLastWorkingDay(holidays));
    }

    /// <summary>
    /// 檢查是否落在基準日許可落後的日曆日之內
    /// </summary>
    /// <param name="date">JSON 解析出來的資料日期</param>
    /// <param name="contextDate">Context 基準日期</param>
    /// <param name="expectedDate">Out: 推算出的標準預期基準日 (Context 最新日曆日)</param>
    /// <param name="minAllowedDate">Out: 最小容許的歷史日曆日 (往過去退 N 個日曆日)</param>
    /// <param name="maxDaysLag">允許資料最多滯後 (落後) 幾天，預設 2 天 (應付 TPEX 下午才更新資料)</param>
    public static bool IsCalendarAcceptable(
        this DateOnly date,
        DateOnly contextDate,
        out DateOnly expectedDate,
        out DateOnly minAllowedDate,
        int maxDaysLag = 2)
    {
        return IsAcceptableCore(date, contextDate, out expectedDate, out minAllowedDate, maxDaysLag, 
            d => d); // 純日曆天，不做額外調整
    }

    private static bool IsAcceptableCore(
        DateOnly date,
        DateOnly contextDate,
        out DateOnly expectedDate,
        out DateOnly minAllowedDate,
        int maxDaysLag,
        Func<DateOnly, DateOnly> adjuster)
    {
        expectedDate = adjuster(contextDate);
        minAllowedDate = expectedDate;
        
        for (int i = 0; i < maxDaysLag; i++)
        {
            // 每次往過去扣 1 天
            minAllowedDate = adjuster(minAllowedDate.AddDays(-1));
        }

        // 資料日期必須在 [minAllowedDate, expectedDate] 之間
        return minAllowedDate <= date && date <= expectedDate;
    }
}