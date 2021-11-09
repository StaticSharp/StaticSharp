function ColorSequence(element) {

    let parent = element.parentElement;

    element.updateWidth = function() {
        let left = parent.anchors.textLeft;
        let right = parent.anchors.textRight;
        var elemHeight = getComputedStyle(element).height.replace(/px/, "") * 1;
        element.style.marginLeft = left + elemHeight + "px";
    }
    parent.onAnchorsChanged.push(element.updateWidth);
}