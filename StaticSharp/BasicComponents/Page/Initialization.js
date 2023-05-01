(function () {

    let loadingDeffered = Reaction.beginDeferred()

    //console.log(document.documentElement)

    function Hide() {
        document.documentElement.style.visibility = "hidden"
    }
    function Show() {
        document.documentElement.style.visibility = "visible"
    }

    Hide()

    function CreateIntersectionObserver(elements, func) {
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
                Show()
                //document.body.style.opacity = 1

                if (location.hash !== "") {
                    window.requestAnimationFrame(() => {
                        location.href = location.hash
                    });                    
                }

            })





            /*if (observer)
                element.BackgroundColor = new Color(1, 0, 0, 1)*/







        }
    }

    document.fonts.ready
        .then(() => {
            console.log("-------------Fonts ready--", performance.now().toFixed(1));
            onLoadEvent()
        })

    window.addEventListener("DOMContentLoaded", function (event) {
        console.log("------DOMContentLoaded--", performance.now().toFixed(1));
        onLoadEvent()
    })


})()