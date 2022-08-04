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

        if (typeof args[0] == 'number') {
            _this.r = (args[0] & 0xFF) / 255
            _this.g = ((args[0] & 0xFF00) >>> 8) / 255
            _this.b = ((args[0] & 0xFF0000) >>> 16) / 255
            _this.a = ((args[0] & 0xFF000000) >>> 24) / 255
            return;
        } else {
            console.warn("Color: Invalid arguments", arguments[0])
            return;
        }
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




function intToColor(num) {
    num >>>= 0;
    var b = num & 0xFF,
        g = (num & 0xFF00) >>> 8,
        r = (num & 0xFF0000) >>> 16,
        a = ((num & 0xFF000000) >>> 24) / 255;
    return "rgba(" + [r, g, b, a].join(",") + ")";
}



function GetCookie(name) {
    let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

var toPercents = (a, b) => ((a / b) * 100).toFixed(3);

const Direction = { "up": "up", "down": "down", "left": "left", "right": "right" }
let xThreshold = 10; //min x swipe for horizontal swipe
let yThreshold = 15; //min y swipe for vertical swipe
function GetDirection(swipe) {

    let absSwipeX = Math.abs(swipe.swipeX);
    let absSwipeY = Math.abs(swipe.swipeY);
    if (absSwipeX < xThreshold && absSwipeY < yThreshold) {
        return null;
    }
    if (absSwipeX - xThreshold > absSwipeY - yThreshold) {
        if (swipe.swipeX > 0) {
            return Direction.right;
        } else {
            return Direction.left;
        }
    } else {
        if (swipe.swipeY > 0) {
            return Direction.down;
        } else {
            return Direction.up;
        }
    }
}

function DetectSwipe(element, action) {
    if (typeof action != 'function') { return; }
    let swipe = {
        clientStartX: 0,
        clientStartY: 0,
        startX: 0,
        startY: 0,
        moveX: 0,
        moveY: 0,
        get swipeX() { return this.moveX - this.startX; },
        get swipeY() { return this.moveY - this.startY; },
    };
    element.addEventListener('touchstart', (e) => {
        var touch = e.touches[0];
        swipe.clientStartX = touch.clientX;
        swipe.clientStartY = touch.clientY;
        swipe.startX = touch.screenX;
        swipe.startY = touch.screenY;
    }, false);
    element.addEventListener('touchmove', (e) => {
        var touch = e.touches[0];
        swipe.moveX = touch.screenX;
        swipe.moveY = touch.screenY;
        direction = GetDirection(swipe);
        if (direction == null) { return; }
        action(direction, swipe, false, e);
    }, false);
    element.addEventListener('touchend', (e) => {
        direction = GetDirection(swipe);
        if (direction == null) { return; }
        action(GetDirection(swipe), swipe, true, e);
    }, false);
}




HTMLElement.prototype.css = function (object) {
    for (const [key, value] of Object.entries(object)) {
        this.style[key] = value;
    }
}

/*function StaticSharpCall(func) {
    let parent = document.currentScript.parentElement;
    //parent.removeChild(document.currentScript);
    func.call(parent,parent)
}*/
function StaticSharpCall(func, parameters) {

    let parent = document.currentScript.parentElement;
    parent.removeChild(document.currentScript);

    func.call(parent, parent, parameters)
    //func.call(parent, parent)
}

function Use(value) {
    return 0;
}




function Watch() {

    const refreshIntervalMs = 500;

    let pageKey = GetCookie("pageKey");
    if (pageKey == undefined) return;


    function CheckRefresh() {
        let request = new XMLHttpRequest();
        request.open("POST", "/api/v1/refresh_required");
        request.onload = function () {
            if (request.status != 200) {
                console.error(`Page refresh query error: ${request.status}: ${request.statusText}`);
                setTimeout(CheckRefresh, refreshIntervalMs);
            } else {
                if (request.response == "true") {
                    console.info("Refreshing page...");
                    document.location.reload();
                } else {
                    setTimeout(CheckRefresh, refreshIntervalMs);
                }
            }
        };
        request.onerror = function () {
            console.error("Page refresh query failed");
            setTimeout(CheckRefresh, refreshIntervalMs);
        };
        request.setRequestHeader('Content-Type', 'application/json');
        var requestBody = {
            pageKey: pageKey,
            location: document.location.pathname
        };
        request.send(JSON.stringify(requestBody));
    }

    setTimeout(CheckRefresh, refreshIntervalMs);
}


Watch()