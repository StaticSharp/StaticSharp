function Border() {
    let _this = this
    _this.Reactive = {
        Left: undefined,
        Top: undefined,
        Right: undefined,
        Bottom: undefined
    }
}


function BlockInitialization(element) {
    element.Reactive = {
        Padding: new Border(),
        Margin: new Border(),
        X: undefined,
        Y: undefined,
        Width: undefined,
        Height: undefined,
        
        //InnerWidth: () => parent.InnerWidth || element.Width,
        //PaddingLeft: () => parent.PaddingLeft || 0
    }
}

function BlockBefore(element) {

    new Reaction(() => {
        element.style.top = (!!element.Y) ? element.Y + "px" : ""
    })
    new Reaction(() => {
        element.style.left = (!!element.X) ? element.X + "px" : ""
    })

    new Reaction(() => {
        element.style.width = (!!element.Width) ? element.Width + "px" : ""
    })

    new Reaction(() => {
        element.style.height = (!!element.Height) ? element.Height + "px" : ""
    })

}

function BlockAfter(element) {

}