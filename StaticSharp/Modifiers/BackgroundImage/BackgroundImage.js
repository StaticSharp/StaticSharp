StaticSharpClass("StaticSharp.BackgroundImage", (modifier, element) => {
    StaticSharp.AbstractBackground(modifier, element)

    modifier.Reactive = {
        Aspect: e => e.NativeWidth / e.NativeHeight,
        Width: e => Num.First(e.Height * e.Aspect , e.NativeWidth),
        Height: e => Num.First(e.Width / e.Aspect, e.NativeHeight),
    }  


})