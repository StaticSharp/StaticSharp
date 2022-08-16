
function Watch() {

    const refreshIntervalMs = 500;

    let pageKey = GetCookie("pageKey");
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
        var requestBody = {
            pageKey: pageKey,
            location: document.location.pathname
        };
        request.send(JSON.stringify(requestBody));
    }

    setTimeout(CheckRefresh, refreshIntervalMs);
}


Watch()