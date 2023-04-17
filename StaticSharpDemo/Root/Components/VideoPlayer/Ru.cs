

using System.Linq;

namespace StaticSharpDemo.Root.Components.VideoPlayer {


    [Representative]
    public partial class Ru : Page {



        public override Block? MainVisual => new Video("T4TEdzSLyi0") {
            Play = true,
            Mute = true,
            Loop = true,
            Controls = false,
            PreferPlatformPlayer = false,
        };
        public override Inlines Description => $"StaticSharp VideoPlayer component.";

        /*public override Blocks UnmanagedChildren => new(){

            new InlineGroup($"Text {new SvgIconInline(SvgIcons.MaterialDesignIcons.Svg){
                BaselineOffset= 0.14,
                InternalLink = Node.Root,
                Scale = 1.2,
                BackgroundColor= Color.Pink,
                ForegroundColor= Color.Black,
            }}\n2"){ 
                BackgroundColor = Color.Red,
            }


            Enumerable.Range(0,10).Select(x=>
                new Block{
                    X = new (e=> 10*(x % 50) + e.Parent.Width * 0.1),
                    Y = 100*(x / 50),
                    Width = 10+x,
                    Height = 10+x,
                    BackgroundColor = Color.FromGrayscale(x%4*0.2)
                }
                //new Paragraph($"Paragraph #{x}"){ }
            )


            new ScrollLayout{ 
                Height = 350,

                Child = new LinearLayout{
                    Children = {
                        new Block(){
                            InternalLink = Node.Root,
                            Width = 200,
                            Height = 200,
                            BackgroundColor = Color.PaleVioletRed,
                            UnmanagedChildren = { 
                                new Image("https://lh3.googleusercontent.com/a/AGNmyxYN2cUQ-CcEkw1ShKBSjVnNTDcp4Bjk1d0nssLp=s96-c"){
                                    Width = 50,
                                    Height= 50,
                                    BackgroundColor = Color.Orange,
                                }
                            }
                        },
                        "Hello",

                        new Block(){
                            X = 300,
                            Width = 200,
                            Height = 200,
                            BackgroundColor= Color.Violet,
                        }
                    }
                }
            }

            
        };*/



        public override Blocks? Content => new() {

            /*new LayoutOverride {
                Child = new Video("T4TEdzSLyi0"){
                    BackgroundColor= Color.Black,
                    GravityHorizontal = 0,
                },

                OverrideWidth = new(e=>e.Parent.Width ),
                Height = new(e=>Js.Math.Min( 0.9 * e.Root.Height, e.Parent.Width / ((Js.Video)e.Child).Aspect) ),
                OverrideX = new(e=>0),
                
            },*/

            new LinearLayout{
                Vertical = new(e=>e.Root.Width<e.Root.Height),
                Children = {

                    new Video("qWGR-eMSnX8"){
                        Width = new(e=>e.Parent.Width),
                        Play = true,
                        Mute = true,
                        PreferPlatformPlayer = false,
                        Controls = true,
                        Loop= true,
                        //Position = new(e=>(e.Parent as Js.ScrollLayout).ScrollYActual/100),
                    }.Assign(out var primaryVideo),

                    new Video("qWGR-eMSnX8"){
                        Width = new(e=>e.Parent.Width),
                        Play = new(e=>primaryVideo.Value.PlayActual),
                        Mute = true,
                        PreferPlatformPlayer = false,
                        Position = new(e=>primaryVideo.Value.PositionActual),
                        Controls = false,
                        Loop= true,
                        //Position = new(e=>(e.Parent as Js.ScrollLayout).ScrollYActual/100),
                    },
                }
            },




            
            new Block{ 
                Height = 3000,
            }


            /*{"VideoProperties",
                $"""
                {("Play", new CheckboxInline(){
                    Value = new(e=>e.Parent.Parent["Video"].As<Js.Video>().PlayActual),
                    Children = { "Play" }
                })}
                {("Mute", new CheckboxInline(){Value = new(e=>e.Parent.Parent["Video"].As<Js.Video>().MuteActual), Children = { "Mute" } })}
                {("PreferPlatformPlayer", new CheckboxInline(){Value = false, Children = { "PreferPlatformPlayer" } })}
                {("Controls", new CheckboxInline(){Value = true, Children = { "Controls" } })}
                {("Loop", new CheckboxInline(){Value = true, Children = { "Loop" } })}
                """
            },

            H4("Autoplay"),
            new Video("T4TEdzSLyi0"){ 
                Play = true,
                Mute = true,
                Loop = true,
                Controls = false,
                PreferPlatformPlayer = false,
            },

            H4("Aspect"),

            new Block(){ 
                Height = new(e=>e.Child<Js.Block>(0).Height),
                BackgroundColor = Color.Gray,
                Children = {
                    new Video("T4TEdzSLyi0"){
                        Play = true,
                        Mute = true,
                        Loop = true,
                        Controls = false,
                        PreferPlatformPlayer = false,

                        Width = new(e=>Js.Math.Min(e.Root.Height * e.Aspect * 0.8, e.Parent.Width))
                    }.CenterHorizontally(),
                    
                    new Paragraph("This video is always less then 80% of window height"){
                        TextAlignmentHorizontal = TextAlignmentHorizontal.Center,
                        BackgroundColor = Color.Black,
                        Radius = 10,
                        Width = new(e=>Js.Math.Min(e.Parent.Width*0.8, e.InternalWidth))
                    }.Center()
                    
                }
            }.FillWidth(),*/
            


        };
    }
}
