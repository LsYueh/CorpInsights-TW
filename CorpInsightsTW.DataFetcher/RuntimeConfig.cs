using CorpInsightsTW.Core.Enums;

namespace CorpInsightsTW.DataFetcher;

/// <summary>
/// 
/// </summary>
/// <param name="Market">市場 (上市、上櫃)</param>
/// <param name="Status">上市狀態 (上市、公發、...)</param>
/// <param name="Taxonomy">申報分類 (一般、金融、全產業、...)</param>
/// <param name="ApCode">報表代號 (t187ap06、t187ap07、全報表)</param>
/// <param name="TwseRootUrl">證交所 OpenAPI 根網址</param>
/// <param name="TwseRootUrl">櫃買中心 OpenAPI 根網址</param>
public record RuntimeConfig(
    StockMarket Market,
    ListingStatus Status,
    XbrlTaxonomy Taxonomy,
    T187ApCode ApCode,
    string TwseRootUrl,
    string TpexRootUrl);