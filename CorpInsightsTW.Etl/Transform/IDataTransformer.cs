using System.Text.Json;

namespace CorpInsightsTW.Etl.Transform;

public interface IDataTransformer
{
    JsonDocument Transform(JsonDocument source);
}