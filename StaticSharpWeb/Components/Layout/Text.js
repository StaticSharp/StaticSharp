function TextBefore(element) {

    let parent = element.parentElement;
    //element.style.marginTop = "20px"
    //element.style.marginBottom = "20px"



    element.Margin.Left = 16
    element.Margin.Right = 16

    element.Margin.Top = 20
    element.Margin.Bottom = 20


    if (!parent.horizontalLayout) {
        new Reaction(() => {
            
            let marginLeft = Math.max(element.Margin.Left, (parent.Padding && parent.Padding.Left) || 0)
            let marginRight = Math.max(element.Margin.Right, (parent.Padding && parent.Padding.Right) || 0)
            let width = parent.Width - marginLeft - marginRight

            element.Width = width

            element.style.marginLeft = marginLeft + "px"
            element.style.width = width + "px"

        })
    }
    





    //element.style.position = "absolute"
    element.Reactive = {
        //Left: undefined,
        //Width: undefined,
        /*ContentHeight: () => {
            console.log(element.offsetHeight, element.Width)
            //element.Width
            return element.offsetHeight
        },
        Height: () => element.ContentHeight*/

    }





    /*element.Reactive.Width.OnChanged(() => {
        console.log(element.clientHeight , element.Width)
        //element.Height = element.offsetHeight
    })*/

    /*new Reaction(() => {
        const textPadding = 16

        if (parent.Width) {
            if ((parent.InnerWidth != undefined) && (parent.PaddingLeft != undefined)) {
                
                element.Left = Math.max(textPadding, parent.PaddingLeft)
                element.Width = Math.min(parent.Width - 2 * textPadding, parent.InnerWidth)
            } else {
                element.Left = 0
                element.Width = parent.Width

            }
        }
    })*/



    /*let parent = element.parentElement;
    element.updateWidth = function() {
        let left = parent.anchors.textLeft;
        let right = parent.anchors.textRight;
        element.style.left = left + "px";
        element.style.width = right - left + "px";
    }
    parent.onAnchorsChanged.push(element.updateWidth);*/

}



function TextAfter(element) {

}