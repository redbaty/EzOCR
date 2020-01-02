using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;

namespace EzOCR
{
    internal class GithubDownloader
    {
        public GithubDownloader(string owner, string repository)
        {
            Repository = repository;
            Owner = owner;
        }

        private string Owner { get; }

        private string Repository { get; }

        public async Task<List<RepositoryContent>> GetFiles(string fileMask)
        {
            var github = new GitHubClient(new ProductHeaderValue("EzOCR"));
            var files = await github.Repository.Content.GetAllContents(Owner, Repository);
            return files.Where(i => i.Name.StartsWith(fileMask))
                        .ToList();
        }
    }
}