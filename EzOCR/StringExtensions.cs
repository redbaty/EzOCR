using System.Threading.Tasks;

namespace EzOCR
{
    public static class StringExtensions
    {
        public static Task<string> CleanAndFlattenString(this Task<string> baseTask) =>
                baseTask.ContinueWith(t => t.Result.Trim(' ', '\n', '\t'));
    }
}