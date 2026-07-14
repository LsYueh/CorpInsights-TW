using System.Text.Json;
using CorpInsightsTW.Core.Enums;

namespace CorpInsightsTW.Etl.Extract;

public interface IDataExtractor
{
    Task<JsonDocument?> ExtractAsync(
        T187ApCode apCode, ListingStatus status, XbrlTaxonomy taxonomy, DateOnly date,
        CancellationToken cancellationToken);
}