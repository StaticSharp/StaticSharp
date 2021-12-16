function TestLanding(element, crop) {

    let parent = element.parentElement;
    const imageAndTextContainer = element.querySelector("#ImageAndTextContainer");
    const imageContainer = imageAndTextContainer.querySelector("#ImageContainer");
    const textContainer = imageAndTextContainer.querySelector("#TextContainer");
    const innerImage = element.getElementsByTagName("img")[0];

    function pixelConvert(intrinsic, rendered, firstposition) {
        return (firstposition * rendered / intrinsic);
    }

    //console.log(innerImage);
    element.updateWidth = function() {
        //console.log(imageContainer);
        var x1 = crop[0];
        var x2 = crop[2];
        var y1 = crop[1];
        //console.log(y1);
        //console.log(pixelConvert(innerImage.naturalWidth, innerImage.clientWidth, y1));
        var y2 = crop[3];
        var h = y2 - y1;
        var w = x2 - x1;
        var textpadding = window.innerWidth - x1;
        var windowRatio = window.innerWidth / window.innerHeight;
        //console.log(windowRatio);
        var tr = y1 - y1 / (innerImage.naturalHeight / innerImage.offsetHeight);
        var tr2 = -y1 / (innerImage.naturalHeight / innerImage.offsetHeight);
        var tr3 = y2 - 1500; //vh to px
        var tr4 = x1 - x1 / (innerImage.naturalWidth / innerImage.offsetWidth);
        //var tr3 = x2 - window.innerWidth;
        //console.log(tr3);
        //var tr3 = y2 - 480;
        //console.log(tr3);
        //console.log(tr);
        if (windowRatio > 1) {
            imageAndTextContainer.css({
                position: "relative",
            })
            imageContainer.css({
                height: "30vh",
                //height: h + "px",
                overflow: "hidden",
            });
            innerImage.css({
                objectFit: "cover",
                transform: "translate(0px, " + -y1 / (innerImage.naturalHeight / innerImage.offsetHeight) + "px)",
            });
        } else {
            imageAndTextContainer.css({
                position: "relative",
            })
            imageContainer.css({
                height: "50vh",
                overflow: "hidden",
            });
            if (innerImage.naturalHeight < innerImage.naturalWidth) {
                if (innerImage.naturalHeight < innerImage.offsetHeight) {
                    innerImage.css({
                        height: "100vh",
                        objectFit: "cover",
                        transform: "translate(0px, " + tr + "px)",
                    });
                } else {
                    innerImage.css({
                        height: "100vh",
                        width: "100%",
                        objectFit: "cover",
                        transform: "translate(0px, " + tr3 + "px)",
                    });
                }
            } else {
                console.log(innerImage.naturalHeight / innerImage.naturalWidth);
                if (innerImage.naturalHeight < innerImage.offsetHeight) {
                    innerImage.css({
                        height: "100vh",
                        objectFit: "cover",
                        transform: "translate(0px, " + tr + "px)",
                    });
                } else {
                    innerImage.css({
                        height: "100vh",
                        width: "100%",
                        objectFit: "cover",
                        transform: "translate(0px, " + -50 + "px)",
                    });
                }
            }
        }
        let left = parent.anchors.wideLeft;
        let right = parent.anchors.wideRight;
        element.style.marginLeft = left + "px";
        element.style.width = right - left + "px";
        //console.log(innerImage.height);

        // if (windowRatio < 1) {
        //     //this.image.style.width = "100%"
        //     //this.image.style.height = "auto"
        //     var offset = (1 - (windowRatio / 1 - h) / (1 - h)) * y1 * 100.0;
        //     //this.image.style.transform = "translate(0, -" + offset + "%)"
        //     innerImage.css({
        //         width: "100%",
        //         height: "auto",
        //         transform: "translate(0px, " + -offset + "px)",
        //     });
        // } else {
        //     //this.image.style.width = "auto"
        //     //this.image.style.height = "100%"
        //     var offset = (1 - (1 / windowRatio - w) / (1 - w)) * x1 * 100.0;
        //     //console.log(offset);
        //     innerImage.css({
        //         width: "100%",
        //         height: "auto",
        //         transform: "translate(0px, " + -offset + "px)",
        //     });
        //     //this.image.style.transform = "translate(-" + offset + "%, 0)"
        // }


        // var iheight = crop[3] - crop[1];
        // var windowwidth = window.innerWidth / 40;
        // var itranslateY = -crop[1] - windowwidth;
        // let left = parent.anchors.wideLeft;
        // let right = parent.anchors.wideRight;
        // element.style.marginLeft = left + "px";
        // element.style.width = right - left + "px";
        // imageContainer.css({
        //     height: iheight + "px",
        //     overflow: "hidden"
        // });
        // innerImage.css({
        //     transform: "translate(0px, " + -y1 + "px)",
        // })
    }
    parent.onAnchorsChanged.push(element.updateWidth);
}