using System;


namespace StaticSharp.Gears;

public abstract record TextProcessorGenome(Genome<IAsset> Source) : Genome<IAsset> {
    class Data {
        public string Extension = null!;
        public string ContentHash = null!;
        public string SourceHash = null!;
    };

    protected override void Create(out IAsset value, out Func<bool>? verify) {

        Data data;
        var source = Source.Get();
        var slot = Cache.GetSlot(Key);
        if (slot.LoadData(out data) && data.SourceHash == source.ContentHash) {
            value = new BinaryAsset(
                slot.LoadContent(),
                data.Extension,
                data.ContentHash
                );
        } else {
            var extension = source.FileExtension;
            var text = Process(source.Text, ref extension);

            

            value = new TextAsset(
                text,
                extension
                );


            data = new();
            data.Extension = value.FileExtension;
            data.SourceHash = source.ContentHash;
            data.ContentHash = value.ContentHash;
            var content = value.Data;

            slot.StoreContent(content).StoreData(data);
        }

        verify = () => {
            return data.SourceHash == Source.Get().ContentHash;
        };

    }

    protected abstract string Process(string text, ref string extension);

}



