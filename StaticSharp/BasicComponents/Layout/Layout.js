




function Layout(element) {
    Block(element)


    element.Reactive = {
        Vertical: false,
        PrimaryGap: 0,
        PrimaryGapGrow: 0,
        PrimaryGravity: -1,

        IntralinearGravity: -1,
        SecondaryGap: 0,
        SecondaryGapGrow: 0,
        FillSecondary: true,

        Multiline: false

    }


    new Reaction(() => {

        let layoutBlock = new LayoutBlock(element.Vertical, element)

        let children = layoutBlock.ReadChildren(element.Children)

        let primaryGap = element.PrimaryGap

        let line = layoutBlock.AddLine()
        for (let i of children) {
            line.AddChild(i, primaryGap)
        }
        let lineSize = line.GetLineSize()        
        element["Internal" + layoutBlock.primaryDimension] = lineSize

        let sizeLimit = element[layoutBlock.primaryDimension]

        if (lineSize > sizeLimit) {
            layoutBlock.lines = []
            line = undefined
            
            for (let i of children) {
                if (!line || !line.AddChild(i, primaryGap, sizeLimit)) {
                    line = layoutBlock.AddLine()
                    line.AddChild(i, primaryGap)
                }
            }
            element.Multiline = true
        } else {
            element.Multiline = false
        }

        for (let i of layoutBlock.lines) {
            i.AlignSecondary(element.IntralinearGravity, element.FillSecondary)
        }

        layoutBlock.AlignLines(element.SecondaryGap)

        let dependOnSecondarySize = element.FillSecondary || element.SecondaryGapGrow > 0

        let finalSecondarySizePrevious = 0
        if (dependOnSecondarySize) {
            //Read secondaryDimension
            finalSecondarySizePrevious = element[layoutBlock.secondaryDimension]
        }

        let secondarySize = layoutBlock.GetSecondarySize()
        element["Internal" + layoutBlock.secondaryDimension] = secondarySize

        if (dependOnSecondarySize) {
            //Read secondaryDimension            
            let finalSecondarySize = element[layoutBlock.secondaryDimension]
            if (finalSecondarySizePrevious != finalSecondarySize)
                return
            let extraPixels = finalSecondarySize - secondarySize
            //if (extraPixels > 0) {
            layoutBlock.GrowLines(extraPixels, element.SecondaryGapGrow)
            //}
        }
        
        for (let i of layoutBlock.lines) {
            i.AlignPrimary(sizeLimit, element.PrimaryGapGrow, element.PrimaryGravity)
        }

        layoutBlock.WriteChildren(children)

    })


}