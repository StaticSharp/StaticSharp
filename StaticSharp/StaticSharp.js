




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



