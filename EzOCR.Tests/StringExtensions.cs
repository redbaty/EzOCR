using System.Threading.Tasks;

namespace EzOCR.Tests
{
    internal static class StringExtensions
    {
        public static Task<string> CleanAndFlattenString(this Task<string> baseTask) =>
                baseTask.ContinueWith(t => t.Result.Trim(' ', '\n', '\t'));
    }
}