using CorpInsightsTW.Etl.Pipeline.Extract;

namespace CorpInsightsTW.Tests.Etl.Pipeline.Extract;

[TestClass]
public sealed class ReportingPeriodHelperTests
{
    [TestMethod]
    [DataRow(2026,  1,  1, 4)]  // 年初第一天 ➜ Q4 (前一年年度)
    [DataRow(2026,  3, 31, 4)]  // Q4 截止日當天 ➜ Q4
    [DataRow(2026,  4,  1, 1)]  // Q1 開始第一天 ➜ Q1
    [DataRow(2026,  5, 15, 1)]  // Q1 截止日當天 ➜ Q1
    [DataRow(2026,  5, 16, 2)]  // Q2 開始第一天 ➜ Q2
    [DataRow(2026,  8, 14, 2)]  // Q2 截止日當天 ➜ Q2
    [DataRow(2026,  8, 15, 3)]  // Q3 開始第一天 ➜ Q3
    [DataRow(2026, 11, 14, 3)]  // Q3 截止日當天 ➜ Q3
    [DataRow(2026, 11, 15, 4)]  // Q4 開始第一天 ➜ Q4 (當年度)
    [DataRow(2026, 12, 31, 4)]  // 年底最後一天 ➜ Q4
    public void GetCurrentFilingQuarter_ReturnsExpectedQuarter(int year, int month, int day, int expectedQuarter)
    {
        // Arrange
        var testDate = new DateTime(year, month, day);

        // Act
        var result = HtmlDataExtractor.GetCurrentFilingQuarter(testDate);

        // Assert
        Assert.AreEqual(expectedQuarter, result, $"日期 {testDate:yyyy-MM-dd} 的預期季度應為 Q{expectedQuarter}，但實際結果為 Q{result}");
    }
}