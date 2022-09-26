
function SwipeDetector(element) {

    element.Reactive = {
        SwipeX: 0,
        SwipeY: 0,
        Swipe: false
    }

    let startX = 0
    let startY = 0

    element.addEventListener('touchstart', (e) => {
        var touch = e.touches[0];
        startX = touch.clientX;
        startY = touch.clientY;
        
    });

    element.addEventListener('touchmove', (e) => {
        var touch = e.touches[0];
        let d = Reaction.beginDeferred()
        element.SwipeX = touch.clientX - startX
        element.SwipeY = touch.clientY - startY
        element.Swipe = true
        d.end()
    }, false);

    element.addEventListener('touchend', (e) => {
        element.Swipe = false
    }, false);

    

}




function PageSideMenus(element) {
    Page(element)

    element.Reactive = {
        ContentWidth: 960,
        SideBarsIconsSize: 48,

        BarsCollapsed: () =>
            element.WindowWidth < Sum(
                element.ContentWidth,
                element.LeftSideBar ? element.LeftSideBar.Width : 0,
                element.RightSideBar ? element.RightSideBar.Width : 0),

        Content: () => element.Child("Content"),

        SideBarOpen: 0, //-1 left , 1 right

        LeftSideBar: () => element.Child("LeftSideBar"),

        RightSideBar: () => element.Child("RightSideBar"),

        TopBar: () => element.Child("Content").Child("TopBar"),



        Footer: undefined,
    }


    SwipeDetector(element)


    new Reaction(() => {
        if (element.TopBar) {
            if (element.LeftSideBar && element.BarsCollapsed) {
                element.TopBar.MarginLeft = element.SideBarsIconsSize
            } else {
                element.TopBar.MarginLeft = 0
            }
        }            
    })
    new Reaction(() => {
        if (element.TopBar) {
            if (element.RightSideBar && element.BarsCollapsed) {
                element.TopBar.MarginRight = element.SideBarsIconsSize
            } else {
                element.TopBar.MarginRight = 0
            }
        }
    })
    new Reaction(() => {
        if (element.TopBar) {
            if ((element.RightSideBar || element.LeftSideBar) && element.BarsCollapsed) {
                element.TopBar.Height = () => Max(element.TopBar.InternalHeight, element.SideBarsIconsSize)
            } else {
                element.TopBar.Height = () => First(element.TopBar.InternalHeight, element.SideBarsIconsSize)
            }
        }
    })


    /*window.addEventListener('touchstart', function () {
        // User has very quick fingers.
        overscroll = false;
    });

    window.addEventListener('touchend', function () {
        // User released touch-drag event when element was in an overscroll state.
        if (document.body.scrollTop < 0) {
            overscroll = true;
        }
    });*/

    window.addEventListener('scroll', e => {
        //console.log(e);
    });


    new Reaction(() => {
        if (element.LeftSideBar) {
            element.LeftSideBar.Depth = 100
            element.LeftSideBar.style.position = "fixed"
            element.LeftSideBar.Height = ()=>element.WindowHeight
        }
    })


    OnTruthify(
        () => !element.Swipe,
        () => {
            if (element.BarsCollapsed) {
                if (element.LeftSideBar && element.SwipeX > element.LeftSideBar.Width / 2) {
                    element.SideBarOpen = -1
                }
            }
        }
    )



    new Reaction(() => {
        if (!element.BarsCollapsed) {
            element.SideBarOpen = 0
        }
    })

    new Reaction(() => {
        if (element.LeftSideBar) {

            if (element.BarsCollapsed) {
                if (element.SideBarOpen == 0) {
                    if (element.Swipe) {
                        element.LeftSideBar.X = Min(-element.LeftSideBar.Width + element.SwipeX, 0)
                    } else {
                        element.LeftSideBar.X = -element.LeftSideBar.Width
                    }
                    //
                } else if (element.SideBarOpen==-1){
                    element.LeftSideBar.X = 0

                }
                
                //element.LeftSideBar.style.visibility = "hidden"
            } else {
                element.LeftSideBar.X = 0
            }            
        }
    })

    //document.documentElement.style.overflowX = "scroll"


    new Reaction(() => {

        let LeftBarSize = (element.LeftSideBar && !element.BarsCollapsed) ? Max(element.LeftSideBar.Width, 0) : 0

        let RightBarSize = (element.RightSideBar && !element.BarsCollapsed) ? Max(element.RightSideBar.Width, 0) : 0



        



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