using CorpInsightsTW.Core.Extensions;

namespace CorpInsightsTW.Tests.Core.Extensions;

[TestClass]
public class WorkingDayExtensionsTests
{
    [TestMethod]
    [DataRow(2026, 7, 31, true)]  // 週五 -> 平日
    [DataRow(2026, 8, 1, false)]  // 週六 -> 週末
    [DataRow(2026, 8, 2, false)]  // 週日 -> 週末
    [DataRow(2026, 8, 3, true)]   // 週一 -> 平日
    public void IsWeekday_ShouldIdentifyCorrectly(int year, int month, int day, bool expected)
    {
        var date = new DateOnly(year, month, day);
        Assert.AreEqual(expected, date.IsWeekday());
    }

    [TestMethod]
    public void ToLastWeekday_ShouldRollbackToFriday_WhenOnWeekend()
    {
        var saturday = new DateOnly(2026, 8, 1);
        var sunday   = new DateOnly(2026, 8, 2);
        var monday   = new DateOnly(2026, 8, 3);

        Assert.AreEqual(new DateOnly(2026, 7, 31), saturday.ToLastWeekday()); // 退回週五
        Assert.AreEqual(new DateOnly(2026, 7, 31),   sunday.ToLastWeekday()); // 退回週五
        Assert.AreEqual(new DateOnly(2026, 8,  3),   monday.ToLastWeekday()); // 保持週一
    }

    [TestMethod]
    public void ToNextWeekday_ShouldAdvanceToMonday_WhenOnWeekend()
    {
        var saturday = new DateOnly(2026, 8, 1);
        var sunday   = new DateOnly(2026, 8, 2);
        var monday   = new DateOnly(2026, 8, 3);

        Assert.AreEqual(new DateOnly(2026, 8, 3), saturday.ToNextWeekday()); // 順延週一
        Assert.AreEqual(new DateOnly(2026, 8, 3),   sunday.ToNextWeekday()); // 順延週一
        Assert.AreEqual(new DateOnly(2026, 8, 3),   monday.ToNextWeekday()); // 保持週一
    }

    [TestMethod]
    public void ToLastWorkingDay_WithHolidays_ShouldSkipWeekendAndHolidays()
    {
        // 模擬情境：
        // 2026/10/08 (週四) -> 工作日
        // 2026/10/09 (週五) -> 國定假日 (Holiday)
        // 2026/10/10 (週六) -> 週末
        // 2026/10/11 (週日) -> 週末
        var holidays = new HashSet<DateOnly>
        {
            new(2026, 10, 9) // 10/09 放假
        };

        var sunday = new DateOnly(2026, 10, 11);

        // 從週日往前找最近的工作日：自動跳過 10/11(日)、10/10(六)、10/09(假)，最後找到 10/08(四)
        var lastWorkingDay = sunday.ToLastWorkingDay(holidays);

        Assert.AreEqual(new DateOnly(2026, 10, 8), lastWorkingDay);
    }

    [TestMethod]
    public void ToNextWorkingDay_WithHolidays_ShouldSkipWeekendAndHolidays()
    {
        // 模擬情境：
        // 2026/10/09 (週五) -> 國定假日 (Holiday)
        // 2026/10/10 (週六) -> 週末
        // 2026/10/11 (週日) -> 週末
        // 2026/10/12 (週一) -> 工作日
        var holidays = new HashSet<DateOnly>
        {
            new(2026, 10, 9)
        };

        var fridayHoliday = new DateOnly(2026, 10, 9);

        // 從週五(假日)往後尋找最近的工作日：自動跳過 10/09(假)、10/10(六)、10/11(日)，最後找到 10/12(一)
        var nextWorkingDay = fridayHoliday.ToNextWorkingDay(holidays);

        Assert.AreEqual(new DateOnly(2026, 10, 12), nextWorkingDay);
    }
}