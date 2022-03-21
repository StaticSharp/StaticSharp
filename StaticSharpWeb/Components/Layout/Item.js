
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
        X: undefined,
        Y: undefined,
        Width: undefined,
        Height: undefined,
        Margin: new Border(),
        ContentHeight: undefined,
    }

}

function ItemAfter(element) {


    element.Reactive = {
        ContentHeight: () => Use(element.Width) + element.clientHeight,
    }
}