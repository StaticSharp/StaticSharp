function CodeBlock(element) {

    ParagraphBase(element)

    /*element.Reactive = {
        ContentWidth: undefined,
        ContentHeight: undefined,
    }

    let rect;
    new Reaction(() => {
        let content = element.children[0]
        rect = content.getBoundingClientRect()
        element.ContentWidth = rect.width
        element.ContentHeight = rect.height
    })

    new Reaction(() => {
        element.InternalWidth = Sum(element.ContentWidth, element.PaddingLeft, element.PaddingRight)
        element.InternalHeight = Sum(element.ContentHeight, element.PaddingTop, element.PaddingBottom)
    })

    new Reaction(() => {
        let content = element.children[0]
        content.style.left = ToCssSize(element.PaddingLeft)
        content.style.top = ToCssSize(element.PaddingTop)
    })

    element.Events.Click = () => {
        if (event.detail === 4) {
            window.getSelection().selectAllChildren(element);
            //alert('4 click!');
        }
    }*/



}