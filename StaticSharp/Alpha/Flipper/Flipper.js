function Flipper(element) {
    Block(element)
    element.isFlipper = true

    CreateSocket(element, "First", element)
    CreateSocket(element, "Second", element)
    element.Reactive = {

        Flipped: () => element.Width < 640,

        HorizontalSpace: 0,
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

        //InternalWidth: undefined,
        InternalHeight: undefined,

        //Width: e => e.InternalWidth,
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


            let left1 = CalcOffset(element, first, "Left")
            let left2 = CalcOffset(element, second, "Left")

            let right1 = CalcOffset(element, first, "Right")
            let right2 = CalcOffset(element, second, "Right")

            let top = CalcOffset(element, first, "Top")
            let bottom = CalcOffset(element, second, "Bottom")

            let spaceMid = Max(
                first.MarginBottom,
                second.MarginTop,
                element.VerticalSpace
            )

            let y = top
            //first.LayoutY = y
            first.Layer.Y = y
            y += first.Height
            y += spaceMid
            //second.LayoutY = y
            second.Layer.Y = y
            y += second.Height
            y += bottom
            element.InternalHeight = y


            //first.LayoutX = left1
            first.Layer.X = left1
            //first.LayoutWidth = element.Width - left1 - right1
            first.Layer.Width = element.Width - left1 - right1
            //second.LayoutX = left2
            second.Layer.X = left2
            //second.LayoutWidth = element.Width - left2 - right2
            second.Layer.Width = element.Width - left2 - right2

        } else {

            let left = CalcOffset(element, first, "Left")
            let right = CalcOffset(element, second, "Right")

            let top1 = CalcOffset(element, first, "Top")
            let top2 = CalcOffset(element, second, "Top")

            let bottom1 = CalcOffset(element, first, "Bottom")
            let bottom2 = CalcOffset(element, second, "Bottom")


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

            element.InternalHeight = height

            first.Layer.Height/*LayoutHeight*/ = element.Height - top1 - bottom1
            second.Layer.Height/*LayoutHeight*/ = element.Height - top2 - bottom2

        }
    })



}
