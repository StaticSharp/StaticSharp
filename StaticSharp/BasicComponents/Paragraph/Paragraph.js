function Paragraph(element) {

    Block(element)


    element.Reactive = {
        Selectable: true,
        InternalWidth: () => Sum(element.MaxContentWidth,element.PaddingLeft, element.PaddingRight),

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

        //content.style.position = "initial"
        content.style.fontSize = testFontSize + "px";
        content.style.width = "min-content"
        var minWidthRect = content.getBoundingClientRect()
        content.style.width = "max-content"
        var maxWidthRect = content.getBoundingClientRect()

        content.style.fontSize = ""
        content.style.width = ""


        element.MaxContentWidth = () =>  element.HierarchyFontSize / testFontSize * maxWidthRect.width
        element.MinContentWidth = () =>  element.HierarchyFontSize / testFontSize * minWidthRect.width
        element.MaxContentHeight = () => element.HierarchyFontSize / testFontSize * minWidthRect.height
        element.MinContentHeight = () => element.HierarchyFontSize / testFontSize * maxWidthRect.height

    })

    new Reaction(() => {
        //console.log("element.HierarchyFontSize", element.HierarchyFontSize, element)
        //console.log("element.Modifier", element, element.Modifier)
        //console.log("element.Modifier.HierarchyFontSize", element, element.Modifier.HierarchyFontSize)

        let content = element.children[0]

        content.style.transformOrigin = ""
        content.style.transform = ""
        content.style.width = ""
        //console.log("element.Width", element.Parent, element)
        element.style.width = element.Width + "px"

        let minContentWidthWithPaddings = Sum(element.MinContentWidth, element.PaddingLeft, element.PaddingRight)

        if (minContentWidthWithPaddings > element.Width) {
            //element.title = "element.MinContentWidth > element.Width"

            content.style.width = "min-content"
            content.style.transformOrigin = "top left"

            let scale = Sum(element.Width, -element.PaddingLeft, -element.PaddingRight) / element.MinContentWidth
            content.style.transform = `scale(${scale}, ${scale})`
            element.InternalHeight = Sum(element.MaxContentHeight * scale, element.PaddingTop, element.PaddingBottom) 
            return
        }

        let maxContentWidthWithPaddings = Sum(element.MaxContentWidth, element.PaddingLeft, element.PaddingRight)
        if (Math.abs(element.Width - maxContentWidthWithPaddings) < 0.001) {
            //element.title = "element.Width == element.MaxContentWidth"

            element.InternalHeight = Sum(element.MinContentHeight, element.PaddingTop, element.PaddingBottom) 
            content.style.width = "max-content"

            return
        }
        
        //content.style.width = Sum(element.style.width, -element.PaddingLeft, -element.PaddingRight) 
        //console.log("element.style.width", element.style.width + 1000, element)


        var rect = content.getBoundingClientRect()
        element.InternalHeight = Sum(rect.height, element.PaddingTop, element.PaddingBottom)

    })

    HeightToStyle(element)
}
