using System.Text.Json;
using CorpInsightsTW.Etl.Core.Common;

namespace CorpInsightsTW.Etl.Core.Load;

public interface IDataLoader
{
    Task LoadAsync(
        EtlContext context,
        IReadOnlyList<JsonElement> batch, int fileTotalCount,
        CancellationToken cancellationToken, int indentLevel = 0);
}