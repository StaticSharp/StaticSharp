﻿using MimeTypes;
using StaticSharp.Gears;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;



namespace StaticSharp {
    public record AssemblyResourceGenome(Assembly Assembly, string Path) : Genome<Asset> {
        public override Task<Asset> CreateAsync() {
            /*if (!LoadData<Data>(out var data)) {
                data.ContentHash = Hash.CreateFromBytes(ReadAllBites()).ToString();
                CreateCacheSubDirectory();
                StoreData(data);
            }
            ContentHash = data.ContentHash;*/

            var extension = System.IO.Path.GetExtension(Path);

            return Task.FromResult(new Asset(
                () => {
                    var resourcePath = Assembly.GetName().Name + "." + Path;
                    using var stream = Assembly.GetManifestResourceStream(resourcePath);
                    //throw

                    using (var memoryStream = new MemoryStream()) {
                        stream.CopyTo(memoryStream);
                        return memoryStream.ToArray();
                    }
                },
                extension,
                MimeTypeMap.GetMimeType(extension),
                ));
        }
    }

    //Instead of inheritance from CacheableHttpRequest, it is better to use aggregation
    namespace Gears {

        public static partial class KeyCalculators {
            public static string GetKey(Assembly assembly) {
                return KeyUtils.Combine<Assembly>(assembly.FullName);
            }
        }


        /*public class AssemblyResourceAsset : CacheableToFile<AssemblyResourceGenome>, Asset {
            class Data {
                public string ContentHash = null!;
            };
            public string MediaType => MimeTypeMap.GetMimeType(FileExtension);
            public string ContentHash { get; private set; } = null!;
            public string FileExtension => Path.GetExtension(Genome.Path);

            protected override Task CreateAsync() {
                if (!LoadData<Data>(out var data)) {
                    data.ContentHash = Hash.CreateFromBytes(ReadAllBites()).ToString();
                    CreateCacheSubDirectory();
                    StoreData(data);
                }
                ContentHash = data.ContentHash;
                return Task.CompletedTask;
            }

            public override byte[] ReadAllBites() {
                if (Content == null) {
                    var resourcePath = Genome.Assembly.GetName().Name + "." + Genome.Path;
                    using var stream = Genome.Assembly.GetManifestResourceStream(resourcePath);
                    //throw

                    using (var memoryStream = new MemoryStream()) {
                        stream.CopyTo(memoryStream);
                        Content = memoryStream.ToArray();
                    }
                }
                return Content;
            }

        }*/
    }
}

