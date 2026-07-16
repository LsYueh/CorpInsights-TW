using System.Text.Json;

namespace CorpInsightsTW.Etl.Load;

public interface IDataLoader
{
    Task LoadAsync(IReadOnlyList<JsonElement> batch, int fileTotalCount, CancellationToken cancellationToken);
}