using System;
using System.IO;

namespace EzOCR
{
    public class TemporaryFile : IDisposable
    {
        private FileInfo ThisFile { get; }

        public string FullPath => ThisFile.FullName;

        public TemporaryFile()
        {
            ThisFile = new FileInfo(Path.GetTempFileName());
        }

        public FileStream OpenReadStream() => ThisFile.OpenRead();

        public FileStream OpenWriteStream() => ThisFile.OpenWrite();

        public void Dispose()
        {
            if (ThisFile?.Exists ?? false) ThisFile.Delete();
        }
    }
}