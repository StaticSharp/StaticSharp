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
    let cropx1 = 460;
    let cropx2 = 680;
    let imageWidth = 1000;
    //1 - min 0 - max
    let swap = 0;

    // function getHeight(length, ratio) {
    //     var height = ((length) / (Math.sqrt((Math.pow(ratio, 2) + 1))));
    //     return Math.round(height);
    // }

    // function getWidth(length, ratio) {
    //     var width = ((length) / (Math.sqrt((1) / (Math.pow(ratio, 2) + 1))));
    //     return Math.round(width);
    // }

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

        // var _TR = (parent.anchors.wideRight - parent.anchors.fillRight);
        // console.log("_TR = " + _TR);

        // var _TL = (parent.anchors.wideLeft - parent.anchors.fillLeft);
        // console.log("_TL = " + _TL);

        // var sum = Math.abs(_TR) + Math.abs(_TL);
        // console.log("Sum = " + sum);

        // var r = _x2W - _x2T;
        // console.log("R = " + r);

        let _x1 = (cropx1 / ratio2);
        let _x2 = (cropx2 / ratio2);

        // let innerW = _x2 - _x1;
        // let inW = ((cropx2 - cropx1) * ratio2);
        // console.log("x2 - x1 = " + inW);
        // let raz = (_x2W - _x2);
        // console.log("Raznica = " + raz);
        let test = _x1W - _x1;
        console.log("Test = " + test);
        //let _textx2 = (window.innerWidth + (windowMiddle / ratio2 - _x2));
        console.log("X1 = " + _x1);
        console.log("X2 = " + _x2);
        //let minWidth = 0;

        let diag = Math.sqrt(1000 * 1000 + 667 * 667);
        console.log("Diag = " + diag);

        let myH = this.image.clientHeight;
        console.log("Image Height = " + myH);
        let myW = this.image.clientWidth;
        console.log("Image Width = " + myW);
        //let diagRen = Math.sqrt(375 * 375 + _x2W * _x2W);
        let diagRen = Math.sqrt(myW * myW + myH * myH);
        //let diagRen = Math.sqrt(375 * 375 + (_x2W + _x2W - _x2) * (_x2W + _x2W - _x2));
        console.log("DiagRendered = " + diagRen);
        let sin = 667 / diag;
        console.log("Sin = " + sin);
        let cos = 1000 / diag;
        console.log("Cos = " + cos);
        let tg = 667 / 1000;
        console.log("Tg = " + tg);

        //0 -> 1
        // let xgrad = _x2 / _x2W;
        let xgrad = cropx2 / 1000;
        console.log("X2Grad = " + xgrad);

        let x2withgrad = _x2 * xgrad;
        console.log("X2withGrad = " + x2withgrad);

        // let newRaz = _x2W * sin;
        // console.log("newRaz = " + newRaz);

        let xdiff = _x2W - _x2;
        // let xdiff = 1000 - cropx1;
        console.log("XDiff = " + xdiff);
        //console.log(this.image.clientHeight);

        //let _y1 = (1 - (1 / (1 - sin * sin))) *_x1;
        // let _y1 = _x1 * tg;
        // console.log("_Y1 = " + _y1);

        let ydiff = xdiff * tg;
        console.log("Ydiff = " + ydiff);

        let wscale = xdiff / cos;
        console.log("WScale1 = " + wscale);
        let wscale228 = (xdiff / cos) / xgrad;
        console.log("WScale228 = " + wscale228);
        //wscale = wscale + (wscale - xdiff) * ratio2;
        //console.log("WScale2 = " + wscale);

        // let ydiff = (this.image.clientHeight - _y1);
        // console.log("YDiff = " + ydiff);


        let wdiff = Math.sqrt(ydiff * ydiff + xdiff * xdiff);
        console.log("WDiff = " + wdiff);

        // var width4 = getWidth(375, ratio2);
        // console.log("A = " + width4);

        // let ydiff = _x2W = _x2;
        // console.log("YDiff = " + ydiff);
        //maxWidth = _x2W + Math.abs(_x2W - _x1) - sum;
        //minWidth = _x2W + Math.abs(_x1W - _x1) + _TR / 2;

        //maxWidth = _x2W + _x2W - _x2;
        //maxWidth = _x2W + wdiff + 0 / ratio2;
        // let test2 = _x2W - _x1W;
        // console.log("Test = " + test2);

        var testdelgrad = test / xgrad;
        if (testdelgrad < 0)
            testdelgrad = 0;
        console.log("Test/Grad = " + testdelgrad);

        // console.log("----------------------------------------");
        // let _diag = Math.sqrt(500 * 500 + 500 * 500);
        // console.log("DiagonalSmall = " + _diag);

        // let _cos = 500 / _diag;
        // console.log("CosSmall = " + _cos);

        // let _x2_x1 = cropx2 - cropx1;
        // console.log("X2 - X1 Small = " + _x2_x1);

        // let _toWidthSmall = _x2_x1 / _cos;
        // console.log("ToWidthSmall = " + _toWidthSmall);

        // let _diagB = Math.sqrt(1000 * 1000 + 1000 * 1000);
        // console.log("DiagonalBig = " + _diagB);

        // let _cosB = 1000 / _diagB;
        // console.log("CosBig = " + _cosB);

        // let _x2_0 = 1000;
        // console.log("X2 - 0 = " + _x2_0);

        // let _toWidthBig = (_diagB * _toWidthSmall / _diag);
        // console.log("ToWidthBig = " + _toWidthBig);

        // console.log("----------------------------------------");

        // let _max_testgrad = 2 * (testdelgrad - 1000);
        // console.log("max-test = " + _max_testgrad);

        // let _neededWidth = 2 * 1000 + _max_testgrad;
        // console.log("NeededWidth = " + _neededWidth);

        // console.log("----------------------------------------");

        console.log("----------------------------------------");

        let _x2W2 = _x2W + xdiff;
        console.log("_x2W2 = " + _x2W2);

        let _olddiag = Math.sqrt(xdiff * xdiff + ydiff * ydiff);
        console.log("OldDiag = " + _olddiag);

        let _imagediag = Math.sqrt(220 * 220 + 120 * 120);
        console.log("_imagediag = " + _imagediag);

        let _cosimage = 220 / _imagediag;
        console.log("imageCos = " + _cosimage);

        // let _y2W2 = ydiff;
        // console.log("_y2W2 = " + _y2W2);
        // let newW2 = Math.sqrt(_x2W2 * _x2W2 + _y2W2 * _y2W2);
        // console.log("newW2 = " + newW2);

        let newW = _x2W2 / cos;
        console.log("newW1 = " + newW);

        _oldFulldiag = Math.sqrt(667 * 667 + 1000 * 1000);
        console.log("OldFullDiag = " + _oldFulldiag);

        _newFulldiag = _oldFulldiag + _olddiag;
        console.log("NewDiag = " + _newFulldiag);

        _newWww = _newFulldiag * cos;
        console.log("newWWW = " + _newWww);

        _xgrad = _x2 / _x2W;
        console.log("GradX2 = " + _xgrad);

        let newolddiag = _olddiag / _xgrad;
        console.log("NewoldDiag = " + newolddiag);

        let newoldFullDiag = newolddiag + _oldFulldiag;
        console.log("NewOldFullDiag = " + newoldFullDiag);

        let WIDTHNEW = 1000 * newoldFullDiag / _oldFulldiag;
        console.log("NEWWIDTH = " + WIDTHNEW);

        console.log("----------------------------------------");

        // maxWidth = _x2W + wdiff + testdelgrad / ratio2;
        maxWidth = WIDTHNEW;

        //minWidth = _x2W + Math.abs(_x1W - _x1);
        minWidth = _x2W + _x1W - _x1;
        //Un
        // if (test > 0)
        //     minWidth = _x2W + _x1W - _x1 + test;
        // else minWidth = _x2W + _x1W - _x1;

        if (minWidth < _x2W)
            minWidth = _x2W;
        else minWidth = _x2W + _x1W - _x1;
        //Un



        console.log("MaxWidth = " + maxWidth);
        console.log("MinWidth = " + minWidth);
        // let innerW1 = _x2 - _x1;
        // console.log("Image width1 = " + innerW1);
        // console.log("newX2 = " + (_x2 + Math.abs(window.innerWidth - imageWidth) + (windowMiddle - cropx1)));
        // //_x2 = _x2 + Math.abs(window.innerWidth - imageWidth) - ((windowMiddle - cropx1) / ratio2);
        // //_x2 = _x2 + Math.abs((window.innerWidth - imageWidth));
        // _x2 = _x2 + Math.abs(window.innerWidth - imageWidth) + (windowMiddle - cropx1);
        // console.log("Right limit = " + rightLimit);
        console.log("Ratio = " + ratio2);
        // console.log("Aspect = " + aspect);
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
        // console.log("CA = " + containerAspect);
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
            // this.image.style.minWidth = minWidth + "px";
            // this.image.style.maxWidth = maxWidth + "px";
            // this.image.style.width = "auto";
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