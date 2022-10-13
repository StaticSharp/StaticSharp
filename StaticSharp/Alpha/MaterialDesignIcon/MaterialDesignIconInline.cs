using MaterialDesignIcons;
using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Js {
        [Mix(typeof(MaterialDesignIcon))]
        [Mix(typeof(Block))]
        public partial class MaterialDesignIconInline: MaterialDesignIcon {
            public double BaselineOffset => NotEvaluatableValue<double>(); 
        }
    }

    namespace Gears {
        public class MaterialDesignIconInlineBindings<FinalJs> : MaterialDesignIconBindings<FinalJs> where FinalJs : new() {
            public Binding<double> BaselineOffset { set { Apply(value); } }
        }
    }

    [Mix(typeof(MaterialDesignIconInlineBindings<Js.MaterialDesignIconInline>))]
    [Mix(typeof(InlineBindings<Js.MaterialDesignIconInline>))]
    [ConstructorJs("MaterialDesignIcon")]
    [ConstructorJs]
    public partial class MaterialDesignIconInline : Inline {

        public IconName Icon { get; }
        public double Scale { get; set; } = 1;
        public MaterialDesignIconInline(MaterialDesignIcons.IconName icon, [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) {
            Icon = icon;
        }

        public MaterialDesignIconInline(MaterialDesignIconInline other, string callerFilePath, int callerLineNumber) : base(other, callerFilePath, callerLineNumber) {
            Icon = other.Icon;
            Scale = other.Scale;
        }

        protected override Task<Tag?> GenerateInlineHtmlInternalAsync(Context context, Tag elementTag, string? format) {
            
            var code = MaterialDesignIcon.GetSvgTag(Icon, out var width, out var height);

            elementTag["data-width"] = width;
            elementTag["data-height"] = height;
            if (Scale != 1) {
                elementTag["data-scale"] = Scale;
            }
            return Task.FromResult<Tag?>(
                new Tag(){
                    new PureHtmlNode(code)
                }
            );
        }


    }





}