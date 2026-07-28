using CorpInsightsTW.Core.Enums;

namespace CorpInsightsTW.Core.Storage;

public record StorageContext
{
    public StockMarket Market { get; }
    public StatementType Type { get; }
    public ListingStatus Status { get; }
    public XbrlTaxonomy Taxonomy { get; }
    public DateOnly Date { get; }

    public StorageContext(
        StockMarket market,
        StatementType type,
        ListingStatus status,
        XbrlTaxonomy taxonomy,
        DateOnly? date = null)
    {
        if (market == StockMarket.All)
            throw new ArgumentException("StorageContext 的 Market 不能為 StockMarket.All", nameof(market));

        if (type == StatementType.All)
            throw new ArgumentException("StorageContext 的 Type 不能為 StatementType.All", nameof(type));

        if (status == ListingStatus.All)
            throw new ArgumentException("StorageContext 的 Status 不能為 ListingStatus.All", nameof(status));

        if (taxonomy == XbrlTaxonomy.All)
            throw new ArgumentException("StorageContext 的 Taxonomy 不能為 XbrlTaxonomy.All", nameof(taxonomy));

        Market   = market;
        Type   = type;
        Status   = status;
        Taxonomy = taxonomy;
        Date     = date ?? DateOnly.FromDateTime(DateTime.Today);
    }
}