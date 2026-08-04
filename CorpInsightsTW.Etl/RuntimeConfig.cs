using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;

namespace CorpInsightsTW.Etl;

public record RuntimeConfig
{
    public StockMarket Market { get; init; }
    public ListingStatus Status { get; init; }
    public XbrlTaxonomy Taxonomy { get; init; }
    public StatementType Type { get; init; }
    public DateOnly Date { get; init; }
    public bool IsDryRun { get; init; } 
    public bool SkipDateCheck { get; init; }

    public RuntimeConfig(
        StockMarket market,
        ListingStatus status,
        XbrlTaxonomy taxonomy,
        StatementType type,
        DateOnly date,
        bool isDryRun = true, // 資料安全至上，但是防不住白痴
        bool skipDateCheck = false)
    {
        if (market == StockMarket.TWSE && status is ListingStatus.O or ListingStatus.U)
        {
            throw new ArgumentException($"TWSE 市場不支援 '{status}' 狀態 (僅支援 All, L, X)。");
        }
        if (market == StockMarket.TPEX && status is ListingStatus.L or ListingStatus.X)
        {
            throw new ArgumentException($"TPEX 市場不支援 '{status}' 狀態 (僅支援 All, O, U)。");
        }

        Market   = market;
        Status   = status;
        Taxonomy = taxonomy;
        Type     = type;
        Date     = date;

        IsDryRun      = isDryRun;
        SkipDateCheck = skipDateCheck;
    }

    // 當前生效的 Code (使用 Extension Method 轉出)
    public string ActiveStatusCode => Status.ToCode();
}