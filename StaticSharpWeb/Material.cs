using StaticSharp;
using StaticSharp.Gears;
using StaticSharp.Html;
using StaticSharpWeb.Html;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharpWeb {

    public interface IMaterial {
        string Title { get; }
        Paragraph? Description { get; }//TODO: IPlainTextProvider
        //IImage TitleImage { get; }
    }

    /*internal interface IFontProvider {
        public Font Font { get; }
    }*/

    [ScriptBefore]
    public abstract class Material : Hierarchical<HierarchicalJs>, IMaterial, IInline, IPage, IPlainTextProvider {
        //public class TChildren : List<object> { }

        //public virtual IImage TitleImage => null;
        public virtual string Title {
            get {
                var n = GetType().Namespace;
                return n[(n.LastIndexOf('.') + 1)..];
            }
        }
        public virtual Paragraph? Description => null;

        public virtual Group? Content => null;
        public virtual IBlock? Footer => null;
        public virtual int ContentWidth => 400;

        //public virtual RightSideBar RightSideBar => null;
        public virtual IBlock? LeftSideBar => null;

        public override List<Modifier> Modifiers => new(){
            new(){
                FontSize = 16,
                FontFamilies = new[]{
                    new FontFamily("Roboto")
                },
                FontStyle = new FontStyle(FontWeight.Regular)
            },
        };

        public override string TagName => "body";

        public Material([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) {}



        //public virtual Font Font => new(Path.Combine(AbsolutePath(), "Fonts", "roboto"), FontWeight.Regular);

        public async Task<string> GeneratePageHtmlAsync(Context context) {

            context.Includes.Require(new Script(AbsolutePath("StaticSharp.js")));
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

            var document = new Tag(null) {
                new Tag("!doctype"){ ["html"] = ""},
                head,
                await GenerateHtmlAsync(context)
            };
            head.Add(await context.Includes.GenerateScriptAsync());
            head.Add(await context.Includes.GenerateFontAsync());
            head.Add(await context.Includes.GenerateStyleAsync());
            return document.GetHtml();
        }


        public override void AddRequiredInclues(IIncludes includes) {
            base.AddRequiredInclues(includes);
            includes.Require(new Script(AbsolutePath("Material.js")));
        }

        
        public override async Task<Tag?> GenerateHtmlInternalAsync(Context context, Tag elementTag) {
            return new Tag(null) {
                await LeftSideBar?.GenerateHtmlAsync(context,"LeftSideBar"),

                await new Column() {
                    Content
                }.GenerateHtmlAsync(context,"Content"),
            };
        }

        public virtual async Task<Tag> GetBodyAsync(Context context) {
            //FIXME: same code in Block.cs

            AddRequiredInclues(context.Includes);
            foreach (var m in Modifiers) {
                m.AddRequiredInclues(context.Includes);
                context = m.ModifyContext(context);
            }


            return new Tag("body") {
                CreateScriptInitialization(),
                Modifiers.Select(x=>x.CreateScriptInitialization()),

                CreateScriptBefore(),
                Modifiers.Select(x=>x.CreateScriptBefore()),

                

                Modifiers.Select(x=>x.CreateScriptAfter()).Reverse(),
                CreateScriptAfter()
            };


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

            
        }

        public async Task<Tag> GenerateInlineHtmlAsync(Context context) {
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
        }

        public async Task<string> GetPlaneTextAsync(Context context) {
            return Title;
        }
    }

    //public class MaterialContent : ElementContainer, ITextAnchorsProvider, IFillAnchorsProvider, IWideAnchorsProvider {}
}