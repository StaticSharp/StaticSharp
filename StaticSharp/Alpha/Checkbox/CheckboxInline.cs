using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    [Mix(typeof(InlineJs))]
    [Mix(typeof(CheckboxJs))]
    public partial class CheckboxInlineJs {
    }



    [Mix(typeof(CheckboxBindings<CheckboxInlineJs>))]
    [ConstructorJs("Checkbox")]

    [ConstructorJs]
    public partial class CheckboxInline : Inline {
        protected override string TagName => "checkboxInline";
        public CheckboxInline(CheckboxInline other, string callerFilePath, int callerLineNumber) : base(other, callerFilePath, callerLineNumber) { }
        public CheckboxInline([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerFilePath, callerLineNumber) { }

        public Inlines Label { get; set; } = new();

        protected override async Task<Tag?> GenerateInlineHtmlInternalAsync(Context context, Tag elementTag, string? format) {

            var input = new Tag("input") {
                ["type"] = "checkbox"
            };


            if (Label.Count != 0) {
                var luid = context.GetUniqueId();
                input.Id = luid;

                var label = new Tag("label") { ["for"] = luid };
                foreach (var i in Label) {
                    label.Add(await i.Value.GenerateInlineHtmlAsync(context, i.Id, i.Format));
                }

                return new Tag() {
                    input,
                    label
                };
            }

            return input;

            /*if (string.IsNullOrEmpty(format)) {
                return Task.FromResult<Tag?>(new Tag("input") {
                    ["type"] = "checkbox"
                });
            } else {
                var luid = context.GetUniqueId();

                var label = new Tag("label") { ["for"] = luid };

                label.Add(format);

                return Task.FromResult<Tag?>(
                    new Tag {
                        new Tag("input",luid) {                            
                            ["type"] = "checkbox"
                        },
                        label,
                    }
                );

            }*/

            
        }

    }






}