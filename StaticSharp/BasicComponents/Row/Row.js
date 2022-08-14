
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
    

    _this.try = function (child) {
        //console.log("try._this.margin", _this.margin)
        let spaceLeft = First(Max(_this.margin, child.MarginLeft),0)
        return {
            newPosition: _this.currentX + spaceLeft + child.Width,
            newMargin: child.MarginRight
        }
    }

    _this.add = function (resultOfTry) {
        _this.currentX = resultOfTry.newPosition
        _this.margin = resultOfTry.newMargin
    }


}

function RowInitialization(element) {
    BlockInitialization(element)

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


}

function RowBefore(element, parameters) {
    BlockBefore(element)
    element.isRow = true

    WidthToStyle(element)
    HeightToStyle(element)

    /*element.LayoutChildren = []

    element.AddChild = function (child) {
        element.LayoutChildren.push(child)
    }*/

}





function RowAfter(element) {
    BlockAfter(element)


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



    return


    //element.style.height = "200px"
    let fontData = GetFontData(element)
/*
    new Reaction(() => {

        //console.log("rebuild ", element)

        let width = element.Width
        let internalWidth = element.Width - element.Padding.Left - element.Padding.Right


        let nodes = element.childNodes

        let y = 0

        const fontStack = [
            {
                ascent:10,
                descent:2,
                spaceWidth:0,
            }
        ]



        function initializeLine() {
            return {
                x: 0.0,
                ascent: 0,
                descent: 0,
                nodes: [],
                space: 0,
            }
        }
        let currentLine = initializeLine();

        var spaceWidth = 0

        //var range = document.createRange();

        //var canvasFontSize = getCanvasFontSize(element)
        //var context = getCanvasContext(canvasFontSize)


        function getWordWidth(node) {
            //console.log(node.innerText)
            //return getTextWidth(node.innerText, context)

            if (node.wordWidth == undefined) {
                let attribute = node.getAttribute("w")
                if (attribute == undefined) {
                    console.error("getWordWidth", node, "attribute w not found")
                    return 0
                } else {
                    node.wordWidth = parseFloat(attribute);
                }
            }
            return node.wordWidth
        }

        
        //range.selectNodeContents(textNode);

        function processLine() {
            let x = 0.0
            
            //console.log(currentLine.items)
            
            for (let n of currentLine.nodes) {

                if (n.tagName == WordTagName) {
                    //child.removeAttribute("x")
                    //child.removeAttribute("y")


                    
                    n.style.left = x.toFixed(2) + "px"
                    n.style.top = y + "px"
                    
                    //child.setAttribute("x", x+"px" );
                    //child.setAttribute("y", y + "px");

                    x += getWordWidth(n) + spaceWidth
                }
            }

            //fixme:
            y += currentLine.ascent
            y += currentLine.descent
            //document.title = x;
            currentLine = initializeLine()

        }

        function processNodes(nodes) {
            for (let n of nodes) {
                
                if (n.nodeType == 1) {

                    
                    if (n.tagName == WordTagName) {
                        
                        let w = getWordWidth(n)
                        if ((currentLine.x + w) > width) {                            
                            processLine()
                        }
                        let font = fontStack[fontStack.length - 1]
                        currentLine.ascent = Math.max(currentLine.ascent, font.ascent)
                        currentLine.descent = Math.max(currentLine.descent, font.descent)
                        

                        currentLine.x += w

                        currentLine.nodes.push(n)
                        continue
                    }
                } else if (n.nodeType == 3) {
                    let text = n.textContent
                    currentLine.x += spaceWidth * text.length

                }
            }
        }
        
        processNodes(nodes)
        processLine()
        
        element.Height = y


        

        

        element.style.height = top+"px"

    })
*/
    //let flexChildren = document.querySelectorAll('.content');
    //let leftPosition = flexChildren[0].offsetLeft;
    

}