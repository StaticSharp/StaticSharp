function Flipper(element) {
    Block(element)
    element.isFlipper = true


    element.Reactive = {
        FlipWidth: ()=> 380,
        First: () => element.FirstChild,
        SecondChild: () => element.FirstChild.NextSibling
    }



    new Reaction(() => {
        element.FirstChild.LayoutWidth = element.Width * 0.5

        element.SecondChild.LayoutX = element.Width * 0.5
        element.SecondChild.LayoutWidth = element.Width * 0.5
    })

    new Reaction(() => {
        element.InternalHeight = Max(
            element.FirstChild.Height,
            element.SecondChild.Height,
        )
    })

    /*element.Reactive = {
        Before: 0,
        Between: 1,
        After: 0
    }*/


    WidthToStyle(element)
    HeightToStyle(element)
}
