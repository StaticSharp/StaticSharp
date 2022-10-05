
var pageHash = "t!dcsctAYNTSYMJaKLcdZPtZ#n@KPIjkK)ppteSZ4t%W)N*3RC8k645V4DUMW5G!";

function Watch() {

    const refreshIntervalMs = 500;
    if (pageHash == undefined) return;

    function CheckRefresh() {
        fetch("/api/get_page_hash")
            .then((response) => response.text())
            .then((text) => {
                let changed = text !== pageHash
                if (changed) {
                    console.info("Refreshing page...", pageHash, text);
                    document.location.reload();
                } else {
                    setTimeout(CheckRefresh, refreshIntervalMs);
                }

            })
            .catch(err => {
                setTimeout(CheckRefresh, refreshIntervalMs);
            })
    }
    setTimeout(CheckRefresh, refreshIntervalMs);
}

console.log("PageHash:", pageHash, performance.now())
Watch()