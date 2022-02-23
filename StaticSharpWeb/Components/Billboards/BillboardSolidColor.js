function BillboardSolidColor(element, MaxContentWidth) {

    let parent = element.parentElement;

    element.billboard = true;

    let previousIsBillboard = element.previousElementSibling.billboard;
    if (!previousIsBillboard) {
        element.style.marginTop = "16px"
    }



    element.onAnchorsChanged = []
    element.anchors = {}

    element.style.display = "flex"
    element.style.flexDirection = "column"
    element.style.justifyContent = "center"


    element.updateWidth = function () {
        let left = parent.anchors.wideLeft;
        let right = parent.anchors.wideRight;
        element.style.left = left + "px";
        element.style.width = right - left + "px";
        //element.style.backgroundColor = "red";

        let contentSpace = parent.anchors.textRight - parent.anchors.textLeft;
        let contentWidth = Math.min(MaxContentWidth, contentSpace);
        let horizontalMargin = 0.5 * (contentSpace - contentWidth);

        element.anchors.textLeft = parent.anchors.textLeft - left + horizontalMargin;
        element.anchors.textRight = element.anchors.textLeft + contentWidth;

        element.onAnchorsChanged.map(x => x());
    }
    parent.onAnchorsChanged.push(element.updateWidth);

}