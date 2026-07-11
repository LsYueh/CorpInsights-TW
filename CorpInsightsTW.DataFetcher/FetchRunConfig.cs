using CorpInsightsTW.Core.Enums;

namespace CorpInsightsTW.DataFetcher;

/// <summary>
/// DataFetcher 執行期所需的強型態唯讀組態設定
/// </summary>
/// <param name="Mode">執行模式 (例如: cron, once)</param>
/// <param name="Status">目標市場狀態 (上市、公發、全市場)</param>
/// <param name="Taxonomy">目標產業分類 (一般、金融、全產業)</param>
/// <param name="ApCode">目標證交所報表代號 (t187ap06、t187ap07、全報表)</param>
/// <param name="TwseRootUrl">證交所 OpenAPI 根網址</param>
public record FetchRunConfig(
    string Mode,
    ListingStatus Status,
    XbrlTaxonomy Taxonomy,
    T187ApCode ApCode,
    string TwseRootUrl
);