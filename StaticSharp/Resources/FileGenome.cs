using StaticSharp.Gears;
using System;

namespace StaticSharp {
    namespace Gears {

        public record FileGenome(string Path) : Genome<IAsset> {
            class Data {
                public DateTime LastWriteTime;
                public string ContentHash = null!;
            };

            protected override void Create(out IAsset value, out Func<bool>? verify) {
                var lastWriteTime = FileAsset.GetLastWriteTime(Path);
                var slot = Cache.GetSlot(Key);
                if (slot.LoadData<Data>(out var data) && data.LastWriteTime == lastWriteTime) {
                    value = new FileAsset(Path, data.ContentHash);
                } else {
                    value = new FileAsset(Path);
                    data.ContentHash = value.ContentHash;
                    data.LastWriteTime = FileAsset.GetLastWriteTime(Path);
                    slot.StoreData(data);
                }
                verify = () => FileAsset.GetLastWriteTime(Path) == data.LastWriteTime;
            }
        }
    }
}

