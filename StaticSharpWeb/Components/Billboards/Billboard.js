function Billboard(element) {

    let parent = element.parentElement;

    element.billboard = true;

    let previousIsBillboard = element.previousElementSibling.billboard;
    if (!previousIsBillboard) {
        element.style.marginTop = "16px"
    }

    element.updateWidth = function () {
        let left = parent.anchors.wideLeft;
        let right = parent.anchors.wideRight;
        element.style.left = left + "px";
        element.style.width = right - left + "px";
        //element.style.backgroundColor = "red";
    }
    parent.onAnchorsChanged.push(element.updateWidth);

}