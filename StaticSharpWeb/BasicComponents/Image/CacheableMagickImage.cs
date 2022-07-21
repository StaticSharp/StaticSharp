using System.Net.Http.Headers;
using System.Text;

namespace StaticSharp {


    //Instead of inheritance from CacheableHttpRequest, it is better to use aggregation
    namespace Gears {



        /*public record ImageConverter(MagickImage MagickImage) : Constructor<MagickImage, CacheableMagickImage> {
            public record 
        }




        public class CacheableMagickImage : Cacheable<MagickImage, CacheableMagickImage.Data>, IStorable, IFile {



            public class Data {
                public string? CharSet;
                public string? MediaType;
            };


            public static readonly string DefaultMediaType = "application/octet-stream";

            public string? CharSet => CachedData.CharSet;
            public string MediaType => CachedData.MediaType;
            public byte[] Content { get; private set; } = null!;


            private string ContentFilePath => Path.Combine(CacheSubDirectory, "content");

            protected override async Task CreateAsync() {

                if (!LoadData()) {
                    CachedData = new();

                    var httpResponseMessage = await HttpClientStatic.Instance.SendAsync(Arguments.HttpRequestMessage);
                    if (!httpResponseMessage.IsSuccessStatusCode) {
                        throw new Exception(); //TODO: details
                    }

                    CachedData.CharSet = (httpResponseMessage.Content.Headers.ContentType?.CharSet);
                    CachedData.MediaType = (httpResponseMessage.Content.Headers.ContentType?.MediaType);

                    //CompleteHeaderTasks();

                    CreateCacheSubDirectory();

                    Content = await httpResponseMessage.Content.ReadAsByteArrayAsync();

                    await File.WriteAllBytesAsync(ContentFilePath, Content);

                    StoreData();
                } else {
                    Content = await File.ReadAllBytesAsync(ContentFilePath);
                }

            }



            public string ContentText {
                get {
                    Encoding encoding = (CharSet == null) ? Encoding.UTF8 : Encoding.GetEncoding(CharSet);
                    return encoding.GetString(Content);
                }
            }


            public Task StoreAsync(string storageRootDirectory) {
                throw new NotImplementedException();
            }



        }*/


    }
}

