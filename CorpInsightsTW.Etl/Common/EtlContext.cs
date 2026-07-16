using CorpInsightsTW.Core.Enums;

namespace CorpInsightsTW.Etl.Common;

/// <summary>
/// 當前 ETL 任務的詮釋資料與上下文背景
/// </summary>
public record EtlContext(
    T187ApCode ApCode,
    ListingStatus Status,
    XbrlTaxonomy Taxonomy,
    DateOnly Date
);