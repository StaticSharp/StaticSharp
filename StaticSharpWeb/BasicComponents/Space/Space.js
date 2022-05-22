function SpaceInitialization(element) {
    element.isSpace = true
    HierarchicalInitialization(element)

    element.Reactive = {
        GrowBefore: 0,
        GrowBetween: 1,
        GrowAfter: 0,
        MinBetween: 8,
    }
}





function SpaceBefore(element) {
    HierarchicalBefore(element)
}