namespace CorpInsightsTW.Core.Extensions;
public static class DateOnlyExtensions
{
    /// <summary>
    /// 若為週末 (六日)，自動往前遞補至最近的週五 (工作日)
    /// </summary>
    public static DateOnly ToLastWeekday(this DateOnly date) => date.DayOfWeek switch
    {
        DayOfWeek.Saturday => date.AddDays(-1),
        DayOfWeek.Sunday   => date.AddDays(-2),
        _                  => date
    };
}