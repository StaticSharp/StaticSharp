
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




HTMLElement.prototype.css = function(object) {
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
    //parent.removeChild(document.currentScript);

    func.call(parent, parameters)
    //func.call(parent, parent)
}
function Watch() {

    const refreshIntervalMs = 500;

    let pageKey = GetCookie("pageKey");
    if (pageKey == undefined) return;


    function CheckRefresh() {
        let request = new XMLHttpRequest();
        request.open("POST", "/api/v1/refresh_required");
        request.onload = function() {
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
        request.onerror = function() {
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