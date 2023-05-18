



function AspectBlock(element) {
    Block(element)

    element.Reactive = {

        NativeWidth: undefined,
        NativeHeight: undefined,
        NativeAspect: e => e.NativeWidth / e.NativeHeight,

        //Aspect: e => e.NativeAspect,
        Width: () => Num.First(element.Height * element.NativeAspect, element.NativeWidth),
        Height: () => Num.First(element.Width / element.NativeAspect, element.NativeHeight),

        Fit: "Inside",
        GravityVertical: 0,
        GravityHorizontal: 0,
    }

    


}