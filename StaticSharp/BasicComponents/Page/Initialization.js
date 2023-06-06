

function DebugRequest(message) {
    console.log("DebugRequest", message)
    fetch("file://"+message, {
        method: "DEBUG"
    })

}

StaticSharp.LoadFont = function (name, weight, italic, format, base64) {
    let dataUrl = `url(data:font/${format};base64,${base64})`
    return new Promise((resolve, reject) => {
        const font = new FontFace(name, dataUrl);
        font.weight = weight;
        font.style = italic ? "italic" : "normal"

        font.load().then(() => {
            document.fonts.add(font);
            resolve();

        }).catch((e) => {
            console.error(e, `Failed to load font ${name}`)
            reject()
        });
    });
}




StaticSharp.Initialization = function () {

    const staticFontsLoaded = StaticSharp.InitializeStaticFonts();

    const domContentLoaded = new Promise((resolve) => {
        document.addEventListener('DOMContentLoaded', () => {
            resolve();
        });
    });


    const domAndFontsReady = Promise.all([staticFontsLoaded, domContentLoaded])

    
    

    

    function MeasureElementsWithIntersectionObserver(elements) {
        return new Promise((resolve) => {
            const observer = new IntersectionObserver((entries) => {
                observer.disconnect();
                resolve(entries)
            })
            for (const i of elements) {
                observer.observe(i);
            }
        })        
    }


    function MeasureInlineContainers() {

        const elements = document.querySelectorAll(".inline-container");
        if (elements.length == 0) {
            return Promise.resolve()
        }
        for (const i of elements) {
            i.style.fontSize = StaticSharp.Paragraph.testFontSize + "px";
            i.style.width = "min-content"
        }

        return MeasureElementsWithIntersectionObserver(elements)
            .then((entries) => {
                for (const entry of entries) {
                    const bounds = entry.boundingClientRect;
                    entry.target.minWidth = bounds.width
                    entry.target.maxHeight = bounds.height
                }
            })
            .then(() => {
                for (const i of elements) {
                    i.style.width = "max-content"
                }                
            })
            .then(()=>MeasureElementsWithIntersectionObserver(elements))
            .then((entries) => {
                for (const entry of entries) {
                    const bounds = entry.boundingClientRect;
                    entry.target.maxWidth = bounds.width
                    entry.target.minHeight = bounds.height
                }

                for (const i of elements) {
                    i.style.width = ""
                    i.style.fontSize = ""
                }
            })
        
    }

    function Hide() {
        document.documentElement.style.visibility = "hidden"
    }
    function Show() {
        document.documentElement.style.visibility = "visible"
    }



    

    function duration(func) {
        let start = performance.now()
        func()
        return performance.now() - start
    }
    function printCurrentTime(text = "current time") {
        console.log(`${text} ${performance.now().toFixed(1)}ms`)
    }

    printCurrentTime("----first script")

    Hide()
    let loadingDeffered = Reaction.beginDeferred()


    domAndFontsReady
        .then(() => printCurrentTime("----dom and fonts ready at"))
        .then(MeasureInlineContainers)
        .then(() => printCurrentTime("----inlines measured at"))
        .then(() => {
            printCurrentTime("----initialization started at")
            let initializationDuration = duration(Initialize)
            console.log(`----initialization script duration ${initializationDuration.toFixed(1)}ms`,)

            let reactionsDuration = duration(()=>loadingDeffered.end())   
            console.log(`----reactions execution duration ${reactionsDuration.toFixed(1)}ms`,)

            window.Initialized = true

            Show()

            printCurrentTime()

            if (location.hash !== "") {
                window.requestAnimationFrame(() => {
                    location.href = location.hash
                });
            }
    })


    //const inlineContainersMeasured = 



    /*let loadEventsToWait = 2
    function onLoadEvent() {
        loadEventsToWait--
        if (loadEventsToWait == 0) {
            //document.body.style.display = "block"
            //document.body.style.visibility = "hidden"

            let readyToLayoutTime = performance.now()
            console.log("----readyToLayoutTime", readyToLayoutTime.toFixed(1));

            MeasureInlineContainers(() => {

                let inlineContainersMeasuredTime = performance.now()

                console.log("----inlineContainersMeasured", (inlineContainersMeasuredTime - readyToLayoutTime).toFixed(1));


                Initialize()

                let initializedTime = performance.now()

                console.log("----Initialize()", (initializedTime - inlineContainersMeasuredTime).toFixed(1));

                //console.profileEnd("Initialize")

                //console.log("window.debug", window.debug)

                loadingDeffered.end()

                let reactionsExecutedTime = performance.now()

                console.log("-----Reactions--", (reactionsExecutedTime - initializedTime).toFixed(1));


                window.Initialized = true

                Show()
                //document.body.style.opacity = 1

                if (location.hash !== "") {
                    window.requestAnimationFrame(() => {
                        location.href = location.hash
                    });                    
                }

            })
        }
    }*/

    /*document.fonts.onloadingdone = function (fontFaceSetEvent) {
        //alert('onloadingdone we have ' + fontFaceSetEvent.fontfaces.length + ' font faces loaded');
    };

    document.fonts.ready
        .then(() => {
            //alert('document.fonts.ready');
            DebugRequest("document.fonts.ready")
            console.log("-------------Fonts ready--", performance.now().toFixed(1));
            onLoadEvent()
        })

    window.addEventListener("DOMContentLoaded", function (event) {
        console.log("------DOMContentLoaded--", performance.now().toFixed(1));
        onLoadEvent()
    })*/


}

StaticSharp.Initialization()