function ReduceFontSizeOnOverflow(element) {

    /*let parent = element.parentElement;

    let scrollWidth = undefined;
    var initialTextAlign = element.style.textAlign

    element.updateFontSize = function () {
        let left = parent.anchors.textLeft
        let right = parent.anchors.textRight
        var width = right - left

        if (element.scrollWidth > width) {
            if (!scrollWidth) {//anti jitter
                scrollWidth = element.scrollWidth
            }

            element.style.transform = `scale(${width / scrollWidth})`
            element.style.transformOrigin = "left"
            element.style.textAlign = "left"
        } else {
            element.style.removeProperty("transform")
            element.style.removeProperty("transform-origin")
            element.style.textAlign = initialTextAlign
        }
    }

    parent.onAnchorsChanged.push(element.updateFontSize);*/

}