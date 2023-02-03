function MenuResponsive(element) {
    Block(element)
    element.isMenuResponsive = true


    element.Reactive = {

        Dropdown: undefined,

        InternalHeight: () => {
            let result = 0
            for (let child of element.Children) {
                result = Max(result, child.Height)
            }
            return result
        }

    }


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


    new Reaction(() => {
        element.Dropdown.Visibility = element.Hover ? 1 : 0
        element.Dropdown.Width = element.Width*0.5
        element.Dropdown.Height = 50

        let children = [...element.Children]
        let c = children[1]
        c.remove()

        element.Dropdown.appendChild(c)

    })



    new Reaction(() => {
        console.log("MenuResponsive layout")
        let children = [...element.Children]
        
        let x = CalcOffset(element, children[0], "Left")
        children[0].LayoutX = x
        let marginRight = First(children[0].MarginRight,0)
        //let width = children[0].Width
        x += children[0].Width

        for (let child of children.slice(1)) {
            //console.log(width, marginRight, Max(marginRight, child.MarginLeft))
            //if (child.isBlock) {
            let margin = Max(marginRight, child.MarginLeft)
            child.LayoutX = x + margin

            x += margin + child.Width
            
            marginRight = First(child.MarginRight, 0)

            
            
            //}
            
        }

        x += CalcOffset(element, children[children.length-1], "Right")
        element.InternalWidth = x

    })


}
