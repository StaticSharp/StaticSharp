function TestLanding(element, crop) {

    let parent = element.parentElement;
    const imageAndTextContainer = element.querySelector("#ImageAndTextContainer");
    const imageContainer = imageAndTextContainer.querySelector("#ImageContainer");
    const textContainer = imageAndTextContainer.querySelector("#TextContainer");
    const innerImage = element.getElementsByTagName("img")[0];
    var swap = false;
    var once = false;


    function pixelConvert(intrinsic, rendered, firstposition) {
        return (firstposition * rendered / intrinsic);
    }

    // window.addEventListener("load", (evt) => {
    //     if (window.innerWidth / window.innerHeight < 1) {
    //         //minimized();
    //     } else {
    //         //wide();
    //     }
    // });

    // var x1 = crop[0];
    // var x2 = crop[2];
    // var y1 = crop[1];
    // var y2 = crop[3];
    // var h = y2 - y1;
    // var w = x2 - x1;

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

        function wide() {
            imageContainer.css({
                height: pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h) + "px",
                overflow: "hidden"
            });
            innerImage.css({
                width: screen.width + (pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, x2) - pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, x1)) + "px",
                transform: "translate(" + t + "px, " + -pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, y1) + "px)",
            });
            textContainer.css({
                top: (imageAndTextContainer.offsetHeight / 2) - (Math.round(textContainer.offsetHeight / 2)) + "px",
                right: "50%"
            });
        }

        function minimized() {
            imageContainer.css({
                height: pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h) / windowRatio + "px",
                overflow: "hidden"
            });
            innerImage.css({
                width: screen.width + (pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, x2) - pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, x1)) + "px",
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
        var maxSize2 = (-pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, x1) + (pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, w) / windowRatio) - pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, w));
        var maxSizeW = (pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, x2) - pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, x1));

        if (imageAndTextBoxesHeightSum > imageAndTextContainerHeight && imageAndTextBoxesWidthSum > imageAndTextContainerWidth) {
            // if (!once) {
            //     console.log("once");
            //     once = true;
            // }
            if (swap) {
                //console.log("W to H");
                wide();
            } else {
                //console.log("H to W");
                minimized();
            }
        } else {
            //console.log(maxSize);
            if (swap) {
                //console.log(-y2);
                if (imageAndTextBoxesHeightSum < imageAndTextContainerHeight) {
                    //|| ((maxSizeW / maxSize2) < -1)
                    //|| maxSize2 > -maxSizeW
                    if (maxSize < 0) {
                        //console.log(maxSizeW);
                        console.log("wide = " + -maxSizeW);
                        console.log("maxSize2 = " + maxSize2);
                        //console.log("maxSizeDiv = " + (maxSizeW / maxSize2));
                        console.log("maxSize1 = " + maxSize);
                        //console.log("wideH");
                        minimized();
                    }
                } else {
                    //console.log("miniH");
                    wide();
                    swap = false;
                }
            } else {
                if (imageAndTextBoxesWidthSum < imageAndTextContainerWidth) {
                    //console.log("wideW");
                    wide();
                } else {
                    if (maxSize < 0) {
                        //console.log(-y2);
                        //console.log(maxSize2);
                        //console.log("miniW");
                        minimized();
                    }
                    swap = true;
                }
            }
        }

        let left = parent.anchors.wideLeft;
        let right = parent.anchors.wideRight;
        element.style.marginLeft = left + "px";
        element.style.width = right - left + "px";
    }
    parent.onAnchorsChanged.push(element.updateWidth);
}