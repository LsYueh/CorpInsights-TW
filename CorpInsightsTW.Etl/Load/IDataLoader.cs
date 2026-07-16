using System.Text.Json;
using CorpInsightsTW.Etl.Common;

namespace CorpInsightsTW.Etl.Load;

public interface IDataLoader
{
    Task LoadAsync(
        EtlContext context,
        IReadOnlyList<JsonElement> batch, int fileTotalCount,
        CancellationToken cancellationToken, int indentLevel = 0);
}