function Hover(element) {
    Modifier.call(this, element)
    this.isHover = true

    let modifier = this


    modifier.Reactive = {
        Value: false,
    }

    new Reaction(() => {
        if (!element.Exists) {
            modifier.Value = false
        }
    })

    element.addEventListener("mouseenter", () => modifier.Value = true, false)
    element.addEventListener("mouseleave", () => modifier.Value = false, false)
}