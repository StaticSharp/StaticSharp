function Footer(element) {

    let parent = element.parentElement.parentElement;
    console.log(parent);
    element.onAnchorsChanged = [];
    element.updateWidth = function() {
        let left = parent.anchors.textLeft;
        let right = parent.anchors.textRight;
        element.style.marginLeft = left + "px";
        element.style.width = right - left + "px";
        //element.style.backgroundColor = "red";

    }
    parent.onAnchorsChanged.push(element.updateWidth);

}