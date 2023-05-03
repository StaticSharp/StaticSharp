
function Page(element) {
    element.classList.add("js")
    element.isRoot = true
    

    Block(element)

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
    }

    /*let animationFramesHistory = []

    function linearRegression(points) {
        let n = points.length
        let xSum = 0, ySum = 0, xxSum = 0, xySum = 0
        for (var i = n - 1; i >= 0; i--) {
            xxSum += xSum;
            xSum += i;
            xySum += ySum;
            ySum += points[i];
        }

        // Calculate slope and intercept
        var slope = (n * xySum - xSum * ySum) / (n * xxSum - xSum * xSum);
        var intercept = (ySum / n) - (slope * xSum) / n;
        return intercept
    }*/

    let previousTime = 0
    function UpdateAnimation(time) {
        /*animationFramesHistory.unshift(time)
        animationFramesHistory.length = Math.min(animationFramesHistory.length, 5)

        console.log(time - previousTime)
        previousTime = time

        if (animationFramesHistory.length > 2) {
            time = linearRegression(animationFramesHistory)
        }*/
        
        /*console.log(time - previousTime)
        previousTime = time*/


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
    createDevicePixelRatioCallback(() => window.DevicePixelRatio = window.devicePixelRatio)



    element.Reactive = {
        Root: element,        

        //LayoutWidth: undefined,
        //LayoutHeight: undefined,

        FontSize: 16,
        HierarchyFontSize: () => element.FontSize,

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
        //element.LayoutWidth = getWindowWidth()
        //element.LayoutHeight = getWindowHeight()
        element.Width = getWindowWidth()
        element.Height = getWindowHeight()
    })

    /*window.ontouchend = () => {
        console.log("ontouchend")
        window.UserInteracted = true
    }*/
    window.onmousedown = () => {
        window.UserInteracted = true
    }

    /*function PrintWindowSize() {
        console.log("onresize", window.innerWidth, window.innerHeight, document.documentElement.clientWidth, document.documentElement.clientHeight, document.body.parentNode.clientHeight)
    }*/
    //PrintWindowSize()

    window.onresize = function (event) {
        //let startTime = performance.now()
        let d = Reaction.beginDeferred()
        //element.LayoutWidth = getWindowWidth()
        element.Width = getWindowWidth() // Why it was "Layout" ??? 
        //element.LayoutHeight = getWindowHeight()
        element.Height = getWindowHeight() // ???
        //PrintWindowSize()
        d.end()
        //console.log("resize", performance.now() - startTime)
    }

    /*const observer = new ResizeObserver(() => { });
    if (observer)
        element.BackgroundColor = new Color("#ff0000")*/


    



    



    

}


