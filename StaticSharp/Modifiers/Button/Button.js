function Button(element) {
    Modifier.call(this, element)
    this.isButton = true

    let modifier = this

    modifier.Reactive = {
    }

    element.addEventListener("click", () => {
        modifier.Script(modifier)
    }, false)
}