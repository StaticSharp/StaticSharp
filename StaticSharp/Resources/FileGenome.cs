
using System;


namespace StaticSharp {   


    namespace Gears {

        public record FileGenome(string Path) : Genome<IAsset> {
            class Data {
                public DateTime LastWriteTime;
                public string ContentHash = null!;
            };

            public override IAsset Create() {
                FileAsset result;

                if (LoadData<Data>(out var data) && data.LastWriteTime == FileAsset.GetLastWriteTime(Path)) {
                    result = new FileAsset(Path, data.ContentHash);
                } else {
                    result = new FileAsset(Path);
                    data.ContentHash = result.ContentHash;
                    data.LastWriteTime = result.LastWriteTime;
                    CreateCacheSubDirectory();
                    StoreData(data);
                }
                return result;
            }
        }
    }
}

