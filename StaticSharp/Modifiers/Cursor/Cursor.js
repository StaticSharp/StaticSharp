function Cursor(element) {
    Modifier.call(this, element)
    this.isCursor = true

    let modifier = this


    modifier.Reactive = {
        Option: "Pointer",
    }

    new Reaction(() => {
        var option = modifier.Option
        if ((option === undefined) || (option === ""))
            element.style.cursor = ""
        else
            element.style.cursor = CamelToKebab(option);
    })
}