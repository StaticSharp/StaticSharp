StaticSharpClass("StaticSharp.DebugModifier", (modifier, element) => {
    StaticSharp.Modifier(modifier, element)

    modifier.Reactive = {
        Radius: 5
    }

    new Reaction(() => {
        //console.log("glass")
        /*if (!document.fullscreenElement) {
            element.requestFullscreen().catch((err) => {
                alert(
                    `Error attempting to enable fullscreen mode: ${err.message} (${err.name})`
                );
            });
        }*/
        return


        if (!modifier.mask) {

            modifier.mask = document.createElement("dialog")
            modifier.mask.style.position = "fixed"
            

            //modifier.mask.style.width = "100%"
            //modifier.mask.style.height = "100%"

            /*function MaskEvent() {
                console.log("MaskEvent", event.type)

                var elements = document.elementsFromPoint(event.pageX, event.pageY)
                var firstElement = elements.find(x => x != modifier.mask)

                //console.log(elements)
                if (elements.includes(element)) {
                    firstElement.dispatchEvent(new event.constructor(event.type, event))
                } else {
                    element.dispatchEvent(new event.constructor(event.type, event))
                }
                event.preventDefault()
                event.stopPropagation()
            }

            for (const key in modifier.mask) {
                if (/^on/.test(key)) {
                    const eventType = key.substr(2);
                    modifier.mask.addEventListener(eventType, MaskEvent);
                }
            }*/

            



            /*function redirectEvent(eventType, fromElement, toElement) {

                

                fromElement.addEventListener(eventType, function (event) {
                    console.log("redirectEvent", event.type)
                    var elements = document.elementsFromPoint(event.pageX, event.pageY)
                    var firstElement = elements.find(x => x != modifier.mask)

                    //console.log(elements)
                    if (elements.includes(toElement)) {
                        firstElement.dispatchEvent(new event.constructor(event.type, event))
                    }
                    event.preventDefault()
                    event.stopPropagation()
                });


            }
            redirectEvent("click", modifier.mask, element, false)
            redirectEvent("mousemove", modifier.mask, element, false)
            redirectEvent("mouseenter", modifier.mask, element, false)
            redirectEvent("mouseleave", modifier.mask, element, false)*/


            /*modifier.mask.addEventListener("click", () => {
                element.dispatchEvent(new event.constructor(event.type, event));
            });*/



            element.addEventListener("click", () => console.log("click"));
            element.addEventListener("mousemove", () => console.log("mousemove"));

            element.addEventListener("mouseenter", () => console.log("mouseenter"), false)
            element.addEventListener("mouseleave", () => console.log("mouseleave"), false)

            /*modifier.mask = document.createElementNS('http://www.w3.org/2000/svg', 'svg');


            modifier.mask.innerHTML = `
<path d="M0 0h200v200h-200z" fill="black" />

  <path d="M75 75h50v50h-50z" fill="white" fill-rule="evenodd" />
`


            modifier.mask.setAttribute('fill', 'black');
            modifier.mask.setAttribute('viewBox', '0 0 20 20');
            modifier.mask.style.position = "fixed"
            modifier.mask.style.width = "20px"
            modifier.mask.style.height = "20px"

            modifier.mask.addEventListener("click", () => console.log("click"));*/

            //modifier.mask.style.pointerEvents = "none"
            modifier.mask.style.opacity = "0.8"

            document.body.extras.appendChild(modifier.mask)
            modifier.mask.disableClose = true
            let modal = false
            modifier.mask.addEventListener("click", () => {

                console.log("modifier.mask.click", modal)

                if (modal) {                    
                    modifier.mask.close()
                    modifier.mask.show()
                } else {
                    modifier.mask.close()
                    modifier.mask.showModal()
                }

                modal = !modal

            });

            //modifier.mask.showModal()
            //modifier.mask.close()
            
        }

        modifier.mask.style.left = `${element.AbsoluteX+1}px`
        modifier.mask.style.top = `${element.AbsoluteY+1}px`
        modifier.mask.style.width = `${element.Width}px`
        modifier.mask.style.height = `${element.Height}px`

        //element.style.clipPath = "url(https://raw.githubusercontent.com/Templarian/MaterialDesign/master/svg/access-point-plus.svg)"// "inset(-20% 30% round 20px)";
    })
})