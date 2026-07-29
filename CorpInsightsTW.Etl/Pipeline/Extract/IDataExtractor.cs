using System.Text.Json;
using CorpInsightsTW.Etl.Core.Context;

namespace CorpInsightsTW.Etl.Pipeline.Extract;

public interface IDataExtractor
{
    Task<JsonDocument?> ExtractAsync(EtlContext context, CancellationToken cancellationToken, int indentLevel = 0);
}