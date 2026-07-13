using System.Text.Json;

namespace CorpInsightsTW.Etl.Extract;

public interface IDataExtractor
{
    Task<JsonDocument?> ExtractAsync(CancellationToken cancellationToken);
}