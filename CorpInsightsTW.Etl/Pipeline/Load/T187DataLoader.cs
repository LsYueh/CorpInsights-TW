using CorpInsightsTW.Core.Enums;
using CorpInsightsTW.Etl.Core.Common;
using CorpInsightsTW.Etl.Dtos;
using CorpInsightsTW.Etl.Repositories.T187Ap06;

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
        IReadOnlyList<IT187RawDto> batch, int fileTotalCount,
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
        IReadOnlyList<IT187RawDto> batch,
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
        IReadOnlyList<IT187RawDto> batch, 
        CancellationToken cancellationToken)
    {
        switch (context.Taxonomy)
        {
            case XbrlTaxonomy.BASI:
                var basiDtos = batch.OfType<Dtos.T187Ap06.BasiDto>();
                var repo = new BasiRepository(_connectionString);
                await repo.UpsertAsync(basiDtos, cancellationToken);
                break;
            case XbrlTaxonomy.BD: break;
            case XbrlTaxonomy.CI: break;
            case XbrlTaxonomy.FH: break;
            case XbrlTaxonomy.INS: break;
            case XbrlTaxonomy.MIM: break;
            default:
                _logger.LogWarning("⚠️ [T187Ap06] 未知的 XBRL Taxonomy: {Taxonomy}", context.Taxonomy);
                break;
        }
    }

    private async Task ProcessT187Ap07BatchAsync(
        EtlContext context, 
        IReadOnlyList<IT187RawDto> batch, 
        CancellationToken cancellationToken)
    {
        switch (context.Taxonomy)
        {
            case XbrlTaxonomy.BASI: break;
            case XbrlTaxonomy.BD: break;
            case XbrlTaxonomy.CI: break;
            case XbrlTaxonomy.FH: break;
            case XbrlTaxonomy.INS: break;
            case XbrlTaxonomy.MIM: break;
            default:
                _logger.LogWarning("⚠️ [T187Ap07] 未知的 XBRL Taxonomy: {Taxonomy}", context.Taxonomy);
                break;
        }
    }
}