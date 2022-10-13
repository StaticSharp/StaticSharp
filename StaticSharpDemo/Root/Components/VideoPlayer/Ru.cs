namespace StaticSharpDemo.Root.Components.VideoPlayer {


    [Representative]
    public partial class Ru : ComponentPage {
        public override Blocks? Content => new() {

            {"video",
                new Video("T4TEdzSLyi0"){
                    Play = new (e=>e.Sibling("videoProperties").Child<Js.Checkbox>("play").ValueActual),
                    Mute = new (e=>e.Sibling("videoProperties").Child<Js.Checkbox>("Mute").ValueActual),
                    PreferPlatformPlayer = new (e=>e.Sibling("videoProperties").Child<Js.Checkbox>("preferPlatformPlayer").ValueActual),
                    Controls = new (e=>e.Sibling("videoProperties").Child<Js.Checkbox>("controls").ValueActual),
                    Loop =  new (e=>e.Sibling("videoProperties").Child<Js.Checkbox>("loop").ValueActual),
                    Volume = new (e=>e.Sibling<Js.Slider>("VolumeSlider").ValueActual)
                }
            },

            "Volume:",
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

        };
    }
}
