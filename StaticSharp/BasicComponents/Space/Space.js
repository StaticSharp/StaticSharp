function SpaceInitialization(element) {
    element.isSpace = true
    HierarchicalInitialization(element)

    element.Reactive = {
        GrowBefore: 0,
        GrowBetween: 1,
        GrowAfter: 0,
        MinBetween: 0,
    }
    //console.log("Space", element.GrowBetween)
}





function SpaceBefore(element) {
    HierarchicalBefore(element)

    let parent = element.parentElement
    if (parent.AddChild)
        parent.AddChild(element)
}