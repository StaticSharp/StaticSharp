function BottomLeftLanding(element) {

    let parent = element.parentElement;
    //const landingContainer = document.getElementById("LandingContainer");
    // const imageContainer = document.getElementById("ImageContainer");
    //const textContainer = document.getElementById("TextContainer");
    const imageContainer = element.querySelector("#InnerImageContainer");
    const textContainer = element.querySelector("#TextContainer");
    const innerImage = element.getElementsByTagName("img")[0];
    //console.log(element);
    //console.log(element);
    //console.log(element.firstChild.firstChild.firstChild);

    element.updateWidth = function() {
        if (window.innerWidth > 1024) {
            element.css({
                gridTemplateColumns: "repeat(12, 1fr)",
            });
            imageContainer.css({
                gridColumn: "6/span 7",
            });
            textContainer.css({
                gridColumn: "span 4",
            });
            innerImage.css({
                maxWidth: "100%",
                maxHeight: "auto",
            });
        }
        if (window.innerWidth < 1024 && window.innerHeight > 768) {
            element.css({
                gridTemplateColumns: "repeat(8, 1fr)",
                gridTemplateRows: "",
                gridGap: ""
            });
            textContainer.css({
                gridColumn: "1/span 4",
                gridRow: ""
            });
            imageContainer.css({
                gridColumn: "span 4",
            });
            innerImage.css({
                maxWidth: "100%",
                maxHeight: "auto",
            });
        }
        if (window.innerWidth < 768) {
            element.css({
                gridTemplateColumns: "repeat(4, 1fr)",
                gridGap: "0px"
                    //gridTemplateRows: "repeat(2, 1fr)",
            });
            imageContainer.css({
                gridColumn: "span 4",
            });
            textContainer.css({
                gridColumn: "0",
                gridRow: "span 2"
            });
            innerImage.css({
                maxWidth: "100%",
                maxHeight: "auto",
            });
        }
        let left = parent.anchors.fillLeft;
        let right = parent.anchors.fillRight;
        element.style.marginLeft = left + "px";
        element.style.width = right - left + "px";
        //if (element.firstChild.firstChild.firstChild.style.width < )
        //element.style.backgroundColor = "red";
    }
    parent.onAnchorsChanged.push(element.updateWidth);
}