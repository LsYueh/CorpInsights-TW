using CorpInsightsTW.Core.Enums;

namespace CorpInsightsTW.Etl;

public record EtlRunConfig(
    ListingStatus Status,
    XbrlTaxonomy Taxonomy,
    T187ApCode ApCode,
    DateOnly Date);