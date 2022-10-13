function Color() {
    let args = [...arguments]
    let _this = this

    _this.r = 0
    _this.g = 0
    _this.b = 0
    _this.a = 1

    if (args.length == 0) {
        return
    }

    if (args.length == 1) {
        let input = args[0]
        if (typeof input == 'number') {
            _this.r = (input & 0xFF) / 255
            _this.g = ((input & 0xFF00) >>> 8) / 255
            _this.b = ((input & 0xFF0000) >>> 16) / 255
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

    if (args.some(x => (typeof x) != 'number')) {
        console.warn("Color: Invalid arguments types", args)
        return;
    }

    if ((args.length >= 3) & (args.length <= 4)) {
        _this.r = args[0]
        _this.g = args[1]
        _this.b = args[2]

        if (args.length == 4) {
            _this.a = args[3]
        }
        return
    }
    
    console.warn("Color: Invalid arguments", args)

}

Color.prototype.toString = function () {
    return `rgba(${Math.round(255 * this.r)},${Math.round(255 * this.g)},${Math.round(255 * this.b)},${this.a})`
}

Color.prototype.lerp = function (targetColor, amount) {
    var bk = (1 - amount);
    var a = this.a * bk + targetColor.a * amount;
    var r = this.r * bk + targetColor.r * amount;
    var g = this.g * bk + targetColor.g * amount;
    var b = this.b * bk + targetColor.b * amount;
    return new Color(r, g, b, a);
}

Color.prototype.contrastColor = function (contrast = 1) {
    var grayscale = (0.2125 * this.r) + (0.7154 * this.g) + (0.0721 * this.b);
    var blackOrWhite = (grayscale > 0.5) ? new Color(0, 0, 0, 1) : new Color(1, 1, 1, 1);
    return this.lerp(blackOrWhite, contrast);
}