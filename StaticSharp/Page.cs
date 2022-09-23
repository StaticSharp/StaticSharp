using StaticSharp;
using StaticSharp.Gears;
using StaticSharp.Html;
using StaticSharpWeb.Html;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharp {

    public interface IMaterial {
        string Title { get; }
        Inlines? Description { get; }//TODO: IPlainTextProvider
        //IImage TitleImage { get; }
    }



    namespace Gears {
        [System.Diagnostics.DebuggerNonUserCode]
        public class PageJs : BaseModifierJs {
            public float WindowWidth =>  NotEvaluatableValue<float>();
            public float WindowHeight => NotEvaluatableValue<float>();
            public float ContentWidth => NotEvaluatableValue<float>();

            public float DevicePixelRatio => NotEvaluatableValue<float>();
            public bool Touch => NotEvaluatableValue<bool>();
            public bool UserInteracted => NotEvaluatableValue<bool>();

            public float FontSize => NotEvaluatableValue<float>();

        }

        public class PageBindings<FinalJs> : BaseModifierBindings<FinalJs> where FinalJs : new() {            
            public Binding<float> ContentWidth { set { Apply(value); } }
            public Binding<float> FontSize { set { Apply(value); } }


        }

    }


    [Mix(typeof(PageBindings<PageJs>))]
    [ConstructorJs]
    [RelatedScript("Watch")]
    [RelatedScript("Color")]
    [RelatedScript("Cookies")]
    [RelatedScript("Depth")]
    public abstract partial class Page : BaseModifier, IMaterial, IPage, IPlainTextProvider {
        protected virtual void Setup() {
            FontSize = 16;
        }

        public virtual string Title {
            get {
                var n = GetType().Namespace;
                return n[(n.LastIndexOf('.') + 1)..];
            }
        }
        public virtual Inlines? Description => null;
        public virtual Blocks? Content => null;
        public virtual IBlock? Footer => null;

        //public virtual RightSideBar RightSideBar => null;
        public virtual IBlock? LeftSideBar => null;
        protected override string TagName => "body";

        public Page([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) {

            FontFamilies = new[] { new FontFamily("Roboto") };
            FontStyle = new FontStyle(FontWeight.Regular);
        }

        public async Task<string> GeneratePageHtmlAsync(Context context) {

            Setup();

            context.Includes.Require(new Style(AbsolutePath("Normalization.scss")));

            context.Includes.Require(new Style(AbsolutePath("Debug.scss")));
            

            var head = new Tag("head"){
                    new Tag("meta"){["charset"] = "utf-8" },
                    new Tag("meta"){
                        ["name"]="viewport",
                        ["content"] = "width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0"
                    },
                    new Tag("title"){
                        Title
                    },
                    //<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;"/>
                };

            
            var body = await GenerateHtmlAsync(context);
            


            var document = new Tag(null) {
                new Tag("!doctype"){ ["html"] = ""},
                head,
                body
            };

            body.Add(
                new Tag("svg") {
                    Style = {
                        ["display"] = "none"
                    },
                    Children = {
                        new Tag("defs"){
                            await context.SvgDefs.GetAllAsync()
                        }
                    }
                }
                );

            
            
            head.Add(context.GenerateScript());
            head.Add(await GenerateFontsAsync(context));
            head.Add(await context.Includes.GenerateStyleAsync());


            

            return document.GetHtml();
        }

        public static async Task<Tag> GenerateFontsAsync(Context context) {
            var fontStyle = new StringBuilder();

            var sortedFonts = context.Fonts.OrderBy(x => x.Key);

            foreach (var i in sortedFonts.Select(x=>x.Value)) {
                fontStyle.AppendLine(await i.GenerateIncludeAsync());
            }
            return new Tag("style") {
                new PureHtmlNode(fontStyle.ToString())
            };
        }

        public virtual async Task<Tag> GenerateHtmlAsync(Context context, string? id = null) {

            await AddRequiredInclues(context);

            context = ModifyContext(context);

            var tag = new Tag(TagName, id) { };

            ModifyTag(tag);

            tag.Add(await CreateConstructorScriptAsync(context));

            tag.Add(await GenerateChildrenHtmlAsync(context, tag));

            return tag;
        }

        protected async Task<Tag?> GenerateChildrenHtmlAsync(Context context, Tag elementTag) {
            return new Tag(null) {
                await LeftSideBar?.GenerateHtmlAsync(context,"LeftSideBar"),

                await new Column {
                    Children = {
                        Content,
                        new Space(){
                            Between = 1
                        },
                        Footer
                    }
                    
                }.GenerateHtmlAsync(context,"Content"),
            };
        }

        //public virtual async Task<Tag> GetBodyAsync(Context context) {
            //FIXME: same code in Block.cs

            /*AddRequiredInclues(context.Includes);
            foreach (var m in Modifiers) {
                m.AddRequiredInclues(context.Includes);
                context = m.ModifyContext(context);
            }


            return new Tag("body") {
                CreateScript(),
                Modifiers.Select(x=>x.CreateScript()),

                CreateScriptBefore(),
                Modifiers.Select(x=>x.CreateScriptBefore()),

                

                Modifiers.Select(x=>x.CreateScriptAfter()).Reverse(),
                CreateScriptAfter()
            };*/


            /*var body = new Hierarchical {
                Modifiers = Modifiers
            };


            var bodyTag = await Body.GenerateHtmlWithChildrenAsync(context,(innerContext) => new Task<Tag>?[]{
                    Task.FromResult(new JSCall("Material", new { ContentWidth = ContentWidth }).Generate(innerContext)),

                    LeftSideBar?.GenerateHtmlAsync(innerContext,"LeftSideBar"),


                    new Column() {
                        Content
                    }.GenerateHtmlAsync(innerContext,"Content")
                }         
            );*/







            /*var body = new Tag("body", 
                new {
                    style = SoftObject.MergeObjects(
                        new {
                            Display = "none",
                            BackgroundColor = context.Theme.Surface,
                            Color = context.Theme.OnSurface,
                            FontSize = FontSize.ToString()+"px",

                        },
                        font
                    )
                }                
                ){

            };*/

            /*var contentTag = await new Column() {
                Content
            }.GenerateHtmlAsync(context);
            contentTag.Attributes["id"] = "Content";
            bodyTag.Add(contentTag);*/

            


            /*content.Name = "div";*/
            


            /*attributes["xmlns"] = "http://www.w3.org/2000/svg";
            attributes["xmlns:xlink"] = "http://www.w3.org/1999/xlink";*/


            /*content.Add(await new Spacer().GenerateHtmlAsync(context));



            if (Footer != null) {
                content.Add((await Footer.GenerateHtmlAsync(context)).ModifyIfNotNull(x => {
                    x.AttributesNotNull["id"] = "Footer";
                }));
            }*/

            


            /*var tasks = new List<Task<INode>>();
            if (RightSideBar != null) {
                body.Add(await RightSideBar.GenerateSideBarAsync(context));
            }
            if (LeftSideBar != null) {
                body.Add(await LeftSideBar.GenerateSideBarAsync(context));
            }

            if (Title != null) {
                body.Add(new Tag("h1", new { id = "Header" }) { Title });
            }
            if(TitleImage != null) {
                tasks.Add(TitleImage.GenerateHtmlAsync(context));
            }
            if (Description != null) {
                tasks.Add(Description.GenerateHtmlAsync(context));
            } else {
                //warning
            }

            if (Content != null) {
                tasks.AddRange(Content.Items.Select(x => x.GenerateHtmlAsync(context)));
            }
            if (Footer != null) {
                tasks.Add(Footer.GenerateHtmlAsync(context));
            }
            foreach (var item in await Task.WhenAll(tasks)) {
                body.Add(item);
            }*/

            
//        }


        /*public Link GetLink([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) {
            if (this is StaticSharpEngine.IRepresentative representative) {
                return new NodeLink(representative.Node, callerFilePath, callerLineNumber);
            } else {
                throw new InvalidOperationException();//TODO: details
            }
            

        }*/

        /*public async Task<Tag> GenerateInlineHtmlAsync(Context context, string? id, string? format) {
            var representative = this as  StaticSharpEngine.IRepresentative;
            //todo: material outside of the tree exceprion
            var uri = context.NodeToUrl(representative?.Node);
            if (uri == null) {
                throw new NullReferenceException();//todo: special exception
            }

            var result = new Tag("a"){
                ["href"] = uri,
                Children = { Title }
            };

            if (Description != null) {
                //result.Attributes["title"] = await Description.GetPlaneTextAsync(context);
            }

            return result;
        }*/

        public async Task<string> GetPlaneTextAsync(Context context) {
            return Title;
        }
    }

    //public class MaterialContent : ElementContainer, ITextAnchorsProvider, IFillAnchorsProvider, IWideAnchorsProvider {}
}