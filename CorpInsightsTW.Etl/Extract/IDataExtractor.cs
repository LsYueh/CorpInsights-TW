using System.Text.Json;
using CorpInsightsTW.Etl.Common;

namespace CorpInsightsTW.Etl.Extract;

public interface IDataExtractor
{
    Task<JsonDocument?> ExtractAsync(EtlContext context, CancellationToken cancellationToken, int indentLevel = 0);
}