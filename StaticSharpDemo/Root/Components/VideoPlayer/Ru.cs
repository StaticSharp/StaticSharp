

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



        public override Blocks? Content => new() {

            new Video("T4TEdzSLyi0"){

                /*Modifiers = {
                    new Shadow{
                        Distance = new(e=>e.Parent.Width)
                    },
                    new Shadow{
                        Distance = 8
                    },
                    new Cursor(Cursor.Type.Pointer)
                },*/

                Name = "Video",
                //Width = new(e=>e.Siblings.Max(x=>x.Width)),

                //Play = new (e=>((Js.Checkbox)e.Siblings.ByName("VideoProperties").Children.ByName("VideoProperties")).ValueActual),
                /*Mute = new (e=>((Js.Checkbox)e.Parent["VideoProperties"]["Mute"]).ValueActual),
                PreferPlatformPlayer = new (e=>((Js.Checkbox)e.Parent["VideoProperties"]["PreferPlatformPlayer"]).ValueActual),
                Controls = new (e=>((Js.Checkbox)e.Parent["VideoProperties"]["Controls"]).ValueActual),
                Loop = new (e=>((Js.Checkbox)e.Parent["VideoProperties"]["Loop"]).ValueActual),
                Volume = new (e=>((Js.Slider)e.Parent["VolumeSlider"]).ValueActual),*/
            }
            ,



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
