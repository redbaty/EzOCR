using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace EzOCR.Tests
{
    public static class ResourceLoader
    {
        public static Stream GetFileFromAssembly(string fileMask)
        {
            fileMask = fileMask.ToLowerInvariant();
            
            var executingAssembly = Assembly.GetExecutingAssembly();
            return executingAssembly.GetManifestResourceStream(executingAssembly.GetManifestResourceNames()
                                                                              .Single(i =>
                                                                                      LikeOperator.LikeString(i.ToLowerInvariant(),
                                                                                              fileMask,
                                                                                              CompareMethod.Text)));
        }
    }
}