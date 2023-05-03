function Button(element) {
    Modifier.call(this, element)
    this.isButton = true

    let modifier = this

    modifier.Reactive = {
        EventPropagation: false
    }

    element.addEventListener("click", () => {
        modifier.Script(modifier)
        if (!modifier.EventPropagation)
            event.stopPropagation()
    }, false)
}