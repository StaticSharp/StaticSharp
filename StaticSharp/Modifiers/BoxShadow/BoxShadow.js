StaticSharpClass("StaticSharp.BoxShadow", (modifier, element) => {
    StaticSharp.AbstractBoxShadow(modifier, element)
    

    modifier.Reactive = {
        Inset: false,
        X : 0,
        Y: 0,
        Spread: 0,
        Blur: 0,
        Color: () => element.HierarchyForegroundColor,     
    }

    modifier.getBoxShadow = function () {
        let prefix = ""
        if (modifier.Inset) {
            prefix = "inset "
        }
        return prefix + `${modifier.X}px ${modifier.Y}px ${modifier.Blur}px ${modifier.Spread}px ${modifier.Color.toString()}`
    }
})