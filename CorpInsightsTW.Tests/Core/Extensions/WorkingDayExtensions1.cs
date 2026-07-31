using CorpInsightsTW.Core.Extensions;

namespace CorpInsightsTW.Tests.Core.Extensions;

[TestClass]
public class WorkingDayExtensionsTests1
{
    // 1. 一般平日測試 (無假日)
    
    [TestMethod]
    public void IsMatchMarketDate_StandardWeekday_ExactMatch_ShouldReturnTrue()
    {
        // Arrange: 2026/07/29 (週三)
        var contextDate = new DateOnly(2026, 7, 29);
        var rowDate = new DateOnly(2026, 7, 29);

        // Act
        bool result = rowDate.IsAcceptable(contextDate, out var expectedDate, out var minAllowedDate, maxDaysLag: 2);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(new DateOnly(2026, 7, 29), expectedDate);
        Assert.AreEqual(new DateOnly(2026, 7, 27), minAllowedDate); // 往前扣 2 個工作日 -> 週一 (07/27)
    }

    [TestMethod]
    public void IsMatchMarketDate_StandardWeekday_TMinusOne_ShouldReturnTrue()
    {
        // Arrange: Context 為 2026/07/29 (週三)，資料為 2026/07/28 (週二) -> 模擬櫃買(TPEX)少一天
        var contextDate = new DateOnly(2026, 7, 29);
        var rowDate = new DateOnly(2026, 7, 28);

        // Act
        bool result = rowDate.IsAcceptable(contextDate, out _, out _, maxDaysLag: 2);

        // Assert
        Assert.IsTrue(result);
    }

    // 邊界條件：未來的日期 (Future Date Leak)

    [TestMethod]
    public void IsMatchMarketDate_FutureDate_ShouldReturnFalse()
    {
        // Arrange: Context 為 2026/07/29 (週三)，資料為未來的 2026/07/30 (週四)
        var contextDate = new DateOnly(2026, 7, 29);
        var rowDate = new DateOnly(2026, 7, 30);

        // Act
        bool result = rowDate.IsAcceptable(contextDate, out var expectedDate, out _, maxDaysLag: 2);

        // Assert
        Assert.IsFalse(result, "未來的資料絕不能通過比對");
        Assert.AreEqual(new DateOnly(2026, 7, 29), expectedDate);
    }

    // 跨週末與 Context 落在週末測試

    [TestMethod]
    public void IsMatchMarketDate_ContextIsMonday_TPexTMinusOne_ShouldCrossWeekendCorrectly()
    {
        // Arrange: Context 為 2026/08/03 (週一)
        // expectedDate 應為 2026/08/03 (週一)
        // 櫃買資料為 2026/07/31 (上週五，即 T-1 交易日)
        var contextDate = new DateOnly(2026, 8, 3);
        var rowDate = new DateOnly(2026, 7, 31);

        // Act
        bool result = rowDate.IsAcceptable(contextDate, out var expectedDate, out var minAllowedDate, maxDaysLag: 2);

        // Assert
        Assert.IsTrue(result, "週一 Context 配上週五的資料是正常的交易日銜接");
        Assert.AreEqual(new DateOnly(2026, 8, 3), expectedDate);
        Assert.AreEqual(new DateOnly(2026, 7, 30), minAllowedDate); // 08/03(一) -> 扣1工作日=07/31(五) -> 扣2工作日=07/30(四)
    }

    [TestMethod]
    public void IsMatchMarketDate_ContextIsSunday_ShouldAlignToLastFriday()
    {
        // Arrange: Context 設在 2026/08/02 (週日)
        // expectedDate 會自動推算為最近的營業日 2026/07/31 (週五)
        var contextDate = new DateOnly(2026, 8, 2);
        var rowDate = new DateOnly(2026, 7, 31);

        // Act
        bool result = rowDate.IsAcceptable(contextDate, out var expectedDate, out _, maxDaysLag: 2);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(new DateOnly(2026, 7, 31), expectedDate);
    }

    // 跨月與國定假日 (Holidays) 綜合測試

    [TestMethod]
    public void IsMatchMarketDate_CrossMonthAndHolidays_ShouldCalculateMinAllowedDateCorrectly()
    {
        // Arrange:
        // 假設 2026/08/03 (週一) 是國定假日
        var holidays = new HashSet<DateOnly>
        {
            new(2026, 8, 3) // 8/3 (一) 國定假日
        };

        // Context 傳入 2026/08/03 (週一，但非工作日)
        // expectedDate 經由 ToLastWorkingDay 會退回 2026/07/31 (週五)
        var contextDate = new DateOnly(2026, 8, 3);
        
        // 扣第 1 個工作日 -> 2026/07/30 (週四)
        // 扣第 2 個工作日 -> 2026/07/29 (週三)
        var rowDateWithinRange = new DateOnly(2026, 7, 29); // 剛好在邊界上
        var rowDateTooOld = new DateOnly(2026, 7, 28);       // 超出 Lag 範圍

        // Act & Assert 1: 合法範圍內
        bool resultValid = rowDateWithinRange.IsAcceptable(
            contextDate, 
            out var expectedDate, 
            out var minAllowedDate, 
            maxDaysLag: 2, 
            holidays: holidays);

        Assert.IsTrue(resultValid);
        Assert.AreEqual(new DateOnly(2026, 7, 31), expectedDate);
        Assert.AreEqual(new DateOnly(2026, 7, 29), minAllowedDate);

        // Act & Assert 2: 太舊的資料
        bool resultInvalid = rowDateTooOld.IsAcceptable(
            contextDate, 
            out _, 
            out _, 
            maxDaysLag: 2, 
            holidays: holidays);

        Assert.IsFalse(resultInvalid, "超過 2 個工作日前的資料應該被剔除");
    }
}