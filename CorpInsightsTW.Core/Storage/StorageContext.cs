using CorpInsightsTW.Core.Enums;

namespace CorpInsightsTW.Core.Storage;

public record StorageContext
{
    public StockMarket Market { get; }
    public StatementType ApCode { get; }
    public ListingStatus Status { get; }
    public XbrlTaxonomy Taxonomy { get; }
    public DateOnly Date { get; }

    public StorageContext(
        StockMarket market,
        StatementType apCode,
        ListingStatus status,
        XbrlTaxonomy taxonomy,
        DateOnly? date = null)
    {
        if (market == StockMarket.All)
            throw new ArgumentException("StorageContext 的 Market 不能為 StockMarket.All", nameof(market));

        if (apCode == StatementType.All)
            throw new ArgumentException("StorageContext 的 ApCode 不能為 T187ApCode.All", nameof(apCode));

        if (status == ListingStatus.All)
            throw new ArgumentException("StorageContext 的 Status 不能為 ListingStatus.All", nameof(status));

        if (taxonomy == XbrlTaxonomy.All)
            throw new ArgumentException("StorageContext 的 Taxonomy 不能為 XbrlTaxonomy.All", nameof(taxonomy));

        Market   = market;
        ApCode   = apCode;
        Status   = status;
        Taxonomy = taxonomy;
        Date     = date ?? DateOnly.FromDateTime(DateTime.Today);
    }
}