using StaticSharp;
using StaticSharp.Gears;
using StaticSharp.Html;
using StaticSharpWeb.Html;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StaticSharpWeb {

    public interface IMaterial {
        string Title { get; }
        Row? Description { get; }//TODO: IPlainTextProvider
        //IImage TitleImage { get; }
    }

    /*internal interface IFontProvider {
        public Font Font { get; }
    }*/

    public abstract class Material : IMaterial, IElement, IPage, IPlainTextProvider {
        //public class TChildren : List<object> { }

        //public virtual IImage TitleImage => null;
        public virtual string Title {
            get {
                var n = GetType().Namespace;
                return n[(n.LastIndexOf('.') + 1)..];
            }
        }
        public virtual Row? Description => null;

        public virtual Column? Content => null;
        public virtual Column? Footer => null;
        public virtual int ContentWidth => 400;

        //public virtual RightSideBar RightSideBar => null;
        //public virtual LeftSideBar LeftSideBar => null;
        public virtual Body Body => new Body() {
            FontSize = 16,
            FontFamily = new FontFamily(AbsolutePath("Fonts/roboto")),
            //FontStyle = new FontStyle(),
        };

        //public virtual Font Font => new(Path.Combine(AbsolutePath(), "Fonts", "roboto"), FontWeight.Regular);

        public async Task<string> GeneratePageHtmlAsync(Context context) {

            context.Includes.Require(new Script(AbsolutePath("StaticSharp.js")));
            context.Includes.Require(new Style(AbsolutePath("Normalization.scss")));

            context.Includes.Require(new Style(AbsolutePath("Debug.scss")));
            

            var head = new Tag("head"){
                    new Tag("meta", new{ charset = "utf-8" }),
                    new Tag("meta", new{
                        name="viewport",
                        content = "width=device-width,initial-scale=1.0,maximum-scale=1.0,user-scalable=0"
                    }),
                    new Tag("title"){
                        Title
                    },
                    //<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;"/>
                };

            var document = new Tag(null) {
                new Tag("!doctype",new{ html = ""}),
                head,
                await GetBodyAsync(context)
            };
            head.Add(await context.Includes.GenerateScriptAsync());
            head.Add(await context.Includes.GenerateFontAsync());
            head.Add(await context.Includes.GenerateStyleAsync());
            return document.GetHtml();
        }

        public virtual async Task<Tag> GetBodyAsync(Context context) {


            /*var cacheableFont = Font.CreateOrGetCached();
            context.Includes.Require(cacheableFont);

            context.Font = Font;
            context.FontSize = FontSize;
            var font = cacheableFont.GenerateUsageCss(context);

            context.Includes.Require(new Script(Bindings.BindingsJsPath));*/




            (var bodyTag, context) = await Body.GenerateHtmlAndContextAsync(context);

            context.Includes.Require(new Script(AbsolutePath("Material.js")));
            bodyTag.Add(new JSCall("Material", new { ContentWidth = ContentWidth }).Generate(context));


            

            

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

            var content = Content;
            if (content != null) {
                var contentTag = await content.GenerateHtmlAsync(context);
                var attributes = contentTag.AttributesNotNull;
                attributes["id"] = "Content";
                bodyTag.Add(contentTag);
            }


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

            return bodyTag;
        }

        public async Task<Tag> GenerateHtmlAsync(Context context) {
            var representative = this as  StaticSharpEngine.IRepresentative;
            //todo: material outside of the tree exceprion
            var uri = context.NodeToUrl(representative?.Node);
            if (uri == null) {
                throw new NullReferenceException();//todo: special exception
            }

            var result = new Tag("a", new { href = uri }) { Title };

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