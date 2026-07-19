using System.Text.Json;
using CorpInsightsTW.Etl.Core.Common;

namespace CorpInsightsTW.Etl.Core.Transform;

public interface IDataTransformer
{
    IEnumerable<(IReadOnlyList<JsonElement> Batch, int TotalCount)> Transform(
        EtlContext context,JsonDocument source, int batchSize, int indentLevel = 0);
}