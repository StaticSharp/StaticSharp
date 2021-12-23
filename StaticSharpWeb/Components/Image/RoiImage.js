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

    //200 400 460 680 720 940
    let cropx1 = 460;
    let cropx2 = 680;
    let imageWidth = 1000;
    //1 - min 0 - max
    let swap = 0;

    this.element.updateWidth = function() {
        let width = this.element.offsetWidth;
        let height = this.element.offsetHeight;
        this.previousContainerWidth = width;
        this.previousContainerHeight = height;

        let x0 = roi[0] / 100;
        let x1 = roi[1] / 100;
        let y0 = roi[2] / 100;
        let y1 = roi[3] / 100;
        let w = x1 - x0;
        let h = y1 - y0;
        let ratio = imageWidth / parseInt(element.style.width);

        var x1Window = (parent.anchors.wideRight - parent.anchors.wideLeft) / 2;
        var x2Window = (parent.anchors.wideRight - parent.anchors.wideLeft);
        console.log("x1Window = " + x1Window);
        console.log("x2Window = " + x2Window);

        let x1Image = (cropx1 / ratio);
        let x2Image = (cropx2 / ratio);

        console.log("x1Image = " + x1Image);
        console.log("x2Image = " + x2Image);

        let initImageDiagonal = Math.sqrt(667 * 667 + 1000 * 1000);
        console.log("initImageDiagonal = " + initImageDiagonal);
        let sin = 667 / initImageDiagonal;
        console.log("Sin = " + sin);
        let cos = 1000 / initImageDiagonal;
        console.log("Cos = " + cos);
        let tg = 667 / 1000;
        console.log("Tg = " + tg);

        console.log("-----------------maxWidth-----------------------");

        let dx = x2Window - x2Image;
        console.log("dx = " + dx);

        let dy = dx * tg;
        console.log("dy = " + dy);

        let diagonalIncreaseRelativeX2Window = Math.sqrt(dx * dx + dy * dy);
        console.log("diagonalIncreaseRelativeX2Window = " + diagonalIncreaseRelativeX2Window);

        let x2Gradient = x2Image / x2Window;
        console.log("xGradient = " + x2Gradient);

        let diagonalIncreaseRelativeX2Image = diagonalIncreaseRelativeX2Window / x2Gradient;
        console.log("diagonalIncreaseRelativeX2Image = " + diagonalIncreaseRelativeX2Image);

        let renderImageDiagonal = diagonalIncreaseRelativeX2Image + initImageDiagonal / ratio;
        console.log("renderImageDiagonal = " + renderImageDiagonal);

        let WidthIncreaseRealiveX2Window = 1000 * renderImageDiagonal / initImageDiagonal;
        console.log("WidthIncreaseRealiveX2Window = " + WidthIncreaseRealiveX2Window);

        console.log("-----------------maxWidth-----------------------");

        console.log("-----------------minWidth-----------------------");

        let middleOfFullImage = x2Window / 2;
        console.log("MiddleOfFullImage = " + middleOfFullImage);

        let dxmin = middleOfFullImage - x1Image;
        console.log("dx = " + dxmin);

        let dymin = dxmin * tg;
        console.log("dy = " + dymin);

        let diagonalIncreaseRelativeX1Window = Math.sqrt(dxmin * dxmin + dymin * dymin);
        if (dxmin < 0)
            diagonalIncreaseRelativeX1Window = -diagonalIncreaseRelativeX1Window;
        console.log("diagonalIncreaseRelativeX2Window = " + diagonalIncreaseRelativeX1Window);

        let x1Gradient = x1Image / x2Window;
        console.log("xGradient = " + x1Gradient);

        let diagonalIncreaseRelativeX1Image = diagonalIncreaseRelativeX1Window / x1Gradient;
        console.log("diagonalIncreaseRelativeX2Image = " + diagonalIncreaseRelativeX1Image);

        let renderImageDiagonalmin = diagonalIncreaseRelativeX1Image + initImageDiagonal / ratio;
        console.log("renderImageDiagonalmin = " + renderImageDiagonalmin);

        let WidthIncreaseRealiveX1Window = 1000 * renderImageDiagonalmin / initImageDiagonal;
        console.log("WidthIncreaseRealiveX1Window = " + WidthIncreaseRealiveX1Window);

        if (WidthIncreaseRealiveX1Window < x2Window)
            WidthIncreaseRealiveX1Window = x2Window;

        console.log("-----------------minWidth-----------------------");

        //UNCOMMNENT
        maxWidth = WidthIncreaseRealiveX2Window;
        //UNCOMMNENT
        //maxWidth = x2Window;

        //UNCOMMNENT
        minWidth = WidthIncreaseRealiveX1Window;
        //UNCOMMNENT

        //minWidth = x2Window + x1Window - x1Image;
        //Un
        // if (test > 0)
        //     minWidth = _x2W + _x1W - _x1 + test;
        // else minWidth = _x2W + _x1W - _x1;

        // if (minWidth < x2Window)
        //     minWidth = x2Window;
        // else minWidth = x2Window + x1Window - x1Image;
        //Un



        console.log("MaxWidth = " + maxWidth);
        console.log("MinWidth = " + minWidth);
        console.log("Ratio = " + ratio);

        let widthAspect = width * aspect;
        let minHeight = widthAspect * h;
        let maxHeight = widthAspect / w;
        height = Math.max(height, minHeight);
        height = Math.min(height, maxHeight);
        var containerAspect = height / width;
        if (containerAspect < aspect) {
            if (swap) {
                //if (myWidthMin < fullWide) //rightLimit
                //    this.image.style.width = fullWide + "px";
                //if (minWidth < _x1W)
                this.image.style.width = minWidth + "px";
            } else {
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