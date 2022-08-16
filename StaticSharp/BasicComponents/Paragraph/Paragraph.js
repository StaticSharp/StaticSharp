function Paragraph(element) {

    Block(element)


    element.Reactive = {

        InternalWidth: () => element.MaxContentWidth,

        InternalHeight: undefined,

        MaxContentWidth: undefined,
        MinContentWidth: undefined,

        MaxContentHeight: undefined,
        MinContentHeight: undefined,


    }

    element.MarginLeft = 10
    element.MarginRight = 10

    element.MarginTop = 8
    element.MarginBottom = 8


    new Reaction(() => {

        const testFontSize = 128;

        let content = element.children[0]
        content.style.fontSize = testFontSize + "px";
        content.style.width = "min-content"
        var minWidthRect = content.getBoundingClientRect()
        content.style.width = "max-content"
        var maxWidthRect = content.getBoundingClientRect()

        content.style.fontSize = ""
        content.style.width = ""


        element.MaxContentWidth = () => element.Modifier.HierarchyFontSize / testFontSize * maxWidthRect.width
        element.MinContentWidth = () => element.Modifier.HierarchyFontSize / testFontSize * minWidthRect.width
        element.MaxContentHeight = () => element.Modifier.HierarchyFontSize / testFontSize * minWidthRect.height
        element.MinContentHeight = () => element.Modifier.HierarchyFontSize / testFontSize * maxWidthRect.height

    })

    new Reaction(() => {
        //console.log("element.Modifier", element, element.Modifier)
        //console.log("element.Modifier.HierarchyFontSize", element, element.Modifier.HierarchyFontSize)

        let content = element.children[0]

        content.style.transformOrigin = ""
        content.style.transform = ""
        content.style.width = ""
        //console.log("element.Width", element.Parent, element)
        element.style.width = element.Width + "px"

        if (element.MinContentWidth > element.Width) {
            //element.title = "element.MinContentWidth > element.Width"

            content.style.width = "min-content"
            content.style.transformOrigin = "top left"

            let scale = element.Width / element.MinContentWidth
            content.style.transform = `scale(${scale}, ${scale})`
            element.InternalHeight = element.MaxContentHeight * scale
            return
        }
        if (Math.abs(element.Width - element.MaxContentWidth) < 0.001) {
            //element.title = "element.Width == element.MaxContentWidth"

            element.InternalHeight = element.MinContentHeight
            content.style.width = "max-content"

            return
        }

        content.style.width = element.style.width

        var rect = content.getBoundingClientRect()
        element.InternalHeight = rect.height

    })

    HeightToStyle(element)
}
