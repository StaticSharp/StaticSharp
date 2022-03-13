

function Column(parameters) {
    

    let element = this
    let parent = element.parentElement;

    this.Reactive = {
        Width: undefined,
        InnerWidth: () => this.Width,
        PaddingLeft: 0
    }

    new Reaction(() => {
        if (this.Width)
            this.style.width = this.Width + "px"
        else
            this.style.width = undefined

    })

    /*new Reaction(() => {
        console.log("Column.PaddingLeft", this.PaddingLeft)
    })*/

    /*

    new Property(undefined)
        .attach(element, "Width");

    new Reaction(() => {
        if (element.Width) element.style.width = element.Width + "px"
    })

    new Property()
        .attach(element, "MaxInnerWidth");

    new Property(() => Math.min(element.Width, element.MaxInnerWidth))
        .attach(element, "InnerWidth")

    new Property(() => (element.Width - element.InnerWidth)*0.5)
        .attach(element, "PaddingLeft")*/

    
    parent[element.id] = element


}
