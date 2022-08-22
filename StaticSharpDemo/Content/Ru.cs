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
            return new Paragraph() { RandomString(numChars, random) };
        }

        IEnumerable<Paragraph> CreateParagraphs(int count) {
            Random random = new Random(0);
            return Enumerable.Range(0, count).Select(i => CreateParagraph(50, random));
        }


        public IInline GithubUrl(string text = "GitHub repository") {
            return new InlineModifier() { text }
            .Url("https://github.com/antilatency/Antilatency.Copilot")
            .ForegroundColor(Color.FromArgb(172, 196, 53));
        }

        public IInline DiscordUrl(string text = "Discord server") {
            return new InlineModifier() { text }
            .Url("https://discord.gg/ZTqmfPsGEr")
            .ForegroundColor(Color.FromArgb(139, 148, 245));
        }

        public override Group? Content => new() {


            new Video("T4TEdzSLyi0"),

            //new Image(new HttpRequestGenome("https://upload.wikimedia.org/wikipedia/commons/4/49/Koala_climbing_tree.jpg")),

            new Column(){ 
                Modifiers = { 
                    new Modifier(){ 
                        Bindings ={ 
                            BackgroundColor = e=>Color.FromArgb(255,32,32,32),
                        }
                    }
                },
                Children = {

                    new Paragraph($"Refer to {GithubUrl()} for more information, and join our {DiscordUrl()} to learn more about getting early access to Copilot."){ 
                        Modifiers = {
                            new (){ 
                                LineHeight = 1.8f,
                                LetterSpacing = 0.2f
                            }
                        }
                    },
                    
                    new Flipper(){
                        Bindings = {
                            MarginLeft = e=>e.ParentBlock.PaddingLeft,
                            MarginRight = e=>e.ParentBlock.PaddingRight,
                        },
                        First = new Column(){
                            Bindings = {
                                MarginLeft = e=>10,
                                MarginRight = e=>10,
                            },
                            Children = {
                                H4("Antilatency Copilot.\nPositional solution for drones"),
                                "Copilot is an Antilatency project. We use our accurate optical-inertial tracking system with Raspberry Pi to provide you with precise indoor navigation and outdoor landing for drones in different use cases."
                            }
                        },
                        Second = new Image(new FileGenome(AbsolutePath("Copilot/SchemeDark.svg"))){
                            Embed = Image.TEmbed.Image,
                            /*Bindings = {
                                MarginLeft = e=>10,
                                MarginRight = e=>10,
                            }*/
                        }
                    }
                }
                
            }.ConsumeParentHorizontalMargins(),

            new Flipper(){
                Bindings = { 
                    MarginLeft = e=>e.ParentBlock.MarginLeft,
                    MarginRight = e=>e.ParentBlock.MarginRight,                    
                },
                First = new Image(new FileGenome(AbsolutePath("Copilot/Delivery.svg"))){ 
                    Embed = Image.TEmbed.Image,
                    Bindings = { 
                        MarginLeft = e=>24,
                        MarginRight = e=>24,
                        MarginTop = e=>24,
                        MarginBottom = e=>24,
                    }
                },
                Second = new Column(){
                    Bindings = { 
                        MarginLeft = e=>10,
                        MarginRight = e=>10,
                    },
                    Children ={
                        new Space(),
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
                    Bindings = {
                        MarginTop = e=>e.Value,
                        Min = e=> 10,
                        Max = e=> 200
                    }
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
