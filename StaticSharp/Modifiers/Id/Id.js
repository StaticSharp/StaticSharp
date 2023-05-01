function Id(element) {
    Modifier.call(this, element)
    this.isId = true

    let modifier = this


    modifier.Reactive = {
        Value: undefined,
    }

    new Reaction(() => {
        var value = modifier.Value || ""
        element.id = value
    })
}