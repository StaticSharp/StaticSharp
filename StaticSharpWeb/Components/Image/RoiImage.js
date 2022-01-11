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

    //200 400 460 680 720 940 / 285 400
    let cropx1 = 460;
    let cropx2 = 680;
    let cropy1 = 155;
    let cropy2 = 400;
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
        if (WidthIncreaseRealiveX1Window < x2Window)
            WidthIncreaseRealiveX1Window = x2Window;
        minWidth = WidthIncreaseRealiveX1Window;

        //-----------------------------------------------//
        //1515 / 1285 - screen.width (maxwidth)
        let windowWidth2 = parent.anchors.wideRight - parent.anchors.wideLeft;
        let windowWidth3 = window.screen.width - leftSlider.clientWidth - rightSlider.clientWidth;
        if (leftSlider.style.visibility == "visible" && rightSlider.style.visibility == "visible")
            windowWidth3 = window.screen.width;
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
        let newX1Image = cropx1 * newRatio2;
        let newX2Image = cropx2 * newRatio2;
        let newY1Image = cropy1 * newRatio2;
        let newY2Image = cropy2 * newRatio2;
        let HeightIncreaseRealiveX1Window3 = WidthIncreaseRealiveX1Window3 * tg
        let diagIncrease = Math.sqrt(WidthIncreaseRealiveX1Window3 * WidthIncreaseRealiveX1Window3 + HeightIncreaseRealiveX1Window3 * HeightIncreaseRealiveX1Window3);
        console.log("dx4 = " + dxmin4);
        console.log("dy4 = " + dymin4);
        let ddiag = Math.sqrt(dxmin4 * dxmin4 + dymin4 * dymin4);

        let newHeight = 0;

        let y1Gradient = cropy1 / imageHeight;
        let y2Gradient = cropy2 / imageHeight;
        newHeight = newY2Image - newY1Image - dymin4 * y1Gradient;

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

        //--------------------------------//
        let translatex = 0;
        if (newHeight < userHeight) {
            let widthIncrease = minWidth * userHeight / newHeight;
            minWidth = widthIncrease;
            let newRatio5 = userHeight / newHeight;
            // let translateRatio = minWidth / temp3;
            // translatex = t1 * (translateRatio - 1) - dymin4; dxmin4
            // translatex = t1 * (translateRatio - 1);
            translatex = x2Window / 2 * (newRatio5 - 1);
            if (dymin4 < 0)
                translatex = translatex * 2;
            // translatex = 0;
            newHeight = userHeight;
        }

        let width = this.element.offsetWidth;
        let height = newHeight;
        this.previousContainerWidth = width;
        this.previousContainerHeight = height;

        //----------------------------------------------------------------//
        let div = textContainer.getElementsByTagName("div")[0];
        // let h2 = textContainer.getElementsByTagName("h2")[0];
        // h2.style.display = "block";

        // // textContainer.innerText = "Высота рамки = " + parseInt(newHeight).toString() +
        // //     "\nТекущая ширинка картинки = " + parseInt(minWidth).toString() +
        // //     "\nЗаказанная высота = " + parseInt(userHeight).toString();
        // if (leftBar.style.visibility == "hidden") {
        //     textContainer.style.paddingLeft = parent.anchors.textLeft + "px";
        //     textContainer.style.width = (minWidth / 2 - parent.anchors.textLeft - dxmin4) + "px";
        // } else {
        //     textContainer.style.paddingLeft = parent.anchors.textLeft - leftSlider.clientWidth + "px";
        //     textContainer.style.width = (minWidth / 2 - parent.anchors.textLeft + leftSlider.clientWidth - dxmin4) + "px";
        // }
        // textContainer.css({
        //     fontSize: "1px",
        // })
        // let font = 1;
        // while (textContainer.clientHeight + h2.offsetTop * 2 < newHeight) {
        //     textContainer.css({
        //         fontSize: font + "px",
        //     })
        //     font = font + 1;
        // }
        // while (textContainer.clientHeight + h2.offsetTop * 2 > newHeight) {
        //     textContainer.css({
        //         fontSize: font + "px",
        //     })
        //     font = font - 1;
        // }
        //----------------------------------------------------------------//

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
        var offset1 = yIncrease + (1 - (containerAspect1 / aspect - h) / (1 - h)) * y0 * 100.0
        console.log(y1Image)
        console.log(y2Image)
        console.log("containerAspect = " + containerAspect);
        console.log("aspect = " + aspect);
        console.log("h = " + h);
        console.log("y0 = " + y0);
        console.log("Width1/2 = " + window.innerWidth);
        console.log("---------")
        if (containerAspect < aspect) {
            let h2 = textContainer.getElementsByTagName("h2")[0];
            h2.style.display = "block";
            // if (leftBar.style.visibility == "hidden") {
            //     textContainer.style.paddingLeft = parent.anchors.textLeft + "px";
            //     textContainer.style.width = (minWidth / 2 - parent.anchors.textLeft - dxmin4) + "px";
            // } else {
            //     textContainer.style.paddingLeft = parent.anchors.textLeft - leftSlider.clientWidth + "px";
            //     textContainer.style.width = (minWidth / 2 - parent.anchors.textLeft + leftSlider.clientWidth - dxmin4) + "px";
            // }
            if (leftBar.style.visibility == "hidden") {
                textContainer.style.paddingLeft = parent.anchors.textLeft + "px";
                textContainer.style.width = (window.innerWidth / 2 - parent.anchors.textLeft - 20) + "px";
            } else {
                textContainer.style.paddingLeft = parent.anchors.textLeft - leftSlider.clientWidth + "px";
                // textContainer.style.width = (minWidth / 2 - parent.anchors.textLeft + leftSlider.clientWidth - dxmin4) + "px";
                textContainer.style.width = ((window.innerWidth - leftSlider.clientWidth - rightSlider.clientWidth) / 2 -
                    (parent.anchors.textLeft - leftSlider.clientWidth) - 20) + "px";
            }
            textContainer.css({
                fontSize: "1px",
            })
            let font = 1;
            while ((textContainer.clientHeight + h2.offsetTop) < newHeight) {
                textContainer.css({
                    fontSize: font + "px",
                })
                font = font + 1;
            }
            while ((textContainer.clientHeight + h2.offsetTop) > newHeight) {
                textContainer.css({
                    fontSize: font + "px",
                })
                font = font - 1;
            }
            h2.css({
                textAlign: "",
            })
            imageContainer.style.height = newHeight + "px"
            this.image.style.width = minWidth + "px"
            this.image.style.height = "auto"
                // var offset = (1 - (containerAspect / aspect - h) / (1 - h)) * y0 * 100.0
                // console.log("offset = " + offset);
            this.image.style.transform = "translate(-" + translatex + "px, -" + offset1 + "%)"
        } else {
            imageContainer.style.height = (newHeight - y2Image * containerAspect) + "px"
            this.image.style.width = "auto"
                // this.image.style.height = "100%"
            this.image.style.height = newHeight + "px"
            var offset = (1 - (aspect / containerAspect - w) / (1 - w)) * x0 * 100.0
            this.image.style.transform = "translate(-" + offset + "%, 0)"
            let h2 = textContainer.getElementsByTagName("h2")[0];
            let textContainerHeight = (cropy1 * ratio4) - h2.offsetTop;
            console.log("textContainerHeight = " + textContainerHeight);
            h2.style.display = "block";
            if (leftBar.style.visibility == "hidden") {
                textContainer.style.paddingLeft = parent.anchors.textLeft + "px";
                textContainer.style.width = (window.innerWidth - parent.anchors.textLeft * 2) + "px";
            } else {
                // textContainer.style.paddingLeft = parent.anchors.textLeft - leftSlider.clientWidth + "px";
                textContainer.style.width = ((window.innerWidth - leftSlider.clientWidth - rightSlider.clientWidth) / 2 -
                    (parent.anchors.textLeft - leftSlider.clientWidth) - 20) + "px";
            }
            textContainer.css({
                fontSize: "1px",
            })
            let font = 1;
            while ((textContainer.clientHeight) < textContainerHeight) {
                textContainer.css({
                    fontSize: font + "px",
                })
                font = font + 1;
            }
            while ((textContainer.clientHeight) > textContainerHeight) {
                textContainer.css({
                    fontSize: font + "px",
                })
                font = font - 1;
            }
            h2.css({
                textAlign: "center",
            })
        }

        // this.element.style.minHeight = minHeight + "px";
        // this.element.style.maxHeight = maxHeight + "px";
    }

    this.parent.onAnchorsChanged.push(this.element.updateWidth);
}