using System.Text.Json.Serialization;

namespace CorpInsightsTW.Etl.Dtos.T163Sb20;

/// <summary>
/// 現金流量表 (MOPS - T163SB20)
/// </summary>
public record CashFlowDto : StatementDto
{
    /// <summary>
    /// 營業活動之淨現金流入（流出）
    /// </summary>
    [JsonPropertyName("營業活動之淨現金流入（流出）"), JsonRequired]
    public decimal OperatingCashFlows { get; init; }

    /// <summary>
    /// 投資活動之淨現金流入（流出）
    /// </summary>
    [JsonPropertyName("投資活動之淨現金流入（流出）"), JsonRequired]
    public decimal InvestingCashFlows { get; init; }

    /// <summary>
    /// 籌資活動之淨現金流入（流出）
    /// </summary>
    [JsonPropertyName("籌資活動之淨現金流入（流出）"), JsonRequired]
    public decimal FinancingCashFlows { get; init; }

    /// <summary>
    /// 匯率變動對現金及約當現金之影響 (FX: foreign exchange)
    /// </summary>
    [JsonPropertyName("匯率變動對現金及約當現金之影響"), JsonRequired]
    public decimal FxEffect { get; init; } // 原: EffectOfExchangeRateChanges

    /// <summary>
    /// 本期現金及約當現金增加（減少）數
    /// </summary>
    [JsonPropertyName("本期現金及約當現金增加（減少）數"), JsonRequired]
    public decimal NetChangeInCash { get; init; }

    /// <summary>
    /// 期初現金及約當現金餘額
    /// </summary>
    [JsonPropertyName("期初現金及約當現金餘額"), JsonRequired]
    public decimal BeginningCashBalance { get; init; }

    /// <summary>
    /// 期末現金及約當現金餘額
    /// </summary>
    [JsonPropertyName("期末現金及約當現金餘額"), JsonRequired]
    public decimal EndingCashBalance { get; init; }
}