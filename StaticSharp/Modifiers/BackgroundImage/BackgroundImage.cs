using AngleSharp.Dom;
using NUglify;
using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;
using static StaticSharp.Image;

namespace StaticSharp {




    public interface JBackgroundImage : JAbstractBackground {
        double NativeWidth { get; }
        double NativeHeight { get; }
    }
        [ConstructorJs]
    public partial class BackgroundImage : AbstractBackground {
        public required Genome<IAsset> ImageGenome;

        public BackgroundImage(Genome<IAsset> imageGenome, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            ImageGenome = imageGenome;
        }
        public BackgroundImage(BackgroundImage other, int callerLineNumber, string callerFilePath)
            : base(other, callerLineNumber, callerFilePath) {
            ImageGenome = other.ImageGenome;
        }


        public override IdAndScript Generate(Tag element, Context context) {
            var result = base.Generate(element, context);

            var imageAsset = ImageGenome.ToWebImage().Result;
            var imageInfo = imageAsset.GetImageInfo();
            var url = context.PathFromHostToCurrentPage.To(context.AddAsset(imageAsset)).ToString();

            result.Script.Add($"{result.Id}.RawImage = \"url({url})\"");

            result.Script.Add($"{result.Id}.NativeWidth = {Javascriptifier.ValueStringifier.Stringify(imageInfo.Width)}");
            result.Script.Add($"{result.Id}.NativeHeight = {Javascriptifier.ValueStringifier.Stringify(imageInfo.Height)}");

            return result;
        }
    }

}
