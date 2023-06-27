

StaticSharpClass("StaticSharp.Page", (element) => {
    
    StaticSharp.BaseModifier(element)

    element.classList.add("js")
    element.isRoot = true


    var html = document.body.parentNode
    function getWindowWidth() {
        return html.clientWidth
        //return document.documentElement.clientWidth //window.innerWidth
    }
    function getWindowHeight() {       
        return html.clientHeight
        //return window.innerHeight // document.documentElement.clientHeight
    }

    let animationFrame = 0

    window.Reactive = {
        Root: element,
        AnimationFrameTime: 0,

        AnimationFrame: () => {
            animationFrame++
            window.requestAnimationFrame(() => {
                window.Reactive.AnimationFrame.makeDirty()
            });
            return animationFrame
        },

        UserInteracted: false,
        DevicePixelRatio: window.devicePixelRatio,
        Touch: false,
        Initialized: false,
    }



    function UpdateAnimation(time) {
        window.AnimationFrameTime = time
        window.requestAnimationFrame((t) => {
            UpdateAnimation(t)
        })
    }

    UpdateAnimation(performance.now());


    let touchMedia = window.matchMedia("(pointer: coarse)")
    window.Touch = touchMedia.matches
    touchMedia.onchange = (e) => {
        window.Touch = e.matches
    }


    element.Reactive = {
        Root: element,        

        Width: getWindowWidth(),
        Height: getWindowHeight(),

        FontSize: 16,
        HierarchyFontSize: e => e.FontSize,

        BackgroundColor: new Color("#fff"),

        HierarchyBackgroundColor: () => element.BackgroundColor,
        HierarchyForegroundColor: () => element.ForegroundColor,

    }

    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield* element.ExistingUnmanagedChildren
        if (element.svgDefs)
            yield element.svgDefs
        if (element.extras)
            yield element.extras
    })

    new Reaction(() => {
        let tergetChildren = [...element.HtmlNodesOrdered]
        SyncChildren(element, tergetChildren)
    })

    WidthToStyle(element)
    HeightToStyle(element)

    /*new Reaction(() => {
        element.Width = getWindowWidth()
        element.Height = getWindowHeight()
    })*/

    function pageActivationHandler() {
        console.log("UserInteracted")
        window.UserInteracted = true
        window.removeEventListener("mousedown", pageActivationHandler);
        window.removeEventListener("keydown", pageActivationHandlerByKey);
    }

    function pageActivationHandlerByKey() {
        const modifierKeys = ["Alt", "Control", "Shift", "CapsLock", "NumLock", "ScrollLock", "Meta",  /*Firefox*/ "Escape", "Tab"];
        if (!modifierKeys.includes(event.key)) {
            pageActivationHandler()
        }
    }

    window.addEventListener("mousedown", pageActivationHandler)
    window.addEventListener("keydown", pageActivationHandlerByKey)


    function createDevicePixelRatioCallback(func) {
        let matchMedia = window.matchMedia(`screen and (resolution: ${window.devicePixelRatio}dppx)`)
        matchMedia.addEventListener(
            "change",
            () => {
                func()
                createDevicePixelRatioCallback(func)
            },
            { once: true }
        );
    }


    createDevicePixelRatioCallback(() => {
        ApplyWindowDimensionsAndDevicePixelRatio
    })

    window.onresize = function (event) {
        ApplyWindowDimensionsAndDevicePixelRatio()
    }

    function ApplyWindowDimensionsAndDevicePixelRatio() {
        let d = Reaction.beginDeferred()
        window.DevicePixelRatio = window.devicePixelRatio
        element.Width = getWindowWidth()
        element.Height = getWindowHeight()
        d.end()
    }
    
    

})



