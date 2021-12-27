function RoiImage(element, aspect, roi) {
    this.element = element;
    this.aspect = aspect;
    this.roi = roi;
    this.previousContainerWidth = -1;
    this.previousContainerHeight = -1;
    this.image = this.element.getElementsByTagName("img")[0];
    const imageContainer = document.querySelector("#ImageContainer");
    const textContainer = document.querySelector("#TextContainer");
    this.parent = this.element.parentElement;
    element.onAnchorsChanged = [];

    //200 400 460 680 720 940
    let cropx1 = 460;
    let cropx2 = 680;
    let cropy1 = 150;
    let cropy2 = 285;
    let imageWidth = 1000;
    let imageHeight = 667;
    //1 - min 0 - max
    let swap = 1.5;

    let userHeight = 200;

    this.element.updateWidth = function() {
        let width = this.element.offsetWidth;
        let height = this.element.offsetHeight;
        this.previousContainerWidth = width;
        this.previousContainerHeight = height;


        //console.log(Calc(100 - 10));

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
        let y1Image = (cropy1 / ratio);
        let y2Image = (cropy2 / ratio);

        console.log("x1Image = " + x1Image);
        console.log("x2Image = " + x2Image);
        console.log("y1Image = " + y1Image);
        console.log("y2Image = " + y2Image);

        let initImageDiagonal = Math.sqrt(imageHeight * imageHeight + imageWidth * imageWidth);
        console.log("initImageDiagonal = " + initImageDiagonal);
        let sin = imageHeight / initImageDiagonal;
        console.log("Sin = " + sin);
        let cos = imageWidth / initImageDiagonal;
        console.log("Cos = " + cos);
        let tg = imageHeight / imageWidth;
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

        let WidthIncreaseRealiveX2Window = imageWidth * renderImageDiagonal / initImageDiagonal;
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
        console.log("diagonalIncreaseRelativeX1Window = " + diagonalIncreaseRelativeX1Window);

        let x1Gradient = x1Image / x2Window;
        console.log("xGradient = " + x1Gradient);

        let diagonalIncreaseRelativeX1Image = diagonalIncreaseRelativeX1Window / x1Gradient;
        console.log("diagonalIncreaseRelativeX1Image = " + diagonalIncreaseRelativeX1Image);

        let renderImageDiagonalmin = diagonalIncreaseRelativeX1Image + initImageDiagonal / ratio;
        console.log("renderImageDiagonalmin = " + renderImageDiagonalmin);

        let WidthIncreaseRealiveX1Window = imageWidth * renderImageDiagonalmin / initImageDiagonal;
        console.log("WidthIncreaseRealiveX1Window = " + WidthIncreaseRealiveX1Window);

        if (WidthIncreaseRealiveX1Window < x2Window)
            WidthIncreaseRealiveX1Window = x2Window;

        console.log("-----------------minWidth-----------------------");

        maxWidth = WidthIncreaseRealiveX2Window;
        minWidth = WidthIncreaseRealiveX1Window;

        //----------------------------------------------------------------//
        let neededWidth = maxWidth + diagonalIncreaseRelativeX2Window;
        console.log("neededWidth = " + neededWidth);
        //----------------------------------------------------------------//

        console.log("MaxWidth = " + maxWidth);
        console.log("MinWidth = " + minWidth);
        console.log("Ratio = " + ratio);

        let newMinHeight = minWidth * tg;
        console.log("newMinHeight = " + newMinHeight);

        let newMaxHeight = maxWidth * tg;
        console.log("newMaxHeight = " + newMaxHeight);

        let currentY1 = cropy1 / ratio / x2Gradient;
        console.log("CurrentY1 = " + currentY1);

        let currentY2 = cropy2 / ratio;
        console.log("CurrentY2 = " + currentY2);

        let currentY2ForMax = (cropy2 / ratio / x2Gradient);
        console.log("CurrentY2ForMax = " + currentY2ForMax);

        let currentY2ForMin = (cropy2 / ratio);
        console.log("CurrentY2ForMin = " + currentY2ForMin);

        let containerMinHeight = 0;
        // let myoffset = 0;
        // let up = 0;
        if (dymin > 0) {
            if (newMinHeight == newMaxHeight) {
                containerMinHeight = currentY2ForMax;

                // up = dy;
            } else {
                containerMinHeight = cropy2 / ratio + dymin;
                // up = currentY1 - dymin / x2Gradient / ratio;
            }
        } else {
            containerMinHeight = currentY2;
            // up = currentY1;
        }

        let myoffset = y1Image + dymin / ratio;
        console.log("myoffset = " + myoffset);

        // console.log("TEST1 = " + (tg * minWidth / ratio));
        // console.log("TEST2 = " + (tg * maxWidth / ratio));
        // console.log("Razina = " + (-(tg * minWidth / ratio) + (tg * maxWidth / ratio)));
        // console.log("myoffset1 = " + (myoffset));
        // console.log("myoffset2 = " + (myoffset / x1Gradient));

        containerMinHeight = containerMinHeight - myoffset;

        let yRatio = 1 / ratio;
        console.log("yRatio = " + yRatio);

        // console.log("Up = " + up);
        console.log("containerMinHeight = " + containerMinHeight);

        let containerMaxHeight = currentY2ForMax - myoffset;
        console.log("containerMaxHeight = " + containerMaxHeight);

        console.log("UserHeight = " + userHeight / ratio);

        // if (userHeight / ratio > containerMaxHeight) {
        //     console.log("User > Max");
        //     imageContainer.style.height = containerMaxHeight + "px";
        // }

        // if (userHeight / ratio < containerMinHeight) {
        //     console.log("User < Min");
        //     imageContainer.style.height = containerMinHeight + "px";
        // }

        // if (userHeight / ratio > containerMinHeight && userHeight / ratio < containerMaxHeight) {
        //     console.log("User fitted");
        //     imageContainer.style.height = userHeight / ratio + "px";
        // }

        //Max = min
        if (userHeight / ratio < containerMaxHeight) {
            imageContainer.style.height = containerMaxHeight + "px";
        } else imageContainer.style.height = (userHeight / ratio) + "px";

        // imageContainer.style.height = "500px";

        // imageContainer.style.height = containerMaxHeight + "px";

        console.log("ContainerHeight = " + imageContainer.style.height);



        // let myoffset = currentY1 / ratio - dymin * x2Gradient / ratio;
        // console.log("My offset = " + myoffset);

        // if (parseInt(this.image.style.width) > maxWidth)
        //     myoffset = currentY1 + dymin * x1Gradient / ratio;



        // console.log(parent.anchors.wideRight);

        let widthAspect = width * aspect;
        let minHeight = widthAspect * h;
        let maxHeight = widthAspect / w;
        height = Math.max(height, minHeight);
        height = Math.min(height, maxHeight);
        var containerAspect = height / width;
        // if (containerAspect < aspect) {
        if (ratio < swap) {
            // if (parseInt(this.image.style.width) < minWidth) {
            //if (swap) {
            // this.image.style.minWidth = minWidth + "px";
            //} else {
            // this.image.style.maxWidth = maxWidth + "px";
            //}
            // this.image.style.width = "100%";
            // this.image.style.minWidth = minWidth + "px";
            // this.image.style.width = (maxWidth + minWidth) / (2 * ratio) + "px";
            // this.image.style.width = maxWidth + "px";

            // this.image.style.width = minWidth + "px";
            this.image.style.width = neededWidth + "px";
            // this.image.style.width = "2300px";

            // this.image.style.width = "auto";
            // imageContainer.style.minHeight = (containerMinHeight) + "px";
            // imageContainer.style.maxHeight = (containerMaxHeight) + "px";
            // imageContainer.style.minHeight = (containerMinHeight) + "px";
            imageContainer.style.height = "300px";
            // this.image.style.height = "auto"
            var offset = (1 - (containerAspect / aspect - h) / (1 - h)) * y0 * 100.0
                // this.image.style.transform = "translate(0, -" + offset + "%)"
                // this.image.style.transform = "translate(0, -" + (myoffset) + "px)";
            this.image.style.transform = "translate(-" + (dxmin + dx) + "px, -" + currentY2 + "px)";
            // this.image.style.transform = "translate(0, -" + up + "px)"
            // textContainer.style.top = currentY2 + "px";
            // console.log(parent.anchors.textLeft);
            textContainer.style.marginLeft = parent.anchors.textLeft + "px";

        } else {
            console.log("SWAPPED");
            this.image.style.height = "auto"
            this.image.style.width = "100%"
            imageContainer.style.minHeight = "207px";
            // imageContainer.style.minHeight = minWidth + "px";
            // imageContainer.style.maxHeight = maxWidth + "px";
            var offset = (1 - (aspect / containerAspect - w) / (1 - w)) * x0 * 100.0
            this.image.style.transform = "translate(-" + offset + "%, 0)";
        }

        // this.element.style.minHeight = minHeight + "px";
        // this.element.style.maxHeight = maxHeight + "px";
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