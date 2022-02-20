using StaticSharpWeb.Html;
using StaticSharpWeb.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace StaticSharpWeb.Components {

    public class VideoResource : IResource {

        public VideoResource(string key, IStorage storage) {
            Key = key;
            _storage = storage;
            _storageDirectory = storage.StorageDirectory;
        }

        private string _storageDirectory;
        public const string Divider = "#";




        public string Key { get; }

        public string Source => Path.Combine(_storageDirectory, "Video", Key);

        public string ImageSource => Path.Combine(_storageDirectory, "Video", Key, ImageName);

        public Dictionary<int, string> Mips = new();

        public string IntermediateVideoCache => Path.Combine(_storage.IntermediateCache, Key);
        public string IntermediateImageCache => Path.Combine(_storage.IntermediateCache, Key, ImageName);

        private string ImageName => $"{Key}.jpg";



        private async Task DownloadVideoAsync(YoutubeClient youTube, MuxedStreamInfo streamInfo) {
            Log.Info.Here($"Youtube video download start: {Key}_{streamInfo.VideoResolution.Height}");
            var filePath = FileNameFormatter(streamInfo);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            await youTube.Videos.Streams.DownloadAsync(streamInfo, filePath);
            Log.Info.Here($"Youtube video download done: {Key}_{streamInfo.VideoResolution.Height}");
        }

        private string FileNameFormatter(MuxedStreamInfo streamInfo) => Path.Combine(
            IntermediateVideoCache,
            $"{streamInfo.VideoResolution.Height}",
            $"{Key}.{streamInfo.Container}"
        );

        private readonly SemaphoreSlim _imageSemaphoreSlim = new(1, 1);
        private readonly IStorage _storage;

        private async Task DownloadImage() {
            await _imageSemaphoreSlim.WaitAsync();
            try {
                if (File.Exists(IntermediateImageCache)) {
                    return;
                }
                using var client = new WebClient();
                await client.DownloadFileTaskAsync(new Uri($"http://i3.ytimg.com/vi/{Key}/maxresdefault.jpg"), IntermediateImageCache);
            } finally {
                _imageSemaphoreSlim.Release();
            }

        }

        public async Task GenerateAsync() {
            var downloaders = new List<Task>();
            if (!Directory.Exists(IntermediateVideoCache)) {
                Directory.CreateDirectory(IntermediateVideoCache);
                var youtubeClient = new YoutubeClient();
                var streamManifest = await youtubeClient.Videos.Streams.GetManifestAsync(Key);
                var streamInfo = streamManifest.GetMuxedStreams();
                downloaders.AddRange(streamInfo.Select(x => DownloadVideoAsync(youtubeClient, x)));
            }
            if (!File.Exists(IntermediateImageCache)) {
                downloaders.Add(DownloadImage());
            }
            if (downloaders.Count > 0) {
                await Task.WhenAll(downloaders);
            }
            foreach (var file in Directory.EnumerateFiles(IntermediateVideoCache, "*", SearchOption.AllDirectories)) {
                var fileName = Path.GetFileName(file);
                var heigth = Path.GetFileName(Path.GetDirectoryName(file));
                if (!int.TryParse(heigth, out int videoHeight)) {
                    await Utils.CopyFileAsync(file, Path.Combine(Source, fileName));
                } else {
                    var path = Path.Combine(Source, heigth, fileName);
                    Mips.Add(videoHeight, path.Replace(_storageDirectory, ""));
                    await Utils.CopyFileAsync(file, path);
                }
            }
        }
    }

    public class Video : IImage {
        private string VideoCode { get; }
        private float Aspect { get; }

        public bool ShowControls { get; set; } = true;
        public bool AutoPlay { get; set; } = false;
        public bool Loop { get; set; } = false;
        public bool Sound { get; set; } = true;

        public Task<VideoResource> ResourceTask { get; private set; }

        private readonly bool _isPosterDefinedByUser;
        private VideoResource VideoResource;
        private Image _image;

        public Video(string videoCode, Image image = null, float aspect = 16f / 9f) {
            VideoCode = videoCode;
            Aspect = aspect;
            _image = image;
            _isPosterDefinedByUser = _image != null;
        }

        //public Video Configure(Action<Video> action) {
        //    action(this);
        //    return this;
        //}

        public Video ConfigureAsBackgroundVideo() {
            ShowControls = false;
            AutoPlay = true;
            Loop = true;
            Sound = false;
            return this;
        }


        public async ValueTask<Image> GetImageAsync(Context context) {
            if (_image == null) {
                VideoResource ??= await context.Storage.AddOrGetAsync(VideoCode, () => new VideoResource(VideoCode, context.Storage));
                _image = new Image(VideoResource.IntermediateImageCache);
            }
            return _image;
        }

        public async Task<INode> GenerateBlockHtmlAsync(Context context) {
            VideoResource ??= await context.Storage.AddOrGetAsync(VideoCode, () => new VideoResource(VideoCode, context.Storage));
            _image ??= new Image(VideoResource.IntermediateImageCache);
            context.Includes.Require(new Style(AbsolutePath("Video.scss")));
            var mips = VideoResource.Mips.Select(x =>
                new KeyValuePair<int, string>(x.Key, new Uri(context.Urls.BaseUri, x.Value.Replace("\\", "/")).ToString()));
            var tag = new Tag("div", new { Class = "VideoPlayer" }) {
                new JSCall(AbsolutePath("Video.js"),//element, code, aspect, showControls, autoPlay, loop, sound, mips, poster
                        VideoCode,
                        Aspect,
                        ShowControls,
                        AutoPlay,
                        Loop,
                        Sound,
                        mips,
                        _isPosterDefinedByUser).Generate(context)
            };
            if (_isPosterDefinedByUser) {
                tag.Add(await _image.GenerateBlockHtmlAsync(context));
            }
            return tag;
        }
    }
}