function Video(element) {
    Block(element)
    element.isVideo = true



    var sourcesJson = element.dataset.sources.replaceAll("'", '"');
    var sources = JSON.parse(sourcesJson)

    //var a = "[{\"size\":{\"x\":176,\"y\":144},\"url\":\"https://rr1---sn-jtu5jt2g0n-aj5y.googlevideo.com/videoplayback?expire=1661194950\u0026ei=Zn4DY9jWIoKjxN8P-oCSoAY\u0026ip=83.168.43.121\u0026id=o-AOC0pZ5C5zrjOivymfB8pI786u6-T5n3hkPb94dO_dmc\u0026itag=17\u0026source=youtube\u0026requiressl=yes\u0026mh=Kb\u0026mm=31%2C29\u0026mn=sn-jtu5jt2g0n-aj5y%2Csn-hgn7yn76\u0026ms=au%2Crdu\u0026mv=m\u0026mvi=1\u0026pl=26\u0026initcwndbps=905000\u0026vprv=1\u0026mime=video%2F3gpp\u0026gir=yes\u0026clen=1625123\u0026dur=200.109\u0026lmt=1658784098951739\u0026mt=1661173050\u0026fvip=5\u0026fexp=24001373%2C24007246\u0026c=ANDROID\u0026rbqsm=fr\u0026txp=5318224\u0026sparams=expire%2Cei%2Cip%2Cid%2Citag%2Csource%2Crequiressl%2Cvprv%2Cmime%2Cgir%2Cclen%2Cdur%2Clmt\u0026sig=AOq0QJ8wRAIga6kRK4p7X7NKGy5fndjnH5jrWS3Q_KsOWbTqnVaA-LsCIAgMDa0p1J3FucP0Sm_i-OOAO4n6I81VK2UNkzq-aljQ\u0026lsparams=mh%2Cmm%2Cmn%2Cms%2Cmv%2Cmvi%2Cpl%2Cinitcwndbps\u0026lsig=AG3C_xAwRQIhAOWd7iZSVmC4z12h-j2aCmSYNzwiPAvEybgL155njL3iAiAjVc5fnChrCB1oQe2IvQPT6-ZJceqQGN6GwOjkH1UnXA%3D%3D'},{'size':{'x':640,'y':360},'url':'https://rr1---sn-jtu5jt2g0n-aj5y.googlevideo.com/videoplayback?expire=1661194950\u0026ei=Zn4DY9jWIoKjxN8P-oCSoAY\u0026ip=83.168.43.121\u0026id=o-AOC0pZ5C5zrjOivymfB8pI786u6-T5n3hkPb94dO_dmc\u0026itag=18\u0026source=youtube\u0026requiressl=yes\u0026mh=Kb\u0026mm=31%2C29\u0026mn=sn-jtu5jt2g0n-aj5y%2Csn-hgn7yn76\u0026ms=au%2Crdu\u0026mv=m\u0026mvi=1\u0026pl=26\u0026initcwndbps=905000\u0026vprv=1\u0026mime=video%2Fmp4\u0026gir=yes\u0026clen=8237128\u0026ratebypass=yes\u0026dur=200.063\u0026lmt=1658784087562218\u0026mt=1661173050\u0026fvip=5\u0026fexp=24001373%2C24007246\u0026c=ANDROID\u0026rbqsm=fr\u0026txp=5319224\u0026sparams=expire%2Cei%2Cip%2Cid%2Citag%2Csource%2Crequiressl%2Cvprv%2Cmime%2Cgir%2Cclen%2Cratebypass%2Cdur%2Clmt\u0026sig=AOq0QJ8wRAIgeyKk1nGQPZKUR5fflyNNac4tcrd99QjFLtpN4B-kDRgCIBRS9ia41ep0buQOE6BM6lf8QZ1VizQYYdur29FgOGyz\u0026lsparams=mh%2Cmm%2Cmn%2Cms%2Cmv%2Cmvi%2Cpl%2Cinitcwndbps\u0026lsig=AG3C_xAwRQIhAOWd7iZSVmC4z12h-j2aCmSYNzwiPAvEybgL155njL3iAiAjVc5fnChrCB1oQe2IvQPT6-ZJceqQGN6GwOjkH1UnXA%3D%3D'},{'size':{'x':1280,'y':720},'url':'https://rr1---sn-jtu5jt2g0n-aj5y.googlevideo.com/videoplayback?expire=1661194950\u0026ei=Zn4DY9jWIoKjxN8P-oCSoAY\u0026ip=83.168.43.121\u0026id=o-AOC0pZ5C5zrjOivymfB8pI786u6-T5n3hkPb94dO_dmc\u0026itag=22\u0026source=youtube\u0026requiressl=yes\u0026mh=Kb\u0026mm=31%2C29\u0026mn=sn-jtu5jt2g0n-aj5y%2Csn-hgn7yn76\u0026ms=au%2Crdu\u0026mv=m\u0026mvi=1\u0026pl=26\u0026initcwndbps=905000\u0026vprv=1\u0026mime=video%2Fmp4\u0026cnr=14\u0026ratebypass=yes\u0026dur=200.063\u0026lmt=1658784718472174\u0026mt=1661173050\u0026fvip=5\u0026fexp=24001373%2C24007246\u0026c=ANDROID\u0026rbqsm=fr\u0026txp=5318224\u0026sparams=expire%2Cei%2Cip%2Cid%2Citag%2Csource%2Crequiressl%2Cvprv%2Cmime%2Ccnr%2Cratebypass%2Cdur%2Clmt\u0026sig=AOq0QJ8wRAIgEhZrQTXFGDY6j9WiuN314GrgfDga-EI_VBUHCf6jNZwCIGfnKLNFxSj_y4dtBzeMcZpTJS_7PYLmC1RNkBokhSXn\u0026lsparams=mh%2Cmm%2Cmn%2Cms%2Cmv%2Cmvi%2Cpl%2Cinitcwndbps\u0026lsig=AG3C_xAwRQIhAOWd7iZSVmC4z12h-j2aCmSYNzwiPAvEybgL155njL3iAiAjVc5fnChrCB1oQe2IvQPT6-ZJceqQGN6GwOjkH1UnXA%3D%3D'}]"


    /*console.log(a)
    console.log(JSON.parse(a))*/

    element.Reactive = {
        Aspect: element.dataset.width / element.dataset.height,
        InternalWidth: () => First(element.Height * element.Aspect, element.dataset.width),
        InternalHeight: () => First(element.Width / element.Aspect, element.dataset.height),

        Controls: true,
    }

    var videoElement = undefined
    function onLoadedMetadata(event) {
        console.log("onLoadedMetadata")
        videoElement.play()
    }

    function onTimeUpdate() {
        console.log("onTimeUpdate", videoElement.currentTime)
    }

    new Reaction(() => {
        //videoElement.currentTime = "5"
        console.log(element.Width)

    })


    new Reaction(() => {
        if (videoElement == undefined) {
            videoElement = document.createElement("video")
            element.appendChild(videoElement)

            videoElement.src = sources[0].url

            videoElement.onloadedmetadata = (event) => { onLoadedMetadata(event) }
            videoElement.ontimeupdate = onTimeUpdate;
        }
        
        videoElement.width = element.Width
        videoElement.height = element.Height
        videoElement.controls = element.Controls
    })



    WidthToStyle(element)
    HeightToStyle(element)
}
