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


    //element.Events.Click = () => {
    //    console.log("element Click")
    //}

    //element.AfterChildren = () => {
    //    console.log("AfterChildren");
    //    element.Dropdown.Events.Click = () => {
    //        console.log("Dropdown Click")
    //        event.stopPropagation()
    //        if (element.FirstChild) {
    //            element.Children.Last().NextSibling = element.Dropdown.Children.Last()
    //        } else {
    //            element.FirstChild = element.Dropdown.Children.Last()
    //        }
    //    }

    //    element.Dropdown.Visibility = e => e.FirstChild ? 1 : 0

    //}


    //new Reaction(() => {
    //    for (let menuItem of element.MenuItems) {
    //        menuItem.LayoutWidth = undefined
    //    }
    //})

    // Children placing
    new Reaction(() => {
        //console.log("MenuResponsive layout")

        // Button
        let rightOffset = CalcOffset(element, element.Button, "Right")
        element.Button./*LayoutX*/Layer.X = element.Width - element.Button.Layer.Width - rightOffset // TODO: element.Width -> element.PreferredWidth?
        let topOffset = CalcOffset(element, element.Button, "Top")
        //element.Button.LayoutY = topOffset
        element.Button.Layer.Y = topOffset

        // Dropdown area
        rightOffset = CalcOffset(element, element.Dropdown, "Right")
        element.Dropdown./*LayoutX*/Layer.X = element.Width - element.Dropdown.Layer.Width - rightOffset

        element.Dropdown./*LayoutY*/Layer.Y = element.Button.Layer.Y + element.Button.Layer.Height +
            Max(First(element.Button.Layer.MarginBottom, 0), First(element.Dropdown.Layer.MarginTop, 0))
        // Children
        //let children = [...element.MenuItems]
        //let dropdownChildren = [...element.Dropdown.Children]

        //let suggestedOffsets = CalcSequentialOffsets(element, children.concat(dropdownChildren), "Left")
        let firstChildOffset = element.Logo.X + element.Logo.Width
        //let marginBeforeChildren = element.Logo.MarginRight
        //let marginAfterChildren = element.Button.MarginRight
        //let widthForChildren = element.Button.X - firstChildOffset // marginstop, bodystop needed

        ///
        let widthForChildrenBodies = element.Button.X - firstChildOffset - element.Logo.MarginRight || 0 - element.Button.MarginLeft
        
        var layoutBlock = new LayoutBlock(false,
            new LayoutRegion(
                element.Logo.X + element.Logo.Width + (element.Logo.RightMargin || 0),
                0,
                element.Logo.MarginRight || 0,
                element.Button.MarginLeft || 0,
                element.MarginTop || 0,
                element.MarginBottom || 0))
        let mainMenuItemsLayout = layoutBlock.ReadChildren(element.MenuItems)
        let dropdownMenuItemsLayout = layoutBlock.ReadChildren(element.Dropdown.Children)
        let allMenuItemsLayout = mainMenuItemsLayout.concat(dropdownMenuItemsLayout)
        
        let line = layoutBlock.AddLine()
        let i = 0;
        while (i < allMenuItemsLayout.length && line.AddChild(allMenuItemsLayout[i], 0, widthForChildrenBodies)) {
            i++
        }
        
        if (i < mainMenuItemsLayout.length) { // need to move some items TO dropdown
            var itemsToTransfer = element.MenuItems.RemoveRange(i, mainMenuItemsLayout.length - i)
            element.Dropdown.Children.InsertRange(0, itemsToTransfer)
        } else if (i > mainMenuItemsLayout.length) { // need to move some items FROM dropdown
            var itemsToTransfer = element.Dropdown.Children.RemoveRange(0, i - mainMenuItemsLayout.length)
            element.MenuItems.InsertRange(mainMenuItemsLayout.length, itemsToTransfer)
        }

        line.AlignSecondary(0, false)
        layoutBlock.AlignLines(0)
        //layoutBlock.GrowLines()
        line.AlignPrimary(widthForChildrenBodies, 0, -1)
         // TODO: offset should be a property of layoutBlock if it is based on region, not element
        layoutBlock.WriteChildren(line.items)

        //let moveToDropdownOccured = false
        
        //for (let i = 0; i < children.length; i++) {
        //    if (suggestedOffsets[i] + children[i].Width + Max(children[i].MarginRight, marginAfterChildren, 0) < widthForChildren) {
        //        children[i].LayoutY = 0 // Margin/Padding?
        //        children[i].LayoutX = suggestedOffsets[i] + firstChildOffset +
        //            Max(marginBeforeChildren, children[0].MarginLeft, 0)
        //    } else {
        //        var itemsToTransfer = element.MenuItems.RemoveRange(i, children.length - i)                
        //        element.Dropdown.Children.InsertRange(0, itemsToTransfer)

        //        moveToDropdownOccured = true
        //        break
        //    }
        //}

        //if (!moveToDropdownOccured) {

        //    for (let i = 0; i < dropdownChildren.length; i++) {
        //        if (suggestedOffsets[children.length + i] + dropdownChildren[i].Width + Max(dropdownChildren[i].MarginRight, marginAfterChildren, 0) < widthForChildren) {
        //            let d = Reaction.beginDeferred()
        //            let itemToTransfer = element.Dropdown.Children.RemoveAt(i)
        //            element.MenuItems.Insert(element.MenuItems.ToArray().length, itemToTransfer)
        //            d.end()

        //            dropdownChildren[i].LayoutY = 0 // Margin/Padding?
        //            dropdownChildren[i].LayoutX = suggestedOffsets[children.length + i] + firstChildOffset +
        //                Max(marginBeforeChildren, children[0].MarginLeft, 0)
        //        } else {
        //            break;
        //        }
        //    }
        //}

        ///###

        //x += CalcOffset(element, children[children.length - 1], "Right")
        // TODO: Margings? Margin collapsing?
        //element.InternalWidth = element.Dropdown.Width + element.Logo.Width + suggestedOffsets[suggestedOffsets.length - 1]
    })


}
