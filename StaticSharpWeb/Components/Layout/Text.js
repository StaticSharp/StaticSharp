function Text() {
    let element = this
    let parent = element.parentElement;
    element.style.marginTop = "20px"
    element.style.marginBottom = "20px"
    //element.style.position = "absolute"
    element.Reactive = {
        Top: undefined,
        Left: undefined,
        Width: undefined,
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

    new Reaction(() => {
        const textPadding = 16

        if (parent.Width) {
            if ((parent.InnerWidth != undefined) && (parent.PaddingLeft != undefined)) {
                
                this.Left = Math.max(textPadding, parent.PaddingLeft)
                this.Width = Math.min(parent.Width - 2 * textPadding, parent.InnerWidth)
            } else {
                this.Left = 0
                this.Width = parent.Width

            }
        }

        /*let parentPaddingLeft = parent.PaddingLeft || 0
        let parentInnerWidth = parent.InnerWidth || parent.Width
        let parentWidth = parent.Width

        

        let left = Math.max(textPadding, parentPaddingLeft | 0)
        let width = Math.min(parentWidth - 2 * textPadding, parentInnerWidth)*/

    })


    new Reaction(() => {
        this.style.left = this.Left + "px"
    })
    new Reaction(() => {
        this.style.width = this.Width + "px"
    })
    new Reaction(() => {
        this.style.top = this.Top + "px"
    })
    /*new Reaction(() => {
        this.style.height = this.Height + "px"
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