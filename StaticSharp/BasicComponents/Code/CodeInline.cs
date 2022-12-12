using StaticSharp.Gears;
using StaticSharp.Html;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    [RelatedStyle]
    public partial class CodeInline : Inline {
        protected override string TagName => "code-inline";

        public string Code { get; init; }
        public CodeInline(string code,
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            Code = code;
        }
        public CodeInline(CodeInline other, int callerLineNumber, string callerFilePath) : base(other, callerLineNumber, callerFilePath) {
            Code = other.Code;
        }

        protected override void ModifyHtml(Context context, Tag elementTag) {
            context.FontFamilies = context.CodeFontFamilies;
            
            if (context.FontFamilies != null) {
                elementTag.Style["font-family"] = string.Join(',', context.FontFamilies.Select(x => x.Name));
            }
            elementTag.Add(new Text(Code,true, callerLineNumber, callerFilePath).GenerateHtml(context,null));

            base.ModifyHtml(context, elementTag);
        }

        public override string GetPlaneText(Context context) {
            return Code + ((IPlainTextProvider)Children).GetPlaneText(context);
        }
    }
}