using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EzOCR
{
    public class TessdataDownloader
    {
        private GithubDownloader GithubDownloader { get; } = new GithubDownloader("tesseract-ocr", "tessdata");

        public async Task EnsureDataFolder(string language = "eng",
                                           string workingDirectory = null,
                                           string dataFolder = "tessdata")
        {
            var remoteFiles = await GithubDownloader.GetFiles(language);
            var dataDirectory = new DirectoryInfo(Path.Combine(workingDirectory ?? Environment.CurrentDirectory,
                    dataFolder ?? "tessdata"));
            var localFiles = dataDirectory.Exists ? dataDirectory.GetFiles() : new FileInfo[0];

            if (!dataDirectory.Exists) dataDirectory.Create();

            var client = new HttpClient();

            foreach (var remoteFile in remoteFiles)
            {
                if (localFiles.Any(o => o.Name == remoteFile.Name))
                {
                    continue;
                }

                var fileStream = await client.GetStreamAsync(remoteFile.DownloadUrl);
                var fileInfo = new FileInfo(Path.Combine(dataDirectory.FullName, remoteFile.Name));

                using var localStream = fileInfo.OpenWrite();
                fileStream.CopyTo(localStream);
            }
        }
    }
}