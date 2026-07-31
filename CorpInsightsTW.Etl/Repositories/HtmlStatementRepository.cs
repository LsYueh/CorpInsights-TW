using CorpInsightsTW.Etl.Dtos;
using Dapper;
using MySqlConnector;

namespace CorpInsightsTW.Etl.Repositories;

public abstract class HtmlStatementRepository<TDto>(string connectionString) : IStatementRepository<TDto> where TDto : StatementDto
{
    protected readonly string ConnectionString = connectionString;

    /// <summary>
    /// 子類別需提供主要報表的 Upsert SQL 指令
    /// </summary>
    protected abstract string MainTableUpsertSql { get; }

    public virtual async Task UpsertAsync(IEnumerable<TDto> dtos, CancellationToken cancellationToken = default)
    {
        var dtoList = dtos.ToList();
        if (dtoList.Count == 0) return;

        await using var conn = new MySqlConnection(ConnectionString);
        await conn.OpenAsync(cancellationToken);

        await using var transaction = await conn.BeginTransactionAsync(cancellationToken);

        try
        {
            await conn.ExecuteAsync(
                new CommandDefinition(MainTableUpsertSql, dtoList, transaction: transaction, cancellationToken: cancellationToken));

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
} 