function Paragraph(element) {


    let parent = element.parentElement;
    console.log("Paragraph Reaction", parent.PaddingLeft)
    new Reaction(() => {
        
        const textPaddingLeft = 16

        let left = Math.max(textPaddingLeft - parent.PaddingLeft , 0);
        element.style.left = left+"px"
    })


    /*element.updateWidth = function() {
        let left = parent.anchors.textLeft;
        let right = parent.anchors.textRight;
        element.style.left = left + "px";
        element.style.width = right - left + "px";
        //element.style.backgroundColor = "red";
    }
    parent.onAnchorsChanged.push(element.updateWidth);*/

}