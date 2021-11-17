function ColorSequence(element) {

    let parent = element.parentElement;

    element.updateWidth = function() {
        let left = parent.anchors.textLeft;
        let right = parent.anchors.textRight;
        element.style.marginLeft = left + "px";
    }
    parent.onAnchorsChanged.push(element.updateWidth);
}