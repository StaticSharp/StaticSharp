function MenuResponsive(element) {
    Block(element)
    element.isMenuResponsive = true

    CreateSocket(element, "Logo", element)
    CreateSocket(element, "Button", element)
    CreateSocket(element, "Dropdown", element)
    
    element.Reactive = {

        InternalHeight: () => {
            let result = undefined
            for (let child of element.Children) {
                result = Max(result, child.Height)
            }
            if (result == undefined)
                result = 64
            return result
        }

    }

    element.HtmlNodesOrdered = new Enumerable(function* () {
        if (element.Logo)
            yield element.Logo
        yield element.Button
        yield element.Dropdown
        
        yield* element.Children
    })


    /*new Reaction(() => {
        let previousChild = undefined

        function getBinding(prev) {
            return () => prev.X + prev.Width
        }
        for (let child of element.Children) {
            if (child.isBlock) {
                child.LayoutX = previousChild ? getBinding(previousChild) : 0
                previousChild = child
            }
        }
        console.log("MenuResponsive layout")

    })*/


    /*new Reaction(() => {
        element.Dropdown.Visibility = element.Hover ? 1 : 0
        element.Dropdown.Width = element.Width*0.5
        element.Dropdown.Height = 50

        let children = [...element.Children]
        let c = children[1]
        c.remove()

        element.Dropdown.appendChild(c)

    })*/

    element.Events.Click = () => {
        console.log("element Click")
        element.Dropdown.FirstChild = element.FirstChild
        /*if (element.Dropdown.FirstChild) {           
            element.Dropdown.Children.Last().NextSibling = element.Children.Last()
        } else {
            element.Dropdown.FirstChild = element.Children.Last()
        }*/
    }

    element.AfterChildren = () => {
        element.Dropdown.Events.Click = () => {
            console.log("Dropdown Click")
            event.stopPropagation()
            if (element.FirstChild) {
                element.Children.Last().NextSibling = element.Dropdown.Children.Last()
            } else {
                element.FirstChild = element.Dropdown.Children.Last()
            }
        }

        element.Dropdown.Visibility = e => e.FirstChild ? 1 : 0

    }




    new Reaction(() => {

        let children = [...element.Children]
        
        let x = CalcOffset(element, children[0], "Left")
        children[0].LayoutX = x
        children[0].LayoutY = 0
        let marginRight = First(children[0].MarginRight,0)
        //let width = children[0].Width
        x += children[0].Width

        for (let child of children.slice(1)) {
            //console.log(width, marginRight, Max(marginRight, child.MarginLeft))
            //if (child.isBlock) {
            let margin = Max(marginRight, child.MarginLeft)
            child.LayoutX = x + margin
            child.LayoutY = 0
            x += margin + child.Width
            
            marginRight = First(child.MarginRight, 0)

            
            
            //}
            
        }

        x += CalcOffset(element, children[children.length-1], "Right")
        element.InternalWidth = x

    })


}
