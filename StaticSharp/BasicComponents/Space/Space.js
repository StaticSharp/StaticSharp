function Space(element) {
    element.isSpace = true
    Hierarchical(element)

    element.Reactive = {
        Before: 1,
        Between: 1,
        After: 1
    }
}
