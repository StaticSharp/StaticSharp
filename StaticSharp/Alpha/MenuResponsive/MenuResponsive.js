function MenuResponsive(element) {
    Block(element)
    element.isMenuResponsive = true

    CreateSocket(element, "Logo", element)
    CreateSocket(element, "Button", element)
    CreateSocket(element, "Dropdown", element)

    CreateCollectionSocket(element, "MenuItems", element)

    element.Reactive = {

        SecondaryGravity: 1,

        Width: e => e.InternalWidth,
        Height: e => e.InternalHeight,

        InternalHeight: e => {
            var verticalNames = new LayoutPropertiesNames(true)

            let result = Sum(e["Padding" + verticalNames.side[0]], e["Padding" + verticalNames.side[1]])

            let mainMenuItems = e.MenuItems.ToArray()
            let dropdownMenuItems = e.Dropdown.Children.ToArray()
            let allItemsToLayout = mainMenuItems.concat(dropdownMenuItems)
            allItemsToLayout.push(e.Button)

            if (e.Logo) {
                allItemsToLayout.push(e.Logo)
            }

            for (let item of dropdownMenuItems) {
                item.Parent = e
            }

            for (let child of allItemsToLayout) {
                let firstOffset = CalcOffset(e, child, verticalNames.side[0])
                let lastOffset = CalcOffset(e, child, verticalNames.side[1])

                let current = Sum(child.Layer[verticalNames.dimension], firstOffset + lastOffset)  // TODO: why Layer?

                result = Max(result, current)
            }

            for (let item of dropdownMenuItems) {
                item.Parent = e.Dropdown
            }
            
            return result
        },

        InternalWidth: e => {
            let region = LinearLayoutRegion.formContainer(e, false)
            let gap = 0 // TODO: add property?

            let mainMenuItems = e.MenuItems.ToArray()
            let dropdownMenuItems = e.Dropdown.Children.ToArray()
            let allMenuItems = mainMenuItems.concat(dropdownMenuItems)

            for (let item of dropdownMenuItems) {
                item.Parent = e
            }

            if (e.Logo) {
                region.border[0].Shift(e.Logo)
            }
            region.border[1].Shift(e.Button)

            for (const [i, child] of allMenuItems.entries()) {
                if (i > 0) {
                    region.border[0].ShiftByPixels(gap)
                }
                region.border[0].Shift(child)
            }

            for (let item of dropdownMenuItems) {
                item.Parent = e.Dropdown
            }
            
            return region.GetSize()
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


    new Reaction(() => {    
        let region = LinearLayoutRegion.formContainer(element, false)
        let gap = 0 // TODO: add property?

        let mainMenuItems = element.MenuItems.ToArray()
        let dropdownMenuItems = element.Dropdown.Children.ToArray()
        let allMenuItems = mainMenuItems.concat(dropdownMenuItems)

        //for (let item of dropdownMenuItems) {
        //    item.Parent = element
        //}

        if (element.Logo) {
            element.Logo.Layer.X = region.border[0].Shift(element.Logo)
        }
        
        let buttonOppositeOffset = region.border[1].Shift(element.Button)        
        let buttonPosition = element.Width - element.Button.Layer.Width - buttonOppositeOffset
        element.Button.Layer.X = buttonPosition


        let menuItemsPositions = []
        for (const [i, item] of allMenuItems.entries()) {
            if (i > 0) {
                region.border[0].ShiftByPixels(gap)
            }

            let position = region.border[0].Shift(item)

            if (region.GetSize() > element.Width) {
                break
            }

            menuItemsPositions[i] = position
        }

        //for (let item of dropdownMenuItems) {
        //    item.Parent = element.Dropdown
        //}

        if (menuItemsPositions.length < mainMenuItems.length) { // need to move some items TO dropdown
            let itemsToTransfer = element.MenuItems.RemoveRange(menuItemsPositions.length, mainMenuItems.length - menuItemsPositions.length)
            element.Dropdown.Children.InsertRange(0, itemsToTransfer)
        } else if (menuItemsPositions.length > mainMenuItems.length) { // need to move some items FROM dropdown
            let itemsToTransfer = element.Dropdown.Children.RemoveRange(0, menuItemsPositions.length - mainMenuItems.length)
            element.MenuItems.InsertRange(mainMenuItems.length, itemsToTransfer)
        }


        // Placing

        

        for (const [i, menuItem] of [...element.MenuItems].entries()) {
            menuItem.Layer.X = menuItemsPositions[i]
        }

        // Dropdown area
        rightOffset = CalcOffset(element, element.Dropdown, "Right")
        element.Dropdown.Layer.X = buttonPosition + element.Button.Layer.Width - element.Dropdown.Layer.Width
    })

    function getYForChild(childElement) {
        let region = LinearLayoutRegion.formContainer(element, true)
        let offsetGravityUp = region.border[0].Shift(childElement)

        region = LinearLayoutRegion.formContainer(element, true)
        let offsetFromBottom = region.border[1].Shift(childElement)
        let offsetGravityDown = element.Height - childElement.Height - offsetFromBottom

        return ((element.SecondaryGravity + 1) * offsetGravityDown + (1 - element.SecondaryGravity) * offsetGravityUp ) / 2
    }

    // Vertical placement
    new Reaction(() => {
        if (element.Logo) {
            element.Logo.Layer.Y = getYForChild(element.Logo)
        }

        let buttonY = getYForChild(element.Button)
        element.Button.Layer.Y = buttonY
        element.Dropdown.Layer.Y = buttonY + element.Button.Layer.Height

        for (let menuItem of [...element.MenuItems]) {
            menuItem.Layer.Y = getYForChild(menuItem)
        }
        
    })
    
}
