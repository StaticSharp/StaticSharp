using Microsoft.AspNetCore.Components.Routing;
using StaticSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpDetachedDemo.Root
{
    [Representative]
    public partial class Ru : Page
    {
        public override string PageLanguage => Node.Language.ToString().ToLower();

        public override string Title
        {
            get
            {
                var n = GetType().Namespace;
                return n[(n.LastIndexOf('.') + 1)..].Replace('_', ' ');
            }
        }

        public override Inlines DescriptionContent => $"<Page description>";

        protected override Task Setup(Context context)
        {
            return base.Setup(context);
        }

        public override Block? MainVisual => new Paragraph("<Main text>");
            
        protected override Blocks BodyContent => new Blocks {
            { "MainVisual", MainVisual }
        };
    }
}
