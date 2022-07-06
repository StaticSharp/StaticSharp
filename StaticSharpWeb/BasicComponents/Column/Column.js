



function ColumnInitialization(element) {


    BlockInitialization(element)

    element.Reactive = {
        ContentWidth: undefined,

        Width: () => Sum(element.ContentWidth, element.PaddingLeft, element.PaddingRight),



        ContentHeight: undefined,
        Height: () => element.ContentHeight,

        A: () => {
            console.log("element.B", element.B)
            console.log("element.C", element.C)

            return Min(element.B, element.C)
        },
        B: () => element.A,
        C: () => 8

    }


    Object.assign(element, {
        stretchChildren: true
    })

    //element.stretchChildren = false

    
}

/*function Unknown() {
    this.a = 5
}
Unknown.prototype.valueOf = function () {
    return undefined;
};

window.unknown = Symbol("unknown")*/


function ColumnBefore(element) {



    console.log(Math.min(null,2))

    /*if (unknown) {
        console.log("unknown is truthy")
    }
    console.log("!!(must be false)", unknown)

    //console.log("isNaN(must be true)", isNaN(unknown))
    console.log("!!(must be false)", !!unknown)
    console.log("+(must be false)", +unknown)


    console.log("B:", element.B)
    console.log("A:", element.A)*/
    

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

        element.LayoutChildren.push(child)

        child.Reactive.LayoutX = () => {
            return Max(element.PaddingLeft, child.MarginLeft) || 0
        }

        if (element.stretchChildren) {
            child.Reactive.LayoutWidth = () => {
                //console.log("child.LayoutWidth", child.Reactive.LayoutWidth.binding.dirty)
                let width = element.Width

                if (width === null) return null
                if (width === undefined) return undefined
                /*if (!element.Width) {
                    return undefined
                }*/

                
                

                var spaceLeft = Max(element.PaddingLeft, child.MarginLeft)
                var spaceRight = Max(element.PaddingRight, child.MarginRight)
                var availableWidth = Sum(element.Width, -spaceLeft, -spaceRight)
                return availableWidth


                
            }
        }
    }



}


function ColumnAfter(element) {
    BlockAfter(element)


    /*element.onclick = function () {
        console.log("Test",element.Test)
        console.log("Test2",element.Test2)
    };*/



    /*for (let i of element.LayoutChildren) {
        if (i.isBlock)
            console.log(i.Reactive.Width, i.Reactive.Width.getRecursiveDependencies())
        
    }*/
    

    element.ContentWidth = () => {
        let result = undefined

        for (let child of element.LayoutChildren) {
            if (child.isBlock) {
                //Try(() => {
                

                    var spaceLeft = Max(element.PaddingLeft, child.MarginLeft) || 0
                    var spaceRight = Max(element.PaddingRight, child.MarginRight) || 0
                    result = Max(result, Sum(child.Width, + spaceLeft + spaceRight))
                //})

                
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