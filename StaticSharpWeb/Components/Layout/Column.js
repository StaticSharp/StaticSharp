

function Column(parameters) {
    

    let element = this
    let parent = element.parentElement;

    this.Reactive = {
        Width: undefined,
        InnerWidth: () => this.Width,
        PaddingLeft: () => (element.Width - element.InnerWidth) * 0.5
    }
    
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
