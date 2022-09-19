
var pageKey = "t!dcsctAYNTSYMJaKLcdZPtZ#n@KPIjkK)ppteSZ4t%W)N*3RC8k645V4DUMW5G!";

function Watch() {

    const refreshIntervalMs = 500;

    //let pageKey = GetCookie("pageKey");
    if (pageKey == undefined) return;


    function CheckRefresh() {
        let request = new XMLHttpRequest();
        request.open("POST", "/api/v1/refresh_required");
        request.onload = function () {
            if (request.status != 200) {
                //console.error(`Page refresh query error: ${request.status}: ${request.statusText}`);
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
        //var requestBody = pageKey
/*        {
            pageKey: pageKey,
            location: document.location.pathname
        };JSON.stringify(requestBody)*/
        request.send(pageKey);
    }

    setTimeout(CheckRefresh, refreshIntervalMs);
}



/*function Watch() {

    const refreshIntervalMs = 500;

    let pageKey = GetCookie("pageKey");
    if (pageKey == undefined) return;


    function CheckRefresh() {

        console.log(document.location.href)

        let request = new XMLHttpRequest();
        request.open("GET", document.location.href);

        request.onload = function () {
            if (request.status == 200) {
                console.log(request.response)
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
}*/

// GetCookie("pageKey");
console.log(pageKey)
Watch()