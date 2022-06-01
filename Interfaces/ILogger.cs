
namespace Visit.Interfaces
{
    public interface ILogger
    {
        void Log(string Func, Exception ex, string Type = "error", string FIleLog = "full.log", short Caret = 1);
        void Log(string Func, string Str, string Type = "Info", string FIleLog = "full.log", short Caret = 1, bool IgnoreFull = false);
    }
}