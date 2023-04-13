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
            let t = (typeof targetValue === "function") ? targetValue() : targetValue
            let mormalizedTime = Math.min(elapsed / duration, 1)
            
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
    const swipeThreshold = 10

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
            element.Width < Num.Sum(
                element.ContentWidth,
                element.LeftSideBar ? element.LeftSideBar.Width : 0,
                element.RightSideBar ? element.RightSideBar.Width : 0),

        //Content: () => element.Child("Content"),

        SideBarOpen: 0, //-1 left , 1 right

        Content: undefined,

        LeftSideBar: undefined,
        LeftSideBarIcon: undefined,

        RightSideBar: undefined,
        RightSideBarIcon: undefined,

        TopBar: () => element.Content.Content.TopBar,

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
        if (!event.cancelable) return
        var touch = event.touches[0];
        //console.log("horizontalTouchMove", event.path)
        let d = Reaction.beginDeferred()
        element.SwipeX = touch.clientX - startX
        element.SwipeY = touch.clientY - startY
        element.Swipe = true
        d.end()
        event.preventDefault()
    }


    element.pan = function (x, y) {
        console.log("pan",element.tagName,x,y)
        return [x,y]
    }

    
    document.Events.TouchStart = {
        handler: () => {
            //if (event.pointerType != "touch") return

            //console.log("document TouchStart", event.path)
            touchStart()
            document.Events.TouchMove = () => {
                //console.log("document PointerMove", event.path, event.clientX, event.cancelable)
                if (event.cancelable) {
                    var touch = event.touches[0];
                    let deltaX = Math.abs(touch.clientX - startX)
                    //console.log("deltaX", deltaX, swipeThreshold)
                    if (deltaX > swipeThreshold) {
                        event.preventDefault()
                        //console.log("horizontalTouchMove...")
                        element.Events.TouchMove = {
                            handler: horizontalTouchMove,
                            passive: false
                        }
                    }
                } else {
                    //console.log("PointerMove = undefined")
                    document.Events.TouchMove = undefined
                    document.Events.TouchEnd = undefined
                }
            }
            element.Events.TouchEnd = () => {
                //console.log("TouchEnd")
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
                element.TopBar.Height = () => Num.Max(element.TopBar.Layer.Height/*InternalHeight*/, element.SideBarsIconsSize)
            } else {
                element.TopBar.Height = () => Num.First(element.TopBar.Layer.Height/*InternalHeight*/, element.SideBarsIconsSize)
            }
        }
    })




    


    new Reaction(() => {
        if (element.LeftSideBar) {
            element.LeftSideBar.Depth = glassZIndex + 1
            //element.LeftSideBar.style.position = "fixed"
            element.LeftSideBar.Height = () => element.Height
        }
    })

    new Reaction(() => {
        if (element.RightSideBar) {
            element.RightSideBar.Depth = glassZIndex + 1
            //element.RightSideBar.style.position = "fixed"
            element.RightSideBar.Height = () => element.Height
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
                        element.RightSideBar.X = element.Width + Clamp(element.SwipeXAboveThreshold, -element.RightSideBar.Width, 0)
                    } else {
                        element.RightSideBar.X = element.Width
                    }
                } else if (element.SideBarOpen == 1) {
                    if (element.Swipe) {
                        element.RightSideBar.X = element.Width - element.RightSideBar.Width + Clamp(element.SwipeXAboveThreshold, 0, element.RightSideBar.Width)
                    } else {
                        element.RightSideBar.X = element.Width - element.RightSideBar.Width
                    }
                } else if (element.SideBarOpen == -1) {
                    element.RightSideBar.X = element.Width 
                }
            } else {
                element.RightSideBar.X = element.Width - element.RightSideBar.Width
            }
        }
    })


    new Reaction(() => {

        let LeftBarSize = (element.LeftSideBar && !element.BarsCollapsed) ? Num.Max(element.LeftSideBar.Width, 0) : 0

        let RightBarSize = (element.RightSideBar && !element.BarsCollapsed) ? Num.Max(element.RightSideBar.Width, 0) : 0

        let width = element.Width - LeftBarSize - RightBarSize
        let innerWidth = Math.min(width, element.ContentWidth)
        let contentSpace = (width - innerWidth) * 0.5

        if (element.Content) {
            //element.Content.Height = element.WindowHeight
            //element.Content.LayoutWidth = width// - 2 * contentSpace
            element.Content.Layer.Width = width// - 2 * contentSpace


            element.Content.Content.PaddingLeft = contentSpace
            element.Content.Content.PaddingRight = contentSpace
            //element.Content.Content.Width = width
            element.Content.Content.Layer.Width = width

            //element.Content.LayoutX = LeftBarSize
            element.Content.Layer.X = LeftBarSize
            //element.Content.LayoutHeight = element.Height//(element.Content.InternalHeight, element.Height)
            element.Content.Layer.Height = element.Height//(element.Content.InternalHeight, element.Height)
        }
    })




    function ConfugureIcon(icon) {
        icon.Reactive = {
            AnimationProgress: 1,
            AnimationTarget: () => element.Content.ScrollYActual > 0 || icon.BarVisible ? 0 : 1
        }

        OnChanged(
            () => icon.AnimationTarget,
            (p, c) => {
                icon.AnimationProgress = AnimateTo(c ? 1 : 0, 200)
            }
        )

        icon.Y = () => icon.MarginTop
        
        icon.Width = () => lerp(collapsedIconWidth, element.SideBarsIconsSize, icon.AnimationProgress)
        icon.Height = () => lerp(collapsedIconHeight, element.SideBarsIconsSize, icon.AnimationProgress)

        icon.Radius = () => lerp(icon.Width, 0.5 * icon.Width, icon.AnimationProgress)
        icon.Visibility = () => (element.BarsCollapsed) ? 1 : 0
        icon.Depth = () => icon.BarVisible ? glassZIndex + 1 : glassZIndex - 1 

    }

    OnChanged(
        () => element.LeftSideBar && element.LeftSideBarIcon,
        (p, c) => {
            if (c) {
                let icon = element.LeftSideBarIcon
                ConfugureIcon(icon)
                icon.Reactive.BarVisible = () => (element.LeftSideBar.Width + element.LeftSideBar.X) > 0                
                
                icon.X = () => icon.MarginLeft * icon.AnimationProgress + element.LeftSideBar.X + element.LeftSideBar.Width

                icon.RadiusTopLeft = () => 0.5 * icon.Width * icon.AnimationProgress
                icon.RadiusBottomLeft = () => 0.5 * icon.Width * icon.AnimationProgress

                icon.Events.Click = () => element.SideBarOpen = -1
            }
        }
    )

    OnChanged(
        () => element.RightSideBar && element.RightSideBarIcon,
        (p, c) => {
            if (c) {
                let icon = element.RightSideBarIcon
                ConfugureIcon(icon)
                icon.Reactive.BarVisible = () => element.RightSideBar.X < element.Width

                icon.X = () => element.RightSideBar.X - icon.MarginRight * icon.AnimationProgress - element.RightSideBarIcon.Width

                icon.RadiusTopRight = () => 0.5 * icon.Width * icon.AnimationProgress
                icon.RadiusBottomRight = () => 0.5 * icon.Width * icon.AnimationProgress

                icon.Events.Click = () => element.SideBarOpen = 1
            }
        }
    )


}