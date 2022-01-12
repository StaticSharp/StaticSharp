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

    //200 400 460 680 720 940 / 285 400 630
    let cropx1 = 460;
    let cropx2 = 680;
    let cropy1 = 155;
    let cropy2 = 400;
    let imageWidth = 1000;
    let imageHeight = 667;

    let userHeight = 100;

    this.element.updateWidth = function() {
        let widthRatio = imageWidth / parseInt(element.style.width);
        console.log("ratio = " + widthRatio);

        var x1Window = (parent.anchors.wideRight - parent.anchors.wideLeft) / 2;
        var x2Window = (parent.anchors.wideRight - parent.anchors.wideLeft);

        // console.log("A = " + (parent.anchors.wideRight - parent.anchors.wideLeft));
        // console.log("B = " + (parent.anchors.fillRight + parent.anchors.fillLeft));

        let x1Image = (cropx1 / widthRatio);
        let x2Image = (cropx2 / widthRatio);
        let y1Image = (cropy1 / widthRatio);
        let y2Image = (cropy2 / widthRatio);

        let initImageDiagonal = Math.sqrt(imageHeight * imageHeight + imageWidth * imageWidth);
        let sin = imageHeight / initImageDiagonal;
        let cos = imageWidth / initImageDiagonal;
        let tg = imageHeight / imageWidth;
        console.log("s = " + sin)
        console.log("x2Window = " + x2Window);
        let middleOfFullImage = x2Window / 2;
        let dx = middleOfFullImage - x1Image;
        let dy = dx * tg;
        let diagonalIncreaseRelativeX1Window = Math.sqrt(dx * dx + dy * dy);
        if (dx < 0)
            diagonalIncreaseRelativeX1Window = -diagonalIncreaseRelativeX1Window;
        let x1Gradient = x1Image / x2Window;
        let x2Gradient = x2Image / x2Window;
        let diagonalIncreaseRelativeX1Image = diagonalIncreaseRelativeX1Window / x1Gradient;
        let renderImageDiagonalmin = diagonalIncreaseRelativeX1Image + initImageDiagonal / widthRatio;
        let WidthIncreaseRealiveX1Window = imageWidth * renderImageDiagonalmin / initImageDiagonal;
        console.log(renderImageDiagonalmin);
        if (WidthIncreaseRealiveX1Window < x2Window)
            WidthIncreaseRealiveX1Window = x2Window;
        let currentWidth = WidthIncreaseRealiveX1Window;
        console.log("minWidth = " + currentWidth);
        console.log("x2Image = " + x2Image);
        console.log("x2Window = " + x2Window);

        //-----------------------------------------------//
        //1515 / 1285 - screen.width (maxwidth)
        let maxWidth = window.screen.width - leftSlider.clientWidth - rightSlider.clientWidth;
        if (leftSlider.style.visibility == "visible" && rightSlider.style.visibility == "visible")
            maxWidth = Math.min(maxWidth, 1515);

        let middleOfFullImageWithMaxWidth = maxWidth / 2;
        let dx2 = middleOfFullImageWithMaxWidth - x1Image;
        let dy2 = dx2 * tg;
        let diagonalIncreaseRelativeX1WindowWithMaxWidth = Math.sqrt(dx2 * dx2 + dy2 * dy2);
        if (dx2 < 0)
            diagonalIncreaseRelativeX1WindowWithMaxWidth = -diagonalIncreaseRelativeX1WindowWithMaxWidth;
        let x1GradientWithMaxWidth = x1Image / x2Window;
        let diagonalIncreaseRelativeX1ImageWithMaxWidth = diagonalIncreaseRelativeX1WindowWithMaxWidth / x1GradientWithMaxWidth;
        let renderImageDiagonalminWithMaxWidth = diagonalIncreaseRelativeX1ImageWithMaxWidth + initImageDiagonal / widthRatio;
        let WidthIncreaseRealiveX1WindowWithMaxWidth = imageWidth * renderImageDiagonalminWithMaxWidth / initImageDiagonal;

        //----------------------------------------------------------------//

        let maxRatio = imageWidth / maxWidth;
        let x1ImageWithMaxWidth = cropx1 / maxRatio;
        let dx3 = maxWidth / 2 - x1ImageWithMaxWidth;
        let dy3 = dx3 * tg;
        let newRatioWithMaxWidth = WidthIncreaseRealiveX1WindowWithMaxWidth / imageWidth;
        let y1ImageWithMaxWidth = cropy1 * newRatioWithMaxWidth;
        let y2ImageWithMaxWidth = cropy2 * newRatioWithMaxWidth;
        let HeightIncreaseRealiveX1WindowWithMaxWidth = WidthIncreaseRealiveX1WindowWithMaxWidth * tg
        console.log("dx4 = " + dx3);
        console.log("dy4 = " + dy3);

        let neederHeight = 0;

        let y1Gradient = cropy1 / imageHeight;
        neederHeight = y2ImageWithMaxWidth - y1ImageWithMaxWidth - dy3 * y1Gradient;

        //----------------------------------------------------------------//

        let x1ImageWithCurrentWidth = (imageWidth / 2 / widthRatio);
        let dx4 = middleOfFullImageWithMaxWidth - x1ImageWithCurrentWidth;
        let dy4 = dx4 * tg;
        let diagonalIncreaseRelativeX1WindowWithCurrentWidth = Math.sqrt(dx4 * dx4 + dy4 * dy4);
        if (dx4 < 0)
            diagonalIncreaseRelativeX1WindowWithCurrentWidth = -diagonalIncreaseRelativeX1WindowWithCurrentWidth;
        let x1Gradient2 = x1ImageWithCurrentWidth / x2Window;
        let diagonalIncreaseRelativeX1ImageWithCurrentWidth = diagonalIncreaseRelativeX1WindowWithCurrentWidth / x1Gradient2;
        let renderImageDiagonalminWithCurrentWidth = diagonalIncreaseRelativeX1ImageWithCurrentWidth + initImageDiagonal / widthRatio;
        let widthIncreaseRealiveX1WindowWithCurrentWidth = imageWidth * renderImageDiagonalminWithCurrentWidth / initImageDiagonal;

        let newRatioWithCurrentWidth = widthIncreaseRealiveX1WindowWithCurrentWidth / imageWidth;

        let y1ImageWithCurrentWidth = cropy1 * newRatioWithCurrentWidth;
        let y2ImageWithCurrentWidth = cropy2 * newRatioWithCurrentWidth;
        console.log(y1ImageWithCurrentWidth, y2ImageWithCurrentWidth);

        let heightForContainerAspect = y2ImageWithCurrentWidth - y1ImageWithCurrentWidth;

        //----------------------------------------------------------------//

        let currentXTranslate = 0;
        let currentWidthForUserHeight = 0;
        let currentHeightForUserHeight = 0;
        console.log("newHeight = " + neederHeight);
        console.log("newHeight2 = " + heightForContainerAspect);
        if (neederHeight < userHeight) {
            // let widthIncrease = minWidth * userHeight / newHeight;
            let widthIncreaseForUserHeight = currentWidth * userHeight / neederHeight;
            console.log("qqq = " + neederHeight);
            console.log("widthIncrease = " + widthIncreaseForUserHeight);
            // minWidth = widthIncrease;
            currentWidthForUserHeight = widthIncreaseForUserHeight;
            console.log("newMinWidth = " + currentWidthForUserHeight);
            let ratioForUserHeight = userHeight / neederHeight;
            console.log("ZZZZZZZZ = " + x2Image * ratioForUserHeight);
            currentXTranslate = x2Window / 2 * (ratioForUserHeight - 1);
            console.log("Какая задана2 = " + userHeight / ratioForUserHeight);
            if (dy3 < 0)
                currentXTranslate = currentXTranslate * 2;
            // translatex = y1Image;
            // newnewHeight = userHeight / newRatio5 + dymin4 / (cropy2 / imageHeight - cropy1 / imageHeight) / newRatio5;
            currentHeightForUserHeight = userHeight;
        }

        console.log("minWidth: " + currentWidth);

        let width = this.element.offsetWidth;
        let height = neederHeight;
        this.previousContainerWidth = width;
        this.previousContainerHeight = height;

        //----------------------------------------------------------------//

        // // textContainer.innerText = "Высота рамки = " + parseInt(newHeight).toString() +
        // //     "\nТекущая ширинка картинки = " + parseInt(minWidth).toString() +
        // //     "\nЗаказанная высота = " + parseInt(userHeight).toString();

        let yIncrease = 1;
        if (dy3 >= 0)
            yIncrease = (dy3 / HeightIncreaseRealiveX1WindowWithMaxWidth) * 10.0
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
        var containerAspect2 = heightForContainerAspect / width;
        var offset1 = yIncrease + (1 - (containerAspect2 / aspect - h) / (1 - h)) * y0 * 100.0
        console.log(y1Image)
        console.log(y2Image)
        console.log("yIncrease = " + yIncrease);
        console.log("containerAspect1 = " + containerAspect2);
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
            while ((textContainer.clientHeight + h2.offsetTop) < neederHeight) {
                textContainer.css({
                    fontSize: font + "px",
                })
                font = font + 1;
            }
            while ((textContainer.clientHeight + h2.offsetTop) > neederHeight) {
                textContainer.css({
                    fontSize: font + "px",
                })
                font = font - 1;
            }
            h2.css({
                textAlign: "",
            })

            if (neederHeight > userHeight) {
                imageContainer.style.height = neederHeight + "px"
                this.image.style.width = currentWidth + "px"
            } else {
                // translatex = 0;
                imageContainer.style.height = currentHeightForUserHeight + "px"
                this.image.style.width = currentWidthForUserHeight + "px"
                    // this.image.style.width = minWidth + "px"
            }
            this.image.style.height = "auto"
                // var offset = (1 - (containerAspect / aspect - h) / (1 - h)) * y0 * 100.0
                // console.log("offset = " + offset);
            this.image.style.transform = "translate(-" + currentXTranslate + "px, -" + offset1 + "%)"
        } else {
            imageContainer.style.height = (neederHeight - y2Image * containerAspect) + "px"
            this.image.style.width = "auto"
                // this.image.style.height = "100%"
            this.image.style.height = neederHeight + "px"
            var offset = (1 - (aspect / containerAspect - w) / (1 - w)) * x0 * 100.0
            this.image.style.transform = "translate(-" + offset + "%, 0)"
            let h2 = textContainer.getElementsByTagName("h2")[0];
            let textContainerHeight = (cropy1 * maxRatio) - h2.offsetTop;
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

        console.log("Какой хочется = " + neederHeight);
        console.log("Какая задана = " + userHeight);
    }

    this.parent.onAnchorsChanged.push(this.element.updateWidth);
}