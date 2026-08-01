namespace CorpInsightsTW.Core.Algorithm;

// 定義運算列舉：加項(1) 或 減項(-1)
public enum Operation
{
    Add = 1,
    Subtract = -1
}

// 定義節點模型：記錄「哪一年、哪一季、加或減」
public record DataNode(int Year, int Quarter, Operation Op);

public class PlanGenerator
{
    /// <summary>
    /// 產生 N 季累計淨利的「端點相加減」查詢計畫
    /// </summary>
    public static List<DataNode> Gen(int currentYear, int currentQuarter, int N)
    {
        var plan = new List<DataNode>();

        // 1. 推算起始點 (N 季前的第一個季度)
        int currentAbs = currentYear * 4 + (currentQuarter - 1);
        int startAbs = currentAbs - (N - 1);
        
        int startYear = startAbs / 4;
        int startQuarter = (startAbs % 4) + 1;

        // 2. 判斷是否在同一個年度內
        if (startYear == currentYear)
        {
            // 同年度：直接拿 [當前季累計] - [起始前一季累計]
            plan.Add(new DataNode(currentYear, currentQuarter, Operation.Add));
            
            // 如果起始季是 Q1，代表從頭算，不需要扣除前一季
            if (startQuarter > 1)
            {
                plan.Add(new DataNode(currentYear, startQuarter - 1, Operation.Subtract));
            }
        }
        else
        {
            // 跨年度：拆分為「今年」、「中間完整年」、「起始年」三段

            // A. 今年部分：拿 [當前季累計] (代表今年 Q1 ~ 當前季)
            plan.Add(new DataNode(currentYear, currentQuarter, Operation.Add));

            // B. 中間完整年部分：每年直接拿 [Q4 年報累計]
            for (int y = currentYear - 1; y > startYear; y--)
            {
                plan.Add(new DataNode(y, 4, Operation.Add));
            }

            // C. 起始年部分：拿 [Q4 年報累計] - [起始前一季累計]
            plan.Add(new DataNode(startYear, 4, Operation.Add));
            
            if (startQuarter > 1)
            {
                plan.Add(new DataNode(startYear, startQuarter - 1, Operation.Subtract));
            }
        }

        return plan;
    }
}