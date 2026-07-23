using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace CorpInsightsTW.Etl.Core.Logging;

public class CleanConsoleFormatter : ConsoleFormatter
{
    // 給這個格式化器起個名字
    public const string FormatterName = "CleanConsole";

    public CleanConsoleFormatter() : base(FormatterName) { }

    public override void Write<TState>(
        in LogEntry<TState> logEntry, 
        IExternalScopeProvider? scopeProvider, 
        TextWriter textWriter)
    {
        string message = logEntry.Formatter(logEntry.State, logEntry.Exception);

        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        
        // 將時間戳記變灰色 (\u001b[90m)，隨後用 \u001b[0m 還原顏色
        // 這樣能讓時間默默存在，不干擾視覺縮排的對齊
        textWriter.WriteLine($"\u001b[90m[{timestamp}]\u001b[0m {message}");
    }
}