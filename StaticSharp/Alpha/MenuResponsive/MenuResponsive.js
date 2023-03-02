function MenuResponsive(element) {
    Block(element)
    element.isMenuResponsive = true

    CreateSocket(element, "Logo", element)
    CreateSocket(element, "Button", element)
    CreateSocket(element, "Dropdown", element)

    CreateCollectionSocket(element, "MenuItems", element)

    element.Reactive = {

        InternalHeight: () => {
            let result = undefined
            for (let child of element.MenuItems) {
                result = Max(result, child.Height)
            }
            if (result == undefined)
                result = 64
            return result
        },

        DropdownExpanded : true

    }

    element.HtmlNodesOrdered = new Enumerable(function* () {
        if (element.Logo)
            yield element.Logo
        yield element.Button
        yield* element.MenuItems
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
        //element.Dropdown.FirstChild = element.FirstChild

        console.log(element.FirstChild);
        console.log(element.Children);
        console.log(element.Children.Last());
        //element.Dropdown.FirstChild = element.Children.Last();


        /*if (element.Dropdown.FirstChild) {           
            element.Dropdown.Children.Last().NextSibling = element.Children.Last()
        } else {
            element.Dropdown.FirstChild = element.Children.Last()
        }*/
    }


    element.AfterChildren = () => {
        console.log("AfterChildren");
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




    //new Reaction(() => {

    //    let children = [...element.Children]
        
    //    let x = CalcOffset(element, children[0], "Left")
    //    children[0].LayoutX = x
    //    children[0].LayoutY = 0
    //    let marginRight = First(children[0].MarginRight,0)
    //    //let width = children[0].Width
    //    x += children[0].Width

    //    for (let child of children.slice(1)) {
    //        //console.log(width, marginRight, Max(marginRight, child.MarginLeft))
    //        //if (child.isBlock) {
    //        let margin = Max(marginRight, child.MarginLeft)
    //        child.LayoutX = x + margin
    //        child.LayoutY = 0
    //        x += margin + child.Width
            
    //        marginRight = First(child.MarginRight, 0)

            
            
    //        //}
            
    //    }

    //    x += CalcOffset(element, children[children.length-1], "Right")
    //    element.InternalWidth = x

    //})


    // Children placing
    new Reaction(() => {
        //console.log("MenuResponsive layout")

        // Button
        let rightOffset = CalcOffset(element, element.Button, "Right")
        element.Button.LayoutX = element.Width - element.Button.Width - rightOffset
        let topOffset = CalcOffset(element, element.Button, "Top")
        element.Button.LayoutY = topOffset

        // Dropdown area
        rightOffset = CalcOffset(element, element.Dropdown, "Right")
        element.Dropdown.LayoutX = element.Width - element.Dropdown.Width - rightOffset

        element.Dropdown.LayoutY = element.Button.Y + element.Button.Height +
            Max(First(element.Button.MarginBottom, 0), First(element.Dropdown.MarginTop, 0))

        // Children
        let children = [...element.MenuItems]
        let dropdownChildren = [...element.Dropdown.Children]

        //let x = CalcOffset(element, element.Logo, "Left")
        //element.Logo.LayoutX = x
        //let marginright = First(element.Logo.MarginRight, 0)
        //x += element.Logo.Width

        //let childrenMovedToDropdown = false

        let suggestedOffsets = CalcSequentialOffsets(element, children.concat(dropdownChildren), "Left")
        let firstChildOffset = element.Logo.X + element.Logo.Width
        let marginBeforeChildren = element.Logo.MarginRight
        let marginAfterChildren = element.Button.MarginRight
        let widthForChildren = element.Button.X - firstChildOffset

        let moveToDropdownOccured = false
        
        for (let i = 0; i < children.length; i++) {
            if (suggestedOffsets[i] + children[i].Width + Max(children[i].MarginRight, marginAfterChildren, 0) < widthForChildren) {
                children[i].LayoutY = 0 // Margin/Padding?
                children[i].LayoutX = suggestedOffsets[i] + firstChildOffset +
                    Max(marginBeforeChildren, children[0].MarginLeft, 0)
            } else {
                var itemsToTransfer = element.MenuItems.RemoveRange(i, children.length - i)                
                element.Dropdown.Children.InsertRange(0, itemsToTransfer)

                moveToDropdownOccured = true
                break
            }
        }

        if (!moveToDropdownOccured) {

            for (let i = 0; i < dropdownChildren.length; i++) {
                if (suggestedOffsets[children.length + i] + dropdownChildren[i].Width + Max(dropdownChildren[i].MarginRight, marginAfterChildren, 0) < widthForChildren) {
                    let d = Reaction.beginDeferred()
                    let itemToTransfer = element.Dropdown.Children.RemoveAt(i)
                    element.MenuItems.Insert(element.MenuItems.ToArray().length, itemToTransfer)
                    d.end()

                    dropdownChildren[i].LayoutY = 0 // Margin/Padding?
                    dropdownChildren[i].LayoutX = suggestedOffsets[children.length + i] + firstChildOffset +
                        Max(marginBeforeChildren, children[0].MarginLeft, 0)
                } else {
                    break;
                }
            }
        }

        ///###

        //x += CalcOffset(element, children[children.length - 1], "Right")
        // TODO: Margings? Margin collapsing?
        element.InternalWidth = element.Dropdown.Width + element.Logo.Width + suggestedOffsets[suggestedOffsets.length - 1]

        


            //if (element.DropdownChildrenList.length == 0) {
            //    element.Dropdown.Visibility = 0
            //    element.DropdownExpanded = false
            //} else {
            //    element.Dropdown.Visibility = 1
            //}


            ////let y = element.Dropdown.Y + element.Dropdown.Height //CalcOffset(element, element.DropdownChildrenList[0], "Top")
            ////let marginBottom = 0
            ////for (let child of element.DropdownChildrenList) {
            ////    child.LayoutX = element.Width - child.Width
            ////    child.LayoutY = y + Max(marginBottom, First(child.MarginTop, 0));

            ////    marginBottom = First(child.MarginBottom, 0)
            ////    y += child.Height

            ////    child.Visibility = element.DropdownExpanded ? 1 : 0;
            ////}

        

        // TODO: dropdown inner height/width
    })


}
