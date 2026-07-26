using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;

namespace CorpInsightsTW.Etl;

public record RuntimeConfig
{
    public StockMarket Market { get; init; }
    public ListingStatus Status { get; init; }
    public XbrlTaxonomy Taxonomy { get; init; }
    public T187ApCode ApCode { get; init; }
    public DateOnly Date { get; init; }
    public bool IsDryRun { get; init; }

    public RuntimeConfig(
        StockMarket market,
        ListingStatus status,
        XbrlTaxonomy taxonomy,
        T187ApCode apCode,
        DateOnly date,
        bool isDryRun)
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
        ApCode   = apCode;
        Date     = date;
        IsDryRun = isDryRun;
    }

    // 當前生效的 Code (使用 Extension Method 轉出)
    public string ActiveStatusCode => Status.ToCode();
}