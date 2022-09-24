function PageSideMenus(element) {
    Page(element)

    element.Reactive = {
        ContentWidth: 960,
        BarsCollapsed: () =>
            element.WindowWidth < Sum(
                element.ContentWidth,
                element.LeftSideBar ? element.LeftSideBar.Width : 0,
                element.RightSideBar ? element.RightSideBar.Width : 0),

        Content: () => element.Child("Content"),
        LeftSideBar: () => element.Child("LeftSideBar"),
        RightSideBar: () => element.Child("RightSideBar"),
        Footer: undefined,
    }




    new Reaction(() => {

        let LeftBarSize = 0
        let RightBarSize = Max(element.RightSideBar?.Width, 0)



        if (element.LeftSideBar) {
            if (element.BarsCollapsed) {
                element.LeftSideBar.style.visibility = "hidden"
            } else {
                element.LeftSideBar.style.visibility = "visible"
                LeftBarSize = Max(element.LeftSideBar.Width, 0)
            }

            element.LeftSideBar.style.position = "fixed"
            element.LeftSideBar.Height = element.WindowHeight
        }



        let width = element.WindowWidth - LeftBarSize - RightBarSize
        let innerWidth = Math.min(width, element.ContentWidth)
        let contentSpace = (width - innerWidth) * 0.5

        if (element.Content) {


            element.Content.Width = width - 2 * contentSpace

            element.Content.MarginLeft = contentSpace

            element.Content.MarginRight = contentSpace

            element.Content.LayoutX = LeftBarSize + contentSpace
            element.Content.LayoutHeight = Math.max(element.Content.InternalHeight, element.WindowHeight)
        }

    })

}