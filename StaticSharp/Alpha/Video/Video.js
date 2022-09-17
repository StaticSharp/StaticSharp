
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

        Controls: () => element.ControlsInput,
        ControlsInput: () => element.Controls,

        Loop: false,

        Play: () => First(element.PlayInput,false),
        PlayInput: () => element.Play,

        Position: () => First(element.PositionInput,0),
        PositionInput: () => element.Position,

        Mute: () => First(element.MuteInput, true),
        MuteInput: false,

        Volume: () => First(element.VolumeInput, 1),
        VolumeInput: () => element.Volume,



        VideoPlayerType: undefined,//youtube, video
        Player: undefined,
        PositionInitialized: false,
        PositionUpdateTimer: 0
    }

    window.setInterval(() => {
        element.PositionUpdateTimer++;
    },50)


    var playerDestructor = undefined
    var getCurrentPosition = undefined


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
            controls: element.Controls ? 1 : 0
        }

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

                    element.Player = player
                    //element.PlayerReady = true
                },
                onStateChange: function (event) {
                    if (event.data == YT.PlayerState.ENDED) {
                        element.PlayInput = false
                    } else if (event.data == YT.PlayerState.PLAYING) {
                        element.PlayInput = true
                    } else if (event.data == YT.PlayerState.PAUSED) {
                        element.PlayInput = false
                    }
                    //console.log("onStateChange", event, element.PlayInput)
                    //onYoutubePlayerStateChange(event)
                }
            }
        })

        getCurrentPosition = function () {
            return player.getCurrentTime()
        }
        
        playerDestructor = function () {
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

    function InitializeHtml5Video() {
        element.VideoPlayerType = "video"

        /*var currentPosition = 0
        if (getCurrentPosition)
            currentPosition = getCurrentPosition()*/

        if (playerDestructor) {
            playerDestructor()
        }

        let player = document.createElement("video")
        element.appendChild(player)
        player.style.width = "100%"

        player.src = sources[1].url
        player.muted = true

        /*function onLoadedMetadata(event) {
            player.currentTime = currentPosition
        }*/

        function onTimeUpdate() {
            //console.log("onTimeUpdate", videoElement.currentTime)
        }

        //player.onloadedmetadata = (event) => { onLoadedMetadata(event) }

        player.ontimeupdate = onTimeUpdate;

        player.onplay = () => element.PlayInput = true
        player.onpause = () => element.PlayInput = false        
        player.onvolumechange = () => element.VolumeInput = player.volume
        

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


    //PositionInput
    new Reaction(() => {
        if (!element.Player)
            return

        if (!element.PositionInitialized)
            return   

        element.PositionUpdateTimer

        if (element.VideoPlayerType == "youtube") {
            element.PositionInput = element.Player.getCurrentTime()
        } else {
            element.PositionInput = element.Player.currentTime
        }        
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
            console.log("element.Volume", element.Volume)
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
