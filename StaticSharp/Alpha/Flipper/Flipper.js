function Flipper(element) {
    Block(element)
    element.isFlipper = true


    element.Reactive = {
        Flipped: () => element.Width < 640,

        HorizontalSpace: 12,
        VerticalSpace: 12,

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

        if (element.Flipped)   {

            element.LayoutFirst.LayoutHeight = undefined
            element.LayoutSecond.LayoutHeight = undefined

            let orderedChildren = [element.LayoutFirst, element.LayoutSecond]
            let margin = First(element.PaddingTop, 0)

            let y = 0
            for (let i of orderedChildren) {
                i.LayoutHeight = undefined
            }

            for (let i of orderedChildren) {
                let spaceLeft = Max(element.PaddingLeft, i.MarginLeft, 0)
                let spaceRight = Max(element.PaddingRight, i.MarginRight, 0)
                let spaceTop = Max(margin, i.MarginTop, 0)

                i.LayoutX = spaceLeft
                i.LayoutWidth = element.Width - spaceLeft - spaceRight
                i.LayoutY = y + spaceTop

                y += Sum(spaceTop, i.InternalHeight)
                margin = Max(i.MarginButtom, element.VerticalSpace, 0)

            }

            element.InternalHeight = y + margin

        } else {

            let left = CalcOffset(element, element.LayoutFirst, "Left")
            let right = CalcOffset(element, element.LayoutSecond, "Right")

            let top1 = CalcOffset(element, element.LayoutFirst, "Top")
            let top2 = CalcOffset(element, element.LayoutSecond, "Top")

            let bottom1 = CalcOffset(element, element.LayoutFirst, "Bottom")
            let bottom2 = CalcOffset(element, element.LayoutSecond, "Bottom")

            //let spaceLeft = Max(element.PaddingLeft, element.First.MarginLeft,0)
            //let spaceRight = Max(element.PaddingRight, element.Second.MarginRight,0)
            let spaceMid = Max(
                element.LayoutFirst.MarginRight,
                element.LayoutSecond.MarginLeft,
                element.HorizontalSpace
            )

            element.LayoutFirst.LayoutX = left
            element.LayoutFirst.LayoutWidth = (element.Width - spaceMid) * 0.5 - left

            element.LayoutSecond.LayoutX = (element.Width + spaceMid) * 0.5 
            element.LayoutSecond.LayoutWidth = (element.Width - spaceMid) * 0.5 - right

            element.LayoutFirst.LayoutY = top1
            element.LayoutSecond.LayoutY = top2

            let height = Max(
                element.LayoutFirst.InternalHeight + top1 + bottom1,
                element.LayoutSecond.InternalHeight + top2 + bottom2,
            )

            element.InternalHeight = height

            element.LayoutFirst.LayoutHeight = element.Height - top1 - bottom1
            element.LayoutSecond.LayoutHeight = element.Height - top2 - bottom2

        }
    })



}
