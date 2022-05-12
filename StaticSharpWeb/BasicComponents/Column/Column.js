
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
        element.Children.push(child)

        child.Reactive.LayoutX = () => {
            return Max(element.Padding.Left, child.Margin.Left) || 0
        }

        child.Reactive.LayoutWidth = () => {
            var paddingLeft = Max(element.Padding.Left, child.Margin.Left) || 0
            var paddingRight = Max(element.Padding.Right, child.Margin.Right) || 0
            var availableWidth = element.Width - paddingLeft - paddingRight
            return availableWidth
        }
    }



    let parent = element.parentElement;

    element.verticalLayout = true
    
    parent[element.id] = element
}


function ColumnAfter(element) {


    //Make ContentHeight
    new Reaction(() => {

        var contentHeight = 0;
        for (let i of element.Children) {
            if (i.tagName == SpaceTagName) {

            } else {
                contentHeight += i.Height
            }

            
        }
        element.ContentHeight = contentHeight;

    })



    //optimize: 2 reactions for width and height
    new Reaction(() => {
        let previousMarginTop = element.Padding.Top || 0
        
        let freeSpace = element.Height - element.ContentHeight

        let y = 0
        
        for (let i of element.Children) {

            if (i.tagName == SpaceTagName) {
                y += 10
                continue
            }


            //console.log("Column", child, y)

            let marginTop = previousMarginTop
            //let marginLeft = 0
            //let width = containerWidth
            
            if (i.Margin) {
                marginTop = Max(i.Margin.Top, previousMarginTop)
                previousMarginTop = i.Margin.Bottom || 0
            } else {
                previousMarginTop = 0
            }

            y += marginTop
            let d = Reaction.beginDeferred()
            
            i.LayoutY = y
            d.end()

            y += i.Height




            //current.style.marginTop = marginTop + "px"
            //current.style.marginLeft = marginLeft + "px"
            //current.style.width = width + "px"
        }

        //element.ContentHeight = y

        //element.style.paddingBottom = previousMarginTop+"px"

    })


    /*let children = element.children

    if (children.length > 0) {
        new Reaction(() => {
            console.log("element.Padding.Top", element.Padding.Top, children[0])
            let firstChild = children[0]
            let m = 0

            if (firstChild.Margin != undefined) {
                m = firstChild.Margin.Top
            }
            firstChild.style.marginTop = Math.max(m, element.Padding.Top) + "px"
        })


        new Reaction(() => {
            let lastChild = children[children.length - 1]
            let m = 0
            if (lastChild.Margin != undefined) {
                m = lastChild.Margin.Bottom
            }
            lastChild.style.marginBottom = Math.max(m, element.Padding.Bottom) + "px"
        })

        

    }

    function createMiddleMarginReaction(previous, current) {
        return function () {
            let pm = 0
            let cm = 0
            if (previous.Margin) {
                pm = previous.Margin.Top
            }
            if (current.Margin) {
                cm = current.Margin.Top
            }
            let margin = Math.max(cm, pm)
            current.style.marginTop = margin+"px"
        }
    }

    for (let i = 1; i < children.length; i++) {
        let previous = children[i-1];
        let current = children[i];
        new Reaction(createMiddleMarginReaction(previous, current))


        //current.Margin.Top = createMiddleMarginBinding(previous,current)

        //new Reaction(createMiddleMarginReaction(previous, current))
        console.log(element.id, i, element.children[i].tagName, children[i].class);
    }*/

    



    /*new Reaction(() => {
        let space = element.Padding.Top

        for (let i = 0; i < element.children.length; i++) {
            let child = children[i];
            child.Margin.


            console.log(element.id, i, element.children[i].tagName);
        }

    })*/

    



    

    /*element.Reactive = {
        Height: () => Use(element.Width) + element.clientHeight,
    }*/


}