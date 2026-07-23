namespace CorpInsightsTW.Etl.Repositories;

public interface IRepository<TDto>
{
    Task UpsertAsync(IEnumerable<TDto> dtos, CancellationToken cancellationToken = default);
}