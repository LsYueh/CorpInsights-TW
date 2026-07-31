using CorpInsightsTW.Core.Extensions;

namespace CorpInsightsTW.Tests.Core.Extensions;

[TestClass]
public sealed class FilingExtensionsTests
{
    [TestMethod]
    [DataRow(2026,  1, 15, 2025, 3)] // 01/15 -> 2025 Q3 (Q4申報中)
    [DataRow(2026,  3, 31, 2025, 3)] // 03/31 -> 2025 Q3 (Q4截止日當天)
    [DataRow(2026,  4,  1, 2025, 4)] // 04/01 -> 2025 Q4 (Q4已出爐，Q1申報中)
    [DataRow(2026,  5, 15, 2025, 4)] // 05/15 -> 2025 Q4 (Q1截止日當天)
    [DataRow(2026,  5, 16, 2026, 1)] // 05/16 -> 2026 Q1 (Q1已出爐)
    [DataRow(2026,  8, 14, 2026, 1)] // 08/14 -> 2026 Q1 (Q2截止日當天)
    [DataRow(2026,  8, 15, 2026, 2)] // 08/15 -> 2026 Q2 (Q2已出爐)
    [DataRow(2026, 11, 14, 2026, 2)] // 11/14 -> 2026 Q2 (Q3截止日當天)
    [DataRow(2026, 11, 15, 2026, 3)] // 11/15 -> 2026 Q3 (Q3已出爐)
    [DataRow(2026, 12, 31, 2026, 3)] // 12/31 -> 2026 Q3 (等待年底過完)
    public void GetFilingPeriod_ShouldReturnCorrectYearAndQuarter(
        int inputYear, int inputMonth, int inputDay, int expectedYear, int expectedQuarter)
    {
        // Arrange
        var date = new DateOnly(inputYear, inputMonth, inputDay);

        // Act
        var (filingYear, filingQuarter) = date.GetFilingPeriod();

        // Assert
        Assert.AreEqual(expectedYear, filingYear);
        Assert.AreEqual(expectedQuarter, filingQuarter);
    }
}