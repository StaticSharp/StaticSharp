
function Page(element) {
    element.classList.add("js")
    element.isRoot = true
    let loadingDeffered = Reaction.beginDeferred()

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

    



    element.HtmlNodesOrdered = new Enumerable(function* () {
        if (element.svgDefs)
            yield element.svgDefs
        yield* element.UnmanagedChildren
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

    //document.body.style.display = "none"


    function CreateIntersectionObserver(elements,func) {
        const observer = new IntersectionObserver((entries) => {
            func(entries);
            /*for (const entry of entries) {
                //const bounds = entry.boundingClientRect;
                console.log(entry);
            }*/
            observer.disconnect();
        })
        for (const i of elements) {
            observer.observe(i);
        }
    }

    //let u = undefined
    /*let f = () => u

    let u = 8

    console.log(f())
    console.log(window.u)*/

    function MeasureInlineContainers(doneFunc) {
        const elements = document.querySelectorAll(".inline-container");
        if (elements.length == 0) {
            doneFunc()
            return
        }
        for (const i of elements) {
            i.style.fontSize = Paragraph.testFontSize + "px";
            i.style.width = "min-content"
        }
        CreateIntersectionObserver(elements, entries => {
            //console.log(entries)
            for (const entry of entries) {
                const bounds = entry.boundingClientRect;
                entry.target.minWidth = bounds.width
                entry.target.maxHeight = bounds.height
            }

            for (const i of elements) {
                i.style.width = "max-content"
            }
            CreateIntersectionObserver(elements, entries => {
                for (const entry of entries) {
                    const bounds = entry.boundingClientRect;
                    entry.target.maxWidth = bounds.width
                    entry.target.minHeight = bounds.height
                }
                for (const i of elements) {
                    i.style.width = ""
                    i.style.fontSize = ""
                }

                doneFunc()
            })
        })
    }


    let loadEventsToWait = 2
    function onLoadEvent() {
        loadEventsToWait--
        if (loadEventsToWait == 0) {
            //document.body.style.display = "block"
            //document.body.style.visibility = "hidden"

            MeasureInlineContainers(() => {
                console.log("-------------Reactions------------", performance.now());
                loadingDeffered.end()
                console.log("-------------Reactions-done-------", performance.now());
                document.body.style.visibility = "visible"
                //document.body.style.opacity = 1

                if (location.hash !== "") {
                    location.href = location.hash
                }

            })


            


            /*if (observer)
                element.BackgroundColor = new Color(1, 0, 0, 1)*/


            
            

            
            
        }
    }

    document.fonts.ready
        .then(() => {
            console.log("-------------Fonts ready--", performance.now());
            onLoadEvent()            
        })

    window.addEventListener("DOMContentLoaded", function (event) {
        console.log("------DOMContentLoaded--", performance.now());
        onLoadEvent()
    })



    

}


