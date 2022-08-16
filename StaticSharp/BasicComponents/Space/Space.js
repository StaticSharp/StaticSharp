function Space(element) {
    element.isSpace = true
    Hierarchical(element)

    element.Reactive = {
        Before: 0,
        Between: 1,
        After: 0
    }
}
