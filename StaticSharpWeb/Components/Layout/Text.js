function Text() {
    let element = this
    let parent = element.parentElement;

    new Reaction(() => {
        let parentPaddingLeft = parent.PaddingLeft || 0
        let parentInnerWidth = parent.InnerWidth || parent.Width
        let parentWidth = parent.Width

        const textPadding = 16

        let left = Math.max(textPadding, parentPaddingLeft | 0)
        let width = Math.min(parentWidth - 2 * textPadding, parentInnerWidth)

        element.style.left = left + "px"

        element.style.width = width + "px"
    })


    /*let parent = element.parentElement;
    element.updateWidth = function() {
        let left = parent.anchors.textLeft;
        let right = parent.anchors.textRight;
        element.style.left = left + "px";
        element.style.width = right - left + "px";
    }
    parent.onAnchorsChanged.push(element.updateWidth);*/

}