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

        protected override async Task ModifyHtmlAsync(Context context, Tag elementTag) {
            context.FontFamilies = context.CodeFontFamilies;
            
            if (context.FontFamilies != null) {
                elementTag.Style["font-family"] = string.Join(',', context.FontFamilies.Select(x => x.Name));
            }
            elementTag.Add(await new Text(Code,true, callerLineNumber, callerFilePath).GenerateHtmlAsync(context,null));

            await base.ModifyHtmlAsync(context, elementTag);
        }

        public override async Task<string> GetPlaneTextAsync(Context context) {
            return Code + await((IPlainTextProvider)Children).GetPlaneTextAsync(context);
        }
    }
}