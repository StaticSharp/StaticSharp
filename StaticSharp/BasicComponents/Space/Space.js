function SpaceInitialization(element) {
    element.isSpace = true
    HierarchicalInitialization(element)

    element.Reactive = {
        Before: 0,
        Between: 1,
        After: 0,
        //MinBetween: 0,
    }
    //console.log("Space", element.GrowBetween)
}





function SpaceBefore(element) {
    HierarchicalBefore(element)


}