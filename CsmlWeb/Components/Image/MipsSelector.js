function MipsSelector(element, mips) {
    //console.log(mips);
    if (Object.keys(mips)[0] === undefined) { return; }
    this.element = element;
    let image = this.element.getElementsByTagName("img")[0];
    document.fonts.ready.then(() => {
        LoadRightImage(element, image, mips);
    });
    this.OnWindowResize = () => {
        LoadRightImage(element, image, mips)
    }

}

function LoadRightImage(element, image, mips) {
    var closestMax = 0;

    let pixelRatio = window.devicePixelRatio;
    let width = element.offsetWidth * pixelRatio;
    let imageSource = image.src.split("/").pop();

    if (width > getKeyByValue(mips, imageSource) || width <= (getKeyByValue(mips, imageSource) / 2)) {
        for (i = 0; closestMax < width; ++i) {
            closestMax = Object.keys(mips)[i];
        }
        if (closestMax === undefined) { closestMax = Object.keys(mips)[Object.keys(mips).length - 1] }
        image.src = image.src.replace(imageSource, "") + mips[closestMax];
        closestMax = 0;
    }
}

function getKeyByValue(object, value) {
    return Object.keys(object).find(key => object[key] === value);
}