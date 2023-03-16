
function Template(element) {

    Block(element)

    element.isTemplate = true
    element.Reactive = {
        ContentWidth: undefined,
        ContentHeight: undefined,
        Width: () => /*element.LayoutWidth || element.ContentWidth*/ element.Width || 0,
        Height: () => /*element.LayoutHeight || element.ContentHeight*/  element.Width || 0,

    }



    new Reaction(() => {
        element.innerHTML = element.Html;
        var rect = element.firstChild.getBoundingClientRect()

        element.ContentWidth = rect.width;
        element.ContentHeight = rect.height;
    })

}
