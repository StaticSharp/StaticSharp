using StaticSharp.Gears;
using StaticSharp.Html;
using StaticSharp.Tree;
using System.Runtime.CompilerServices;


namespace StaticSharp {

    namespace Js {
        public interface Page : BaseModifier {
            public double Width  { get; }
            public double Height  { get; }
            public double FontSize  { get; }

        }
    }

    namespace Gears {
        public class PageBindings<FinalJs> : BlockBindings<FinalJs> {
            //public Binding<double> FontSize { set { Apply(value); } }

        }
    }


    




    public interface IMainVisual {
        void GetMeta(Dictionary<string,string> meta, Context context);
    }



    [Mix(typeof(PageBindings<Js.Page>))]
    [ConstructorJs]
    [Scripts.Storage]
    [RelatedStyle("../Normalization")]

    public abstract partial class Page : Block {
        protected virtual void Setup(Context context) {
            FontSize = 16;
        }
        public virtual Genome<IAsset>? Favicon => null;
        public virtual string? SiteName => null;
        public abstract string PageLanguage { get; }
        public abstract string Title { get; }
        public abstract object? MainVisual { get; }
        
        public abstract Inlines? Description { get; }
        public abstract Node VirtualNode { get; }

        protected override string TagName => "body";


        public override abstract Blocks UnmanagedChildren { get; }


        public Page([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
            : base(callerLineNumber, callerFilePath) {
            CodeFontFamilies = new[] { new FontFamilyGenome("Roboto Mono") };
            FontFamilies = new(){ new FontFamilyGenome("Roboto") };
            Weight = StaticSharp.FontWeight.Regular;

        }

        protected override void AddRequiredInclues(Context context) {

            base.AddRequiredInclues(context);

            if (context.DeveloperMode) {
                var genome = new Scripts.DeveloperModeAttribute().GetGenome();
                context.AddScript(genome);
            }
            
        }

        private Tag GenerateMetaTags(Context context) {


            var meta = new Dictionary<string, string>();
            if (SiteName!=null)
                meta["og:site_name"] = SiteName;

            meta["og:title"] = Title;
            meta["twitter:title"] = Title;
            meta["twitter:card"] = "summary_large_image";

            var url = context.NodeToAbsoluteUrl(VirtualNode).ToString();
            meta["og:url"] = url;
            meta["twitter:url"] = url;

            if (Description != null) {
                string description = Description.GetPlainText(context);
                meta["description"] = description;
                meta["og:description"] = description;
                meta["twitter:description"] = description;
            }

            meta["og:type"] = "website";

            if (MainVisual is IMainVisual mainVisual) {
                mainVisual.GetMeta(meta,context);
            }
            var result = new Tag() {
                meta.Select(x=>Tag.Meta(x.Key,x.Value))
            };

            return result;
        }


        public Tag? GenerateFavicon(Context context) {
            //<link rel="icon" type="image/x-icon" href="/images/favicon.ico">
            if (Favicon == null)
                return null;
            var asset = Favicon.Result;

            var url = context.PathFromHostToCurrentPage.To(context.AddAsset(asset));
            return new Tag("link") {
                ["rel"] = "icon",
                ["type"] = asset.GetMediaType(),
                ["href"] = url,
            };
        }

        public string GeneratePageHtml(Context context) {

            Setup(context);
            

            var head = new Tag("head"){
                    new Tag("meta"){["charset"] = "utf-8" },
                    new Tag("meta"){
                        ["name"]="viewport",
                        ["content"] = "width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0"
                    },
                    new Tag("title"){
                        Title
                    },
                    GenerateMetaTags(context),
                    GenerateFavicon(context)
                    //<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;"/>
                };


            var body = GenerateHtml(context);
            //body.Style["visibility"] = "hidden";


            var document = new Tag(null) {
                new Tag("!doctype"){ ["html"] = ""},
                new Tag("html") {
                    ["lang"] = PageLanguage,
                    Children ={
                        head,
                        body
                    }
                }
                
            };

            head.Add(context.GenerateScript());
            head.Add(context.GenerateStyle());
            head.Add(context.GenerateFonts());

            return document.GetHtml();
        }

        protected override void ModifyHtml(Context context, Tag elementTag) {

            base.ModifyHtml(context, elementTag);

            var svgDefsTags = context.SvgDefs.GetOrderedItems().ToArray();
            if (svgDefsTags.Length > 0) {
                elementTag.Add(
                    new Tag("svg") {
                        Style = {
                        ["display"] = "none"
                        },
                        Children = {
                        new Tag("defs"){
                            context.SvgDefs.GetOrderedItems()
                        }
                        }
                    });
                elementTag.Add(CreateScript_AssignPreviousTagToParentProperty("svgDefs"));
            }

            /*var children = Children.ToArray();
            if (children.Length > 0) {
                BeginChildren(elementTag);
                elementTag.Add(Children.GenerateHtml(context));
            }


            elementTag.Add(BodyContent.GenerateHtml(context));*/

            //elementTag.Add(BodyContent.GenerateHtml(context));
            
        }


    }

}