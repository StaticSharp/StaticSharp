using StaticSharp.Gears;
using StaticSharp.Html;

namespace StaticSharp{

    namespace Js {
        public interface Inline : Hierarchical {

        }
    }

    namespace Gears {
        public class InlineBindings<FinalJs> : BaseModifierBindings<FinalJs> {
        
        }
    }

    [ConstructorJs]
    public partial class Inline : BaseModifier, IInline {

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
                MarginTop= value;
                MarginBottom = value;
            }
        }
        public double Margins {
            set {
                MarginsHorizontal = value;
                MarginsVertical= value;
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



        public Inlines Children { get; init; } = new();

        protected Inline(Inline other,
            int callerLineNumber = 0,
            string callerFilePath = ""
            ) : base(other, callerLineNumber, callerFilePath) {
            Children = new(other.Children);
        }
        public Inline(
            int callerLineNumber = 0,
            string callerFilePath = "") : base(callerLineNumber, callerFilePath) { }

        public Inline(
            string text,
            int callerLineNumber = 0,
            string callerFilePath = "") : base(callerLineNumber, callerFilePath) {
            Children.Add(text);
        }


        public virtual string GetPlaneText(Context context) => "";

        protected override void ModifyHtml(Context context, Tag elementTag) {
            if (MarginLeft != null || MarginRight != null || MarginTop != null || MarginBottom != null) {
                elementTag.Style["margin"] = $"{MarginTop ?? 0}em {MarginRight ?? 0}em {MarginBottom ?? 0}em {MarginLeft ?? 0}em";
            }
            
            if (PaddingLeft != null || PaddingRight != null || PaddingTop != null || PaddingBottom != null) {
                elementTag.Style["padding"] = $"{PaddingTop ?? 0}em {PaddingRight ?? 0}em {PaddingBottom ?? 0}em {PaddingLeft ?? 0}em";
            }

            foreach (var c in Children) {
                var childTag = c.GenerateHtml(context);
                elementTag.Add(childTag);
            }
            base.ModifyHtml(context, elementTag);
        }

        public override string ToString() {
            throw new System.InvalidOperationException("Cast from Inline to String is forbidden.");
        }

    }
}