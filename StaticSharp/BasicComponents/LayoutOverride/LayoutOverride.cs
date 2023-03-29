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
            public Block Content { get; }

            public double? OverrideX { get; }
            public double? OverrideY { get; }

            public double? OverrideWidth { get; }
            public double? OverrideHeight { get; }
        }
    }


    namespace Gears
    {
        public class LayoutOverrideBindings<FinalJs> : BlockBindings<FinalJs>
        {
            public Binding<double?> OverrideX { set { Apply(value); } }
            public Binding<double?> OverrideY { set { Apply(value); } }

            public Binding<double?> OverrideWidth { set { Apply(value); } }
            public Binding<double?> OverrideHeight { set { Apply(value); } }
        }
    }


    [Mix(typeof(LayoutOverrideBindings<Js.LayoutOverride>))]
    [ConstructorJs]
    public partial class LayoutOverride: Block
    {
        public required Block Content { get; set; }

        public LayoutOverride([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
            : base(callerLineNumber, callerFilePath) { }

        protected override void ModifyHtml(Context context, Tag elementTag)
        {
            elementTag.Add(CreateScript_SetCurrentSocket(nameof(Content)));
            elementTag.Add(Content.GenerateHtml(context));
            base.ModifyHtml(context, elementTag);
        }
    }
}
