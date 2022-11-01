function MaterialDesignIcon(element) {
    /*new Reaction(() => {
        //element.style.fill = element.ForegroundColor
    })*/

    new Reaction(() => {
        let content = element.children[0]
        content.style.fill = element.HierarchyForegroundColor
    })
}