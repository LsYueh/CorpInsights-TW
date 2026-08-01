using CorpInsightsTW.Core.Algorithm;

namespace CorpInsightsTW.Tests.Core.Algorithm;

[TestClass]
public class PlanGeneratorTests
{
    [TestMethod]
    public void Gen_SingleQuarter_SameYear_ReturnsCorrectNodes()
    {
        // 測試情境：2026 Q2 查單季 (N=1) => 應該是 +2026 Q2, -2026 Q1
        var plan = PlanGenerator.Gen(2026, 2, 1);

        Assert.HasCount(2, plan);
        Assert.Contains(new DataNode(2026, 2, Operation.Add), plan);
        Assert.Contains(new DataNode(2026, 1, Operation.Subtract), plan);
    }

    [TestMethod]
    public void Gen_YTD_SameYear_ReturnsCorrectNodes()
    {
        // 測試情境：2026 Q2 查今年累計 (N=2) => 應該是 +2026 Q2
        var plan = PlanGenerator.Gen(2026, 2, 2);

        Assert.HasCount(1, plan);
        Assert.Contains(new DataNode(2026, 2, Operation.Add), plan);
    }

    [TestMethod]
    public void Gen_TTM_CrossOneYear_ReturnsCorrectNodes()
    {
        // 測試情境：2026 Q2 查近四季 TTM (N=4) => +2026 Q2, +2025 Q4, -2025 Q2
        var plan = PlanGenerator.Gen(2026, 2, 4);

        Assert.HasCount(3, plan);
        Assert.Contains(new DataNode(2026, 2, Operation.Add), plan);
        Assert.Contains(new DataNode(2025, 4, Operation.Add), plan);
        Assert.Contains(new DataNode(2025, 2, Operation.Subtract), plan);
    }

    [TestMethod]
    public void Gen_16Quarters_CrossMultipleYears_ReturnsCorrectNodes()
    {
        // 測試情境：2026 Q2 查近 16 季 (N=16) 
        // 預期：+2026 Q2, +2025 Q4, +2024 Q4, +2023 Q4, +2022 Q4, -2022 Q2
        var plan = PlanGenerator.Gen(2026, 2, 16);

        Assert.HasCount(6, plan);
        
        // 驗證今年的頭
        Assert.Contains(new DataNode(2026, 2, Operation.Add), plan);
        
        // 驗證中間完整的三個年度
        Assert.Contains(new DataNode(2025, 4, Operation.Add), plan);
        Assert.Contains(new DataNode(2024, 4, Operation.Add), plan);
        Assert.Contains(new DataNode(2023, 4, Operation.Add), plan);
        
        // 驗證起始年度的頭尾
        Assert.Contains(new DataNode(2022, 4, Operation.Add), plan);
        Assert.Contains(new DataNode(2022, 2, Operation.Subtract), plan);
    }
}