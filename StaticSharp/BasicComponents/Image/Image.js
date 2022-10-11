
function Image(element) {
    Block(element)



    element.Reactive = {        

        Aspect: element.dataset.width / element.dataset.height,

        InternalWidth: () => First(element.Height * element.Aspect, element.dataset.width),
        InternalHeight: () => First(element.Width / element.Aspect, element.dataset.height),
        //Width: () => element.LayoutWidth || (element.Height * element.Aspect) || element.dataset.width,

        //Height: () => element.LayoutHeight || (element.Width / element.Aspect) || element.dataset.height,



    }

    new Reaction(() => {
        let content = element.children[0]
        content.style.width = "100%"
        content.style.height = "100%"
    })


    
}
