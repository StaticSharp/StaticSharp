
function ColumnInitialization(element) {

    BlockInitialization(element)

    element.Reactive = {
        ContentWidth: undefined,
        ContentHeight: undefined,
        Height: () => element.ContentHeight,
    }


    Object.assign(element, {
        stretchChildren: false
    })

    //element.stretchChildren = false

    
}

function ColumnBefore(element) {
    BlockBefore(element)

    



    WidthToStyle(element)
    HeightToStyle(element)


    element.LayoutChildren = []
    


    element.AddChild = function (child) {
        //console.log("AddChild", element, child)
        element.LayoutChildren.push(child)

        child.Reactive.LayoutX = () => {
            return Max(element.PaddingLeft, child.MarginLeft) || 0
        }

        if (element.stretchChildren) {
            child.Reactive.LayoutWidth = () => {
                var paddingLeft = Max(element.PaddingLeft, child.MarginLeft) || 0
                var paddingRight = Max(element.PaddingRight, child.MarginRight) || 0
                var availableWidth = element.Width - paddingLeft - paddingRight
                return availableWidth
            }
        }
    }



}


function ColumnAfter(element) {
    BlockAfter(element)

    
    /*for (let i of element.LayoutChildren) {
        if (i.isBlock)
            console.log(i.Reactive.Width, i.Reactive.Width.getRecursiveDependencies())
        
    }*/
    

    element.ContentWidth = () => {
        let result = undefined
        for (let i of element.LayoutChildren) {
            if (i.isBlock) {
                
                if (!i.Reactive.Width.dependsOn(element.Reactive.ContentWidth)) {                    
                    result = Max(result, i.Width)
                }
            }
                //console.log(i.Reactive.Width, i.Reactive.Width.getRecursiveDependencies())

        }
        return result
    }

    new Reaction(() => {
        console.log(element.ContentWidth)
    })


    let previousMarginTop
    let freeSpaceUnits
    let freeSpacePixels
    let contentHeight

    function addElement(child,assignDimensions) {
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
            if (!child.Height) return false

            let margin = Max(child.MarginTop, previousMarginTop)
            previousMarginTop = child.MarginBottom || 0
            contentHeight += margin
            if (assignDimensions) {
                child.LayoutY = contentHeight
            }
            contentHeight += child.Height
            return true
        }
    }



    new Reaction(() => {

        previousMarginTop = element.PaddingTop || 0
        freeSpaceUnits = 0
        contentHeight = 0

        for (let i of element.LayoutChildren) {
            if (!addElement(i, false)) {
                return
            }
        }

        previousMarginTop = element.PaddingTop || 0
        element.ContentHeight = contentHeight;
        if (!element.Height)
            return

        freeSpacePixels = element.Height - contentHeight
        contentHeight = 0

        for (let i of element.LayoutChildren) {
            if (!addElement(i, true)) {
                return
            }
        }
    })

    //let w = element.LayoutChildren[0].Width
    

}