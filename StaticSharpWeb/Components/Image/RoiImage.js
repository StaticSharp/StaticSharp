function RoiImage(element, aspect, roi) {
    this.element = element;
    this.aspect = aspect;
    this.roi = roi;
    this.previousContainerWidth = -1;
    this.previousContainerHeight = -1;
    this.image = this.element.getElementsByTagName("img")[0];
    const imageContainer = document.querySelector("#ImageContainer");
    const textContainer = document.querySelector("#TextContainer");
    var leftBar = document.querySelector("#leftBar");
    var rightBar = document.querySelector("#rightBar");
    var leftSlider = document.querySelector("#LeftSlider");
    var rightSlider = document.querySelector("#RightSlider");
    this.parent = this.element.parentElement;
    element.onAnchorsChanged = [];

    //200 400 460 680 720 940
    let cropx1 = 500;
    let cropx2 = 680;
    let cropy1 = 150;
    let cropy2 = 285;
    let imageWidth = 1000;
    let imageHeight = 667;
    //1 - min 0 - max
    let swap = 1.5;

    let userHeight = 100;

    // let tgImage = (cropy2 - cropy1) / (cropx2 - cropx1);
    // console.log("tgImage = " + tgImage);

    this.element.updateWidth = function() {
        let ratio = imageWidth / parseInt(element.style.width);
        console.log("ratio = " + ratio);

        var x1Window = (parent.anchors.wideRight - parent.anchors.wideLeft) / 2;
        var x2Window = (parent.anchors.wideRight - parent.anchors.wideLeft);

        // console.log("A = " + (parent.anchors.wideRight - parent.anchors.wideLeft));
        // console.log("B = " + (parent.anchors.fillRight + parent.anchors.fillLeft));

        let x1Image = (cropx1 / ratio);
        let x2Image = (cropx2 / ratio);
        let y1Image = (cropy1 / ratio);
        let y2Image = (cropy2 / ratio);

        let initImageDiagonal = Math.sqrt(imageHeight * imageHeight + imageWidth * imageWidth);
        let sin = imageHeight / initImageDiagonal;
        let cos = imageWidth / initImageDiagonal;
        let tg = imageHeight / imageWidth;
        console.log("x2Window = " + x2Window);
        let middleOfFullImage = x2Window / 2;
        let dxmin = middleOfFullImage - x1Image;
        let dymin = dxmin * tg;
        let diagonalIncreaseRelativeX1Window = Math.sqrt(dxmin * dxmin + dymin * dymin);
        if (dxmin < 0)
            diagonalIncreaseRelativeX1Window = -diagonalIncreaseRelativeX1Window;
        let x1Gradient = x1Image / x2Window;
        let x2Gradient = x2Image / x2Window;
        let diagonalIncreaseRelativeX1Image = diagonalIncreaseRelativeX1Window / x1Gradient;
        let renderImageDiagonalmin = diagonalIncreaseRelativeX1Image + initImageDiagonal / ratio;
        let WidthIncreaseRealiveX1Window = imageWidth * renderImageDiagonalmin / initImageDiagonal;
        console.log(renderImageDiagonalmin);
        // let dxmin2 = middleOfFullImage - x2Image;
        // let dymin2 = dxmin2 * tg;
        // let diagonalIncreaseRelativeX2Window = Math.sqrt(dxmin2 * dxmin2 + dymin2 * dymin2);
        // if (dxmin < 0)
        //     diagonalIncreaseRelativeX2Window = -diagonalIncreaseRelativeX2Window;
        // let diagonalIncreaseRelativeX2Image = diagonalIncreaseRelativeX2Window / x2Gradient;
        // let renderImageDiagonalmin2 = diagonalIncreaseRelativeX2Image + initImageDiagonal / ratio;
        // let WidthIncreaseRealiveX2Window = imageWidth * renderImageDiagonalmin2 / initImageDiagonal;
        // console.log("asdasd = " + (WidthIncreaseRealiveX2Window - WidthIncreaseRealiveX1Window))
        // console.log(Math.abs(middleOfFullImage - x2Image) + x2Image * ratio);
        if (WidthIncreaseRealiveX1Window < x2Window)
            WidthIncreaseRealiveX1Window = x2Window;
        minWidth = WidthIncreaseRealiveX1Window;

        console.log("minWidth = " + minWidth);

        console.log("window.screen = " + window.screen.width);

        //-----------------------------------------------//
        //1515 / 1285 - screen.width (maxwidth)
        let windowWidth2 = parent.anchors.wideRight - parent.anchors.wideLeft;
        console.log("windowWidth2 = " + windowWidth2);

        console.log(leftSlider.clientWidth)
        console.log(rightSlider.clientWidth)
        let windowWidth3 = window.screen.width - leftSlider.clientWidth - rightSlider.clientWidth;
        if (leftSlider.style.visibility == "visible" && rightSlider.style.visibility == "visible")
            windowWidth3 = window.screen.width;
        console.log("windowWidth3 = " + windowWidth3);
        if (leftSlider.style.visibility == "visible" && rightSlider.style.visibility == "visible")
            windowWidth3 = Math.min(windowWidth3, 1515);

        let middleOfFullImage3 = windowWidth3 / 2; //!
        let dxmin3 = middleOfFullImage3 - x1Image;
        let dymin3 = dxmin3 * tg;
        let diagonalIncreaseRelativeX1Window3 = Math.sqrt(dxmin3 * dxmin3 + dymin3 * dymin3);
        if (dxmin3 < 0)
            diagonalIncreaseRelativeX1Window3 = -diagonalIncreaseRelativeX1Window3;
        let x1Gradient3 = x1Image / x2Window;
        let diagonalIncreaseRelativeX1Image3 = diagonalIncreaseRelativeX1Window3 / x1Gradient3;
        let renderImageDiagonalmin3 = diagonalIncreaseRelativeX1Image3 + initImageDiagonal / ratio;
        let WidthIncreaseRealiveX1Window3 = imageWidth * renderImageDiagonalmin3 / initImageDiagonal;

        //----------------------------------------------------------------//

        let currentHeight = WidthIncreaseRealiveX1Window3 * tg;
        let middleOfFullImage4 = WidthIncreaseRealiveX1Window3 / 2;
        // !!!!!!!!!!!!!!! let ratio4 = tg;
        let ratio4 = imageWidth / windowWidth3; //!
        let x1Image4 = cropx1 / ratio4;
        let dxmin4 = windowWidth3 / 2 - x1Image4; //!
        let dymin4 = dxmin4 * tg;
        let newRatio2 = WidthIncreaseRealiveX1Window3 / imageWidth;
        console.log("newRatio2 = " + newRatio2);
        let newX1Image = cropx1 * newRatio2;
        let newX2Image = cropx2 * newRatio2;
        let newY1Image = cropy1 * newRatio2;
        let newY2Image = cropy2 * newRatio2;
        let HeightIncreaseRealiveX1Window3 = WidthIncreaseRealiveX1Window3 * tg
        console.log("HeightIncreaseRealiveX1Window3 = " + HeightIncreaseRealiveX1Window3);
        let diagIncrease = Math.sqrt(WidthIncreaseRealiveX1Window3 * WidthIncreaseRealiveX1Window3 + HeightIncreaseRealiveX1Window3 * HeightIncreaseRealiveX1Window3);
        console.log("diagIncrease = " + diagIncrease);
        console.log("tg = " + tg);
        console.log("dx4 = " + dxmin4);
        console.log("dy4 = " + dymin4);
        let ddiag = Math.sqrt(dxmin4 * dxmin4 + dymin4 * dymin4);
        console.log("ddiag = " + ddiag);

        // let t = ratio4 * dymin4 / (2 * WidthIncreaseRealiveX1Window3 * tg) * 100.0;
        let k = ratio4 * 100.0 * ((cropy1 / imageHeight) - ((cropy1 + dymin4) / HeightIncreaseRealiveX1Window3));
        console.log("k = " + k);
        let k1 = ratio4 * 100.0 * dymin4 / (2 * HeightIncreaseRealiveX1Window3);
        console.log("k1 = " + k1);
        let k2 = ratio4 * 100.0 * ddiag / (2 * HeightIncreaseRealiveX1Window3);
        console.log("k2 = " + k2);
        let t = k1;
        if (t < 0) {
            console.log("t < 0");
            // t = 1 * ratio4 * -dymin4 / (2 * WidthIncreaseRealiveX1Window3 * tg) * 100.0;
            t = 0;
        }
        console.log("t = " + t);

        let newHeight = 0;

        let y1Gradient = cropy1 / imageHeight;
        let y2Gradient = cropy2 / imageHeight;
        newHeight = newY2Image - newY1Image - dymin4 * y1Gradient;
        // let temp = newY2Image - newY1Image - dymin4 * y1Gradient;
        // console.log("temp = " + temp);


        // if (dymin4 > 0)
        //     // newHeight = newY2Image - newY1Image - dymin4 * x1Gradient;
        //     newHeight = newY2Image - newY1Image - dymin4 * y1Gradient;
        // // else newHeight = newY2Image - newY1Image + dy * x1Gradient; // ëèáî ïåðåðàñ. ëèáî dymin4 êàê ïðè ñëó÷àå âûøå
        // else newHeight = (newY2Image - newY1Image) / x1Gradient3;
        // newHeight = newY2Image - newY1Image - dymin4 * x1Gradient;

        //----------------------------------------------------------------//

        let _x1Image = (500 / ratio);
        let _x2Image = (cropx2 / ratio);

        let _middleOfFullImage3 = 1515 / 2;
        let _dxmin3 = middleOfFullImage3 - _x1Image;
        let _dymin3 = _dxmin3 * tg;
        let _diagonalIncreaseRelativeX1Window3 = Math.sqrt(_dxmin3 * _dxmin3 + _dymin3 * _dymin3);
        if (_dxmin3 < 0)
            _diagonalIncreaseRelativeX1Window3 = -_diagonalIncreaseRelativeX1Window3;
        let _x1Gradient3 = _x1Image / x2Window;
        let _diagonalIncreaseRelativeX1Image3 = _diagonalIncreaseRelativeX1Window3 / _x1Gradient3;
        let _renderImageDiagonalmin3 = _diagonalIncreaseRelativeX1Image3 + initImageDiagonal / ratio;
        let _WidthIncreaseRealiveX1Window3 = imageWidth * _renderImageDiagonalmin3 / initImageDiagonal;

        let _newRatio2 = _WidthIncreaseRealiveX1Window3 / imageWidth;

        let _newY1Image = cropy1 * _newRatio2;
        let _newY2Image = cropy2 * _newRatio2;

        let newHeight2 = _newY2Image - _newY1Image;
        console.log("newHeight = " + newHeight);
        console.log("newHeight2 = " + newHeight2);

        //--------------------------------//
        let translatex = 0;
        if (newHeight < userHeight) {
            let temp3 = minWidth;
            let widthIncrease = minWidth * userHeight / newHeight;
            console.log("widthIncrease = " + widthIncrease);
            minWidth = widthIncrease;
            // translatex = widthIncrease / 4 - dxmin;
            // console.log("transateX = " + translatex);
            let t1 = temp3 / 2;
            let translateRatio = minWidth / temp3;
            translatex = t1 * (translateRatio - 1) - dymin4;
            console.log("translatex = " + translatex);
            newHeight = userHeight;
        }
        // console.log("newHeight2 = " + newHeight);

        let width = this.element.offsetWidth;
        //let height = this.element.offsetHeight;
        let height = newHeight;
        this.previousContainerWidth = width;
        this.previousContainerHeight = height;

        textContainer.innerText = "Высота рамки = " + parseInt(newHeight).toString() +
            "\nТекущая ширинка картинки = " + parseInt(minWidth).toString() +
            "\nЗаказанная высота = " + parseInt(userHeight).toString();

        console.log(x1Gradient, x1Gradient3, x2Gradient, y1Gradient, y2Gradient, ratio4);

        let yIncrease = 1;
        if (dymin4 >= 0)
            yIncrease = (dymin4 / HeightIncreaseRealiveX1Window3) * 10.0
        let x0 = cropx1 / imageWidth;
        let x1 = cropx2 / imageWidth;
        let y0 = cropy1 / imageHeight;
        let y1 = cropy2 / imageHeight;
        let w = x1 - x0;
        let h = y1 - y0;

        let widthAspect = width * aspect;
        let minHeight = widthAspect * h;
        let maxHeight = widthAspect / w;
        height = Math.max(height, minHeight);
        height = Math.min(height, maxHeight);
        var containerAspect = height / width;
        var containerAspect1 = newHeight2 / width;
        console.log("containerAspect1 = ", containerAspect1);
        var offset1 = yIncrease + (1 - (containerAspect1 / aspect - h) / (1 - h)) * y0 * 100.0
        console.log("offset1 = " + offset1);
        // var containerAspect = newHeight / width;
        if (containerAspect < aspect) {
            // this.image.style.width = "100%"
            imageContainer.style.height = newHeight + "px"
            this.image.style.width = minWidth + "px"
            this.image.style.height = "auto"
            console.log("containerAspect = " + containerAspect);
            console.log("aspect = " + aspect);
            console.log("h = " + h);
            console.log("y0 = " + y0);
            var offset = (1 - (containerAspect / aspect - h) / (1 - h)) * y0 * 100.0
            console.log("offset = " + offset);
            this.image.style.transform = "translate(-" + translatex + "px, -" + offset1 + "%)"
                // this.image.style.transform = "translate(-" + 0 + "px, -" + offset + "px)"
        } else {
            imageContainer.style.height = newHeight + "px"
            this.image.style.width = "auto"
            this.image.style.height = "100%"
            var offset = (1 - (aspect / containerAspect - w) / (1 - w)) * x0 * 100.0
            this.image.style.transform = "translate(-" + offset + "%, 0)"
        }

        // this.element.style.minHeight = minHeight + "px";
        // this.element.style.maxHeight = maxHeight + "px";
    }
    this.parent.onAnchorsChanged.push(this.element.updateWidth);
}