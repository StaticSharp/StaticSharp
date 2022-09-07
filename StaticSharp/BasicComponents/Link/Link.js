function Link(element) {
    BaseModifier(element)

    element.Reactive = {
        HRef: undefined,
        NewTab: false
    }

    new Reaction(() => {
        element.setAttribute("href", element.HRef)
    })
    new Reaction(() => {
        element.NewTab
            ? element.setAttribute("target", "_blank")
            : element.removeAttribute("target")
        
    })

}