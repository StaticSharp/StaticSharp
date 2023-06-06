StaticSharpClass("StaticSharp.BorderRadius", (modifier, element) => {
    StaticSharp.Modifier(modifier, element)

    modifier.Reactive = {
        Radius: undefined,
        RadiusTopLeft: e => e.Radius,
        RadiusTopRight: e => e.Radius,
        RadiusBottomLeft: e => e.Radius,
        RadiusBottomRight: e => e.Radius,
    }


    new Reaction(() => {        
        element.style.borderTopLeftRadius = ToCssSize(modifier.RadiusTopLeft)
        element.style.borderTopRightRadius = ToCssSize(modifier.RadiusTopRight)
        element.style.borderBottomLeftRadius = ToCssSize(modifier.RadiusBottomLeft)
        element.style.borderBottomRightRadius = ToCssSize(modifier.RadiusBottomRight)
    })
})