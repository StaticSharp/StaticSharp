StaticSharpClass("StaticSharp.Flipper", (element) => {
    StaticSharp.Block(element)

    CreateSocket(element, "First", element)
    CreateSocket(element, "Second", element)
    element.Reactive = {

        Vertical: () => element.Width < 640,
        Proportion: 0.5,
        Gap: e => e.MinContactMargin,


        MinContactMargin: e => {
            if (e.Vertical) {
                return Num.Min(e.LayoutFirst.MarginBottom, e.LayoutSecond.MarginTop) || 0
            } else {
                return Num.Min(e.LayoutFirst.MarginRight, e.LayoutSecond.MarginLeft) || 0
            }
        },
        MaxContactMargin: e => {
            if (e.Vertical) {
                return Num.Max(e.LayoutFirst.MarginBottom, e.LayoutSecond.MarginTop, 0)
            } else {
                return Num.Max(e.LayoutFirst.MarginRight, e.LayoutSecond.MarginLeft, 0)
            }
        },

        /*RightToLeft: false,
        BottomToTop: false,
        InversedDirection: () => element.Vertical ? element.BottomToTop : element.RightToLeft,*/


        Reverse: false,        
        LayoutProportion: e => e.Reverse ? 1 - e.Proportion : e.Proportion,
        LayoutFirst: e => e.Reverse ? e.Second : e.First,
        LayoutSecond: e => e.Reverse ? e.First : e.Second,


        MarginTop: () => {
            if (element.PaddingTop != undefined)
                return undefined
            if (element.Vertical)
                return element.LayoutFirst.MarginTop
            else
                return Num.Max(element.LayoutFirst.MarginTop, element.LayoutSecond.MarginTop)
        },

        MarginBottom: () => {
            if (element.PaddingBottom != undefined)
                return undefined
            if (element.Vertical)
                return element.LayoutSecond.MarginBottom
            else
                return Num.Max(element.LayoutFirst.MarginBottom, element.LayoutSecond.MarginBottom)
        },

        MarginLeft: () => {
            if (element.PaddingLeft != undefined)
                return undefined
            if (!element.Vertical)
                return element.LayoutFirst.MarginLeft
            else
                return Num.Max(element.LayoutFirst.MarginLeft, element.LayoutSecond.MarginLeft)
        },
        MarginRight: () => {
            if (element.PaddingRight != undefined)
                return undefined
            if (!element.Vertical)
                return element.LayoutSecond.MarginRight
            else
                return Num.Max(element.LayoutFirst.MarginRight, element.LayoutSecond.MarginRight)
        },

        /*InternalWidth: e => {
            let first = e.LayoutFirst
            let second = e.LayoutSecond

            //let names = new LayoutPrpertiesNames(false)
            let region = LinearLayoutRegion.formContainer(e, false)
            if (e.Vertical) {
                region.border[0].ShiftMaxOf([first, second])
            } else {
                region.border[0].Shift(first)
                region.border[0].Shift(second)
            }
            let result = region.GetSize()
            return result
        },

        InternalHeight: e => {
            let first = e.LayoutFirst
            let second = e.LayoutSecond

            //let names = new LayoutPrpertiesNames(false)
            let region = LinearLayoutRegion.formContainer(e, true)
            if (e.Vertical) {
                region.border[0].Shift(first)
                region.border[0].Shift(second)
            } else {
                region.border[0].ShiftMaxOf([first, second])
            }
            let result = region.GetSize()
            return result
        },*/

        OrderedChildren: e => [e.LayoutFirst, e.LayoutSecond],
        InternalSecondarySize: e => LayoutAlgorithms.ParallelMeasure(!e.Vertical, e, e.OrderedChildren),
        InternalPrimarySize: e => LayoutAlgorithms.SequenceMeasure(e.Vertical, e, e.OrderedChildren, e.Gap),

        InternalWidth: e => e.Vertical ? e.InternalSecondarySize : e.InternalPrimarySize,
        InternalHeight: e => !e.Vertical ? e.InternalSecondarySize : e.InternalPrimarySize,


        Width: e => e.InternalWidth,
        Height: e => e.InternalHeight,
    }

    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield element.First
        yield element.Second
        yield* element.UnmanagedChildren
    })

    new Reaction(() => {

        let first = element.LayoutFirst
        let second = element.LayoutSecond

        LayoutAlgorithms.ParallelLayout(!element.Vertical, element, [first, second], undefined)

        let gap = element.Gap

        if (element.Vertical) {
            if (Num.IsNaNOrNull(element.Height))
                return
            LayoutAlgorithms.SequenceLayout(true, element, [first, second], element.InternalHeight, undefined, 1, gap, 0)


        } else {//Horizontal
            let width = element.Width
            if (Num.IsNaNOrNull(width))
                return

            let proportion = element.LayoutProportion
            //console.log("proportion", proportion)
            
            if (!Num.IsNaNOrNull(proportion) > 0) {
                proportion = Num.Clamp(proportion,0,1)

                let gapTravel = width - gap
                let leftArea = gapTravel * proportion + gap
                //console.log(leftArea)
                let rightArea = gapTravel * (1 - proportion) + gap
                let rightAreaPosition = leftArea - gap

                let layoutFirst = new LinearLayoutRegion(false)
                layoutFirst.border[0].SetFromContainer(element)
                layoutFirst.border[1].bodyStop = gap
                layoutFirst.border[1].marginStop = 0
                let position = layoutFirst.border[0].Shift(first, 0)

                let firstMidOffset = layoutFirst.GetSize()
                let firstSize = leftArea - firstMidOffset

                first.Layer.X = position
                first.Layer.Width = firstSize

                let layoutSecond = new LinearLayoutRegion(false)
                layoutSecond.border[1].SetFromContainer(element)
                layoutSecond.border[0].bodyStop = gap
                layoutSecond.border[0].marginStop = 0
                position = layoutSecond.border[0].Shift(second, 0)

                let secondMidOffset = layoutSecond.GetSize()
                let secondSize = rightArea - secondMidOffset

                second.Layer.X = rightAreaPosition + position

                second.Layer.Width = secondSize



                return
                //second.LayoutX = (element.Width + spaceMid) * 0.5
                //second.LayoutWidth = (element.Width - spaceMid) * 0.5 - right
                //second.Layer.X = (element.Width + spaceMid) * 0.5
                //second.Layer.Width = (element.Width - spaceMid) * 0.5 - right

            } else {

                LayoutAlgorithms.SequenceLayout(false, element, [first, second], element.InternalWidth, undefined, 1, gap, 0)
                return
            }
        }
    })



})
