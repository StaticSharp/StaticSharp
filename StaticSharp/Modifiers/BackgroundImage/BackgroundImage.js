
function BackgroundImage(element) {
    AbstractBackground.call(this, element)
    this.isBackgroundImage = true
    let modifier = this

    modifier.Reactive = {
        //ImageUrl: undefined,
        //NativeWidth: undefined,
        //NativeHeight: undefined,
        Aspect: e => e.NativeWidth / e.NativeHeight,
        Width: e => Num.First(e.Height * e.Aspect , e.NativeWidth),
        Height: e => Num.First(e.Width / e.Aspect, e.NativeHeight),
    }

    /*let getBackgroundBase = modifier.getBackground
    modifier.getBackground = function () {
        let result = getBackgroundBase()
        result.backgroundImage = `url(${modifier.ImageUrl})`
        return result
    }*/
    


}