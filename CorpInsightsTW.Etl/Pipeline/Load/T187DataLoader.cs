using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Etl.Core.Common;
using CorpInsightsTW.Etl.Dtos;
using CorpInsightsTW.Etl.Repositories;

namespace CorpInsightsTW.Etl.Pipeline.Load;

public class T187DataLoader(
    ILogger<T187DataLoader> logger,
    string connectionString) : IDataLoader
{
    private readonly string _connectionString = connectionString;
    private readonly ILogger<T187DataLoader> _logger = logger;

    private static string GetIndent(int level) => new(' ', level * 4);

    // 追蹤跨批次累計的總筆數
    private int _totalProcessedCount = 0;

    public async Task LoadAsync(EtlContext context,
        IReadOnlyList<IT187Dto> batch, int fileTotalCount,
        CancellationToken cancellationToken, int indentLevel = 0)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (batch == null || batch.Count == 0) return;

        string indent = GetIndent(indentLevel);

        try
        {
            await ExecAsync(context, batch, cancellationToken);

            // 處理完成後才更新累計筆數並印出 Log
            int currentTotal = Interlocked.Add(ref _totalProcessedCount, batch.Count);

            _logger.LogInformation(
                "{Indent}💾 [T187Load] 寫入成功 | AP: {ApCode} | Taxonomy: {Taxonomy} | 本次批次: {BatchCount} 筆 | 進度: {CurrentTotal}/{FileTotal}",
                indent,
                context.ApCode,
                context.Taxonomy,
                batch.Count,
                currentTotal,
                fileTotalCount);

            // 當處理到檔案的最後一筆時，重置計數器（方便下一個檔案進來時重新計算）
            if (currentTotal >= fileTotalCount)
                Interlocked.Exchange(ref _totalProcessedCount, 0);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("{Indent}⏹️ [T187Load] 批次寫入作業已取消 | AP: {ApCode} | Taxonomy: {Taxonomy}",
                indent, context.ApCode, context.Taxonomy);
            
            // 發生取消時重置計數器
            Interlocked.Exchange(ref _totalProcessedCount, 0);
            throw; // 繼續向上拋出以終止 pipeline
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "{Indent}❌ [T187Load] 批次寫入失敗！ | AP: {ApCode} | Taxonomy: {Taxonomy} | 本批次筆數: {BatchCount} | 錯誤訊息: {Message}",
                indent,
                context.ApCode,
                context.Taxonomy,
                batch.Count,
                ex.Message);

            // 發生例外時，重置累計計數器，確保不會影響後續任務
            Interlocked.Exchange(ref _totalProcessedCount, 0);

            // 拋出例外供上層管道 (Pipeline) 捕捉或進行 Retry / Rollback 處理
            throw;
        }
    }

    public async Task ExecAsync(
        EtlContext context, 
        IReadOnlyList<IT187Dto> batch,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        switch (context.ApCode)
        {
            case T187ApCode.T187AP06: await ProcessT187Ap06BatchAsync(context, batch, cancellationToken); break;
            case T187ApCode.T187AP07: await ProcessT187Ap07BatchAsync(context, batch, cancellationToken); break;
            default:
                _logger.LogWarning("⚠️ 未知的 ApCode: {ApCode}", context.ApCode);
                break;
        }

        await Task.CompletedTask;
    }

    private async Task ProcessT187Ap06BatchAsync(
        EtlContext context, 
        IReadOnlyList<IT187Dto> batch, 
        CancellationToken cancellationToken)
    {
        switch (context.Taxonomy)
        {
            case XbrlTaxonomy.BASI: await ExecUpsertAsync(new Repositories.T187Ap06.BasiRepository(_connectionString), batch, cancellationToken); break;
            case XbrlTaxonomy.BD  : await ExecUpsertAsync(new Repositories.T187Ap06.BdRepository  (_connectionString), batch, cancellationToken); break;
            case XbrlTaxonomy.CI  : await ExecUpsertAsync(new Repositories.T187Ap06.CiRepository  (_connectionString), batch, cancellationToken); break;
            case XbrlTaxonomy.FH  : await ExecUpsertAsync(new Repositories.T187Ap06.FhRepository  (_connectionString), batch, cancellationToken); break;
            case XbrlTaxonomy.INS : await ExecUpsertAsync(new Repositories.T187Ap06.InsRepository (_connectionString), batch, cancellationToken); break;
            case XbrlTaxonomy.MIM : await ExecUpsertAsync(new Repositories.T187Ap06.MimRepository (_connectionString), batch, cancellationToken); break;
            default:
                _logger.LogWarning("⚠️ [T187Ap06] 未知的 XBRL Taxonomy: {Taxonomy}", context.Taxonomy);
                break;
        }
    }

    private async Task ProcessT187Ap07BatchAsync(
        EtlContext context, 
        IReadOnlyList<IT187Dto> batch, 
        CancellationToken cancellationToken)
    {
        switch (context.Taxonomy)
        {
            case XbrlTaxonomy.BASI: await ExecUpsertAsync(new Repositories.T187Ap07.BasiRepository(_connectionString), batch, cancellationToken); break;
            case XbrlTaxonomy.BD  : await ExecUpsertAsync(new Repositories.T187Ap07.BdRepository  (_connectionString), batch, cancellationToken); break;
            case XbrlTaxonomy.CI  : await ExecUpsertAsync(new Repositories.T187Ap07.CiRepository  (_connectionString), batch, cancellationToken); break;
            case XbrlTaxonomy.FH  : await ExecUpsertAsync(new Repositories.T187Ap07.FhRepository  (_connectionString), batch, cancellationToken); break;
            case XbrlTaxonomy.INS : await ExecUpsertAsync(new Repositories.T187Ap07.InsRepository (_connectionString), batch, cancellationToken); break;
            case XbrlTaxonomy.MIM : await ExecUpsertAsync(new Repositories.T187Ap07.MimRepository (_connectionString), batch, cancellationToken); break;
            default:
                _logger.LogWarning("⚠️ [T187Ap07] 未知的 XBRL Taxonomy: {Taxonomy}", context.Taxonomy);
                break;
        }
    }

    /// <summary>
    /// IRepository 泛型通用呼叫器
    /// </summary>
    private static async Task ExecUpsertAsync<TDto>(
        IRepository<TDto> repository, 
        IReadOnlyList<IT187Dto> batch, 
        CancellationToken cancellationToken)
    {
        var dtos = batch.OfType<TDto>().ToList();
        if (dtos.Count > 0)
        {
            await repository.UpsertAsync(dtos, cancellationToken);
        }
    }
}