using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StaticSharpDemo.Root {



    [Representative]
    public partial class Ru : Material{

        
        
        static string RandomString(int length, Random random) {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789     ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        Paragraph CreateParagraph(int numChars, Random random) {
            return new Paragraph(RandomString(numChars, random));
        }

        IEnumerable<Paragraph> CreateParagraphs(int count) {
            Random random = new Random(0);
            return Enumerable.Range(0, count).Select(i => CreateParagraph(50, random));
        }


        public LinkInline GithubUrl(string text = "GitHub repository") {
            return new LinkInline {
                HRef = "https://github.com/antilatency/Antilatency.Copilot",
                NewTab = true,
                ForegroundColor = Color.FromArgb(172, 196, 53),
                Children = {
                    text
                }
            };
        }

        public LinkInline DiscordUrl(string text = "Discord server") {
            return new LinkInline {
                HRef = "https://discord.gg/ZTqmfPsGEr",
                NewTab = true,
                ForegroundColor = Color.FromArgb(139, 148, 245),
                Children = {
                    text
                }
            };
        }

        public override Blocks? Content => new() {


            {"video",
                new Video("T4TEdzSLyi0"){
                    Play = true,//new (e=>e.Sibling("videoProperties").Child<CheckboxJs>("play").ValueActual),
                    Mute = new (e=>e.Sibling("videoProperties").Child<Js.Checkbox>("Mute").ValueActual),
                    PreferPlatformPlayer = new (e=>e.Sibling("videoProperties").Child<Js.Checkbox>("preferPlatformPlayer").ValueActual),
                    Controls = new (e=>e.Sibling("videoProperties").Child<Js.Checkbox>("controls").ValueActual),
                    Loop =  new (e=>e.Sibling("videoProperties").Child<Js.Checkbox>("loop").ValueActual),
                    Volume = new (e=>e.Sibling<Js.Slider>("VolumeSlider").ValueActual)
                }
            },

            {"VolumeSlider",
                new Slider { Value = new(e=>e.Sibling<Js.Video>("video").VolumeActual) }
            },

            {"videoProperties",
                $"""
                {new CheckboxInline(){ Value = new(e=>e.Parent.Sibling<Js.Video>("video").PlayActual), Label = { "Play" } }:#play}
                {new CheckboxInline(){ Label = { "Mute" } }:#Mute}
                {new CheckboxInline(){ Label = { "Prefer platform player" } }:#preferPlatformPlayer}
                {new CheckboxInline(){ Label = { "Controls" } }:#controls}
                {new CheckboxInline(){ Label = { "Loop" } }:#loop}
                """
            },


            new Column(){
                BackgroundColor = Color.FromArgb(255,32,32,32),
                Children = {
                    $"Refer to {GithubUrl()} for more information, and join our {DiscordUrl()} to learn more about getting early access to Copilot.",

                    new Flipper() {
                        MarginLeft = new (e=>e.ParentBlock.PaddingLeft),
                        MarginRight = new (e=>e.ParentBlock.PaddingRight),

                        First = new Column(){
                            MarginLeft = 10,
                            MarginRight = 10,

                            Children = {
                                new Space(),
                                H4("Antilatency Copilot.\nPositional solution for drones").Modify(x=>{
                                    x.LineHeight = 1.3f;
                                }),
                                "Copilot is an Antilatency project. We use our accurate optical-inertial tracking system with Raspberry Pi to provide you with precise indoor navigation and outdoor landing for drones in different use cases.",
                                new Space(0,2),
                            }
                        },
                        Second = new Image(new FileGenome(AbsolutePath("Copilot/SchemeDark.svg"))){
                            Embed = Image.TEmbed.Image,
                        }
                    }
                }

            }.FillWidth().InheritHorizontalPaddings(),

            new Flipper(){
                MarginLeft = new(e=>e.ParentBlock.MarginLeft),
                MarginRight = new(e=>e.ParentBlock.MarginRight),

                First = new Image(new FileGenome(AbsolutePath("Copilot/Delivery.svg"))){
                    Embed = Image.TEmbed.Image,
                    MarginLeft = 24,
                    MarginRight = 24,
                    MarginTop = 24,
                    MarginBottom = 24,
                },
                Second = new Column(){
                    MarginLeft = 10,
                    MarginRight = 10,

                    Children ={
                        new Space(),                        
                        //$"{new Checkbox():#id}",
                        H5("Increased delivery efficiency"),
                        "Autonomous landing at delivery points, automatic delivery scenarios in large warehouses and enterprises.",
                        new Space(),
                    }
                }
            },

            "This product is still under development, but you can get early access to Copilot, join discussions, and share your ideas in our community on Discord",


            /*new Image(new FileGenome(AbsolutePath("TestPsdImage.psd"))),

            new Template(new FileGenome(AbsolutePath("Reactive.template"))),

            H1($"H1"),
            "Abc",
            "Abc",*/
            

            /*H1("H1"),*/
            /*new Modifier {
                FontSize = new((element) => element.Sibling<SliderJs>("Slider").Value + 2),
                Children = { $"Modifier test" }
            },*/

            //CreateParagraphs(100),

            {
                "Slider",
                new Slider {
                    
                    //MarginTop = e=>e.Value,
                    Min = 10,
                    Max = 200
                }
            },

            /*$"A B C D {4}",
            new Space(),

            $"Bold: {new InlineModifier { FontStyle = new FontStyle { Weight = FontWeight.Bold }, Children = { "Text" } }}",
            $"Если   понадобится компонент,которого нет среди стандартных.",
            $"Можно создать компонент прям в проекте вашего сайта."*/
        };
    }
}
