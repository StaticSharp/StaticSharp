function Flipper(element) {
    Block(element)
    element.isFlipper = true

    CreateSocket(element, "First", element)
    CreateSocket(element, "Second", element)
    element.Reactive = {

        Flipped: () => element.Width < 640,

        HorizontalSpace: 0,//TODO: Replace With Gap
        VerticalSpace: 0,

        RightToLeft: false,
        BottomToTop: false,

        InversedDirection: () => element.Flipped ? element.BottomToTop : element.RightToLeft,

        LayoutFirst: () => element.InversedDirection ? element.Second : element.First,
        LayoutSecond: () => element.InversedDirection ? element.First : element.Second,


        MarginTop: () => {
            if (element.PaddingTop != undefined)
                return undefined
            if (element.Flipped)
                return element.LayoutFirst.MarginTop
            else
                return Max(element.LayoutFirst.MarginTop, element.LayoutSecond.MarginTop)
        },

        MarginBottom: () => {
            if (element.PaddingBottom != undefined)
                return undefined
            if (element.Flipped)
                return element.LayoutSecond.MarginBottom
            else
                return Max(element.LayoutFirst.MarginBottom, element.LayoutSecond.MarginBottom)
        },

        MarginLeft: () => {
            if (element.PaddingLeft != undefined)
                return undefined
            if (!element.Flipped)
                return element.LayoutFirst.MarginLeft
            else
                return Max(element.LayoutFirst.MarginLeft, element.LayoutSecond.MarginLeft)
        },
        MarginRight: () => {
            if (element.PaddingRight != undefined)
                return undefined
            if (!element.Flipped)
                return element.LayoutSecond.MarginRight
            else
                return Max(element.LayoutFirst.MarginRight, element.LayoutSecond.MarginRight)
        },

        InternalWidth: e => {
            let first = e.LayoutFirst
            let second = e.LayoutSecond

            //let names = new LayoutPrpertiesNames(false)
            let region = LinearLayoutRegion.formContainer(e,false)
            if (e.Flipped) {
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
            if (e.Flipped) {
                region.border[0].Shift(first)
                region.border[0].Shift(second)                
            } else {
                region.border[0].ShiftMaxOf([first, second])
            }
            let result = region.GetSize()
            return result
        },

        Width: e => e.InternalWidth,
        Height: e => e.InternalHeight,
    }

    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield element.First
        yield element.Second
        yield* element.Children
    })

    new Reaction(() => {
        
        let first = element.LayoutFirst
        let second = element.LayoutSecond

        if (element.Flipped)   {
            

            //first.LayoutHeight = undefined
            //second.LayoutHeight = undefined
            //first.Layer.Height = undefined
            //second.Layer.Height = undefined


            let left1 = CalcOffset(element, first.Layer, "Left")
            let left2 = CalcOffset(element, second.Layer, "Left")

            let right1 = CalcOffset(element, first.Layer, "Right")
            let right2 = CalcOffset(element, second.Layer, "Right")

            let top = CalcOffset(element, first.Layer, "Top")
            let bottom = CalcOffset(element, second.Layer, "Bottom")

            let spaceMid = Max(
                first.Layer.MarginBottom,
                second.Layer.MarginTop,
                element.VerticalSpace
            )

            let y = top
            //first.LayoutY = y
            first.Layer.Y = y
            y += first.Layer.Height
            y += spaceMid
            //second.LayoutY = y
            second.Layer.Y = y
            y += second.Layer.Height
            y += bottom


            //first.LayoutX = left1
            first.Layer.X = left1
            //first.LayoutWidth = element.Width - left1 - right1
            first.Layer.Width = element.Width - left1 - right1
            first.Layer.Height = first.Layer.Height

            //second.LayoutX = left2
            second.Layer.X = left2
            //second.LayoutWidth = element.Width - left2 - right2
            second.Layer.Width = element.Width - left2 - right2
            second.Layer.Height = second.Layer.Height
        } else {

            let left = CalcOffset(element, first.Layer, "Left")
            let right = CalcOffset(element, second.Layer, "Right")

            let top1 = CalcOffset(element, first.Layer, "Top")
            let top2 = CalcOffset(element, second.Layer, "Top")

            let bottom1 = CalcOffset(element, first.Layer, "Bottom")
            let bottom2 = CalcOffset(element, second.Layer, "Bottom")


            let spaceMid = Max(
                first.MarginRight,
                second.MarginLeft,
                element.HorizontalSpace
            )

            //first.LayoutX = left
            //first.LayoutWidth = (element.Width - spaceMid) * 0.5 - left
            first.Layer.X = left
            first.Layer.Width = (element.Width - spaceMid) * 0.5 - left

            //second.LayoutX = (element.Width + spaceMid) * 0.5
            //second.LayoutWidth = (element.Width - spaceMid) * 0.5 - right
            second.Layer.X = (element.Width + spaceMid) * 0.5
            second.Layer.Width = (element.Width - spaceMid) * 0.5 - right

            //first.LayoutY = top1
            //second.LayoutY = top2
            first.Layer.Y = top1
            second.Layer.Y = top2

            let height = Max(
                first.Layer.Height/*InternalHeight*/ + top1 + bottom1,
                second.Layer.Height/*InternalHeight*/ + top2 + bottom2,
            )


            first.Layer.Height/*LayoutHeight*/ = element.Height - top1 - bottom1
            second.Layer.Height/*LayoutHeight*/ = element.Height - top2 - bottom2

        }
    })



}
