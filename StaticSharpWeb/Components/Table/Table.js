function Table(element) {

    let parent = element.parentElement;
    element.onAnchorsChanged = [];

    element.updateWidth = function() {
        let left = parent.anchors.textLeft;
        let right = parent.anchors.textRight;
        element.style.marginLeft = left + "px";
        element.style.width = right - left + "px";
    }

    document.addEventListener('DOMContentLoaded', function() {
        let tds = element.getElementsByTagName("td");
        for (let i = 0; i < tds.length; i++) {
            tds[i].onAnchorsChanged = [];
        }
        parent.onAnchorsChanged.push(element.updateWidth);
    });
}