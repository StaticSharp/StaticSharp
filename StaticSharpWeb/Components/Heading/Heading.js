function Heading(element) {
    let parent = element.parentElement;


    /*console.log(element.id)
    console.log(decodeURIComponent(window.location.hash) == "#" + element.id)

    let hashHighlightElement = undefined;
    function UpdateHashHighlight() {

    }*/


    element.updateWidth = function () {
        let left = parent.anchors.textLeft;
        let right = parent.anchors.textRight;
        element.style.left = left + "px";
        element.style.width = right - left + "px";

        
        
        //element.style.backgroundColor = "red";
    }
    parent.onAnchorsChanged.push(element.updateWidth);


}