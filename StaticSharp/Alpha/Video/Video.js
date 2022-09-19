
var YouTube = {
    initialize: function () {
        youtubeIFrameApiScript = document.createElement("script")
        youtubeIFrameApiScript.src = "https://www.youtube.com/iframe_api"

        let timeout = setTimeout(() => {
            document.head.removeChild(youtubeIFrameApiScript)
            youtubeIFrameApiScript.onload = undefined
            youtubeIFrameApiScript.onerror = undefined
            let d = Reaction.beginDeferred()
            YouTube.Status = "error"
            d.end()
        }, 500)

        youtubeIFrameApiScript.onload = function () {
            clearTimeout(timeout)
            YT.ready(function () {
                let d = Reaction.beginDeferred()
                YouTube.Status = "ready"
                d.end()
            })            
        }
        youtubeIFrameApiScript.onerror = function () {
            clearTimeout(timeout)
            let d = Reaction.beginDeferred()
            YouTube.Status = "error"
            d.end()
        }

        document.head.appendChild(youtubeIFrameApiScript)
        YouTube.Status = "loading"
    }
}

YouTube.Reactive = {
    Status: undefined
}


var uniqueID = 0;
function getUniqueID() {
    uniqueID++;
    return "juid" + uniqueID;
}


function Video(element) {
    Block(element)
    element.isVideo = true

    let youtubeId = element.dataset.youtubeId

    var sourcesJson = element.dataset.sources.replaceAll("'", '"');
    var sources = JSON.parse(sourcesJson)


    element.Reactive = {
        Aspect: element.dataset.width / element.dataset.height,
        InternalWidth: () => First(element.Height * element.Aspect, element.dataset.width),
        InternalHeight: () => First(element.Width / element.Aspect, element.dataset.height),

        PreferPlatformPlayer: true,

        Controls: true,

        Loop: false,

        Play: false,
        PlayActual: () => element.Play,

        Position: 0,

        //use timeupdate for hrml5
        PositionActual: () => {
            console.log("update element.PositionActual")
            if (!element.Player || !element.PositionInitialized)
                return element.Position

            let result = 0

            if (element.VideoPlayerType == "youtube") {
                result = element.Player.getCurrentTime()
            } else {
                result = element.Player.currentTime
            }

            if (element.PlayActual) {                
                window.setTimeout(() => {
                    element.Reactive.PositionActual.makeDirty();
                },50)
            }
            return result;

        },

        Mute: false,
        MuteActual: () => element.Mute,

        Volume: 1,
        VolumeActual: () => element.Volume,



        VideoPlayerType: undefined,//youtube, video
        Player: undefined,
        PositionInitialized: false,

        SourceIndex: undefined


    }

    //PositionActual
    /*new Reaction(() => {
        console.log("element.PositionActual",element.PositionActual)
    })*/



    /*window.setInterval(() => {
        element.PositionUpdateTimer++;
    },50)*/


    var playerDestructor = undefined


    new Reaction(() => {
        if (!element.Player)
            element.PositionInitialized = false
    })


    //element.style.overflow = "hidden"



    /*
     if (!showControls) {
                iframe.style.pointerEvents = "none";
            }


     */

    new Reaction(() => {
        if (element.VideoPlayerType == "html5") {
            if (element.Player) {
                if (element.Width == undefined)
                    return;

                /*let lossFunction = (source) => {
                    return Math.abs(element.Width - source.size.x)
                }*/
                let lossFunction = (source) => {
                    let pixelWidth = element.Root.DevicePixelRatio * element.Width
                    if (source.size.x < pixelWidth) {
                        return 8 * (pixelWidth - source.size.x)
                    }
                    return source.size.x - pixelWidth
                }

                let closestSourceLoss = lossFunction(sources[0])
                let closestSourceIndex = 0
                for (let i = 1; i < sources.length; i++) {
                    let loss = lossFunction(sources[i])
                    if (loss < closestSourceLoss) {
                        closestSourceLoss = loss
                        closestSourceIndex = i
                    }
                }

                element.SourceIndex = closestSourceIndex
            }
        }
    })

    new Reaction(() => {
        if (element.VideoPlayerType == "html5") {
            if (!element.Player)
                return

            let position = element.Player.currentTime
            element.Player.src = sources[element.SourceIndex].url
            element.Player.currentTime = position
        }
    })



    function InitializeYoutubeIFrame() {

        /*var currentPosition = 0
        if (getCurrentPosition)
            currentPosition = getCurrentPosition()*/

        if (playerDestructor) {
            playerDestructor()            
        }

        element.VideoPlayerType = "youtube"

        var iframe = document.createElement("div");
        iframe.id = getUniqueID();
        element.appendChild(iframe);


        let playerVars = {
            rel: 0,
            //"autoplay": autoPlay ? 1 : 0,

            //???showinfo: 0,

            controls: element.Controls ? 1 : 0,
            origin: window.location.origin
        }
        console.log("window.location.origin", window.origin)
        if (element.Loop) {
            playerVars.loop = 1
            playerVars.playlist = youtubeId
        }

        /*if (!element.Controls) {
            iframe.style.pointerEvents = "none"        
            iframe.style.top = "-50%"
            iframe.style.position = "absolute"
            iframe.style.width = "100%"
            iframe.style.height = "200%"
        } else {
            iframe.style.width = "100%"
            iframe.style.height = "100%"
        }*/
        iframe.style.width = "100%"
        iframe.style.height = "100%"


        let volumeTimeout = undefined

        let player = new YT.Player(iframe.id, {
            //height: '100%',
            //width: '100%',
            videoId: youtubeId,
            host: "http://www.youtube-nocookie.com",
            playerVars: playerVars,
            events: {
                onReady: () => {                    
                    /*if (currentPosition !== 0) {
                        player.seekTo(currentPosition, true)
                    }*/
                    

                    element.VolumeActual = () => {
                        clearTimeout(volumeTimeout)

                        if (!element.Player)
                            return element.Volume

                        let result = element.Player.getVolume() * 0.01

                        volumeTimeout = window.setTimeout(() => {
                            element.Reactive.VolumeActual.makeDirty()
                        }, 100)
                        return result
                    }


                    element.Player = player
                    //element.PlayerReady = true
                },
                onStateChange: function (event) {
                    if (event.data == YT.PlayerState.ENDED) {
                        element.PlayActual = false
                    } else if (event.data == YT.PlayerState.PLAYING) {
                        element.PlayActual = true
                    } else if (event.data == YT.PlayerState.PAUSED) {
                        element.PlayActual = false
                    }
                }
            }
        })

        getCurrentPosition = function () {
            return player.getCurrentTime()
        }
        
        playerDestructor = function () {
            clearTimeout(volumeTimeout)

            playerDestructor = undefined
            getCurrentPosition = undefined

            player.destroy()
            element.removeChild(iframe)

            let d = Reaction.beginDeferred()
            element.Player = undefined
            //element.PlayerReady = false
            d.end()
        }

        
    }



    /*new Reaction(() => {
        if (element.Hover) {
            element.Player.src = sources[1].url
        } else {
            element.Player.src = sources[0].url
        }

    })*/


    let source

    element.onclick = () => {
        /*let source = document.createElement("source")
        source.type = "video/mp4"
        source.sizes = "1280x720"
        //source.media = "(min-width:70000px)"
        source.src = sources[1].url
        element.Player.appendChild(source)*/


        //source.src = sources[1].url
    }

    function InitializeHtml5Video() {
        element.VideoPlayerType = "html5"

        /*var currentPosition = 0
        if (getCurrentPosition)
            currentPosition = getCurrentPosition()*/

        if (playerDestructor) {
            playerDestructor()
        }

        let player = document.createElement("video")
        element.appendChild(player)
        player.style.width = "100%"
        player.style.height = "100%"

        player.src = sources[1].url
        player.muted = true

        //<source type="video/mp4" src="/uploads/video_Small.mp4">
        /*let source = document.createElement("source")
        source.type = "video/mp4"
        source.sizes = "1280x720"
        //source.media = "(min-width:70000px)"
        source.src = sources[1].url
        player.appendChild(source)

        source = document.createElement("source")
        source.type = "video/mp4"
        source.sizes = "640x360"
        //source.media = "(min-width:200px)"
        source.src = sources[0].url
        player.appendChild(source)*/
        //https://stackoverflow.com/questions/38626993/change-video-quality-with-sources-pointing-to-different-quality-versions

        /*function onLoadedMetadata(event) {
            player.currentTime = currentPosition
        }*/

        function onTimeUpdate() {
            //console.log("onTimeUpdate", videoElement.currentTime)
        }

        //player.onloadedmetadata = (event) => { onLoadedMetadata(event) }

        player.ontimeupdate = onTimeUpdate;

        player.onplay = () => element.PlayActual = true
        player.onpause = () => element.PlayActual = false  



        element.VolumeActual = player.volume
        player.onvolumechange = () => element.VolumeActual = player.volume
        

        /*player.onended = (event) => {
            console.log("player.onended",event)
        }*/

        //player.volume = 1


        element.Player = player

        getCurrentPosition = function () {
            return player.currentTime
        }

        playerDestructor = function () {
            playerDestructor = undefined
            getCurrentPosition = undefined
            player.onpause = undefined

            element.Player = undefined

            element.removeChild(player)            
        }
    }





    new Reaction(() => {

        if (element.PreferPlatformPlayer) {
            if (element.dataset.youtubeId !== undefined) {
                if (YouTube.Status === undefined) {
                    YouTube.initialize()
                }
                if (YouTube.Status == "ready") {
                    InitializeYoutubeIFrame()
                }
                if (YouTube.Status == "error") {
                    InitializeHtml5Video()
                }
            }
        } else {
            InitializeHtml5Video()
        }
    })



    //Position
    new Reaction(() => {
        
        if (!element.Player)
            return

        const positionTolerance = 0.05;

        if (element.VideoPlayerType == "youtube") {
            let delta = element.Player.getCurrentTime() - element.Position
            if (Math.abs(delta) > positionTolerance) {
                element.Player.seekTo(element.Position, true)
            }
        } else {
            let delta = element.Player.currentTime - element.Position
            if (Math.abs(delta) > positionTolerance) {
                element.Player.currentTime = element.Position
            }
        }
        element.PositionInitialized = true

    })




    
    

    //Play
    new Reaction(() => {
        if (!element.Player)
            return

        if (!element.PositionInitialized)
            return     
        
        if (element.VideoPlayerType == "youtube") {
            if (element.Play) {
                console.log("Play")
                element.Player.playVideo()
            } else {
                element.Player.pauseVideo()
            }
        } else {
            if (element.SourceIndex == undefined)
                return

            element.Play ? element.Player.play() : element.Player.pause()
        }        
    })



    //Mute
    new Reaction(() => {
        if (element.Player) {
            if (element.VideoPlayerType == "youtube") {
                if (element.Mute) {
                    element.Player.mute()                    
                } else {
                    element.Player.unMute()
                }
            } else {
                if (element.Mute != undefined) {
                    element.Player.muted = !element.Root.UserInteracted || element.Mute
                }
            }
        }
    })

    //Volume
    new Reaction(() => {
        if (element.Player) {
            //console.log("element.Volume", element.Volume)
            if (element.Volume == undefined)
                return

            if (element.VideoPlayerType == "youtube") {
                element.Player.setVolume(100 * element.Volume);
            } else {
                element.Player.volume = element.Volume
            }
        }
    })


    //Loop
    new Reaction(() => {
        if (element.Player) {
            if (element.VideoPlayerType == "youtube") {

            } else {
                element.Player.loop = element.Loop
            }
        }
    })

    //Controls
    new Reaction(() => {
        if (element.Player) {
            if (element.VideoPlayerType == "youtube") {


            } else {
                element.Player.controls = element.Controls                
            }
        }
    })

    

    WidthToStyle(element)
    HeightToStyle(element)
}
