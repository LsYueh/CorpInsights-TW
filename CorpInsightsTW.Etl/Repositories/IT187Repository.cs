namespace CorpInsightsTW.Etl.Repositories;

public interface IT187Repository<TDto>
{
    Task UpsertAsync(IEnumerable<TDto> dtos, CancellationToken cancellationToken = default);
}