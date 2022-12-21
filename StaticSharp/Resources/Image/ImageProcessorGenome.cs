using ImageMagick;

using System;


namespace StaticSharp.Gears;


public abstract record ImageProcessorGenome(Genome<IAsset> Source) : Genome<IAsset> {
    class Data {
        public string Extension = null!;
        public string ContentHash = null!;
        public string SourceHash = null!;
    };

    protected override void Create(out IAsset value, out Func<bool>? verify) {

        Data data;
        var source = Source.Result;
        var slot = Cache.GetSlot(Key);
        if (slot.LoadData(out data) && data.SourceHash == source.ContentHash ) {
            value = new BinaryAsset(
                slot.LoadContent(),
                data.Extension,
                data.ContentHash
                );                
        } else {
                
            var image = new MagickImage(source.Data);
            image = Process(image);

            var extension = "." + image.FormatInfo?.Format.ToString().ToLower() ?? "?";

            value = new BinaryAsset(
                image.ToByteArray(),
                extension
                );


            data = new();
            data.Extension = value.Extension;
            data.SourceHash = source.ContentHash;
            data.ContentHash = value.ContentHash;
            var content = value.Data;

            slot.StoreContent(content).StoreData(data);

            //SaveData(slot, source, value);
        }

        verify = () => {
            return data.SourceHash == Source.Result.ContentHash;
        };

    }

    protected abstract MagickImage Process(MagickImage image);

}



