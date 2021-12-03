using StaticSharpWeb.Html;
using StaticSharpWeb.Resources;
using StaticSharpWeb;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharpWeb.Components {
    public class SearchSequencer {
        [Flags]
        public enum SeqrchSequence : byte {
            Nothing = 0,
            Version = 1,
            Directory = 1 << 1,
            AllDirectories = 1 << 2,
            SpecifiedDirectory = 1 << 3,
            SvnLikeRepository = 1 << 4
        }

        public int SettingsDepth = 0;
        private readonly Queue<SeqrchSequence> Options = new Queue<SeqrchSequence>();
        public SeqrchSequence GetNextOption() {
            if (Options == null || Options.Count == 0) {
                throw new Exception("No options left in SearchSequencer");
            }
            return Options.Dequeue();
        }


        public SearchSequencer Clone() {
            var result = new SearchSequencer {
                SpecifyedPath = SpecifyedPath,
                SettingsDepth = SettingsDepth
            };
            foreach (var option in Options) {
                result.Options.Enqueue(option);
            }
            return result;
        }

        public SearchSequencer Version {
            get {
                SettingsDepth++;
                Options.Enqueue(SeqrchSequence.Version);
                return this;
            }
        }

        public SearchSequencer AnyDirectory {
            get {
                SettingsDepth++;
                Options.Enqueue(SeqrchSequence.Directory);
                return this;
            }
        }

        public SearchSequencer AllDirectories {
            get {
                SettingsDepth++;
                Options.Enqueue(SeqrchSequence.AllDirectories);
                return this;
            }
        }

        public SearchSequencer SvnLikeRepository {
            get {
                SettingsDepth++;
                Options.Enqueue(SeqrchSequence.SvnLikeRepository);
                return this;
            }
        }

        public SearchSequencer SpecifyThePath(string path) {
            SpecifyedPath = path;
            return this;
        }

        public string SpecifyedPath { get; private set; } = string.Empty;

    }




    public sealed class Downloadable : IBlock {
        string PrimaryName { get; set; }
        string SourcePath { get; set; }

        public DownloadableResource Resource { get; private set; }

        SearchSequencer SearchSequence { get; }
        //TODO search Sequencer!!!!
        public Downloadable(string sourcePath, string primaryName = null, SearchSequencer searchSequence = null) {
            SourcePath = sourcePath;
            PrimaryName = primaryName ?? Path.GetFileNameWithoutExtension(sourcePath);
            SearchSequence = searchSequence;
        }



        public async Task<INode> GenerateBlockHtmlAsync(Context context) {
            //SourcePath + PrimaryName
            Resource ??= await context.Storage.AddOrGetAsync(
                SourcePath, 
                () => new DownloadableResource(SourcePath, PrimaryName, context.Storage, SearchSequence)
            );

            context.Includes.Require(new Style(new RelativePath($"{nameof(Downloadable)}.scss")));
            var tag = new Tag("div", new { Class = nameof(Downloadable) }) {
                new JSCall(new RelativePath($"{nameof(Downloadable)}.js"), PrimaryName, Resource.OptionsTree).Generate(context)
            };
            return tag;
        }
    }

    public static class DownlodableStatic{
        public static void Add<T>(this T collection, Downloadable item) where T : IVerifiedBlockReceiver {
            collection.AddBlock(item);
        }
    }

    public class DownloadableResource : IResource {

        public DownloadableResource(string path, string name, IStorage storage, SearchSequencer searchSequence = null)
            => (_path, _name, _storage, _searchSequencer) = (path, name, storage, searchSequence);
        public string Key => _path;

        //public string Source => _source;
        public string Source => _source;

        public object OptionsTree { get; private set; }
        public long FileSize => _fileSize;


        private readonly SearchSequencer _searchSequencer;
        private readonly string _path;
        private readonly string _name;
        //private readonly string _primaryName;
        private readonly IStorage _storage;
        private long _fileSize;
        private string _source;

        private const string DIRNAME = "Downloadable";

        private async Task BuildTreeBySemVerAsync(Dictionary<string, object> result, IEnumerable<string> subDirectories, SearchSequencer searchSequence, int index, string name) {
            try {
                var versions = subDirectories.ToDictionary(x => new SemVer(Path.GetFileName(x)))
                    .OrderByDescending(x => x.Key).ToList();
                if (versions.Count == 0) return;
                var stable = versions.FirstOrDefault(x => x.Key.labelsString == "");
                var latest = versions.First();
                if (latest.Key != stable.Key) {
                    if (versions.Remove(stable)) {
                        versions.Insert(0, stable);
                    }
                }

                foreach (var v in versions) {
                    var e = await BuildOptionsTreeAsync(Path.Combine(v.Value, searchSequence.SpecifyedPath), searchSequence.Clone(), index + 1, name + "_" + v.Key);
                    var optionName = v.Key.ToString();
                    if (v.Key == stable.Key) {
                        optionName = "Stable " + optionName;
                    }
                    if (v.Key == latest.Key) {
                        optionName = "Latest " + optionName;
                    }
                    if (e != null)
                        result.Add(optionName, e);
                }
            } catch (ArgumentException e) {
                Log.Error.OnObject(this, e.Message);
            }
        }

        private static void EnumerateFolderAndAddToList(List<string> collection, string path) {
            collection ??= new List<string>();
            if (!string.IsNullOrEmpty(path)) { collection.AddRange(Directory.GetDirectories(path)); }
        }

        public async Task<object> BuildOptionsTreeAsync(string path, SearchSequencer searchSequence, int index, string name) {
            if (searchSequence == null || searchSequence.SettingsDepth == index) {
                return await FinalizeOptionsBranchAsync(path, name);
            }
            var subDirectories = Directory.GetDirectories(path).Where(x => !new DirectoryInfo(x).Name.StartsWith("."));
            if (subDirectories.Count() == 1) {
                var subDirectoryName = Path.GetFileName(subDirectories.First());
                return BuildOptionsTreeAsync(subDirectories.First(), searchSequence, index + 1, $"{name}_{subDirectoryName}");
            }
            var result = new Dictionary<string, object>();

            switch (searchSequence.GetNextOption()) {
                case SearchSequencer.SeqrchSequence.SvnLikeRepository: {
                    var trunkPath = subDirectories.Where(x => new DirectoryInfo(x).Name == "trunk").FirstOrDefault();
                    trunkPath = Path.Combine(trunkPath, searchSequence.SpecifyedPath);
                    trunkPath = await FinalizeOptionsBranchAsync(trunkPath, name);
                    if (trunkPath != null)
                        result.Add("Trunk", trunkPath);

                    var branches = subDirectories.Where(x => new DirectoryInfo(x).Name == "branches").FirstOrDefault();
                    var tags = subDirectories.Where(x => new DirectoryInfo(x).Name == "tags").FirstOrDefault();
                    List<string> listOfVersions = new();
                    EnumerateFolderAndAddToList(listOfVersions, branches);
                    EnumerateFolderAndAddToList(listOfVersions, tags);
                    await BuildTreeBySemVerAsync(result, listOfVersions, searchSequence, index, name);
                    break;
                }

                case SearchSequencer.SeqrchSequence.AllDirectories:
                    result.Add("All", await FinalizeOptionsBranchAsync(path, name));
                    break;
                case SearchSequencer.SeqrchSequence.Version:
                    await BuildTreeBySemVerAsync(result, subDirectories, searchSequence, index, name);
                    break;

                default: {
                    foreach (var d in subDirectories) {
                        var subDirectoryName = Path.GetFileName(d);
                        var e = await BuildOptionsTreeAsync(d, searchSequence.Clone(), index + 1, name + "_" + subDirectoryName);
                        if (e != null)
                            result.Add(subDirectoryName, e);
                    }

                    break;
                }
            }
            if (result.Count == 0) return null;
            return result;

        }

        public string CalculateHash(IEnumerable<string> paths) {
            var source = string.Join("?", paths.Select(x => Path.GetRelativePath(_storage.StorageDirectory, x)).OrderBy(x => x));
            return Hash.CreateFromString(source).ToString();
        }

        private async Task<string> FinalizeOptionsBranchAsync(string path, string name) {
            string[] files;

            if (File.Exists(path)) {
                files = new[] { path };
            } else {
                if (!Directory.Exists(path)) return null;
                files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            }

            if (files.Length == 0) return null;

            var hash = CalculateHash(files);

            // string fileName = files.Length == 1
            //     ? _name + Path.GetExtension(files[0])
            //     : _name + ".zip";
            var outputPath = Path.Combine(_storage.StorageDirectory, DIRNAME, hash);
            outputPath = Path.Combine(outputPath, Path.GetFileName(path));
            if (files.Length != 1)
                outputPath += ".zip";
            //else outputPath += ".zip";
            //else outputPath = Path.Combine(outputPath, Path.GetFileName(path));

            //string filename = hash;
            string filename = Path.Combine(hash, Path.GetFileName(path));

            if (File.Exists(outputPath)) { File.Delete(outputPath); }
            if(!Directory.Exists((Path.Combine(_storage.StorageDirectory, DIRNAME, hash))))
                Directory.CreateDirectory(Path.Combine(_storage.StorageDirectory, DIRNAME, hash));
            if (files.Length == 1) {
                await Utils.CopyFileAsync(files[0], outputPath);
                //filename = Path.Combine(outputPath, Path.GetFileName(path));
                //filename = Path.Combine(filename, Path.GetFileName(path));
            } else {
                using var zip = ZipFile.Open(outputPath, ZipArchiveMode.Create);
                foreach (var f in files) {
                    zip.CreateEntryFromFile(f, Path.GetRelativePath(_path, f), CompressionLevel.Optimal);
                }
                filename += ".zip";
                //filename = Path.Combine(filename, Path.GetFileName(path)) + ".zip";
            }
            _fileSize = new FileInfo(outputPath).Length;
            return filename + '|' + Utils.HumanizeSize(_fileSize);
        }

        public async Task GenerateAsync() {
           OptionsTree = File.Exists(_path)
               ? await FinalizeOptionsBranchAsync(_path, _name)
               : await BuildOptionsTreeAsync(_path, _searchSequencer, 0, _name);
        }
    }
}
