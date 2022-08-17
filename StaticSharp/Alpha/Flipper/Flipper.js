function Flipper(element) {
    Block(element)
    element.isFlipper = true


    element.Reactive = {
        FlipWidth:  640,
        HorizontalSpace: 12,
        VerticalSpace: 12,
        First: () => element.FirstChild,
        SecondChild: () => element.FirstChild.NextSibling
    }



    new Reaction(() => {

        if (element.Width > element.FlipWidth) {
            let left = CalcOffset(element, element.FirstChild, "Left")
            let right = CalcOffset(element, element.SecondChild, "Right")

            let top1 = CalcOffset(element, element.FirstChild, "Top")
            let top2 = CalcOffset(element, element.SecondChild, "Top")

            let bottom1 = CalcOffset(element, element.FirstChild, "Bottom")
            let bottom2 = CalcOffset(element, element.SecondChild, "Bottom")

            //let spaceLeft = Max(element.PaddingLeft, element.FirstChild.MarginLeft,0)
            //let spaceRight = Max(element.PaddingRight, element.SecondChild.MarginRight,0)
            let spaceMid = Max(
                element.FirstChild.MarginRight,
                element.SecondChild.MarginLeft,
                element.HorizontalSpace
            )

            element.FirstChild.LayoutX = left
            element.FirstChild.LayoutWidth = (element.Width - spaceMid) * 0.5 - left

            element.SecondChild.LayoutX = (element.Width + spaceMid) * 0.5 
            element.SecondChild.LayoutWidth = (element.Width - spaceMid) * 0.5 - right

            element.FirstChild.LayoutY = top1
            element.SecondChild.LayoutY = top2

            let height = Max(
                element.FirstChild.InternalHeight + top1 + bottom1,
                element.SecondChild.InternalHeight + top2 + bottom2,
            )

            element.InternalHeight = height
            element.FirstChild.LayoutHeight = height - top1 - bottom1
            element.SecondChild.LayoutHeight = height - top2 - bottom2

        } else {
            let orderedChildren = [element.FirstChild, element.SecondChild]
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


        /*element.FirstChild.LayoutWidth = element.Width * 0.5

        element.SecondChild.LayoutX = element.Width * 0.5
        element.SecondChild.LayoutWidth = element.Width * 0.5*/
    })

    new Reaction(() => {
        
    })

    /*element.Reactive = {
        Before: 0,
        Between: 1,
        After: 0
    }*/


    WidthToStyle(element)
    HeightToStyle(element)
}
