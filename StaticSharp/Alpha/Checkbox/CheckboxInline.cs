using StaticSharp.Gears;
using StaticSharp.Html;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Js {
        [Mix(typeof(Inline))]
        [Mix(typeof(Checkbox))]
        public partial class CheckboxInline {
        }
    }


    [Mix(typeof(CheckboxBindings<Js.CheckboxInline>))]
    [ConstructorJs("Checkbox")]

    [ConstructorJs]
    public partial class CheckboxInline : Inline {
        protected override string TagName => "checkboxInline";
        public CheckboxInline(CheckboxInline other, string callerFilePath, int callerLineNumber) : base(other, callerLineNumber, callerFilePath) { }
        public CheckboxInline([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) : base(callerLineNumber, callerFilePath) { }

        protected override async Task ModifyHtmlAsync(Context context, Tag elementTag) {

            var result = new Tag("input") {
                ["type"] = "checkbox"
            };


            if (Children.Count != 0) {
                var luid = context.GetUniqueId();
                result.Id = luid;

                var label = new Tag("label") { ["for"] = luid };
                foreach (var i in Children) {
                    var child = await i.Value.GenerateHtmlAsync(context,new Role(true,i.Key));
                    label.Add(child);
                }

                result = new Tag() {
                    result,
                    label
                };
            }

            elementTag.Add(result);


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