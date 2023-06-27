StaticSharpClass("StaticSharp.Paragraph", (element) => {
    StaticSharp.Block(element)

    element.Reactive = {

        NoWrap: false,

        InternalWidth: () => Num.Sum(element.MaxContentWidth, element.PaddingLeft, element.PaddingRight),

        InternalHeight: () => Measure(),

        Width: e => e.InternalWidth,
        Height: e => e.InternalHeight,


        MaxContentWidth: e => e.HierarchyFontSize / StaticSharp.Paragraph.testFontSize * e.inlineContainer.maxWidth,
        MinContentWidth: e => e.HierarchyFontSize / StaticSharp.Paragraph.testFontSize * e.inlineContainer.minWidth,
        MaxContentHeight: e => e.HierarchyFontSize / StaticSharp.Paragraph.testFontSize * e.inlineContainer.maxHeight,
        MinContentHeight: e => e.HierarchyFontSize / StaticSharp.Paragraph.testFontSize * e.inlineContainer.minHeight,


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



    function Layout() {
        return LayoutOrMeasure(true)
    }
    function Measure() {
        return LayoutOrMeasure(false)
    }
    function LayoutOrMeasure(layout) {

        if (element.Width == undefined) {
            return 0
        }
        let content = element.inlineContainer

        if (layout) {
            element.style.width = ToCssSize(element.Width)
            content.style.fontSize = ToCssSize(element.HierarchyFontSize)
            content.style.transformOrigin = ""
            content.style.transform = ""
            content.style.width = ""
            content.style.display = ""
            content.style.left = ToCssSize(element.PaddingLeft)
            content.style.top = ToCssSize(element.PaddingTop)
        }

        let noWrap = element.NoWrap
        let minContentWidth = noWrap ? element.MaxContentWidth : element.MinContentWidth
        let minContentWidthWithPaddings = Num.Sum(minContentWidth, element.PaddingLeft, element.PaddingRight)

        if (element.Width < minContentWidthWithPaddings) {

            let scale = Num.Sum(element.Width, -element.PaddingLeft, -element.PaddingRight) / minContentWidth

            if (scale > 0) {
                if (layout) {
                    content.style.width = noWrap ? "max-content" : "min-content"
                    content.style.transformOrigin = "top left"
                    content.style.transform = `scale(${scale}, ${scale})`
                } else {
                    let contentHeight = noWrap ? element.MinContentHeight : element.MaxContentHeight
                    return Num.Sum(contentHeight * scale, element.PaddingTop, element.PaddingBottom)
                }                
            } else {
                if (layout) {
                    content.style.display = "none"
                } else {
                    return Num.Sum(element.PaddingTop, element.PaddingBottom)
                }                
            }
            return
        }

        let maxContentWidthWithPaddings = Num.Sum(element.MaxContentWidth, element.PaddingLeft, element.PaddingRight)
        let extraPixels = element.Width - maxContentWidthWithPaddings

        if (extraPixels > -0.001) {
            if (layout) {
                if (extraPixels > 0.001) {
                    content.style.width = ToCssSize(Num.Sum(element.Width, -element.PaddingLeft, -element.PaddingRight))
                } else {
                    content.style.width = "max-content"
                }
            } else {
                return Num.Sum(element.MinContentHeight, element.PaddingTop, element.PaddingBottom)
            }
            return
        }

        content.style.width = ToCssSize(Num.Sum(element.Width, -element.PaddingLeft, -element.PaddingRight))

        if (!layout) {
            let rect = content.getBoundingClientRect()
            return Num.Sum(rect.height, element.PaddingTop, element.PaddingBottom)
        }


    }

    new Reaction(() => {
        Layout()
    })

    HeightToStyle(element)

})

StaticSharp.Paragraph.testFontSize = 128