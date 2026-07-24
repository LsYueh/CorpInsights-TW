using CorpInsightsTW.Core.Enums;

namespace CorpInsightsTW.DataFetcher;

/// <summary>
/// 
/// </summary>
/// <param name="Status">目標市場狀態 (上市、公發、全市場)</param>
/// <param name="Taxonomy">目標產業分類 (一般、金融、全產業)</param>
/// <param name="ApCode">目標證交所報表代號 (t187ap06、t187ap07、全報表)</param>
/// <param name="TwseRootUrl">證交所 OpenAPI 根網址</param>
public record RuntimeConfig(
    ListingStatus Status,
    XbrlTaxonomy Taxonomy,
    T187ApCode ApCode,
    string TwseRootUrl);