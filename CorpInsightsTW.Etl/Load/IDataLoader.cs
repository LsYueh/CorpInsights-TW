using System.Text.Json;

namespace CorpInsightsTW.Etl.Load;

public interface IDataLoader
{
    Task LoadAsync(IEnumerable<JsonElement> rows, CancellationToken cancellationToken);
}