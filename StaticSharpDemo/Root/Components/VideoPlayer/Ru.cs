namespace StaticSharpDemo.Root.Components.VideoPlayer {


    [Representative]
    public partial class Ru : ComponentPage {



        public override Block? MainVisual => new Video("T4TEdzSLyi0") {
            Play = true,
            Mute = true,
            Loop = true,
            Controls = false,
            PreferPlatformPlayer = false,
        };
        public override Inlines Description => $"StaticSharp VideoPlayer component.";
        public override Blocks? Content => new() {

            {"Video",
                new Video("T4TEdzSLyi0"){
                    Play = new (e=>e.Parent["VideoProperties"]["Play"].As<Js.Checkbox>().ValueActual),
                    Mute = new (e=>e.Parent["VideoProperties"]["Mute"].As<Js.Checkbox>().ValueActual),
                    PreferPlatformPlayer = new (e=>e.Parent["VideoProperties"]["PreferPlatformPlayer"].As<Js.Checkbox>().ValueActual),
                    Controls = new (e=>e.Parent["VideoProperties"]["Controls"].As<Js.Checkbox>().ValueActual),
                    Loop = new (e=>e.Parent["VideoProperties"]["Loop"].As<Js.Checkbox>().ValueActual),
                    Volume = new (e=>e.Parent["VolumeSlider"].As<Js.Slider>().ValueActual),
                }
            },



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

                        Width = new(e=>Js.Math.Min(e.Root.Height * e.Aspect * 0.8, e.ParentBlock.Width))
                    }.CenterHorizontally(),
                    
                    new Paragraph("This video is always less then 80% of window height"){
                        TextAlignmentHorizontal = TextAlignmentHorizontal.Center,
                        BackgroundColor = Color.Black,
                        Radius = 10,
                        Width = new(e=>Js.Math.Min(e.ParentBlock.Width*0.8, e.InternalWidth))
                    }.Center()
                    
                }
            }.FillWidth(),*/
            


        };
    }
}
