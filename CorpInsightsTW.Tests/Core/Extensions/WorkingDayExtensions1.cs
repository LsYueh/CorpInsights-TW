using CorpInsightsTW.Core.Extensions;

namespace CorpInsightsTW.Tests.Core.Extensions;

[TestClass]
public class WorkingDayExtensionsTests1
{
    // 1. 一般平日測試 (無假日)
    
    [TestMethod]
    public void IsWorkingDay_StandardWeekday_ExactMatch_ShouldReturnTrue()
    {
        // Arrange: 2026/07/29 (週三)
        var contextDate = new DateOnly(2026, 7, 29);
        var rowDate = new DateOnly(2026, 7, 29);

        // Act
        bool result = rowDate.IsWorkingDayAcceptable(contextDate, out var expDate, out var minAllowedDate, maxDaysLag: 2);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(new DateOnly(2026, 7, 29), expDate);
        Assert.AreEqual(new DateOnly(2026, 7, 27), minAllowedDate); // 往前扣 2 個工作日 -> 週一 (07/27)
    }

    [TestMethod]
    public void IsWorkingDay_StandardWeekday_TMinusOne_ShouldReturnTrue()
    {
        // Arrange: Context 為 2026/07/29 (週三)，資料為 2026/07/28 (週二) -> 模擬櫃買(TPEX)少一天
        var contextDate = new DateOnly(2026, 7, 29);
        var rowDate = new DateOnly(2026, 7, 28);

        // Act
        bool result = rowDate.IsWorkingDayAcceptable(contextDate, out _, out _, maxDaysLag: 2);

        // Assert
        Assert.IsTrue(result);
    }

    // 邊界條件：未來的日期 (Future Date Leak)

    [TestMethod]
    public void IsWorkingDay_FutureDate_ShouldReturnFalse()
    {
        // Arrange: Context 為 2026/07/29 (週三)，資料為未來的 2026/07/30 (週四)
        var contextDate = new DateOnly(2026, 7, 29);
        var rowDate = new DateOnly(2026, 7, 30);

        // Act
        bool result = rowDate.IsWorkingDayAcceptable(contextDate, out var expDate, out _, maxDaysLag: 2);

        // Assert
        Assert.IsFalse(result, "未來的資料絕不能通過比對");
        Assert.AreEqual(new DateOnly(2026, 7, 29), expDate);
    }

    // 跨週末與 Context 落在週末測試

    [TestMethod]
    public void IsWorkingDay_ContextIsMonday_TPexTMinusOne_ShouldCrossWeekendCorrectly()
    {
        // Arrange: Context 為 2026/08/03 (週一)
        // expectedDate 應為 2026/08/03 (週一)
        // 櫃買資料為 2026/07/31 (上週五，即 T-1 交易日)
        var contextDate = new DateOnly(2026, 8, 3);
        var rowDate = new DateOnly(2026, 7, 31);

        // Act
        bool result = rowDate.IsWorkingDayAcceptable(contextDate, out var expDate, out var minAllowedDate, maxDaysLag: 2);

        // Assert
        Assert.IsTrue(result, "週一 Context 配上週五的資料是正常的交易日銜接");
        Assert.AreEqual(new DateOnly(2026, 8, 3), expDate);
        Assert.AreEqual(new DateOnly(2026, 7, 30), minAllowedDate); // 08/03(一) -> 扣1工作日=07/31(五) -> 扣2工作日=07/30(四)
    }

    [TestMethod]
    public void IsWorkingDay_ContextIsSunday_ShouldAlignToLastFriday()
    {
        // Arrange: Context 設在 2026/08/02 (週日)
        // expectedDate 會自動推算為最近的營業日 2026/07/31 (週五)
        var contextDate = new DateOnly(2026, 8, 2);
        var rowDate = new DateOnly(2026, 7, 31);

        // Act
        bool result = rowDate.IsWorkingDayAcceptable(contextDate, out var expDate, out _, maxDaysLag: 2);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(new DateOnly(2026, 7, 31), expDate);
    }

    // 跨月與國定假日 (Holidays) 綜合測試

    [TestMethod]
    public void IsWorkingDay_CrossMonthAndHolidays_ShouldCalculateMinAllowedDateCorrectly()
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
        bool resultValid = rowDateWithinRange.IsWorkingDayAcceptable(
            contextDate, 
            out var expDate, 
            out var minAllowedDate, 
            maxDaysLag: 2, 
            holidays: holidays);

        Assert.IsTrue(resultValid);
        Assert.AreEqual(new DateOnly(2026, 7, 31), expDate);
        Assert.AreEqual(new DateOnly(2026, 7, 29), minAllowedDate);

        // Act & Assert 2: 太舊的資料
        bool resultInvalid = rowDateTooOld.IsWorkingDayAcceptable(
            contextDate, 
            out _, 
            out _, 
            maxDaysLag: 2, 
            holidays: holidays);

        Assert.IsFalse(resultInvalid, "超過 2 個工作日前的資料應該被剔除");
    }

    [TestMethod]
    public void IsCalendarAcceptable_WithWeekendContextDate_ShouldCalculateCorrectCalendarRange()
    {
        // Arrange: 假設基準日為 2026-08-02 (星期日)
        DateOnly contextDate = new(2026, 8, 2);
        int maxDaysLag = 2;

        // 測試資料：2026-08-01 (星期六，落後 1 天，屬於允許範圍內)
        DateOnly validDate = new(2026, 8, 1);
        
        // 測試資料：2026-07-30 (星期四，落後 3 天，超出預設 2 天範圍)
        DateOnly invalidDate = new(2026, 7, 30);

        // Act & Assert - 驗證有效日期
        bool isValid = validDate.IsCalendarAcceptable(
            contextDate, 
            out DateOnly expDate, 
            out DateOnly minAllowedDate, 
            maxDaysLag);

        Assert.IsTrue(isValid);
        Assert.AreEqual(new DateOnly(2026, 8, 2), expDate);
        // 純日曆天扣 2 天：08/02 -> 08/01 -> 07/31
        Assert.AreEqual(new DateOnly(2026, 7, 31), minAllowedDate); 

        // Act & Assert - 驗證超出範圍的日期
        bool isInvalid = invalidDate.IsCalendarAcceptable(
            contextDate, 
            out _, 
            out _, 
            maxDaysLag);

        Assert.IsFalse(isInvalid);
    }
}