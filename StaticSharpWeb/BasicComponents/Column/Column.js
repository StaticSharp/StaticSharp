
function ColumnInitialization(element) {

    BlockInitialization(element)

    element.Reactive = {
        ContentHeight: undefined,
        Height: () => element.ContentHeight,
    }

    
}

function ColumnBefore(element) {
    BlockBefore(element)

    WidthToStyle(element)
    HeightToStyle(element)

    element.Children = []

    element.AddChild = function (child) {
        //console.log("AddChild", element, child)
        element.Children.push(child)

        child.Reactive.LayoutX = () => {
            return Max(element.PaddingLeft, child.MarginLeft) || 0
        }

        child.Reactive.LayoutWidth = () => {
            var paddingLeft = Max(element.PaddingLeft, child.MarginLeft) || 0
            var paddingRight = Max(element.PaddingRight, child.MarginRight) || 0
            var availableWidth = element.Width - paddingLeft - paddingRight
            return availableWidth
        }
    }



}


function ColumnAfter(element) {
    BlockAfter(element)

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

        for (let i of element.Children) {
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

        for (let i of element.Children) {
            if (!addElement(i, true)) {
                return
            }
        }
    })
}