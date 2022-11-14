


function Link(element) {
    element.Reactive = {
        HRef: undefined,
        NewTab: false
    }

    

    new Reaction(() => {
        let href = element.HRef
        if (!AbsoluteUrlRegExp.test(href)) {
            if (href != "")
                href = "/" + href
            href = window.location.href + href
        }
        element.setAttribute("href", href)
    })
    new Reaction(() => {
        element.NewTab
            ? element.setAttribute("target", "_blank")
            : element.setAttribute("target", "_self") //element.removeAttribute("target")
        
    })

}