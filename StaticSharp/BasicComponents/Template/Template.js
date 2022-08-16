
function Template(element) {

    Block(element)

    element.isTemplate = true
    element.Reactive = {
        ContentWidth: undefined,
        ContentHeight: undefined,
        Width: () => element.LayoutWidth || element.ContentWidth || 0,
        Height: () => element.LayoutHeight || element.ContentHeight || 0,

    }



    new Reaction(() => {
        element.innerHTML = element.Html;
        var rect = element.firstChild.getBoundingClientRect()

        element.ContentWidth = rect.width;
        element.ContentHeight = rect.height;
    })

    WidthToStyle(element)
    HeightToStyle(element)
}
