using System.Text.Json;
using CorpInsightsTW.Etl.Core.Common;
using CorpInsightsTW.Etl.Dtos;

namespace CorpInsightsTW.Etl.Pipeline.Transform;

public interface IDataTransformer
{
    IEnumerable<(IReadOnlyList<IT187Dto> Batch, int TotalCount)> Transform(
        EtlContext context, JsonDocument source, int batchSize, int indentLevel = 0);
}