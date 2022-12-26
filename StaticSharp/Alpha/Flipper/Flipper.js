function Flipper(element) {
    Block(element)
    element.isFlipper = true


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

    }



    new Reaction(() => {
        let first = element.LayoutFirst
        let second = element.LayoutSecond

        if (element.Flipped)   {
            

            first.LayoutHeight = undefined
            second.LayoutHeight = undefined


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
            first.LayoutY = y
            y += first.Height
            y += spaceMid
            second.LayoutY = y
            y += second.Height
            y += bottom
            element.InternalHeight = y


            first.LayoutX = left1
            first.LayoutWidth = element.Width - left1 - right1
            second.LayoutX = left2
            second.LayoutWidth = element.Width - left2 - right2


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

            first.LayoutX = left
            first.LayoutWidth = (element.Width - spaceMid) * 0.5 - left

            second.LayoutX = (element.Width + spaceMid) * 0.5 
            second.LayoutWidth = (element.Width - spaceMid) * 0.5 - right

            first.LayoutY = top1
            second.LayoutY = top2

            let height = Max(
                first.InternalHeight + top1 + bottom1,
                second.InternalHeight + top2 + bottom2,
            )

            element.InternalHeight = height

            first.LayoutHeight = element.Height - top1 - bottom1
            second.LayoutHeight = element.Height - top2 - bottom2

        }
    })



}
