function BlockWithChildren(element) {
    Block(element)
    CreateCollectionSocket(element, "Children", element)

    //let baseHtmlNodesOrdered
    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield* element.Children
        yield* element.UnmanagedChildren
    })
}