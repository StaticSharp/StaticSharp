StaticSharpClass("StaticSharp.BackdropBlur", (modifier, element) => {
    StaticSharp.AbstractBackdropFilter(modifier, element)

    modifier.Reactive = {
        Radius: 5
    }

    modifier.getFilter = function () {
        return `blur(${modifier.Radius}px)`
    }  
})