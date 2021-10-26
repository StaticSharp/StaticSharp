function Grid(element) {

    let parent = element.parentElement
    
    element.updateWidth = function () {
        let left = parent.anchors.textLeft;
        let right = parent.anchors.textRight;
        element.style.marginLeft = left + "px";
        element.style.width = right - left + "px";
        //element.style.backgroundColor = "red";
    }
    
    if (parent.onAnchorsChanged == null) {
        parent.onAnchorsChanged = [];
    }
    console.log("GRID parent: ", parent, parent.onAnchorsChanged)
    parent.onAnchorsChanged.push(element.updateWidth);

}