using Scopes;
using StaticSharp.Gears;
using StaticSharp.Html;

namespace StaticSharp {


    public interface JAspectBlockResizableContent : JAspectBlock {

    }

    [ConstructorJs]
    public abstract partial class AspectBlockResizableContent : AspectBlock {
        
        public sealed override void ModifyTagAndScript(Context context, Tag tag, Group scriptBeforeConstructor, Group scriptAfterConstructor) {
            base.ModifyTagAndScript(context, tag, scriptBeforeConstructor, scriptAfterConstructor);

            var contentId = context.CreateId();

            CreateContent(context, tag, scriptBeforeConstructor, scriptAfterConstructor, contentId, out var width, out var height);

            scriptBeforeConstructor.Add($"{tag.Id}.content = {TagToJsValue(contentId)}");

            scriptAfterConstructor.Add($"{tag.Id}.NativeWidth = {width.ToStringInvariant()}");
            scriptAfterConstructor.Add($"{tag.Id}.NativeHeight = {height.ToStringInvariant()}");            
        }

        public abstract void CreateContent(Context context, Tag tag, Group scriptBeforeConstructor, Group scriptAfterConstructor, string contentId, out double width, out double height);
    }
}