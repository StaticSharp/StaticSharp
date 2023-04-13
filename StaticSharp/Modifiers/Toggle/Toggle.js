function Toggle(element) {
    Modifier.call(this, element)
    this.isToggle = true

    let modifier = this


    modifier.Reactive = {
        Value: false,
    }

    element.addEventListener("click", () => {modifier.Value = !modifier.Value}, false)
}