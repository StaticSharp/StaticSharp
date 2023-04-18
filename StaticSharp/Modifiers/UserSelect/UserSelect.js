


function UserSelect(element) {
    Modifier.call(this,element)
    this.isUserSelect = true

    let modifier = this



    modifier.Reactive = {
        Option: "None"
    }


    new Reaction(() => {
        var option = modifier.Option
        if ((option === undefined) || (option === ""))
            element.style.userSelect = ""
        else
            element.style.userSelect = CamelToKebab(option);
    })
}