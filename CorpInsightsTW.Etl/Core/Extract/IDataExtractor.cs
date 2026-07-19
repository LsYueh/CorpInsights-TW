using System.Text.Json;
using CorpInsightsTW.Etl.Core.Common;

namespace CorpInsightsTW.Etl.Core.Extract;

public interface IDataExtractor
{
    Task<JsonDocument?> ExtractAsync(EtlContext context, CancellationToken cancellationToken, int indentLevel = 0);
}