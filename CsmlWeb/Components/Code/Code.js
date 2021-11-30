function Code(element) {
    console.log("COODE", element);
    let parent = element.parentElement;
    
    element.updateWidth = function () {
        let left = parent.anchors.textLeft;
        let right = parent.anchors.textRight;
        element.style.marginLeft = left + "px";
        element.style.width = right - left + "px";
        
    }
}