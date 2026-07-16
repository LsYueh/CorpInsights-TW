using System.Text.Json;

namespace CorpInsightsTW.Etl.Transform;

public interface IDataTransformer
{
    IEnumerable<(IReadOnlyList<JsonElement> Batch, int TotalCount)> Transform(
        JsonDocument source, int batchSize, int indentLevel = 0);
}