



function ColumnInitialization(element) {


    BlockInitialization(element)

    element.Reactive = {
        ContentWidth: undefined,

        Width: () => Sum(element.ContentWidth, element.PaddingLeft, element.PaddingRight),



        ContentHeight: undefined,
        Height: () => element.ContentHeight,



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

            let spaceLeft = Max(element.PaddingLeft, child.MarginLeft, 0)
            let spaceRight = Max(element.PaddingRight, child.MarginRight, 0)


            Reaction.current.dirtImmune = true
            child.LayoutWidth = element.Width - spaceLeft - spaceRight
            Reaction.current.dirtImmune = false


            child.LayoutX = Max(element.PaddingLeft, child.MarginLeft, 0)

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
                if (!child.Height) return false

                let margin = Max(child.MarginTop, previousMargin)
                previousMargin = child.MarginBottom || 0
                contentHeight += margin
                if (assignDimensions) {
                    child.LayoutY = contentHeight
                }
                contentHeight += child.Height
                return true
            }
        }


        previousMargin = element.PaddingTop || 0
        freeSpaceUnits = 0
        contentHeight = 0
        for (let i of element.LayoutChildren) {
            if (!addElement(i, false)) {
                return
            }
        }
        //Here "previousMargin" contains last-child.MarginBottom
        contentHeight += previousMargin


        previousMargin = element.PaddingTop || 0
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