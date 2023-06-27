using Scopes;
using StaticSharp.Gears;
using StaticSharp.Html;

namespace StaticSharp {

    public interface JAbstractInline : JHierarchical {

    }

    [ConstructorJs]
    public abstract partial class AbstractInline : BaseModifier, IInline {
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

        protected AbstractInline(AbstractInline other,
            int callerLineNumber = 0,
            string callerFilePath = ""
            ) : base(other, callerLineNumber, callerFilePath) {

            MarginLeft = other.MarginLeft;
            MarginRight = other.MarginRight;
            MarginTop = other.MarginTop;
            MarginBottom = other.MarginBottom;

            PaddingLeft = other.PaddingLeft;
            PaddingRight = other.PaddingRight;
            PaddingTop = other.PaddingTop;
            PaddingBottom = other.PaddingBottom;

        }

        public abstract string GetPlainText(Context context);

        public override void ModifyTagAndScript(Context context, Tag tag, Group scriptBeforeConstructor, Group scriptAfterConstructor) {
            base.ModifyTagAndScript(context, tag, scriptBeforeConstructor, scriptAfterConstructor);
            
            if (MarginLeft != null || MarginRight != null || MarginTop != null || MarginBottom != null) {
                tag.Style["margin"] = $"{MarginTop ?? 0}em {MarginRight ?? 0}em {MarginBottom ?? 0}em {MarginLeft ?? 0}em";
            }

            if (PaddingLeft != null || PaddingRight != null || PaddingTop != null || PaddingBottom != null) {
                tag.Style["padding"] = $"{PaddingTop ?? 0}em {PaddingRight ?? 0}em {PaddingBottom ?? 0}em {PaddingLeft ?? 0}em";
            }           

        }

    }
}