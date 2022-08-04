
function TemplateInitialization(element) {

    BlockInitialization(element)

    element.isTemplate = true
    element.Reactive = {
        ContentWidth: undefined,
        ContentHeight: undefined,
        Width: () => element.LayoutWidth || element.ContentWidth || 0,
        Height: () => element.LayoutHeight || element.ContentHeight || 0,

    }
}





function TemplateBefore(element) {
    BlockBefore(element)

    function htmlToElement(html) {
        var template = document.createElement('template');
        html = html.trim(); // Never return a text node of whitespace as the result
        template.innerHTML = html;
        return template.content.firstChild;
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

function TemplateAfter(element) {
    BlockAfter(element)
}