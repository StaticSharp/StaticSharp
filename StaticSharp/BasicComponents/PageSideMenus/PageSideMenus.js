
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

var glass = undefined

function getGlass() {
    if (!glass) {
        glass = document.createElement("glass")
        document.body.appendChild(glass)
        Glass(glass)
    }
    return glass
}


function Glass(element) {
    element.style.width = "100vw"
    element.style.height = "100vh"
    //element.style.backgroundColor = "black"
    element.style.zIndex = 99
    element.style.position = "fixed"
    

    element.Reactive = {
        Color: new Color(0xff000000),
        Visibility: 1,
        MaxOpacity: 0.8
    }
    new Reaction(() => {
        element.style.backgroundColor = element.Color
    })

    new Reaction(() => {
        //element.style.backdropFilter = `blur(${element.Visibility*10}px)`;
        element.style.opacity = element.Visibility * element.MaxOpacity
        element.style.display = element.Visibility == 0 ? "none" : "block"
    })
}



function PageSideMenus(element) {
    Page(element)


    let glass = getGlass()

    const minSwipeToOpen = 40
    const minSwipeToClose = 20
    const swipeThreshold = 20

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

        SwipeXAboveThreshold: () => {
            if (element.SwipeX > swipeThreshold)
                return element.SwipeX - swipeThreshold
            if (element.SwipeX < -swipeThreshold)
                return element.SwipeX + swipeThreshold
            return 0
        },


        MinSwipeToOpen_LeftSideBar: () => element.LeftSideBar ? Min(minSwipeToOpen, element.LeftSideBar.Width) : undefined,        
        MinSwipeToOpen_RightSideBar: () => element.RightSideBar ? Min(minSwipeToOpen, element.RightSideBar.Width) : undefined,

        SwipeX: 0,
        SwipeY: 0,
        Swipe: false
    }

    let startX = 0
    let startY = 0
    let swipeDirection = undefined

    /*function touchStart(event) {
        var touch = event.touches[0];
        startX = touch.clientX;
        startY = touch.clientY;
        
    }*/
    
    function elementTouchMove(event) {
        if (event.cancelable) {
            var touch = event.touches[0];
            let deltaX = Math.abs(touch.clientX - startX)
            if (deltaX > swipeThreshold) {
                element.removeEventListener('touchmove', elementTouchMove)
                element.addEventListener('touchmove', horizontalTouchMove, { passive: false })
            }
        }     
    }

    element.addEventListener('touchstart', (event) => {
        swipeDirection = undefined
        touchStart(event)
    }, { passive: false });

    element.addEventListener('touchmove', elementTouchMove)
    element.addEventListener('touchend', touchEnd)

    function horizontalTouchMove(event) {
        var touch = event.touches[0];
        console.log(touch.clientX,startX)
        let d = Reaction.beginDeferred()
        element.SwipeX = touch.clientX - startX
        element.SwipeY = touch.clientY - startY
        element.Swipe = true
        d.end()
        event.preventDefault()
    }

    function touchEnd(event) {
        element.Swipe = false
        if (element.SideBarOpen == 0) {
            //element.addEventListener('touchmove', elementTouchMove)
        }
    }

   
    function touchStart(event) {
        var touch = event.touches[0];
        startX = touch.clientX;
        startY = touch.clientY;
        //event.preventDefault()
    }

    function touchMove(event) {
        var touch = event.touches[0];
        let d = Reaction.beginDeferred()
        element.SwipeX = touch.clientX - startX
        element.SwipeY = touch.clientY - startY
        element.Swipe = true
        d.end()
        event.preventDefault()
    }
    function touchEnd(event) {
        element.Swipe = false
        event.preventDefault()
    }

    function addTouchEventListeners(target) {
        target.addEventListener('touchstart', touchStart, { passive: false });
        target.addEventListener('touchmove', touchMove, { passive: false });
        target.addEventListener('touchend', touchEnd);
    }

    //addTouchEventListeners(element)    
    addTouchEventListeners(glass) 


    /*new Reaction(() => {
        if (element.LeftSideBar) 
            addTouchEventListeners(element.LeftSideBar)
        if (element.RightBarSize)
            addTouchEventListeners(element.RightBarSize)        
    })*/



    //SwipeDetector(element)


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

    new Reaction(() => {
        if (element.RightSideBar) {
            element.RightSideBar.Depth = 100
            element.RightSideBar.style.position = "fixed"
            element.RightSideBar.Height = () => element.WindowHeight
        }
    })


    

    OnTruthify(
        () => !element.Swipe,
        () => {
            if (element.BarsCollapsed) {
                if (element.SideBarOpen == 0) {
                    if (element.LeftSideBar && element.SwipeXAboveThreshold > element.MinSwipeToOpen_LeftSideBar) {
                        element.SideBarOpen = -1
                        return
                    }
                    if (element.RightSideBar && element.SwipeXAboveThreshold < -element.MinSwipeToOpen_RightSideBar) {
                        element.SideBarOpen = 1
                        return
                    }
                } else {
                    if (Math.sign(element.SwipeXAboveThreshold) == Math.sign(element.SideBarOpen) && Math.abs(element.SwipeXAboveThreshold) > minSwipeToClose) {
                        element.SideBarOpen = 0
                    }
                }
            }
        }
    )

    new Reaction(() => {
        if (!element.BarsCollapsed) {
            element.SideBarOpen = 0
        }
    })

    //Glass Visibility
    new Reaction(() => {
        if (!element.BarsCollapsed) {
            glass.Visibility = 0
        } else {
            if (element.SideBarOpen == 0) {
                if (element.Swipe) {
                    if (element.SwipeXAboveThreshold > 0) {
                        if (element.LeftSideBar) {
                            glass.Visibility = Math.min(Math.max(element.SwipeXAboveThreshold / element.MinSwipeToOpen_LeftSideBar, 0), 1)
                            return
                        }
                    }
                    if (element.SwipeXAboveThreshold < 0) {
                        if (element.RightSideBar) {
                            glass.Visibility = Math.min(Math.max(-element.SwipeXAboveThreshold / element.MinSwipeToOpen_RightSideBar, 0), 1)
                            return
                        }
                    }
                }
            } else {
                if (element.Swipe) {
                    glass.Visibility = Math.min(Math.max(1 - element.SwipeXAboveThreshold * element.SideBarOpen / minSwipeToClose, 0), 1)
                    return
                } else {
                    glass.Visibility = 1
                    return
                }
            }
        }
        glass.Visibility = 0

    })


    new Reaction(() => {
        if (element.LeftSideBar) {
            if (element.BarsCollapsed) {
                if (element.SideBarOpen == 0) {
                    if (element.Swipe) {
                        element.LeftSideBar.X = Min(-element.LeftSideBar.Width + element.SwipeXAboveThreshold, 0)                        
                    } else {
                        element.LeftSideBar.X = -element.LeftSideBar.Width
                    }
                    //
                } else if (element.SideBarOpen==-1){                    
                    if (element.Swipe) {
                        element.LeftSideBar.X = Min(0, Max(element.SwipeXAboveThreshold, -element.LeftSideBar.Width))
                        
                    } else {
                        element.LeftSideBar.X = 0
                    }
                }
            } else {
                element.LeftSideBar.X = 0
            }            
        }
    })

    new Reaction(() => {
        if (element.RightSideBar) {
            if (element.BarsCollapsed) {
                if (element.SideBarOpen == 0) {
                    if (element.Swipe) {
                        element.RightSideBar.X = element.WindowWidth + Max(element.SwipeXAboveThreshold, -element.RightSideBar.Width)
                        
                    } else {
                        element.RightSideBar.X = element.WindowWidth

                    }
                } else if (element.SideBarOpen == 1) {
                    if (element.Swipe) {
                        element.RightSideBar.X = element.WindowWidth - element.RightSideBar.Width + Max(0, element.SwipeXAboveThreshold)
                    } else {
                        element.RightSideBar.X = element.WindowWidth - element.RightSideBar.Width
                    }
                }
            } else {
                element.RightSideBar.X = element.WindowWidth - element.RightSideBar.Width
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