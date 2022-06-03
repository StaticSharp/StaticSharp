



function ColumnInitialization(element) {


    BlockInitialization(element)

    element.Reactive = {
        ContentWidth: undefined,

        Width: () => Sum(element.ContentWidth, element.PaddingLeft, element.PaddingRight),



        ContentHeight: undefined,
        Height: () => element.ContentHeight,

        Test: () => element.Test2,
        Test1: () => undefined,
        Test2: () => Try(() => {
            console.log("Test1", element.Test1)
            if (element.Test1) return element.Test1
            else return element.Test
            },8)

    }


    Object.assign(element, {
        stretchChildren: true
    })

    //element.stretchChildren = false

    
}

function ColumnBefore(element) {
    BlockBefore(element)

    /*console.log(element.Test2)
    console.log("before asign")
    element.Test1 = 5
    console.log("after asign")
    console.log(element.Test2)*/



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
                if (!Try(() => element.Width)) {
                    console.log("element.Width not available", element)
                    return undefined
                } else {
                    console.log("element.Width available", element)
                }
                    

                return Try(() => {
                    var spaceLeft = Max(element.PaddingLeft, child.MarginLeft) || 0
                    var spaceRight = Max(element.PaddingRight, child.MarginRight) || 0
                    var availableWidth = element.Width - spaceLeft - spaceRight
                    return availableWidth
                },undefined)
                
            }
        }
    }



}


function ColumnAfter(element) {
    BlockAfter(element)


    element.onclick = function () {
        console.log("Test",element.Test)
        console.log("Test2",element.Test2)
    };



    /*for (let i of element.LayoutChildren) {
        if (i.isBlock)
            console.log(i.Reactive.Width, i.Reactive.Width.getRecursiveDependencies())
        
    }*/
    

    element.ContentWidth = () => {
        let result = undefined
        for (let child of element.LayoutChildren) {
            if (child.isBlock) {
                Try(() => {
                    var spaceLeft = Max(element.PaddingLeft, child.MarginLeft) || 0
                    var spaceRight = Max(element.PaddingRight, child.MarginRight) || 0
                    console.log("child.Width", child.Width, child)
                    result = Max(result, Sum(child.Width, + spaceLeft + spaceRight))
                })

                
            }                //console.log(i.Reactive.Width, i.Reactive.Width.getRecursiveDependencies())

        }
        return result
    }

    /*new Reaction(() => {
        console.log(element.ContentWidth)
    })*/


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