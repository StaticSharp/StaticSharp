function Button(element) {
    Modifier.call(this, element)
    this.isButton = true

    let modifier = this

    modifier.Reactive = {
        EventPropagation: false
    }

    element.addEventListener("click", () => {
        modifier.Script(modifier)
        event.stopPropagation()
    }, false)
}