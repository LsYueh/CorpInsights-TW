using System.Text.Json;

namespace CorpInsightsTW.Etl.Load;

public interface IDataLoader
{
    Task LoadAsync(JsonDocument data, CancellationToken cancellationToken);
}