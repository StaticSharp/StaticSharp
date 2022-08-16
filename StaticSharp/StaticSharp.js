




function intToColor(num) {
    num >>>= 0;
    var b = num & 0xFF,
        g = (num & 0xFF00) >>> 8,
        r = (num & 0xFF0000) >>> 16,
        a = ((num & 0xFF000000) >>> 24) / 255;
    return "rgba(" + [r, g, b, a].join(",") + ")";
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



