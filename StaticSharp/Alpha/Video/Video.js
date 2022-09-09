
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


        Controls: () => !element.Hover,

        Play: false,
        Position: 0,
        Sound: true,
        Loop: true,

        VideoPlayerType: undefined,//youtube, html5

        Player: undefined,
        YoutubePlayerReady: false
    }

    var playerDestructor = undefined
    var getCurrentPosition = undefined






    /*
     if (!showControls) {
                iframe.style.pointerEvents = "none";
            }


     */



    function InitializeYoutubeIFrame() {

        var currentPosition = 0
        if (getCurrentPosition)
            currentPosition = getCurrentPosition()

        if (playerDestructor) {
            playerDestructor()            
        }

        element.VideoPlayerType = "youtube"

        var iframe = document.createElement("div");
        iframe.id = getUniqueID();
        element.appendChild(iframe);

        let player = new YT.Player(iframe.id, {
            height: '100%',
            width: '100%',
            videoId: youtubeId,
            host: "http://www.youtube-nocookie.com",
            playerVars: {
                "rel": 0,
                //"autoplay": autoPlay ? 1 : 0,
                "controls": element.Controls ? 1 : 0,
                //"mute": autoPlay ? 1 : (sound ? 0 : 1),
            },
            events: {
                'onReady': function (event) {                    
                    if (currentPosition !== 0) {
                        player.seekTo(currentPosition, true)
                    }
                    element.YoutubePlayerReady = true
                },
                'onStateChange': function (event) {
                    console.log(event)
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
            element.YoutubePlayerReady = false
            d.end()
        }
        
        element.Player = player

        /*callWhenYouTubeIFrameApiReady(function () {
            
            element.appendChild(iframe);
            var controls = showControls ? 1 : 0;
            player = new YT.Player(iframe.id, {

                height: '100%',
                width: '100%',
                videoId: code,
                host: "http://www.youtube-nocookie.com",
                playerVars: {
                    "rel": 0,
                    "autoplay": autoPlay ? 1 : 0,
                    "controls": showControls ? 1 : 0,
                    "mute": autoPlay ? 1 : (sound ? 0 : 1),
                },
                events: {
                    'onReady': function (event) {
                        onYoutubePlayerReady(event)
                    },
                    'onStateChange': function (event) { onYoutubePlayerStateChange(event) }
                }
            });
        })*/
    }

    function InitializeHtml5Video() {
        element.VideoPlayerType = "video"

        var currentPosition = 0
        if (getCurrentPosition)
            currentPosition = getCurrentPosition()

        if (playerDestructor) {
            playerDestructor()
        }

        let player = document.createElement("video")
        element.appendChild(player)
        player.style.width = "100%"

        player.src = sources[0].url


        function onLoadedMetadata(event) {
            player.currentTime = currentPosition
        }

        function onTimeUpdate() {
            //console.log("onTimeUpdate", videoElement.currentTime)
        }

        player.onloadedmetadata = (event) => { onLoadedMetadata(event) }
        player.ontimeupdate = onTimeUpdate;



        element.Player = player

        getCurrentPosition = function () {
            return player.currentTime
        }

        playerDestructor = function () {
            playerDestructor = undefined
            getCurrentPosition = undefined

            element.removeChild(player)

            let d = Reaction.beginDeferred()
            element.Player = undefined
            d.end()
        }


    }





    new Reaction(() => {
        //console.log("element._ ", element.__Play)
        



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

    //Play
    new Reaction(() => {
        if (element.VideoPlayerType == "youtube" && element.YoutubePlayerReady) {
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

    //Sound
    new Reaction(() => {
        if (element.VideoPlayerType == "youtube" && element.YoutubePlayerReady) {
            if (element.Sound) {
                element.Player.unMute()
            } else {
                element.Player.mute()
            }
        }
    })


    

    





    new Reaction(() => {

        /*if (YouTube.Status == "ready") {


        }
        if (YouTube.Status == "error") {


        }*/

        /*if (videoElement == undefined) {
            videoElement = document.createElement("video")
            element.appendChild(videoElement)

            videoElement.src = sources[0].url

            videoElement.onloadedmetadata = (event) => { onLoadedMetadata(event) }
            videoElement.ontimeupdate = onTimeUpdate;
        }
        
        videoElement.width = element.Width
        videoElement.height = element.Height
        videoElement.autoplay = element.AutoPlay
        videoElement.muted = element.AutoPlay || (!element.Sound)
        videoElement.loop = element.Loop;
        videoElement.controls = element.Controls*/

        //element.AutoPlay

        //videoElement.play()
    })



    WidthToStyle(element)
    HeightToStyle(element)
}
