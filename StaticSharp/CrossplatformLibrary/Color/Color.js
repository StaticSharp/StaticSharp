function Color() {
    let args = [...arguments]
    let _this = this

    _this.r = 0
    _this.g = 0
    _this.b = 0
    _this.a = undefined

    if (args.length == 0) {
        return
    }

    if (args.length == 1) {
        let input = args[0]
        if (typeof input == 'number') {
            _this.b = (input & 0xFF) / 255
            _this.g = ((input & 0xFF00) >>> 8) / 255
            _this.r = ((input & 0xFF0000) >>> 16) / 255
            _this.a = ((input & 0xFF000000) >>> 24) / 255
            return;
        }

        if (typeof input == 'string') {
            
            if (input.substr(0, 1) == "#") {
                var collen = (input.length - 1) / 3;
                var fact = [17, 1, 0.062272][collen - 1] / 255;
                _this.r = parseInt(input.substr(1, collen), 16) * fact
                _this.g = parseInt(input.substr(1 + collen, collen), 16) * fact
                _this.b = parseInt(input.substr(1 + 2 * collen, collen), 16) * fact 
                return
            }

            //} return input.split("(")[1].split(")")[0].split(",").map(x => +x)

        }

        console.warn("Color: Invalid arguments", input)
    }

    if ((args.length >= 3) & (args.length <= 4)) {
        if (args.slice(0, 3).some(x => (typeof x) != 'number')) {
            console.warn("Color: Invalid arguments types", args)
            return;
        }

        _this.r = args[0]
        _this.g = args[1]
        _this.b = args[2]

        if (args[3] != undefined) {
            if ((typeof args[3]) != 'number') {
                console.warn("Color: Invalid arguments types", args)
                return;
            }
            _this.a = args[3]
        }
        return
    }
    
    console.warn("Color: Invalid arguments", args)

}

Color.prototype.toString = function () {
    let a = this.a || 1
    //if (a == undefined)
    //    a = 1
    return `rgba(${Math.round(255 * this.r)},${Math.round(255 * this.g)},${Math.round(255 * this.b)},${a})`
}

/**
 * @param {Color} a
 * @param {Color} b
 */
Color.CplusC = function (a, b) {
    return new Color(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a || a.a || b.a);
}

/**
 * @param {Color} a
 * @param {number} b
 */
Color.CplusN = function (a, b) {
    return new Color(a.r + b, a.g + b, a.b + b, a.a + b);
}

/**
 * @param {number} a
 * @param {Color} b
 */
Color.NplusC = function (a, b) {
    return new Color(a + b.r, a + b.g, a + b.b, a + b.a);
}

/**
 * @param {Color} a
 * @param {Color} b
 */
Color.CminusC = function (a, b) {
    return new Color(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a || a.a || b.a);
}

/**
 * @param {Color} a
 * @param {number} b
 */
Color.CminusN = function (a, b) {
    return new Color(a.r - b, a.g - b, a.b - b, a.a - b);
}

/**
 * @param {number} a
 * @param {Color} b
 */
Color.NminusC = function (a, b) {
    return new Color(a - b.r, a - b.g, a - b.b, a - b.a);
}

/**
 * @param {Color} a
 * @param {Color} b
 */
Color.CmulC = function (a, b) {
    return new Color(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a || a.a || b.a);
}

/**
 * @param {Color} a
 * @param {number} b
 */
Color.CmulN = function (a, b) {
    return new Color(a.r * b, a.g * b, a.b * b, a.a * b);
}

/**
 * @param {number} a
 * @param {Color} b
 */
Color.NmulC = function (a, b) {
    return new Color(a * b.r, a * b.g, a * b.b, a * b.a);
}

/**
 * @param {number} value
 */
Color.FromGrayscale = function (value) {
    return new Color(value, value, value);
}

/**
 * @param {number} rgb
 */
Color.FromIntRGB = function (rgb) {
    return new Color(
        ((rgb >> 16) & 0xff) / 255,
        ((rgb >> 8) & 0xff) / 255,
        (rgb & 0xff) / 255,
    );
}

/**
 * @param {number} r
 * @param {number} g
 * @param {number} b
 */
Color.FromIntChannelsRGB = function(r, g, b) {
    return new Color(r / 255, g / 255, b / 255);
}

/**
 * @param {number} r
 * @param {number} g
 * @param {number} b
 * @param {number} a
 */
Color.FromIntChannelsRGBA = function (r, g, b, a) {
    return new Color(r/255, g/255, b/255, a/255);
}

/**
 * @param {Color} a
 * @param {Color} b
 * @param {number} t
 */
Color.Lerp = function (a, b, t) {
    return a.Lerp(b,t)
}

/**
 * @param {Color} targetColor
 * @param {number} amount
 */
Color.prototype.Lerp = function (targetColor, amount) {
    var bk = (1 - amount);
    var a = Num.First(this.a * bk + targetColor.a * amount, this.a, targetColor.a);
    var r = this.r * bk + targetColor.r * amount;
    var g = this.g * bk + targetColor.g * amount;
    var b = this.b * bk + targetColor.b * amount;
    return new Color(r, g, b, a);
}

/**
 * @param {number} contrast
 */
Color.prototype.ContrastColor = function (contrast = 1) {
    var grayscale = (0.2125 * this.r) + (0.7154 * this.g) + (0.0721 * this.b);
    var blackOrWhite = (grayscale > 0.5) ? new Color(0, 0, 0, 1) : new Color(1, 1, 1, 1);
    return this.Lerp(blackOrWhite, contrast);
}