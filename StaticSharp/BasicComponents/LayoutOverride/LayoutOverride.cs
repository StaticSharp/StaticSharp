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

    public interface JLayoutOverride : JBlock {
        public JBlock Child { get; }

        public double? OverrideX { get; set; }
        public double? OverrideY { get; set; }

        public double? OverrideWidth { get; set; }
        public double? OverrideHeight { get; set; }
    }

    [ConstructorJs]
    public partial class LayoutOverride: Block {
        [Socket]
        public required Block Child { get; set; }

        protected LayoutOverride(LayoutOverride other, int callerLineNumber, string callerFilePath) : base(other, callerLineNumber, callerFilePath) {
            Child = other.Child;
        }

    }
}
