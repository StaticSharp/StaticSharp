function ColumnInitialization(element) {
    BlockInitialization(element)

    element.Reactive = {
        ContentWidth: undefined,

        Width: () => Sum(element.ContentWidth, element.PaddingLeft, element.PaddingRight),

        //PaddingLeft: () => element.Height,

        ContentHeight: undefined,
        Height: () => element.ContentHeight,
    }    
}



function ColumnBefore(element) {
   

    BlockBefore(element)


    WidthToStyle(element)
    HeightToStyle(element)


    element.LayoutChildren = []

    element.AddChild = function (child) {
        element.LayoutChildren.push(child)
    }
}


function ColumnAfter(element) {
    BlockAfter(element)

    
    

    new Reaction(() => {

        let contentWidth = undefined
        for (let child of element.LayoutChildren) {
            Reaction.current.dirtImmune = true
            child.LayoutWidth = undefined
            Reaction.current.dirtImmune = false


            let spaceLeft = Max(element.PaddingLeft, child.MarginLeft, 0)
            let spaceRight = Max(element.PaddingRight, child.MarginRight, 0)
            contentWidth = Max(contentWidth,
                Sum(child.Width, spaceLeft + spaceRight, -element.PaddingLeft, -element.PaddingRight))

            //contentWidth = Max(contentWidth, child.Width)
            
        }

        element.ContentWidth = contentWidth

        for (let child of element.LayoutChildren) {

            let spaceLeft = Max(element.PaddingLeft, child.MarginLeft)
            let spaceRight = Max(element.PaddingRight, child.MarginRight)


            Reaction.current.dirtImmune = true
            child.LayoutWidth = element.Width - spaceLeft - spaceRight
            Reaction.current.dirtImmune = false


            child.LayoutX = Max(element.PaddingLeft, child.MarginLeft)

        }
    })
    

    new Reaction(() => {


        let previousMargin
        let freeSpaceUnits
        let freeSpacePixels
        let contentHeight

        function addElement(child, assignDimensions) {
            if (child.isSpace) {
                contentHeight += child.MinBetween
                if (assignDimensions) {
                    contentHeight += freeSpacePixels / freeSpaceUnits * child.GrowBetween
                } else {
                    freeSpaceUnits += child.GrowBetween
                }
                return true;
            }

            if (child.isBlock) {
                /*if (!child.Height) {
                    console.log("Column !child.Height", child)
                    return false

                }*/

                //console.log("child.Height", Max(child.Height, 0), child)


                let margin = Max(child.MarginTop, previousMargin)
                previousMargin = child.MarginBottom || 0


                contentHeight += margin
                if (assignDimensions) {
                    child.LayoutY = contentHeight
                }
                contentHeight += Max(child.Height, 0)


                return true
            }

            console.warn("Column: Unknown element type", child)
            return true
        }



        previousMargin = element.PaddingTop || 0
        freeSpaceUnits = 0
        contentHeight = 0


        for (let i of element.LayoutChildren) {
            if (!addElement(i, false)) {
                return
            }
        }

        //console.log("ColumnAfter Reaction", element, element.LayoutChildren)

        //Here "previousMargin" contains last-child.MarginBottom
        contentHeight += Max(previousMargin, element.PaddingBottom)
        //console.log("Vertical layout", element, contentHeight)

        previousMargin = element.PaddingTop || 0
        element.ContentHeight = contentHeight;
        if (!element.Height)
            return

        freeSpacePixels = element.Height - contentHeight;// Math.max( element.Height - contentHeight, 0)

        if (freeSpacePixels < 0) {
            element.style.overflowY = "scroll"
            freeSpacePixels = 0
            if (!element.innerSizeHolder) {
                element.innerSizeHolder = document.createElement('holder')
                element.innerSizeHolder.style.position = "absolute"
                element.appendChild(element.innerSizeHolder)
                element.innerSizeHolder.style.width = "1px"
            }
            element.innerSizeHolder.style.height = contentHeight+"px"
        } else {
            element.style.overflowY = ""
            if (element.innerSizeHolder) {
                element.removeChild(element.innerSizeHolder)
                element.innerSizeHolder = undefined
            }
        }

        //console.log("freeSpacePixels", freeSpacePixels)

        contentHeight = 0

        for (let i of element.LayoutChildren) {
            if (!addElement(i, true)) {
                return
            }
        }
    })

    //let w = element.LayoutChildren[0].Width
    

}