function FullCenterLanding(element) {

    let parent = element.parentElement;
    const imageContainer = element.querySelector("#InnerImageContainer");
    const textContainer = element.querySelector("#TextContainer");
    const innerImage = element.getElementsByTagName("img")[0];

    element.updateWidth = function() {
        element.css({
            gridTemplateColumns: "repeat(4, 1fr)",
            gridGap: "0px"
        });
        imageContainer.css({
            gridColumn: "span 4",
            gridRow: "span 2",
        });
        textContainer.css({
            gridColumn: "span 4",
            gridRow: ""
        });
        innerImage.css({
            maxWidth: "100%",
            maxHeight: "auto",
        });
        let left = parent.anchors.fillLeft;
        let right = parent.anchors.fillRight;
        element.style.marginLeft = left + "px";
        element.style.width = right - left + "px";
    }
    parent.onAnchorsChanged.push(element.updateWidth);
}