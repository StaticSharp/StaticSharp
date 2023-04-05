function MenuResponsive(element) {
    BlockWithChildren(element)
    element.isMenuResponsive = true

    CreateSocket(element, "Logo", element)
    CreateSocket(element, "Button", element)
    CreateSocket(element, "Dropdown", element)

    element.Reactive = {

        PrimaryGravity: 1,
        SecondaryGravity: 0,

        DropdownExpanded: false,

        Width: e => e.InternalWidth,
        Height: e => e.InternalHeight,

        InternalHeight: e => {
            var verticalNames = new LayoutPropertiesNames(true)

            let result = Sum(e["Padding" + verticalNames.side[0]], e["Padding" + verticalNames.side[1]])

            let mainMenuItems = e.Children.ToArray()
            let dropdownMenuItems = e.Dropdown.Children.ToArray()
            let allItemsToLayout = mainMenuItems.concat(dropdownMenuItems)

            if (e.Logo) {
                allItemsToLayout.push(e.Logo)
            }

            //for (let item of dropdownMenuItems) {
            //    item.Parent = e
            //}

            for (let child of allItemsToLayout) {
                let firstOffset = CalcOffset(e, child, verticalNames.side[0])
                let lastOffset = CalcOffset(e, child, verticalNames.side[1])

                let current = Sum(child.Layer[verticalNames.dimension], firstOffset + lastOffset)  // TODO: why Layer?

                result = Max(result, current)
            }

            //for (let item of dropdownMenuItems) {
            //    item.Parent = e.Dropdown
            //}
            
            return result
        },

        InternalWidth: e => {
            let region = LinearLayoutRegion.formContainer(e, false)

            let mainMenuItems = [...e.Children]
            let dropdownMenuItems = [...e.Dropdown.Children]
            let allMenuItems = mainMenuItems.concat(dropdownMenuItems)

            //for (let item of dropdownMenuItems) {
            //    item.Parent = e
            //}

            if (e.Logo) {
                region.border[0].Shift(e.Logo)
            }

            for (let child of allMenuItems) {
                region.border[0].Shift(child)
            }

            //for (let item of dropdownMenuItems) {
            //    item.Parent = e.Dropdown
            //}
            
            return region.GetSize()
        },
    }


    let baseHtmlNodesOrdered = element.HtmlNodesOrdered
    element.HtmlNodesOrdered = new Enumerable(function* () {
        let logo = element.Logo
        if (logo && logo.Exists)
            yield logo

        let button = element.Button
        if (button.Exists)
            yield button

        let dropdown = element.Dropdown
        if (dropdown.Exists)
            yield dropdown

        yield* baseHtmlNodesOrdered
    })


    let layoutHorizontallyHelper = function (allMenuItems, doLayoutButton) {
        let menuItemsPositions = []

        let region = LinearLayoutRegion.formContainer(element, false)

        if (doLayoutButton) {
            let buttonOppositeOffset = region.border[1].Shift(element.Button)
            let buttonPosition = element.Width - element.Button.Layer.Width - buttonOppositeOffset
            element.Button.Layer.X = buttonPosition
            element.Dropdown.Layer.X = buttonPosition + element.Button.Layer.Width - element.Dropdown.Layer.Width // TODO: maybe move to CSharp?
        }

        if (element.Logo) {
            element.Logo.Layer.X = region.border[0].Shift(element.Logo)
        }

        let extraPixels = element.Width - region.GetSize()
        for (const [i, item] of allMenuItems.entries()) {

            let position = region.border[0].Shift(item)
            if (region.GetSize() > element.Width) {
                break
            }

            extraPixels = element.Width - region.GetSize()
            menuItemsPositions[i] = position
        }

        return { menuItemsPositions, extraPixels }
    }


    // Horizontal placement and reparenting
    new Reaction(() => {    
        let mainMenuItems = [...element.Children]
        let dropdownMenuItems = [...element.Dropdown.Children]
        let allMenuItems = mainMenuItems.concat(dropdownMenuItems)

        //for (let item of dropdownMenuItems) {
        //    item.Parent = element
        //}

        let { menuItemsPositions, extraPixels } = layoutHorizontallyHelper(allMenuItems, false)

        if (menuItemsPositions.length < allMenuItems.length) {
            ({ menuItemsPositions, extraPixels } = layoutHorizontallyHelper(allMenuItems, true))
        }

        //for (let item of dropdownMenuItems) {
        //    item.Parent = element.Dropdown
        //}

        if (menuItemsPositions.length < mainMenuItems.length) { // need to move some items TO dropdown
            let itemsToTransfer = element.Children.RemoveRange(menuItemsPositions.length, mainMenuItems.length - menuItemsPositions.length)
            element.Dropdown.Children.InsertRange(0, itemsToTransfer)
        } else if (menuItemsPositions.length > mainMenuItems.length) { // need to move some items FROM dropdown
            let itemsToTransfer = element.Dropdown.Children.RemoveRange(0, menuItemsPositions.length - mainMenuItems.length)
            element.Children.InsertRange(mainMenuItems.length, itemsToTransfer)
        }

        let primaryGravityShift = extraPixels * (0.5 * element.PrimaryGravity + 0.5)
        for (const [i, menuItemPosition] of menuItemsPositions.entries()) {
            allMenuItems[i].Layer.X = menuItemPosition + primaryGravityShift
        }

        if (!element.Dropdown.Children.Any()) {
            element.DropdownExpanded = false
        }
    })


    let getYForChild = function (childElement) {
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

        for (let menuItem of [...element.Children]) {
            menuItem.Layer.Y = getYForChild(menuItem)
        }
        
    })

    
    element.AfterChildren = () => {
        element.Button.Events.Click = () => {
            element.DropdownExpanded = !element.DropdownExpanded && element.Dropdown.Children.Any()
        }
    }
}
