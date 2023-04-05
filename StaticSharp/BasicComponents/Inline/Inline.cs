using Scopes;
using StaticSharp.Gears;
using StaticSharp.Html;

namespace StaticSharp {

    namespace Js {
        public interface Inline : Hierarchical {

        }
    }

    namespace Gears {
        public class InlineBindings<FinalJs> : BaseModifierBindings<FinalJs> {
        
        }
    }


    [ConstructorJs]
    public abstract partial class Inline : BaseModifier, IInline {
        public double? MarginLeft { get; set; }
        public double? MarginRight { get; set; }
        public double? MarginTop { get; set; }
        public double? MarginBottom { get; set; }

        public double MarginsHorizontal {
            set {
                MarginLeft = value;
                MarginRight = value;
            }
        }
        public double MarginsVertical {
            set {
                MarginTop = value;
                MarginBottom = value;
            }
        }
        public double Margins {
            set {
                MarginsHorizontal = value;
                MarginsVertical = value;
            }
        }


        public double? PaddingLeft { get; set; }
        public double? PaddingRight { get; set; }
        public double? PaddingTop { get; set; }
        public double? PaddingBottom { get; set; }

        public double PaddingsHorizontal {
            set {
                PaddingLeft = value;
                PaddingRight = value;
            }
        }
        public double PaddingsVertical {
            set {
                PaddingTop = value;
                PaddingBottom = value;
            }
        }
        public double Paddings {
            set {
                PaddingsHorizontal = value;
                PaddingsVertical = value;
            }
        }

        protected Inline(Inline other,
            int callerLineNumber = 0,
            string callerFilePath = ""
            ) : base(other, callerLineNumber, callerFilePath) {
        }

        public Inline(
            int callerLineNumber = 0,
            string callerFilePath = "") : base(callerLineNumber, callerFilePath) { }


        public virtual string GetPlainText(Context context) => "";

        public override void ModifyTagAndScript(Context context, Tag tag, Group script) {
            base.ModifyTagAndScript(context, tag, script);
            
            if (MarginLeft != null || MarginRight != null || MarginTop != null || MarginBottom != null) {
                tag.Style["margin"] = $"{MarginTop ?? 0}em {MarginRight ?? 0}em {MarginBottom ?? 0}em {MarginLeft ?? 0}em";
            }

            if (PaddingLeft != null || PaddingRight != null || PaddingTop != null || PaddingBottom != null) {
                tag.Style["padding"] = $"{PaddingTop ?? 0}em {PaddingRight ?? 0}em {PaddingBottom ?? 0}em {PaddingLeft ?? 0}em";
            }           

        }

    }
}