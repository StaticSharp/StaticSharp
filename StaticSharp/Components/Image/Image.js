function Image(element) {
    let parent = element.parentElement;
    //console.log(element.firstChild);
    element.updateWidth = function() {
        let left = parent.anchors.fillLeft;
        let right = parent.anchors.fillRight;
        element.style.marginLeft = left + "px";
        element.style.width = right - left + "px";
        //element.style.minWidth = "1280px";
        //element.style.minHeight = "500px";
        element.style.height = "50vh";
        element.style.overflow = "hidden";
        element.style.backgroundColor = "rgb(60, 61, 63)";
        //let innerHeight = Math.max(element.style.height, element.style.minHeight);
        //console.log(element.style.height);
        //TranslateInnerImage(element);
        //element.image.style.transform = "translate: (0px, -100px)";
        //element.style.backgroundColor = "red";
    }

    try {
        parent.onAnchorsChanged.push(element.updateWidth);
    } catch {

    }
}

// function TranslateInnerImage(element) {
//     let innerHeight = element.style.height;
//     console.log(innerHeight);
//     element.firstChild.style.transform = "translate(0px, 0px)";
// }