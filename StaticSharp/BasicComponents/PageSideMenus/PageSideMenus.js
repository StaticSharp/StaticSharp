var glass = undefined

function getGlass() {
    if (!glass) {
        glass = document.createElement("glass")
        document.body.appendChild(glass)
        Glass(glass)
    }
    return glass
}

const glassZIndex = 99
function Glass(element) {
    element.style.width = "100vw"
    element.style.height = "100vh"
    //element.style.backgroundColor = "black"
    element.style.zIndex = glassZIndex
    element.style.position = "fixed"


    element.Reactive = {
        Color: new Color(0xff000000),
        Visibility: 1,
        MaxOpacity: 0.7
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

function lerp(a, b, t) {
    return a + t * (b - a)
}

function AnimateTo(targetValue, duration) {
    let result = (property) => {

        let startValue = property.value
        let startTime = performance.now()

        return () => {
            let elapsed = performance.now() - startTime
            if (elapsed < duration) {
                window.AnimationFrame
            }
            let mormalizedTime = Math.min(elapsed / duration, 1)
            let t = (typeof targetValue === "function") ? targetValue() : targetValue
            return lerp(startValue, t, mormalizedTime)
        }
    }
    result.isBindingConstructor = true
    return result
}




function PageSideMenus(element) {
    Page(element)


 
    let glass = getGlass()

    const minSwipeToOpen = 40
    const minSwipeToClose = 20
    const swipeThreshold = 20

    let toggle = false
    element.Events.Click = () => {
        
        /*toggle = !toggle
        element.ContentWidth = AnimateTo(
            () => toggle ? (element.WindowWidth - 100) : 500,
            10000)*/
    }

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
        LeftSideBarIcon: () => element.Child("LeftSideBarIcon"),

        RightSideBar: () => element.Child("RightSideBar"),
        RightSideBarIcon: () => element.Child("RightSideBarIcon"),

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
        Swipe: false,

        LeftSideBarSwipeProgress : 0,
        RightSideBarSwipeProgress: 0,

    }





    let startX = 0
    let startY = 0

    const iconMargin = 10
    const collapsedIconWidth = 6
    const collapsedIconHeight = 120
    

    function touchStart() {
        var touch = event.touches[0];
        startX = touch.clientX;
        startY = touch.clientY;
        
    }
    function horizontalTouchMove() {
        var touch = event.touches[0];
        let d = Reaction.beginDeferred()
        element.SwipeX = touch.clientX - startX
        element.SwipeY = touch.clientY - startY
        element.Swipe = true
        d.end()
        event.preventDefault()
    }

    element.Events.TouchStart = {

        handler: () => {
            touchStart()
            element.Events.TouchMove = () => {
                if (event.cancelable) {
                    var touch = event.touches[0];
                    let deltaX = Math.abs(touch.clientX - startX)
                    if (deltaX > swipeThreshold) {
                        element.Events.TouchMove = {
                            handler: horizontalTouchMove,
                            passive: false
                        }
                    }
                } else {
                    element.Events.TouchMove = undefined
                    element.Events.TouchEnd = undefined
                }
            }
            element.Events.TouchEnd = () => {
                element.Swipe = false
                element.Events.TouchMove = undefined
                element.Events.TouchEnd = undefined
            }
        },
        passive: false        
    }
    glass.Events.TouchStart = {
        handler: () => {
            touchStart()

            glass.Events.TouchMove = {
                handler: horizontalTouchMove,
                passive: false
            }

            glass.Events.TouchEnd = () => {
                element.Swipe = false
                glass.Events.TouchMove = undefined
                glass.Events.TouchEnd = undefined
            }
        },
        passive: false
    }

    glass.Events.Click = () => {
        console.log("clicked")
        element.SideBarOpen = 0
    }

    


    new Reaction(() => {
        if (element.TopBar) {
            if (element.LeftSideBar && element.BarsCollapsed) {
                element.TopBar.MarginLeft = element.SideBarsIconsSize + iconMargin
            } else {
                element.TopBar.MarginLeft = 0
            }
        }
    })
    new Reaction(() => {
        if (element.TopBar) {
            if (element.RightSideBar && element.BarsCollapsed) {
                element.TopBar.MarginRight = element.SideBarsIconsSize + iconMargin
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




    


    new Reaction(() => {
        if (element.LeftSideBar) {
            element.LeftSideBar.Depth = glassZIndex + 1
            element.LeftSideBar.style.position = "fixed"
            element.LeftSideBar.Height = () => element.WindowHeight
        }
    })

    new Reaction(() => {
        if (element.RightSideBar) {
            element.RightSideBar.Depth = glassZIndex + 1
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
                        element.LeftSideBar.X = -element.LeftSideBar.Width + Clamp(element.SwipeXAboveThreshold, 0, element.LeftSideBar.Width)
                    } else {
                        element.LeftSideBar.X = -element.LeftSideBar.Width
                    }
                } else if (element.SideBarOpen == -1) {
                    if (element.Swipe) {
                        element.LeftSideBar.X = Clamp(element.SwipeXAboveThreshold, -element.LeftSideBar.Width, 0)

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
                        element.RightSideBar.X = element.WindowWidth + Clamp(element.SwipeXAboveThreshold, -element.RightSideBar.Width, 0)
                    } else {
                        element.RightSideBar.X = element.WindowWidth
                    }
                } else if (element.SideBarOpen == 1) {
                    if (element.Swipe) {
                        element.RightSideBar.X = element.WindowWidth - element.RightSideBar.Width + Clamp(element.SwipeXAboveThreshold, 0, element.RightSideBar.Width)
                    } else {
                        element.RightSideBar.X = element.WindowWidth - element.RightSideBar.Width
                    }
                } else if (element.SideBarOpen == -1) {
                    element.RightSideBar.X = element.WindowWidth 
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
            element.Content.LayoutHeight = Max(element.Content.InternalHeight, element.WindowHeight)
        }
    })

    new Reaction(() => {
        element.style.width = ToCssSize(element.WindowWidth)
        element.style.height = ToCssSize(element.Content.Height)
    })













    function CreateButton() {
        let button = document.createElement("icon")
        button.style.position = "fixed"
        button.style.backgroundColor = "red"
        const transitionDuration = 0.2
        button.style.transition = `all ${transitionDuration}s, left 0s`;
        element.appendChild(button)
        return button
    }

    let leftButton = undefined

    new Reaction(() => {
        if (element.LeftSideBar && element.LeftSideBarIcon) {
            element.LeftSideBarIcon.Y = () => iconMargin
            element.LeftSideBarIcon.X = () => iconMargin + element.LeftSideBar.X + element.LeftSideBar.Width
            element.LeftSideBarIcon.Width = () => element.SideBarsIconsSize
            element.LeftSideBarIcon.Radius = () => 0.5 * element.SideBarsIconsSize
            element.LeftSideBarIcon.Visibility = () => (element.BarsCollapsed)?1:0
        }
    })

    new Reaction(() => {
        

        if (element.LeftSideBar && element.LeftSideBarIcon) {
            if (element.BarsCollapsed) {
                leftButton.style.display = "block"
                let barVisible = (element.LeftSideBar.Width + element.LeftSideBar.X) > 0
                if (element.ScrollY > 0 || barVisible) {
                    leftButton.style.left = element.LeftSideBar.X + element.LeftSideBar.Width + "px"
                    leftButton.style.zIndex = barVisible ? glassZIndex + 1 : glassZIndex - 1 
                    leftButton.style.top = (iconMargin) + "px"
                    leftButton.style.marginLeft = 0
                    leftButton.style.width = collapsedIconWidth + "px"
                    leftButton.style.height = collapsedIconHeight + "px"
                    leftButton.style.borderRadius = collapsedIconWidth + "px"
                    leftButton.style.borderTopLeftRadius = 0
                    leftButton.style.borderBottomLeftRadius = 0
                } else {

                    leftButton.style.zIndex = 98 
                    leftButton.style.left = 0
                    leftButton.style.top = iconMargin + "px"
                    leftButton.style.marginLeft = iconMargin + "px"
                    leftButton.style.width = element.SideBarsIconsSize + "px"
                    leftButton.style.height = element.SideBarsIconsSize + "px"
                    leftButton.style.borderRadius = (0.5 * element.SideBarsIconsSize) + "px"
                }
            } else {
                console.log("element.LeftSideBarIcon.Visibility = 0")
                element.LeftSideBarIcon.Visibility = 0
                //leftButton.style.display = "none"
            }
        }
    })

    let rightButton = undefined

    new Reaction(() => {

        if (element.RightSideBar) {
            if (rightButton === undefined) {
                rightButton = CreateButton()
                rightButton.style.backgroundColor = "blue"
            }
            if (element.BarsCollapsed) {
                rightButton.style.display = "block"
                let barVisible = (element.RightSideBar.X) < element.WindowWidth
                if (element.ScrollY > 0 || barVisible) {

                    rightButton.style.left = element.RightSideBar.X + "px"
                    rightButton.style.zIndex = barVisible ? glassZIndex + 1 : glassZIndex - 1 
                    rightButton.style.top = (iconMargin) + "px"
                    rightButton.style.marginLeft =  - collapsedIconWidth + "px"
                    rightButton.style.width = collapsedIconWidth + "px"
                    rightButton.style.height = collapsedIconHeight + "px"
                    rightButton.style.borderRadius = collapsedIconWidth + "px"
                    rightButton.style.borderTopRightRadius = 0
                    rightButton.style.borderBottomRightRadius = 0


                } else {
                    rightButton.style.zIndex = 98
                    rightButton.style.left = (element.RightSideBar.X)+"px"
                    rightButton.style.top = iconMargin + "px"
                    rightButton.style.marginLeft = (-element.SideBarsIconsSize - iconMargin) + "px"
                    rightButton.style.width = element.SideBarsIconsSize + "px"
                    rightButton.style.height = element.SideBarsIconsSize + "px"
                    rightButton.style.borderRadius = (0.5 * element.SideBarsIconsSize) + "px"
                }
            } else {
                rightButton.style.display = "none"
            }
        }
    })




}