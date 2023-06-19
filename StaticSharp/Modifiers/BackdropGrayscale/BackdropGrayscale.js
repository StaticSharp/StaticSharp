StaticSharpClass("StaticSharp.BackdropGrayscale", (modifier, element) => {
    StaticSharp.AbstractBackdropFilter(modifier, element)

    modifier.Reactive = {
        Amount: 1
    }

    modifier.getFilter = function () {
        return `grayscale(${modifier.Amount})`
    }
})