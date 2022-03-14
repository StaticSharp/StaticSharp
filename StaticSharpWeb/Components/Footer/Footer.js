function Footer() {
    element = this
    let parent = element.parentElement;

    element.style.height = "100px"
    element.style.backgroundColor = "yellow"
    //console.log(element.parentElement);

    /*element.onAnchorsChanged = [];
    element.updateWidth = function () {
        let left = parent.anchors.textLeft;
        let right = parent.anchors.textRight;
        element.style.marginLeft = left + "px";
        element.style.width = right - left + "px";
        //element.style.backgroundColor = "red";
    }
    parent.onAnchorsChanged.push(element.updateWidth);*/

}