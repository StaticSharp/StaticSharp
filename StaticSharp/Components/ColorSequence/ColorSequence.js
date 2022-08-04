function ColorSequence(element) {
    let parent = element.parentElement;

    element.updateWidth = function() {
            let left = parent.anchors.textLeft;
            let right = parent.anchors.textRight;
            element.style.marginLeft = left + "px";
            element.style.width = right - left + "px";
        }
        //document.addEventListener('DOMContentLoaded', function() {
    try {
        parent.onAnchorsChanged.push(element.updateWidth);
    } catch {

    }
    //});
}