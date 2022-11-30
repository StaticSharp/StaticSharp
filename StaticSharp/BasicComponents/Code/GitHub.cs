
using Octokit;
using System;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharpWeb.Components {

    /*namespace GitHub {

        public class Owner {
            public string Name { get; set; }

            public Owner(string name) => Name = name;

            private RepositoryBranch GetRepositoryBranch(string repositoryName, string reference, IStorage storage)
                => new(this, repositoryName, reference, storage);


            public RepositoryBranch GetRepositoryBranchPinned(string repositoryName, string reference, IStorage storage)
                //if (RepositoryBranch.IgnorePinning)
                => new(this, repositoryName, reference, storage);
            //var sha = client.Repository.Commit.GetSha1(Name, repositoryName, reference).Result;
            //return new RepositoryBranch(this, repositoryName, sha);

        }

        public class RepositoryBranch {
            //public static bool IgnorePinning { get; set; } = false;
            public Owner Owner { get; set; }
            public string Name { get; set; }
            public string Reference { get; set; }
            private IStorage _storage;

            public RepositoryBranch(Owner owner, string name, string reference, IStorage storage) {
                Owner = owner;
                Name = name;
                Reference = reference;
                _storage = storage;
            }

            private GithubResource _resource;
            public GithubResource Resource(string path) => _resource ??= new GithubResource(this, path, _storage);

        }

        public class GithubResource : IResource {
            public string Content;


            public RepositoryBranch RepositoryBranch { get; set; }
            public string Path { get; }

            public string Key { get; }

            public string Source => _source;

            //private string Content { get; set; }
            public GithubResource(RepositoryBranch repositoryBranch, string path, IStorage storage,
                        [CallerFilePath] string callerFilePath = "",
                        [CallerLineNumber] int callerLineNumber = 0) {
                RepositoryBranch = repositoryBranch;
                Path = path;
                
                var hash = RepositoryBranch.Owner.Name + RepositoryBranch.Name + RepositoryBranch.Reference + path;
                Key = string.Concat(hash.Split(System.IO.Path.GetInvalidFileNameChars()));
                _storage = storage;

            }


            public async Task GenerateAsync() {
                var path = System.IO.Path.Combine(_storage.StorageDirectory, "GitHubFiles", Key);
                if (File.Exists(path)) {
                    _source = await File.ReadAllTextAsync(path);
                } else {
                    var client = new WebClient();
                    try {
                        _source = await client.DownloadStringTaskAsync(RawUri);
                        await File.WriteAllTextAsync(path, _source);
                    } catch (Exception e) {
                        Log.Error.On(_callerFilePath, _callerLineNumber, e.Message);
                    }
                }
            }

            private IStorage _storage;
            private string _source;

            private const string _gitHubCom = "https://github.com";
            private const string _rawGitHubCom = "https://raw.githubusercontent.com";

            private int _callerLineNumber;
            private string _callerFilePath;

            public string HtmlUri => $"{_gitHubCom}/{RepositoryBranch.Owner.Name}/{RepositoryBranch.Name}/blob/{RepositoryBranch.Reference}/{Path}";
            public string RawUri => $"{_rawGitHubCom}/{RepositoryBranch.Owner.Name}/{RepositoryBranch.Name}/{RepositoryBranch.Reference}/{Path}";
        }
    }*/
}