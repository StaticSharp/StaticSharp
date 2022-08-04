
function Border() {
    let _this = this
    _this.Reactive = {
        Left: 0,
        Top: 0,
        Right: 0,
        Bottom: 0
    }
}



function ItemBefore(element) {
    element.Reactive = {
        X: 0,
        Y: 0,
        Width: undefined,
        Height: undefined,
        Margin: new Border(),
        
    }

    new Reaction(() => {
        element.style.top = element.Y+"px"        
    })
    new Reaction(() => {
        element.style.left = element.X + "px"
    })

    new Reaction(() => {
        element.style.width = (!!element.Width) ? element.Width + "px" : ""
    })

    new Reaction(() => {
        element.style.height = (!!element.Height) ? element.Height + "px" : ""
    })


    //element.setAttribute("transform", `translate(${element.X},${element.Y})`)
}

function ItemAfter(element) {


    element.Reactive = {
        ContentHeight: () => Use(element.Width) + element.clientHeight,
    }
}