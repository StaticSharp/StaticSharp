﻿function Paragraph(element) {

    let parent = element.parentElement;

    element.updateWidth = function() {
        let left = parent.anchors.textLeft;
        let right = parent.anchors.textRight;
        element.style.left = left + "px";
        element.style.width = right - left + "px";
        //element.style.backgroundColor = "red";
    }
    parent.onAnchorsChanged.push(element.updateWidth);

}