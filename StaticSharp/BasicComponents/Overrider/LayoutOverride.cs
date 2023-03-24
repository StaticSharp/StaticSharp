using AngleSharp.Html;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StaticSharp.Gears;
using StaticSharp.Html;
using SvgIcons;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace StaticSharp
{

    namespace Js
    {
        public interface LayoutOverride : Block
        {
            public Block Target { get; }
        }
    }


    namespace Gears
    {
        public class LayoutOverrideBindings<FinalJs> : BlockBindings<FinalJs>
        {
            public Binding<double> OverrideX { set { Apply(value); } }
            public Binding<double> OverrideY { set { Apply(value); } }

            public Binding<double> OverrideWidth { set { Apply(value); } }
            public Binding<double> OverrideHeight { set { Apply(value); } }

            public Binding<double> OverridePaddingLeft { set { Apply(value); } }
            public Binding<double> OverridePaddingRight { set { Apply(value); } }
            public Binding<double> OverridePaddingTop { set { Apply(value); } }
            public Binding<double> OverridePaddingBottom { set { Apply(value); } }
        }
    }


    [Mix(typeof(LayoutOverrideBindings<Js.LayoutOverride>))]
    [ConstructorJs]
    public partial class LayoutOverride: Block, IBlock
    {
        public Block Target { get; set; }

        public LayoutOverride(Block target, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
            : base(callerLineNumber, callerFilePath) {
            Target = target;
        }

        protected override void ModifyHtml(Context context, Tag elementTag)
        {
            elementTag.Add(CreateScript_SetCurrentSocket(nameof(Target)));
            elementTag.Add(Target.GenerateHtml(context));
            base.ModifyHtml(context, elementTag);
        }
    }
}
