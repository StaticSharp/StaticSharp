function RoiImage(element, aspect, roi) {
    this.element = element;
    this.aspect = aspect;
    this.roi = roi;
    this.previousContainerWidth = -1;
    this.previousContainerHeight = -1;
    this.image = this.element.getElementsByTagName("img")[0];
    //const imageContainer = document.querySelector("#ImageContainer");
    this.parent = this.element.parentElement;
    element.onAnchorsChanged = [];

    // let windowMiddle = Ranc - Lanc / 2;
    let windowMiddle = window.innerWidth / 2;
    //console.log("Widnow middle = " + windowMiddle);

    let fullWide = window.innerWidth;
    //console.log("Full wide = " + fullWide);
    //console.log(element);
    //console.log(parent);
    // console.log(element);
    // element.firstChild.addEventListener("click", (evt) => {
    //     console.log("asd");
    // })
    // image.addEventListener("resize", (evt) => {
    //     console.log("111");
    // })
    //console.log(element.style);
    //200 400 460 680 720 940
    let cropx1 = 720;
    let cropx2 = 940;
    let imageWidth = 1000;
    //1 - min 0 - max
    let swap = 0;
    this.element.updateWidth = function() {
        //window.addEventListener("resize", (evt) => {
        let width = this.element.offsetWidth;
        let height = this.element.offsetHeight;
        this.previousContainerWidth = width;
        this.previousContainerHeight = height;

        //console.log(width);

        let x0 = roi[0] / 100;
        let x1 = roi[1] / 100;
        let y0 = roi[2] / 100;
        let y1 = roi[3] / 100;
        let w = x1 - x0;
        let h = y1 - y0;
        //console.log(element.style.width);
        let ratio2 = imageWidth / parseInt(element.style.width);
        //let ratio3 = 
        //console.log("TEST = " + (cropx2 * parseInt(element.style.width) / imageWidth));

        var _x1W = (parent.anchors.wideRight - parent.anchors.wideLeft) / 2;
        var _x2W = (parent.anchors.wideRight - parent.anchors.wideLeft);
        console.log("_x1W = " + _x1W);
        console.log("_x2W = " + _x2W);

        // var anch = parent.anchors.fillRight - _x1W;
        // console.log("R = " + anch);

        var _TR = (parent.anchors.wideRight - parent.anchors.fillRight);
        console.log("_TR = " + _TR);

        var _TL = (parent.anchors.wideLeft - parent.anchors.fillLeft);
        console.log("_TL = " + _TL);

        var sum = Math.abs(_TR) + Math.abs(_TL);
        console.log("Sum = " + sum);

        // var r = _x2W - _x2T;
        // console.log("R = " + r);

        let _x1 = (cropx1 / ratio2);
        let _x2 = (cropx2 / ratio2);

        let innerW = _x2 - _x1;
        let inW = ((cropx2 - cropx1) * ratio2);
        console.log("x2 - x1 = " + inW);
        console.log("Raznica = " + (_x2W - _x2));
        let test = _x1W - _x1;
        console.log("Test = " + test);
        //let _textx2 = (window.innerWidth + (windowMiddle / ratio2 - _x2));
        console.log("X1 = " + _x1);
        console.log("X2 = " + _x2);
        let minWidth = 0;
        //maxWidth = _x2W + Math.abs(_x2W - _x1) - sum;
        //minWidth = _x2W + Math.abs(_x1W - _x1) + _TR / 2;

        maxWidth = _x2W + _x2W - _x2;

        //minWidth = _x2W + Math.abs(_x1W - _x1);

        //Un
        // if (test > 0)
        //     minWidth = _x2W + _x1W - _x1 + test;
        // else minWidth = _x2W + _x1W - _x1;

        // if (minWidth < _x2W)
        //     minWidth = _x2W;
        // else minWidth = _x2W + _x1W - _x1;
        //Un

        minWidth = _x2W + _x1W - _x1;

        console.log("MaxWidth = " + maxWidth);
        console.log("MinWidth = " + minWidth);
        // let innerW1 = _x2 - _x1;
        // console.log("Image width1 = " + innerW1);
        // console.log("newX2 = " + (_x2 + Math.abs(window.innerWidth - imageWidth) + (windowMiddle - cropx1)));
        // //_x2 = _x2 + Math.abs(window.innerWidth - imageWidth) - ((windowMiddle - cropx1) / ratio2);
        // //_x2 = _x2 + Math.abs((window.innerWidth - imageWidth));
        // _x2 = _x2 + Math.abs(window.innerWidth - imageWidth) + (windowMiddle - cropx1);
        // console.log("Right limit = " + rightLimit);
        // console.log("Ratio = " + ratio2);
        // // width: (1000 + (imageContainer.clientWidth - 1000) + ((1000 - crop[2]) / ratio2)) + "px", //anchors
        // //let myWidthMax = (1000 + (element.clientWidth - 1000) + ((1000 - cropx2) / ratio2));
        // let betweenX2AndMiddle = rightLimit - cropx2;
        // console.log("Between middle and x2 = " + betweenX2AndMiddle);

        // let innerW2 = _x2 - _x1;
        // console.log("Image width2 = " + innerW2);

        // // let myWidthMax = window.innerWidth + windowMiddle - innerW1;


        // // let myWidthMin = 2 * cropx1 / ratio2;
        // // console.log("Needed Min width = " + myWidthMin);
        // let betweenX1AndMiddle = windowMiddle - cropx1;
        // console.log("Between middle and x1 = " + betweenX1AndMiddle);
        // //let _textx2 = (cropx2 / ratio2 + innerW1 + betweenX1AndMiddle);
        // //let _textx2 = (window.innerWidth + (windowMiddle / ratio2 - _x2));
        // //console.log("TestX2 = " + _textx2);
        // //_x2 = _x2 - _textx2;
        // //console.log("TestX2 = " + Math.abs((window.innerWidth - imageWidth)));
        // //let myWidthMax = rightLimit + (rightLimit / ratio2 - _x2);
        // let myWidthMax = rightLimit + _x2 - innerW2;
        // //let myWidthMax = rightLimit + _textx2;
        // console.log("Needed Max width = " + myWidthMax);
        // let myWidthMin = window.innerWidth + (windowMiddle / ratio2 - _x1);
        // console.log("Needed Min width = " + myWidthMin);

        let widthAspect = width * aspect;
        let minHeight = widthAspect * h;
        let maxHeight = widthAspect / w;
        height = Math.max(height, minHeight);
        height = Math.min(height, maxHeight);
        var containerAspect = height / width;
        //console.log(imageContainer.style.width);
        if (containerAspect < aspect) {
            if (swap) {
                //if (myWidthMin < fullWide) //rightLimit
                //    this.image.style.width = fullWide + "px";
                //if (minWidth < _x1W)
                this.image.style.width = minWidth + "px";
            } else {
                //console.log("TEST = " + (myWidthMax - 1000));
                //if ((cropx2 / ratio2) > fullWide)
                //if (_x2 > fullWide)
                //    this.image.style.width = fullWide + "px";
                //if (myWidthMax > fullWide)
                //    this.image.style.width = fullWide + "px";
                //if (_x2 > rightLimit)
                this.image.style.width = maxWidth + "px";
            }
            this.image.style.height = "auto"
            var offset = (1 - (containerAspect / aspect - h) / (1 - h)) * y0 * 100.0
            this.image.style.transform = "translate(0, -" + offset + "%)"
        } else {
            console.log("SWAPED");
            this.image.style.width = "auto"
            this.image.style.height = "100%"
            var offset = (1 - (aspect / containerAspect - w) / (1 - w)) * x0 * 100.0
            this.image.style.transform = "translate(-" + offset + "%, 0)"
        }

        this.element.style.minHeight = minHeight + "px";
        this.element.style.maxHeight = maxHeight + "px";
        //})
    }

    // document.addEventListener('DOMContentLoaded', function() {
    //     //console.log(element);
    //     let tds = document.getElementsByClassName("ImageAndTextContainer");
    //     console.log(tds[0]);
    //     for (let i = 0; i < tds.length; i++) {
    //         tds[i].onAnchorsChanged = [];
    //     }
    //     //console.log(tds);
    //     parent.onAnchorsChanged.push(element.updateWidth);
    //     element.updateWidth();
    // });

    this.parent.onAnchorsChanged.push(this.element.updateWidth);

    // this.OnWindowResize = function() {
    //     let width = this.element.offsetWidth;
    //     let height = this.element.offsetHeight;
    //     this.previousContainerWidth = width;
    //     this.previousContainerHeight = height;

    //     let x0 = roi[0] / 100;
    //     let x1 = roi[1] / 100;
    //     let y0 = roi[2] / 100;
    //     let y1 = roi[3] / 100;
    //     let w = x1 - x0;
    //     let h = y1 - y0;

    //     let widthAspect = width * aspect;
    //     let minHeight = widthAspect * h;
    //     let maxHeight = widthAspect / w;
    //     height = Math.max(height, minHeight);
    //     height = Math.min(height, maxHeight);
    //     var containerAspect = height / width;
    //     if (containerAspect < aspect) {
    //         this.image.style.width = "100%"
    //         this.image.style.height = "auto"
    //         var offset = (1 - (containerAspect / aspect - h) / (1 - h)) * y0 * 100.0
    //         this.image.style.transform = "translate(0, -" + offset + "%)"
    //     } else {
    //         this.image.style.width = "auto"
    //         this.image.style.height = "100%"
    //         var offset = (1 - (aspect / containerAspect - w) / (1 - w)) * x0 * 100.0
    //         this.image.style.transform = "translate(-" + offset + "%, 0)"
    //     }

    //     this.element.style.minHeight = minHeight + "px";
    //     this.element.style.maxHeight = maxHeight + "px";
    // }

    // this.OnResourcesLoaded = function() {
    //     this.OnWindowResize();
    // }

    // this.OnDOMContentLoaded = function(event) {
    //     this.OnWindowResize();
    // }

}