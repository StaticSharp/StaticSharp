function Table(element) {

    let parent = element.parentElement;
    //element.onAnchorsChanged = [];

    element.updateWidth = function() {
            let left = parent.anchors.textLeft;
            let right = parent.anchors.textRight;
            element.style.marginLeft = left + "px";
            element.style.width = right - left + "px";
        }
        //let tds = element.getElementsByTagName("td");

    document.addEventListener('DOMContentLoaded', function() {
        let tds = element.getElementsByTagName("td");
        for (let i = 0; i < tds.length; i++) {
            tds[i].onAnchorsChanged = [];
        }
        //console.log(tds);
        //console.log(tds[1].parentElement.parentElement.parentElement.parentElement);
        parent.onAnchorsChanged.push(element.updateWidth);
        element.updateWidth();
    });
    //console.log(parent);
}