

function RowBefore(element) {

    let parent = element.parentElement;

    element.horizontalLayout = true

    /*element.style.display = "flex"
    element.style.flexDirection = "row";

    element.style.flexWrap = "wrap";
    element.style.justifyContent = "space-between";*/


    element.Reactive = {
        Padding: new Border(),
        Width: () => parent.Width,
        //Height: undefined,
        //InnerWidth: () => parent.InnerWidth || element.Width,
        //PaddingLeft: () => parent.PaddingLeft || 0
    }

    element.Padding.Left = () => (parent.Padding && parent.Padding.Left) || 0
    element.Padding.Right = () => (parent.Padding && parent.Padding.Right) || 0

    element.Padding.Left = 100
    element.Padding.Right = 100

    new Reaction(() => {
        if (element.Width)
            element.style.width = element.Width + "px"
        else
            element.style.width = undefined

    })

    parent[element.id] = element

    


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
}



function RowAfter(element) {

    //element.style.height = "200px"
    let fontData = GetFontData(element)

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
        /*
         {
            ascent
            descent
            spaceWidth
         }
         */

        const WordNodeTagName = "SPAN"

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

        var range = document.createRange();

        var canvasFontSize = getCanvasFontSize(element)
        var context = getCanvasContext(canvasFontSize)


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

                if (n.tagName == WordNodeTagName) {
                    //child.removeAttribute("x")
                    //child.removeAttribute("y")
                    /*range.selectNodeContents(n);
                    var rect = range.getBoundingClientRect();
                    console.log(n.innerText, rect)*/

                    
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
                    /*if (n.tagName == "text") {

                        n.setAttribute("x", 0 + "px");
                        n.setAttribute("y", 0 + "px");

                        console.log("n.dataset.a", n.dataset.a)
                        
                        fontStack.push({
                            ascent: n.dataset.a,
                            descent: n.dataset.d,
                            spaceWidth: n.dataset.sw,
                        })

                        processNodes(n.childNodes)

                        fontStack.pop()
                        continue
                    }*/
                    
                    if (n.tagName == WordNodeTagName) {
                        
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


        /*


        let height = 0
        
        let containerSpaceLeft = element.Padding.Left
        let containerSpaceRight = element.Padding.Right
        let top = 0;
        let i = 0
        while (i < children.length) {

            let previousSpaceLeft = containerSpaceLeft
            let rowHeight = 0
            let rowWidth = 0

            
            let j = 0

            let row = []
            let rowSpacers = 0
            let accumulatedSpaces = 0
            while (i < children.length) {
                let current = children[i]
                
                let spacer = current.getAttribute("Spacer")
                spacer = spacer && parseFloat(spacer)

                if (spacer != undefined) {
                    
                    accumulatedSpaces += spacer
                } else {                    

                    let descriptor = {
                        width: current.offsetWidth,
                        left: 0,
                        right: 0,
                        element: current
                    }


                    let currentWidth = current.offsetWidth
                    if (current.Margin) {
                        descriptor.left = current.Margin.Left
                        descriptor.right = current.Margin.Right
                    }

                    let totalFreeSpaceRequired =
                        Math.max(previousSpaceLeft, descriptor.left)
                        + currentWidth
                        + Math.max(containerSpaceRight, descriptor.right)


                    if (width < (rowWidth + totalFreeSpaceRequired)) {
                        if (j > 0)
                            break;
                    }

                    if (accumulatedSpaces > 0) {
                        row.push(accumulatedSpaces)
                        rowSpacers += accumulatedSpaces
                        accumulatedSpaces = 0
                    }

                    row.push(descriptor)


                    current.style.top = top + "px"


                    rowWidth += Math.max(previousSpaceLeft, descriptor.left) + currentWidth

                    rowHeight = Math.max(rowHeight, current.offsetHeight)

                    previousSpaceLeft = descriptor.right

                    j++
                }                
                i++                
            }

            let spacersWidth = width - (rowWidth + Math.max(previousSpaceLeft, containerSpaceRight))
            let pixelsPerSpacer = (rowSpacers > 0) ? spacersWidth / rowSpacers : 0


            //console.log("rowWidth", rowWidth, "spacersWidth", spacersWidth, "rowSpacers", rowSpacers, "pixelsPerSpacer", pixelsPerSpacer)

            previousSpaceLeft = containerSpaceLeft
            rowWidth = 0
            let currentSpacerSize = 0


            for (let current of row) {
                
                if (typeof current == "number") {
                    currentSpacerSize += current * pixelsPerSpacer
                } else {

                    let spaceLeft = Math.max(previousSpaceLeft, current.left) + currentSpacerSize

                    current.element.style.left = rowWidth + spaceLeft + "px"
                    rowWidth += spaceLeft + current.width
                    previousSpaceLeft = current.right
                    currentSpacerSize = 0
                }
            }

            top += rowHeight
        }*/

        

        

        element.style.height = top+"px"

    })

    //let flexChildren = document.querySelectorAll('.content');
    //let leftPosition = flexChildren[0].offsetLeft;
    

}