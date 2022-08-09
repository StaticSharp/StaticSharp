
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

    _this.lineHeight = 0

    _this.newLineHardY = 0
    _this.newLineSoftY = 0


    let newLine = function () {
        _this.lineIsEmpty = true
        _this.margin = startMargin
        _this.currentX = 0
        _this.currentLineHardY = _this.newLineHardY;
        _this.currentLineSoftY = _this.newLineSoftY;
        _this.lineHeight = 0
    }

    _this.layout = function (child) {
        let spaceLeft = First(Max(_this.margin, child.MarginLeft), 0)
        let spaceRight = Max(endMargin, child.MarginRight)

        //console.log("spaceLeft", spaceLeft)


        if ((_this.currentX + spaceLeft + child.InternalWidth + spaceRight) > maxWidth) {
            newLine()
            spaceLeft = First(Max(_this.margin, child.MarginLeft), 0)

            if ((spaceLeft + child.InternalWidth + spaceRight) > maxWidth) {
                child.LayoutWidth = maxWidth - spaceLeft - spaceRight
            } else {
                child.LayoutWidth = undefined
            }
        } else {
            child.LayoutWidth = undefined
        }

        var yA = Sum(_this.currentLineHardY, child.MarginTop)
        var yB = _this.currentLineSoftY

        var y = Max(yA, yB)


        _this.lineHeight = Max(_this.lineHeight, child.Height)
        _this.currentX += spaceLeft

        child.LayoutX = _this.currentX


        _this.currentX += child.Width

        child.LayoutY = y

        _this.margin = child.MarginRight

        var hardBottom = y + child.Height

        _this.newLineHardY = Max(_this.newLineHardY, hardBottom)
        _this.newLineSoftY = Max(_this.newLineSoftY, Sum(hardBottom, child.MarginBottom))

        //console.log("child.LayoutX", child.LayoutX)
        //console.log("child.LayoutY", child.LayoutY)
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

            for (let child of element.LayoutChildren) {
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

    /*element.Reactive.X = () => {
        return Max(parent.Padding.Left, element.Margin.Left) || 0
    }


    element.Reactive.Width = () => {
        var paddingLeft = Max(parent.Padding.Left, element.Margin.Left) || 0
        var paddingRight = Max(parent.Padding.Right, element.Margin.Right) || 0
        var parentWidth = parent.Width - paddingLeft - paddingRight
        return Min(element.ContentWidth, parentWidth)
    }*/
}

function RowBefore(element, parameters) {
    BlockBefore(element)
    element.isRow = true

    WidthToStyle(element)
    HeightToStyle(element)

    element.LayoutChildren = []

    element.AddChild = function (child) {
        element.LayoutChildren.push(child)
    }

}

function GetFontData(element) {
    if (element.fontData == undefined) {
        let attribute = element.getAttribute("f")
        if (attribute == undefined) {
            element.fontData = null
        } else {
            var parameters = attribute.split(" ").map(x => parseFloat(x));
            element.fontData = {
                ascent: parameters[0],
                descent: parameters[1],
                spaceWidth: parameters[2],
            }            
        }
    }
    return element.fontData
}

/*
function getCanvasContext(font) {
    // re-use canvas object for better performance
    const canvas = getTextWidth.canvas || (getTextWidth.canvas = document.createElement("canvas"));
    const context = canvas.getContext("2d");
    context.font = font;
    return context
}


function getTextWidth(text, context) {
    const metrics = context.measureText(text);
    return metrics.width;
}

function getCssStyle(element, prop) {
    return window.getComputedStyle(element, null).getPropertyValue(prop);
}

function getCanvasFontSize(el = document.body) {
    const fontWeight = getCssStyle(el, 'font-weight') || 'normal';
    const fontSize = getCssStyle(el, 'font-size') || '16px';
    const fontFamily = getCssStyle(el, 'font-family') || 'Times New Roman';

    return `${fontWeight} ${fontSize} ${fontFamily}`;
}*/



function RowAfter(element) {
    BlockAfter(element)


    new Reaction(() => {

        /*let contentWidth = undefined

        let line = []
        let lineContentWidth = 0
        let lineGrowUnits = 0
        let previousMargin = First(element.PaddingLeft,0)*/

        let builder = new RowBuilder(element);

        for (let child of element.LayoutChildren) {
            builder.layout(child)
            /*var t = builder.try(child)
            console.log(t.newPosition,element.Width)

            if (t.newPosition > element.Width) {
                builder.newLine()
                builder.layout(child)
            } else {
                builder.layout(child)
                builder.add(t)
            }*/
        }
        var hA = Sum(builder.newLineHardY, element.PaddingBottom)
        var hB = builder.newLineSoftY

        element.Height = Max(hA, hB)

        /*for (let child of element.LayoutChildren) {

            if (child.isBlock) {
                let margin = Max(child.MarginTop, previousMargin)
                previousMargin = First(child.MarginRight, 0)

                lineContentWidth += margin
                //if (assignDimensions) {
                child.LayoutX = lineContentWidth
                //}
                lineContentWidth += Max(child.Width, 0)
            }


            

            line.push(child)

            console.log(child)

        }*/

        //element.ContentWidth = contentWidth

        /*for (let child of element.LayoutChildren) {

            let spaceLeft = Max(element.PaddingLeft, child.MarginLeft)
            let spaceRight = Max(element.PaddingRight, child.MarginRight)


            Reaction.current.dirtImmune = true
            child.LayoutWidth = element.Width - spaceLeft - spaceRight
            Reaction.current.dirtImmune = false


            child.LayoutX = Max(element.PaddingLeft, child.MarginLeft)

        }*/
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