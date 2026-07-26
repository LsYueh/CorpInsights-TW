using CommandLine;

namespace CorpInsightsTW.Etl;

public class CliOptions
{
    [Option('m', "market", Required = false, Default = "all", 
        HelpText = "市場: 'all' (全部), twse: 上市, tpex: 上櫃")]
    public string Market { get; set; } = "all";
    
    [Option('s', "status", Required = false, Default = "all", 
        HelpText = "上市狀態: 'all' (全部), L: 上市, X: 公發, O: 上櫃, U: 興櫃")]
    public string Status { get; set; } = "all";

    [Option('t', "taxonomy", Required = false, Default = "all",
        HelpText = "申報分類: 'all' (全部), 'ci' (一般行業), 'basi' (金融業), 'bd' (證券期貨業), 'fh' (金控業), 'ins' (保險業), 'mim' (異業別合併)")]
    public string Taxonomy { get; set; } = "all";

    [Option('r', "report", Required = false, Default = "all",
        HelpText = "報表代號: 'all' (全部), 't187ap06' (綜合損益表), 't187ap07' (資產負債表)")]
    public string ApCode { get; set; } = "all";

    [Option("date", Required = false, Default = null,
        HelpText = "指定匯入資料日期 (格式: yyyyMMdd)，預設為今日")]
    public string? Date { get; set; }

    [Option("dry", Required = false, Default = false,
        HelpText = "指定是否僅進行模擬執行（不實際寫入/匯入資料）")]
    public bool DryRun { get; set; }
}