function BorderRadius(element) {
    Modifier.call(this,element)
    this.isBorderRadius = true

    let modifier = this

    modifier.Reactive = {
        Radius: undefined,
        RadiusTopLeft: e => e.Radius,
        RadiusTopRight: e => e.Radius,
        RadiusBottomLeft: e => e.Radius,
        RadiusBottomRight: e => e.Radius,
    }


    new Reaction(() => {
        console.log(modifier)
        element.style.borderTopLeftRadius = ToCssSize(modifier.RadiusTopLeft)
        element.style.borderTopRightRadius = ToCssSize(modifier.RadiusTopRight)
        element.style.borderBottomLeftRadius = ToCssSize(modifier.RadiusBottomLeft)
        element.style.borderBottomRightRadius = ToCssSize(modifier.RadiusBottomRight)
    })
}