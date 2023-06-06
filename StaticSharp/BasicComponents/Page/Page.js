

StaticSharpClass("StaticSharp.Page", (element) => {
    
    StaticSharp.Block(element)

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

        //LayoutWidth: undefined,
        //LayoutHeight: undefined,

        FontSize: 16,
        HierarchyFontSize: e => e.FontSize,

        BackgroundColor: new Color("#fff"),

        HierarchyBackgroundColor: () => element.BackgroundColor,
        HierarchyForegroundColor: () => element.ForegroundColor,

    }

    
    let baseHtmlNodesOrdered = element.HtmlNodesOrdered
    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield* baseHtmlNodesOrdered
        if (element.svgDefs)
            yield element.svgDefs
        if (element.extras)
            yield element.extras
    })





    new Reaction(() => {
        element.Width = getWindowWidth()
        element.Height = getWindowHeight()
    })


    window.onmousedown = () => {
        window.UserInteracted = true
    }



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



