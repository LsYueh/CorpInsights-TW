using CorpInsightsTW.Core.Extensions;

namespace CorpInsightsTW.Tests.Core.Extensions;

[TestClass]
public class MinguoExtensionsTests
{
    [TestMethod]
    [DataRow(2026, 7, 30, "yyy/MM/dd", "115/07/30")]
    [DataRow(2026, 7, 30, "民國yyy年MM月dd日", "民國115年07月30日")]
    [DataRow(2026, 7, 30, "yyyMMdd", "1150730")]
    [DataRow(2010, 5, 20, "yyy/MM/dd", "099/05/20")] // 測試早期年份補零
    public void ToMinguoDateString_ShouldFormatCorrectly(int year, int month, int day, string format, string expected)
    {
        // Arrange
        var date = new DateOnly(year, month, day);

        // Act
        var result = date.ToMinguoDateString(format);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DataRow("115/07/30", "yyy/MM/dd", 2026, 7, 30)]
    [DataRow("1150730", "yyyMMdd", 2026, 7, 30)]
    [DataRow("民國115年07月30日", "民國yyy年MM月dd日", 2026, 7, 30)]
    [DataRow("099/05/20", "yyy/MM/dd", 2010, 5, 20)]
    public void TryParseMinguoDate_ValidString_ShouldParseToGregorianDateOnly(
        string input, string format, int expectedYear, int expectedMonth, int expectedDay)
    {
        // Act
        var success = input.TryParseMinguoDate(format, out DateOnly result);

        // Assert
        Assert.IsTrue(success);
        Assert.AreEqual(new DateOnly(expectedYear, expectedMonth, expectedDay), result);
    }

    [TestMethod]
    public void TryParseMinguoDate_InvalidString_ShouldReturnFalse()
    {
        // Arrange
        var invalidInput = "invalid-date";

        // Act
        var success = invalidInput.TryParseMinguoDate("yyy/MM/dd", out DateOnly result);

        // Assert
        Assert.IsFalse(success);
        Assert.AreEqual(default(DateOnly), result);
    }

    [TestMethod]
    public void GetMinguoYear_ShouldReturnCorrectYear()
    {
        // Arrange
        var date = new DateOnly(2026, 7, 30);

        // Act & Assert
        Assert.AreEqual(115, date.GetMinguoYear());
        Assert.AreEqual("115", date.GetMinguoYearString(padLeft: false));
    }
}