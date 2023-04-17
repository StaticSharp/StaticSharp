function Cursor(element) {
    Modifier.call(this, element)
    this.isCursor = true

    let modifier = this


    modifier.Reactive = {
        Variant: undefined,
    }

    new Reaction(() => {
        var variant = modifier.Variant
        if ((variant === undefined) || (variant === ""))
            element.style.cursor = ""
        else
            element.style.cursor = CamelToKebab(variant);
    })
}