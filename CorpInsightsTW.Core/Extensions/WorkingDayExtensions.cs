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
    /// 往前尋找最近的有效工作日 (跳過六日與指定的國定假日)
    /// </summary>
    public static DateOnly ToLastWorkingDay(this DateOnly date, HashSet<DateOnly>? holidays = null)
    {
        var current = date;
        
        // 如果是週末或是國定假日，就一直往前扣一天
        while (!current.IsWeekday() || (holidays != null && holidays.Contains(current)))
        {
            current = current.AddDays(-1);
        }
        
        return current;
    }
}