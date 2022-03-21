

function RowBefore(element) {

    let parent = element.parentElement;

    element.horizontalLayout = true

    /*element.style.display = "flex"
    element.style.flexDirection = "row";

    element.style.flexWrap = "wrap";
    element.style.justifyContent = "space-between";*/


    element.Reactive = {
        Padding: new Border(),
        Width: () => parent.Width,
        //Height: undefined,
        //InnerWidth: () => parent.InnerWidth || element.Width,
        //PaddingLeft: () => parent.PaddingLeft || 0
    }

    element.Padding.Left = () => (parent.Padding && parent.Padding.Left) || 0
    element.Padding.Right = () => (parent.Padding && parent.Padding.Right) || 0

    element.Padding.Left = 100
    element.Padding.Right = 100

    new Reaction(() => {
        if (element.Width)
            element.style.width = element.Width + "px"
        else
            element.style.width = undefined

    })

    parent[element.id] = element

}


function RowAfter(element) {

    element.style.height = "200px"
    new Reaction(() => {
        let width = element.Width
        let internalWidth = element.Width - element.Padding.Left - element.Padding.Right


        let children = element.children

        for (let current of children) {
            current.style.position = "absolute"
        }


        let height = 0
        
        let containerSpaceLeft = element.Padding.Left
        let containerSpaceRight = element.Padding.Right
        let top = 0;
        let i = 0
        while (i < children.length) {

            let previousSpaceLeft = containerSpaceLeft
            let rowHeight = 0
            let rowWidth = 0

            
            let j = 0

            let row = []
            let rowSpacers = 0
            let accumulatedSpaces = 0
            while (i < children.length) {
                let current = children[i]
                
                let spacer = current.getAttribute("Spacer")
                spacer = spacer && parseFloat(spacer)

                if (spacer != undefined) {
                    
                    accumulatedSpaces += spacer
                } else {                    

                    let descriptor = {
                        width: current.offsetWidth,
                        left: 0,
                        right: 0,
                        element: current
                    }


                    let currentWidth = current.offsetWidth
                    if (current.Margin) {
                        descriptor.left = current.Margin.Left
                        descriptor.right = current.Margin.Right
                    }

                    let totalFreeSpaceRequired =
                        Math.max(previousSpaceLeft, descriptor.left)
                        + currentWidth
                        + Math.max(containerSpaceRight, descriptor.right)


                    if (width < (rowWidth + totalFreeSpaceRequired)) {
                        if (j > 0)
                            break;
                    }

                    if (accumulatedSpaces > 0) {
                        row.push(accumulatedSpaces)
                        rowSpacers += accumulatedSpaces
                        accumulatedSpaces = 0
                    }

                    row.push(descriptor)


                    current.style.top = top + "px"


                    rowWidth += Math.max(previousSpaceLeft, descriptor.left) + currentWidth

                    rowHeight = Math.max(rowHeight, current.offsetHeight)

                    previousSpaceLeft = descriptor.right

                    j++
                }                
                i++                
            }

            let spacersWidth = width - (rowWidth + Math.max(previousSpaceLeft, containerSpaceRight))
            let pixelsPerSpacer = (rowSpacers > 0) ? spacersWidth / rowSpacers : 0


            //console.log("rowWidth", rowWidth, "spacersWidth", spacersWidth, "rowSpacers", rowSpacers, "pixelsPerSpacer", pixelsPerSpacer)

            previousSpaceLeft = containerSpaceLeft
            rowWidth = 0
            let currentSpacerSize = 0


            for (let current of row) {
                
                if (typeof current == "number") {
                    currentSpacerSize += current * pixelsPerSpacer
                } else {

                    let spaceLeft = Math.max(previousSpaceLeft, current.left) + currentSpacerSize

                    current.element.style.left = rowWidth + spaceLeft + "px"
                    rowWidth += spaceLeft + current.width
                    previousSpaceLeft = current.right
                    currentSpacerSize = 0
                }
            }

            top += rowHeight
        }

        

        

        element.style.height = top+"px"

    })

    //let flexChildren = document.querySelectorAll('.content');
    //let leftPosition = flexChildren[0].offsetLeft;
    

}