using System.Text.Json;

namespace CorpInsightsTW.Etl.Transform;

public interface IDataTransformer
{
    IEnumerable<JsonElement> Transform(JsonDocument source);
}