
function RowMeasurer(startMargin) {
    let _this = this
    _this.margin = startMargin
    _this.currentX = 0

    _this.add = function (child) {
        let spaceLeft = First(Max(_this.margin, child.MarginLeft), 0)
        _this.currentX += spaceLeft + child.Width
        _this.margin = child.MarginRight
    }

    _this.finish = function (endMargin) {
        let spaceRight = First(Max(endMargin, _this.margin), 0)
        _this.currentX += spaceRight
    }
}

function RowBuilder(element) {
    let _this = this

    let startMargin = First(element.PaddingLeft, 0)
    let endMargin = First(element.PaddingRight, 0)
    let maxWidth = element.Width

    _this.margin = startMargin;
    _this.currentX = 0

    _this.currentLineHardY = 0
    _this.currentLineSoftY = element.PaddingTop
    _this.currentY = 0


    //_this.newLineHardY = 0
    //_this.newLineSoftY = 0

    _this.lineIsEmpty = true

    _this.currentLine = []

    _this.finalizeCurrentLine = function () {

        let margin = startMargin 
        let x = 0
        let newLineHardY = 0
        let newLineSoftY = 0

        let availableSpace = maxWidth - _this.currentX - Max(_this.margin, element.PaddingRight)
        
        let spaceUnits = 0
        for (let i = 0; i < _this.currentLine.length; i++) {
            let child = _this.currentLine[i]
            if (child.isSpace) {
                switch (i) {
                    case 0:
                        spaceUnits += child.Before
                        break;
                    case _this.currentLine.length-1:
                        spaceUnits += child.After
                        break;
                    default:
                        spaceUnits += child.Between
                }

            }
        }

        let pixelBySpaceUnits = spaceUnits>0 ?  availableSpace / spaceUnits : 0

        for (let i = 0; i < _this.currentLine.length; i++){

            let child = _this.currentLine[i]

            if (child.isBlock) {
                let spaceLeft = First(Max(margin, child.MarginLeft), 0)

                var yA = Sum(_this.currentLineHardY, child.MarginTop)
                var yB = _this.currentLineSoftY
                var y = Max(yA, yB)

                x += spaceLeft
                child.LayoutX = x
                x += child.Width
                child.LayoutY = y
                margin = child.MarginRight

                var hardBottom = y + child.Height

                newLineHardY = Max(newLineHardY, hardBottom)
                newLineSoftY = Max(newLineSoftY, Sum(hardBottom, child.MarginBottom))

                //continue
            }

            if (child.isSpace) {
                switch (i) {
                    case 0:
                        x += child.Before * pixelBySpaceUnits
                        break;
                    case _this.currentLine.length - 1:
                        x += child.After * pixelBySpaceUnits
                        break;
                    default:
                        x += child.Between * pixelBySpaceUnits
                }
            }
        }

        let lastSpace = undefined
        if (_this.currentLine.length > 0) {
            let last = _this.currentLine[_this.currentLine.length - 1]
            if (last.isSpace) {
                lastSpace = last
            }
        }
        _this.currentLine = []
        if (lastSpace) {
            _this.currentLine.push(lastSpace)
        }


        _this.lineIsEmpty = true
        _this.margin = startMargin
        _this.currentX = 0
        _this.currentLineHardY = newLineHardY;
        _this.currentLineSoftY = newLineSoftY;

    }



    _this.layout = function (child) {
        let spaceLeft = First(Max(_this.margin, child.MarginLeft), 0)
        let spaceRight = Max(endMargin, child.MarginRight)

        if (child.isBlock) {

            if ((_this.currentX + spaceLeft + child.InternalWidth + spaceRight) > maxWidth) {
                if (_this.currentLine.length > 0) {
                    _this.finalizeCurrentLine()
                    spaceLeft = First(Max(_this.margin, child.MarginLeft), 0)
                }

                if ((spaceLeft + child.InternalWidth + spaceRight) > maxWidth) {
                    child.LayoutWidth = maxWidth - spaceLeft - spaceRight
                } else {
                    child.LayoutWidth = undefined
                }
            } else {
                child.LayoutWidth = undefined
            }
            _this.currentX += spaceLeft + child.Width
            _this.margin = child.MarginRight

            //_this.currentLine.push(child)

        } else {

            //_this.currentLineSpaceUnits = 0
        }
        _this.currentLine.push(child)
    }
    




}

function Row(element) {
    Block(element)

    element.isRow = true

    element.Reactive = {

        InternalWidth: () => {
            let measurer = new RowMeasurer(
                First(element.PaddingLeft, 0)
            );

            for (let child of element.children) {
                measurer.add(child)
            }
            measurer.finish();
            return measurer.currentX
        },

        //Width: () => element.InternalWidth,

        ContentWidth: undefined,
        ContentHeight: undefined,

        /*Width: () => First(
            element.LayoutWidth,
            Sum(
                element.ContentWidth,
                element.PaddingLeft,
                element.PaddingRight
            )
        ),*/
        //Height: 150//() => Sum(element.ContentHeight, element.PaddingTop, element.PaddingButtom),
    }

    new Reaction(() => {

        /*let contentWidth = undefined

        let line = []
        let lineContentWidth = 0
        let lineGrowUnits = 0
        let previousMargin = First(element.PaddingLeft,0)*/

        let builder = new RowBuilder(element);

        for (let child of element.children) {
            builder.layout(child)

        }
        builder.finalizeCurrentLine()

        var hA = Sum(builder.currentLineHardY, element.PaddingBottom)
        var hB = builder.currentLineSoftY

        element.Height = Max(hA, hB)


    })

    WidthToStyle(element)
    HeightToStyle(element)
}
