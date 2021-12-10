function List(element) {

    let parent = element.parentElement;
    element.onAnchorsChanged = [];
    element.updateWidth = function() {
        let left = parent.anchors.textLeft - 20;
        let right = parent.anchors.textRight - 40;
        element.style.marginLeft = left + "px";
        element.style.width = right - left + "px";
    }
    parent.onAnchorsChanged.push(element.updateWidth);

}