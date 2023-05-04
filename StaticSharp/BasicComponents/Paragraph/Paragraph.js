function Paragraph(element) {

    Block(element)

    //CreateSocket(element, "FirstInline", element)

    element.Reactive = {

        NoWrap: false,

        InternalWidth: () => Num.Sum(element.MaxContentWidth, element.PaddingLeft, element.PaddingRight),

        InternalHeight: undefined,

        Width: e => e.InternalWidth,
        Height: e => e.InternalHeight,


        MaxContentWidth:  e => e.HierarchyFontSize / Paragraph.testFontSize * e.inlineContainer.maxWidth,
        MinContentWidth:  e => e.HierarchyFontSize / Paragraph.testFontSize * e.inlineContainer.minWidth,
        MaxContentHeight: e => e.HierarchyFontSize / Paragraph.testFontSize * e.inlineContainer.maxHeight,
        MinContentHeight: e => e.HierarchyFontSize / Paragraph.testFontSize * e.inlineContainer.minHeight,


        PaddingLeft: () => (element.BackgroundColor != undefined) ? 10 : undefined,
        PaddingRight: () => (element.BackgroundColor != undefined) ? 10 : undefined,
        PaddingTop: () => (element.BackgroundColor != undefined) ? 8 : undefined,
        PaddingBottom: () => (element.BackgroundColor != undefined) ? 8 : undefined,

        MarginLeft: () => (element.BackgroundColor != undefined) ? 0 : 10,
        MarginRight: () => (element.BackgroundColor != undefined) ? 0 : 10,
        MarginTop: () => (element.BackgroundColor != undefined) ? 0 : 8,
        MarginBottom: () => (element.BackgroundColor != undefined) ? 0 : 8,
    }





    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield element.inlineContainer
        yield* element.UnmanagedChildren
    })



    new Reaction(() => {
        let content = element.inlineContainer
        /*
        Left,
        Center,
        Right,
        Justify,
        JustifyIncludingLastLine
         */
        if (element.TextAlignmentHorizontal == "JustifyIncludingLastLine") {
            content.style.textAlign = "justify"
            content.style.textAlignLast = "justify"
        } else {
            content.style.textAlignLast = ""
            if (element.TextAlignmentHorizontal === undefined) {
                content.style.textAlign = ""
            } else {
                content.style.textAlign = element.TextAlignmentHorizontal.toLowerCase()
            }
        }
    })


    new Reaction(() => {
        
        if (element.Width == undefined) {
            return
        }

        element.style.width = ToCssSize(element.Width)
        let content = element.inlineContainer
        content.style.transformOrigin = ""
        content.style.transform = ""
        content.style.width = ""
        content.style.display = ""
        content.style.left = ToCssSize(element.PaddingLeft)
        content.style.top = ToCssSize(element.PaddingTop)

        let noWrap = element.NoWrap
        let minContentWidth = noWrap ? element.MaxContentWidth : element.MinContentWidth

        let minContentWidthWithPaddings = Num.Sum(minContentWidth, element.PaddingLeft, element.PaddingRight)

        if (element.Width < minContentWidthWithPaddings) {

            let scale = Num.Sum(element.Width, -element.PaddingLeft, -element.PaddingRight) / minContentWidth

            if (scale > 0) {
                content.style.width = noWrap ? "max-content" : "min-content"
                content.style.transformOrigin = "top left"
                content.style.transform = `scale(${scale}, ${scale})`
                let contentHeight = noWrap ? element.MinContentHeight : element.MaxContentHeight

                element.InternalHeight = Num.Sum(contentHeight * scale, element.PaddingTop, element.PaddingBottom)
            } else {
                content.style.display = "none"
                element.InternalHeight = Num.Sum(element.PaddingTop, element.PaddingBottom)
            }
            return
        }

        let maxContentWidthWithPaddings = Num.Sum(element.MaxContentWidth, element.PaddingLeft, element.PaddingRight)
        let extraPixels = element.Width - maxContentWidthWithPaddings

        if (extraPixels > -0.001) {
            
            let internalHeight = Num.Sum(element.MinContentHeight, element.PaddingTop, element.PaddingBottom)
            element.InternalHeight = internalHeight
            //element.title = `InternalHeight(${internalHeight}) = ${element.MinContentHeight} + ${element.PaddingTop} + ${element.PaddingBottom}`
            if (extraPixels > 0.001) {
                content.style.width = ToCssSize(Num.Sum(element.Width, -element.PaddingLeft, -element.PaddingRight))
            } else {
                content.style.width = "max-content"
            }
            return
        }

        content.style.width = ToCssSize(Num.Sum(element.Width, -element.PaddingLeft, -element.PaddingRight))
        var rect = content.getBoundingClientRect()
        element.InternalHeight = Num.Sum(rect.height, element.PaddingTop, element.PaddingBottom)

    })

    HeightToStyle(element)
}

Paragraph.testFontSize = 18