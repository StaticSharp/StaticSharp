
function AbstractBackground(element) {
    Modifier.call(this, element)
    this.isAbstractBackground = true
    let modifier = this

    modifier.Reactive = {
        RawImage: undefined,

        X: 0,
        Y: 0,
        Width: undefined,
        Height: undefined,

        Repeat: "repeat",
        BlendMode: "normal"
    }


    modifier.getBackground = function () {
        let width = modifier.Width
        let widthCss = (width === undefined) ? "auto" : width + "px"
        let height = modifier.Height
        let heightCss = (height === undefined) ? "auto" : height + "px"

        return {
            backgroundImage: modifier.RawImage,
            backgroundPosition: `${modifier.X}px ${modifier.Y}px`,
            backgroundSize: widthCss + " " + heightCss,
            backgroundRepeat: CamelToKebab(modifier.Repeat),
            backgroundBlendMode: CamelToKebab(modifier.BlendMode)
        }
    }
    

    new Reaction(() => {

        var coModifiers = element.Modifiers.filter(x => x.isAbstractBackground)
        if (coModifiers[0] == modifier) {
            let data = coModifiers.map(x => x.getBackground()).reverse()
            for (let i of Object.keys(data[0]))
                element.style[i] = data.map(x => x[i]).join(',')
        }
    })

}