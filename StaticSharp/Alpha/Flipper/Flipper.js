function Flipper(element) {
    Block(element)
    element.isFlipper = true


    element.Reactive = {
        FlipWidth:  640,
        HorizontalSpace: 12,
        VerticalSpace: 12,
        //First: () => element.First,
        //Second: () => element.First.NextSibling
    }



    new Reaction(() => {

        if (element.Width > element.FlipWidth) {
            let left = CalcOffset(element, element.First, "Left")
            let right = CalcOffset(element, element.Second, "Right")

            let top1 = CalcOffset(element, element.First, "Top")
            let top2 = CalcOffset(element, element.Second, "Top")

            let bottom1 = CalcOffset(element, element.First, "Bottom")
            let bottom2 = CalcOffset(element, element.Second, "Bottom")

            //let spaceLeft = Max(element.PaddingLeft, element.First.MarginLeft,0)
            //let spaceRight = Max(element.PaddingRight, element.Second.MarginRight,0)
            let spaceMid = Max(
                element.First.MarginRight,
                element.Second.MarginLeft,
                element.HorizontalSpace
            )

            element.First.LayoutX = left
            element.First.LayoutWidth = (element.Width - spaceMid) * 0.5 - left

            element.Second.LayoutX = (element.Width + spaceMid) * 0.5 
            element.Second.LayoutWidth = (element.Width - spaceMid) * 0.5 - right

            element.First.LayoutY = top1
            element.Second.LayoutY = top2

            let height = Max(
                element.First.InternalHeight + top1 + bottom1,
                element.Second.InternalHeight + top2 + bottom2,
            )

            element.InternalHeight = height

            element.First.LayoutHeight = element.Height - top1 - bottom1
            element.Second.LayoutHeight = element.Height - top2 - bottom2

        } else {

            element.First.LayoutHeight = undefined
            element.Second.LayoutHeight = undefined

            let orderedChildren = [element.First, element.Second]
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
        }
    })



}
