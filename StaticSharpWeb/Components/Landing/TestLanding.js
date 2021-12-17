function TestLanding(element, crop) {

    let parent = element.parentElement;
    const imageAndTextContainer = element.querySelector("#ImageAndTextContainer");
    const imageContainer = imageAndTextContainer.querySelector("#ImageContainer");
    const textContainer = imageAndTextContainer.querySelector("#TextContainer");
    const innerImage = element.getElementsByTagName("img")[0];

    function pixelConvert(intrinsic, rendered, firstposition) {
        return (firstposition * rendered / intrinsic);
    }

    element.updateWidth = function() {
        var x1 = crop[0];
        var x2 = crop[2];
        var y1 = crop[1];
        var y2 = crop[3];
        var h = y2 - y1;
        var w = x2 - x1;
        var windowRatio = window.innerWidth / window.innerHeight;
        var a = pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, x2);
        var b = window.innerWidth - a;
        var t = 0;
        if (b < 0)
            t = b;
        //if (windowRatio > 1) { // Когда тексту будет не хватать места
        // console.log(textContainer.offsetWidth);
        // console.log(pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, w));
        // console.log(imageAndTextContainer.offsetWidth);
        // console.log(imageAndTextContainer.offsetHeight);
        // console.log(textContainer.offsetHeight);

        // console.log(textContainer.offsetWidth + pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, w));
        // console.log(imageAndTextContainer.offsetWidth);

        // console.log(textContainer.offsetHeight + pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h));
        // console.log(imageAndTextContainer.offsetHeight);

        // (textContainer.offsetWidth + pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, w) < imageAndTextContainer.offsetWidth ||
        // textContainer.offsetHeight + pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h) < imageAndTextContainer.offsetHeight)
        // if (textContainer.offsetWidth + pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, w) < imageAndTextContainer.offsetWidth) {
        //     console.log("w");
        // }
        // var check = 0;
        // if (textContainer.offsetHeight + pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h) < imageAndTextContainer.offsetHeight && check == 0) {
        //     imageContainer.css({
        //         height: pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h) + "px",
        //         overflow: "hidden"
        //     });
        //     innerImage.css({
        //         width: screen.width + (x2 - x1) + "px",
        //         transform: "translate(" + t + "px, " + -pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, y1) + "px)",
        //     });
        //     textContainer.css({
        //         top: (imageAndTextContainer.offsetHeight / 2) - (Math.round(textContainer.offsetHeight / 2)) + "px",
        //         right: "50%"
        //     });
        //     console.log("h");
        // }
        function wide() {
            imageContainer.css({
                height: pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h) + "px",
                overflow: "hidden"
            });
            innerImage.css({
                width: screen.width + (x2 - x1) + "px",
                transform: "translate(" + t + "px, " + -pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, y1) + "px)",
            });
            textContainer.css({
                top: (imageAndTextContainer.offsetHeight / 2) - (Math.round(textContainer.offsetHeight / 2)) + "px",
                right: "50%"
            });
        }
        //console.log(windowRatio);

        function minimized() {
            imageContainer.css({
                height: pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h) / windowRatio + "px",
                //height: pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h) + "px",
                overflow: "hidden"
            });
            innerImage.css({
                width: screen.width + (x2 - x1) + "px",
                //transform: "translate(" + t + "px, " + (-pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, y1)) + "px)",
                transform: "translate(" + t + "px, " + (-pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, y1) + (pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h) / windowRatio) - pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h)) + "px)",
            });
            textContainer.css({
                top: "0px",
                right: ""
            });
        }

        var imageAndTextBoxesWidthSum = textContainer.offsetWidth + pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, w);
        var imageAndTextContainerWidth = imageAndTextContainer.offsetWidth;

        var imageAndTextBoxesHeightSum = textContainer.offsetHeight + pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h);
        var imageAndTextContainerHeight = imageAndTextContainer.offsetHeight;

        var maxSize = (-pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, y1) + (pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h) / windowRatio) - pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h));

        // if (imageAndTextBoxesWidthSum < imageAndTextContainerWidth) {
        //     wide();
        //     console.log("width");
        // }
        // if (imageAndTextBoxesHeightSum < imageAndTextContainerHeight) {
        //     console.log("height");
        // }
        // if (imageAndTextBoxesWidthSum > imageAndTextContainerWidth && imageAndTextBoxesHeightSum > imageAndTextContainerHeight) {
        //     console.log("what");
        // }

        if (imageAndTextBoxesWidthSum < imageAndTextContainerWidth) {
            console.log("wide");
            wide();
        } else {
            if (maxSize < 0) {
                console.log("minimized");
                if (imageAndTextBoxesHeightSum < imageAndTextContainerHeight) {
                    console.log("help");
                }
                minimized();
            }
            // imageAndTextBoxesWidthSum = imageAndTextBoxesHeightSum;
            // imageAndTextContainerWidth = imageAndTextContainerHeight;
        }
        // if (imageAndTextBoxesWidthSum > imageAndTextContainerWidth && imageAndTextBoxesHeightSum < imageAndTextContainerHeight) {
        //     console.log("what");
        // }

        // if (imageAndTextBoxesWidthSum < imageAndTextContainerWidth) {
        //     wide();
        // }
        // if (imageAndTextBoxesHeightSum < imageAndTextContainerHeight) {
        //     if (maxSize < 0) {
        //         minimized();
        //     }
        // }
        // if (imageAndTextBoxesWidthSum > imageAndTextContainerWidth && imageAndTextBoxesHeightSum > imageAndTextContainerHeight) {
        //     console.log("what");
        // }
        // if (windowRatio > 1) {
        //     //console.log(tr);
        //     imageAndTextContainer.css({
        //         position: "relative",
        //     })
        //     imageContainer.css({
        //         height: "40vh",
        //         //height: h + "px",
        //         overflow: "hidden",
        //     });
        //     innerImage.css({
        //         //height: "100vh",
        //         //width: "140vw",
        //         objectFit: "cover",
        //         width: innerImage.naturalWidth + x2 + (x2 - x1) + "px",
        //         // transform: "translate(" + (innerImage.naturalWidth - x2) + "px, " + -y1 / (innerImage.naturalHeight / innerImage.offsetHeight) + "px)",
        //         // transform: "translate(0px, " + -y1 / (innerImage.naturalHeight / innerImage.offsetHeight) + "px)",
        //         transform: "translate(0px,  " + -pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, y1) + "px)",
        //     });
        // } else {
        //     imageAndTextContainer.css({
        //         position: "relative",
        //     })
        //     imageContainer.css({
        //         height: "50vh",
        //         overflow: "hidden",
        //     });
        //     if (innerImage.naturalHeight < innerImage.naturalWidth) {
        //         //console.log(tr);
        //         if (innerImage.naturalHeight < innerImage.offsetHeight) {
        //             innerImage.css({
        //                 height: "100vh",
        //                 objectFit: "cover",
        //                 transform: "translate(" + 0 + "px, " + tr + "px)",
        //             });
        //         } else {
        //             innerImage.css({
        //                 height: "100vh",
        //                 width: "100%",
        //                 objectFit: "cover",
        //                 transform: "translate(0px, " + tr3 + "px)",
        //             });
        //         }
        //     } else {

        //         if (innerImage.naturalHeight < innerImage.offsetHeight) {
        //             innerImage.css({
        //                 height: "100vh",
        //                 objectFit: "cover",
        //                 transform: "translate(0px, " + tr + "px)",
        //             });
        //         } else {
        //             innerImage.css({
        //                 height: "100vh",
        //                 width: "100%",
        //                 objectFit: "cover",
        //                 transform: "translate(0px, " + -50 + "px)",
        //             });
        //         }
        //     }
        // }


        let left = parent.anchors.wideLeft;
        let right = parent.anchors.wideRight;
        element.style.marginLeft = left + "px";
        element.style.width = right - left + "px";
    }
    parent.onAnchorsChanged.push(element.updateWidth);
}