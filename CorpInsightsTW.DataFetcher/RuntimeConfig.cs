using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Core.Extensions;

namespace CorpInsightsTW.DataFetcher;

public record RuntimeConfig
{
    public StockMarket Market { get; init; }
    public ListingStatus Status { get; init; }
    public XbrlTaxonomy Taxonomy { get; init; }
    public StatementType ApCode { get; init; }
    public string TwseRootUrl { get; init; }
    public string TpexRootUrl { get; init; }

    public RuntimeConfig(
        StockMarket market,
        ListingStatus status,
        XbrlTaxonomy taxonomy,
        StatementType apCode,
        string twseRootUrl,
        string tpexRootUrl)
    {
        if (market == StockMarket.TWSE && status is ListingStatus.O or ListingStatus.U)
        {
            throw new ArgumentException($"TWSE 市場不支援 '{status}' 狀態 (僅支援 All, L, X)。");
        }
        if (market == StockMarket.TPEX && status is ListingStatus.L or ListingStatus.X)
        {
            throw new ArgumentException($"TPEX 市場不支援 '{status}' 狀態 (僅支援 All, O, U)。");
        }

        Market      = market;
        Status      = status;
        Taxonomy    = taxonomy;
        ApCode      = apCode;
        TwseRootUrl = twseRootUrl;
        TpexRootUrl = tpexRootUrl;
    }

    // 當前生效的 Code (使用 Extension Method 轉出)
    public string ActiveStatusCode => Status.ToCode();

    // 取得當前使用的 Root URL
    public string TargetRootUrl => Market switch
    {
        StockMarket.TWSE => TwseRootUrl,
        StockMarket.TPEX => TpexRootUrl,
        _ => throw new InvalidOperationException($"未知的股票市場類型 : {Market}")
    };
}