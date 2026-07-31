namespace CorpInsightsTW.Etl.Repositories;

public interface IStatementRepository<TDto>
{
    Task UpsertAsync(IEnumerable<TDto> dtos, CancellationToken cancellationToken = default);
}