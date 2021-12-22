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

    function getWidth(length, ratio) {
        var width = ((length) / (Math.sqrt((1) / (Math.pow(ratio, 2) + 1))));
        return Math.round(width);
    }

    function getHeight(length, ratio) {
        var height = ((length) / (Math.sqrt((Math.pow(ratio, 2) + 1))));
        return Math.round(height);
    }

    //var ratio = 0;

    window.onload = function() {
        //console.log("X2 = " + crop[2]);
        // 1000 = image width;
        // 1420 = imageContainer width;
        //console.log(imageContainer.clientWidth);
        var ratio = (1000 / (1000 + (imageContainer.clientWidth - crop[2]) - (1000 - crop[2]) + Math.round(parent.anchors.fillRight - crop[2])));
        var ratio2 = 1000 / innerImage.clientWidth;
        //console.log("Ratio = " + ratio2);
        //console.log("R " + ratio);
        //console.log("RIGHT = " + Math.round(parent.anchors.fillRight));
        //console.log("X2 = " + (crop[2] / ratio2));
        //console.log("RAZ = " + (Math.round(parent.anchors.fillRight) - (crop[2] / ratio2)));
        //console.log("X1 = " + crop[2]);
        //console.log("X2 = " + (crop[2] / ratio));
        var k = (crop[3] - crop[1]) / ratio2;
        //console.log(k);
        // console.log("y2 - y1 = " + k);
        // console.log("h = " + (imageContainer.clientHeight));
        // console.log("Raznica = " + (imageContainer.clientHeight / ratio2 - k));

        //console.log(imageContainer.clientHeight - (crop[3] / ratio2));
        //console.log(Math.round(parent.anchors.fillRight) - (crop[2] / ratio));
        //var t = (Math.round(parent.anchors.fillRight) - (crop[2] / ratio2));


        //console.log((1000 + (1420 - crop[2]) - (1000 - crop[2]) + Math.round(parent.anchors.fillRight - crop[2])));
        imageContainer.css({
            height: "450px",
            //height: (crop[3] / ratio2) + "px",
            overflow: "hidden"
        });
        var t2 = -(imageContainer.clientHeight - crop[3] / ratio2);
        var t1 = ((-crop[2] / ratio2) + (imageContainer.clientHeight - k));
        // console.log(t1);
        // console.log(t2);
        var t3 = (innerImage.clientHeight - crop[3]) / ratio2;
        // console.log(t3);

        // console.log(t1 + t3);
        // console.log(imageContainer.clientHeight);
        // console.log(k);
        var _y2 = (crop[3] / ratio2);
        var _y1 = (crop[1] / ratio2);
        var _h = innerImage.clientHeight;
        console.log("Y1 = " + _y1);
        console.log("Y2 = " + _y2);
        // console.log("Height = " + _h);
        // console.log("Raz = " + (_h - _y2));

        innerImage.css({
            //width: (t + 1000 + (imageContainer.clientWidth - crop[2]) - (1000 - crop[2]) + Math.round((parent.anchors.fillRight - crop[2]))) + "px",
            width: (1000 + (imageContainer.clientWidth - 1000) + ((1000 - crop[2]) / ratio2)) + "px", //anchors
            //transform: "translate(0px, " + ((-crop[1] / ratio2) + (imageContainer.clientHeight - ((crop[3] / ratio2) - (crop[1] / ratio2)))) + "px)",
            // transform: "translate(0px, " + ((-crop[2] / ratio2) + (imageContainer.clientHeight - k)) + "px)",
            transform: "translate(0px, " + -((crop[3] / ratio2) - imageContainer.clientHeight + _y1) + "px)",
            //transform: "translate(0px, 0px)",
        });
        // console.log(textContainer.offsetHeight);
        console.log(imageContainer.clientHeight);
        console.log(textContainer.offsetHeight);

        console.log(imageContainer.clientHeight / 2 - textContainer.offsetHeight / 2);
        textContainer.css({
            top: (imageContainer.clientHeight / 2) - (textContainer.offsetHeight / 2) + "px",
            //top: "10px",
            //bottom: "10px",
            // textAlign: "center",
            //top: (imageAndTextContainer.offsetHeight / 2) + "px",
            right: "50%",
            width: (imageAndTextContainer.clientWidth / 2) + "px",
        })
    }

    // window.addEventListener("resize", (evt) => {
    //     var ratio2 = 1000 / innerImage.clientWidth;
    //     var _y2 = (crop[3] / ratio2);
    //     var _y1 = (crop[1] / ratio2);
    //     console.log("Y1C = " + _y1);
    //     innerImage.css({
    //         width: (1000 + (imageContainer.clientWidth - 1000) + ((1000 - crop[2]) / ratio2)) + "px", //anchors
    //         transform: "translate(0px, " + -((crop[3] / ratio2) - imageContainer.clientHeight + _y1) + "px)",
    //     });
    //     // if ((crop[3] / ratio2) < imageContainer.height) {
    //     //     console.log("STOP");
    //     // }
    // })

    element.updateWidth = function() {
        let left = parent.anchors.wideLeft;
        let right = parent.anchors.wideRight;
        element.style.marginLeft = left + "px";
        element.style.width = right - left + "px";

        let leftText = parent.anchors.textLeft;
        let rightText = parent.anchors.textRight;
        textContainer.style.marginLeft = leftText + "px";
    }


    //element.updateWidth = function() {
    // //console.log("R2 " + ratio);
    // var x1 = pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, crop[0]);
    // var x2 = pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, crop[2]);
    // var y1 = pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, crop[1]);
    // var y2 = pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, crop[3]);;
    // var h = y2 - y1;
    // var w = x2 - x1;
    // //get convert parameters before this func;
    // //h = (crop[3] - crop[1]);

    // // var maxHeight = y2;
    // // var maxWidth = x2;
    // // var ratioW = maxWidth / innerImage.clientWidth;
    // // var ratioH = maxHeight / innerImage.clientHeight;

    // // console.log("W " + ratioW);
    // // console.log("H " + ratioH);

    // var windowRatio = window.innerWidth / window.innerHeight;
    // // var a = pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, x2);
    // // var b = window.innerWidth - a;
    // // var t = 0;
    // // if (b < 0)
    // //     t = b;
    // var ratio = (imageContainer.clientHeight / imageContainer.clientWidth);
    // // console.log(ratio);

    // // var h1 = getHeight(innerImage.clientHeight, ratio);
    // // var w1 = getWidth(h1, ratio);

    // // console.log("W = " + w1);
    // // console.log("H = " + h1);

    // // while (window.innerWidth - y2 > 0) !!!

    // //console.log((380 + window.innerWidth));
    // //console.log(innerImage.clientHeight);

    // // console.log(380 - (y2 - y1));
    // // console.log("Y1 = " + -y1);
    // // console.log("NEW Y = " + (-y1 + (380 - (y2 - y1))));

    // // console.log("Right anchors = " + parent.anchors.fillRight);
    // // console.log("x2 = " + x2);

    // console.log("Разница = " + Math.round((parent.anchors.fillRight - x2)));
    // var t = Math.round(parent.anchors.fillRight - x2);

    // function wide() {
    //     imageContainer.css({
    //         // height: const
    //         // convert to h client (pixelConvert);
    //         height: "380px",
    //         overflow: "hidden"
    //     });
    //     innerImage.css({
    //         //1500? 
    //         width: (t + 1000 + (imageContainer.clientWidth - crop[2]) - (1000 - crop[2]) + Math.round((parent.anchors.fillRight - crop[2]))) + "px",
    //         //width: (screen.width) + "px",
    //         transform: "translate(0px, " + (-y1 + (380 - (y2 - y1))) + "px)",
    //     });
    //     textContainer.css({
    //         top: (imageAndTextContainer.offsetHeight / 2) - (Math.round(textContainer.offsetHeight / 2)) + "px",
    //         right: "50%",
    //         width: (imageAndTextContainer.clientWidth / 2) + "px",
    //     });
    // }

    // wide();

    // function minimized() {
    //     imageContainer.css({
    //         height: pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h) / windowRatio + "px",
    //         overflow: "hidden"
    //     });
    //     innerImage.css({
    //         width: screen.width + (pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, x2) - pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, x1)) + "px",
    //         transform: "translate(" + t + "px, " + (-pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, y1) + (pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h) / windowRatio) - pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h)) + "px)",
    //     });
    //     textContainer.css({
    //         top: "0px",
    //         right: ""
    //     });
    // }

    // var imageAndTextBoxesWidthSum = textContainer.offsetWidth + pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, w);
    // var imageAndTextContainerWidth = imageAndTextContainer.offsetWidth;

    // var imageAndTextBoxesHeightSum = textContainer.offsetHeight + pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h);
    // var imageAndTextContainerHeight = imageAndTextContainer.offsetHeight;

    // var maxSize = (-pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, y1) + (pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h) / windowRatio) - pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, h));
    // var maxSize2 = (-pixelConvert(innerImage.naturalHeight, innerImage.clientHeight, x1) + (pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, w) / windowRatio) - pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, w));
    // var maxSizeW = (pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, x2) - pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, x1));

    // if (y2 < 380) {
    //     console.log("STOP");
    // }

    // // if (imageAndTextBoxesHeightSum > imageAndTextContainerHeight && imageAndTextBoxesWidthSum > imageAndTextContainerWidth) {
    // //     if (swap) {
    // //         wide();
    // //     } else {
    // //         minimized();
    // //     }
    // // } else {
    // //     if (swap) {
    // //         if (imageAndTextBoxesHeightSum < imageAndTextContainerHeight) {
    // //             if (maxSize < 0) {
    // //                 minimized();
    // //             }
    // //         } else {
    // //             wide();
    // //             swap = false;
    // //         }
    // //     } else {
    // //         if (imageAndTextBoxesWidthSum < imageAndTextContainerWidth) {
    // //             wide();
    // //         } else {
    // //             if (maxSize < 0) {
    // //                 minimized();
    // //             }
    // //             swap = true;
    // //         }
    // //     }
    // // }


    // let left = parent.anchors.wideLeft;
    // let right = parent.anchors.wideRight;
    // element.style.marginLeft = left + "px";
    // element.style.width = right - left + "px";

    // let leftText = parent.anchors.textLeft;
    // let rightText = parent.anchors.textRight;
    // textContainer.style.marginLeft = leftText + "px";
    //textContainer.style.width = rightText - leftText + "px";

    // let leftImage = parent.anchors.fillLeft;
    // let rightImage = parent.anchors.fillRight;
    // imageAndTextContainer.style.marginLeft = leftImage + "px";
    // imageAndTextContainer.style.width = right - rightImage + "px";
    //}
    parent.onAnchorsChanged.push(element.updateWidth);
}