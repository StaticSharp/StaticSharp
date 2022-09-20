namespace StaticSharpDemo.Root.Components.VideoPlayer {


    [Representative]
    public partial class Ru : Material {
        public override Blocks? Content => new() {

            {"video",
                new Video("T4TEdzSLyi0"){
                    Play = new (e=>e.Sibling("videoProperties").Child<CheckboxJs>("play").ValueActual),
                    Mute = new (e=>e.Sibling("videoProperties").Child<CheckboxJs>("Mute").ValueActual),
                    PreferPlatformPlayer = new (e=>e.Sibling("videoProperties").Child<CheckboxJs>("preferPlatformPlayer").ValueActual),
                    Controls = new (e=>e.Sibling("videoProperties").Child<CheckboxJs>("controls").ValueActual),
                    Loop =  new (e=>e.Sibling("videoProperties").Child<CheckboxJs>("loop").ValueActual),
                    Volume = new (e=>e.Sibling<SliderJs>("VolumeSlider").ValueActual)
                }
            },

            "Volume:",
            {"VolumeSlider",
                new Slider { Value = new(e=>e.Sibling<VideoJs>("video").VolumeActual) }
            },

            {"videoProperties",
                $"""
                {new CheckboxInline(){ Value = new(e=>e.Parent.Sibling<VideoJs>("video").PlayActual), Label = { "Play" } }:#play}
                {new CheckboxInline(){ Label = { "Mute" } }:#Mute}
                {new CheckboxInline(){ Label = { "Prefer platform player" } }:#preferPlatformPlayer}
                {new CheckboxInline(){ Label = { "Controls" } }:#controls}
                {new CheckboxInline(){ Label = { "Loop" } }:#loop}
                """
            },

        };
    }
}
