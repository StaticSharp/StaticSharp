function BlockWithChildren(element) {
    Block(element)
    CreateCollectionSocket(element, "Children", element)



    let baseHtmlNodesOrdered = element.HtmlNodesOrdered

    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield* baseHtmlNodesOrdered
        yield* element.ExistingChildren
    })
}