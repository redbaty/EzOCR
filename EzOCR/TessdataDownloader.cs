using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EzOCR
{
    public class TessdataDownloader
    {
        private GithubDownloader GithubDownloader { get; } = new("tesseract-ocr", "tessdata");

        public async Task EnsureDataFolder(string language = "eng",
                                           string workingDirectory = null,
                                           string dataFolder = "tessdata")
        {
            var remoteFiles = await GithubDownloader.GetFiles(language);
            var dataDirectory = new DirectoryInfo(Path.Combine(workingDirectory ?? Environment.CurrentDirectory, dataFolder ?? "tessdata"));
            var localFiles = dataDirectory.Exists ? dataDirectory.GetFiles() : [];

            if (!dataDirectory.Exists) dataDirectory.Create();

            using var client = new HttpClient();

            foreach (var remoteFile in remoteFiles.Where(remoteFile => localFiles.All(o => o.Name != remoteFile.Name)))
            {
                await using var fileStream = await client.GetStreamAsync(remoteFile.DownloadUrl);
                var fileInfo = new FileInfo(Path.Combine(dataDirectory.FullName, remoteFile.Name));

                await using var localStream = fileInfo.OpenWrite();
                await fileStream.CopyToAsync(localStream);
            }
        }
    }
}