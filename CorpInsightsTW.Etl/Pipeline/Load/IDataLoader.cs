using CorpInsightsTW.Etl.Core.Common;
using CorpInsightsTW.Etl.Dtos;

namespace CorpInsightsTW.Etl.Pipeline.Load;

public interface IDataLoader
{
    Task LoadAsync(
        EtlContext context,
        IReadOnlyList<IT187RawDto> batch, int fileTotalCount,
        CancellationToken cancellationToken, int indentLevel = 0);
}