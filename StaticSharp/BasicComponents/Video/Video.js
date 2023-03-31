
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
    AspectBlock(element)
    element.isVideo = true

    let youtubeId = element.dataset.youtubeId

    var sourcesJson = element.dataset.sources.replace(/'/g, '"');// replaceAll("'", '"');
    var sources = JSON.parse(sourcesJson)



    element.Reactive = {
        Selectable: false,

        //Aspect: element.dataset.width / element.dataset.height,
        //InternalWidth: () => First(element.Height * element.Aspect, element.dataset.width),
        //InternalHeight: () => First(element.Width / element.Aspect, element.dataset.height),

        PreferPlatformPlayer: true,

        Controls: true,

        Loop: false,

        Play: false,
        PlayActual: () => element.Play,

        Position: 0,

        PositionActual: undefined,

        Mute: false,
        MuteActual: () => element.Mute,

        Volume: 1,
        VolumeActual: () => element.Volume,



        VideoPlayerType: undefined,//youtube, video

        Player: undefined,
        Positioner: undefined,

        PositionInitialized: false,

        SourceIndex: undefined


    }




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
                    let pixelWidth = window.DevicePixelRatio * element.Width
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


    function FitOutside(child, childAspect, parentWidth, parentHeight) {

    }

    function FitInside(child, childAspect, parentWidth, parentHeight) {
        let parentAspect = parentWidth / parentHeight

        if (parentAspect > childAspect) {
            child.style.top = 0
            child.style.height = element.Height + "px"

            let calculatedWidth = element.Height * element.Aspect
            child.style.left = 0.5 * (element.Width - calculatedWidth) + "px"
            child.style.width = calculatedWidth + "px"
        } else {
            child.style.left = 0
            child.style.width = element.Width + "px"

            let calculatedHeight = element.Width / element.Aspect
            child.style.top = 0.5 * (element.Height - calculatedHeight) + "px"
            child.style.height = calculatedHeight + "px"
        }
    }
    


    new Reaction(() => {
        let positioner = element.Positioner
        if (!positioner)
            return

        FitInside(positioner, element.Aspect, element.Width, element.Height)        
    })


    function InitializeYoutubeIFrame() {
        if (playerDestructor) {
            playerDestructor()            
        }

        element.VideoPlayerType = "youtube"

        var positioner = document.createElement("youtube-iframe-positioner");
        element.appendChild(positioner);
        element.Positioner = positioner

        var iframe = document.createElement("div");
        iframe.id = getUniqueID();

        positioner.appendChild(iframe);
        iframe.style.width = "100%"
        iframe.style.height = "100%"

        let playerVars = {
            rel: 0,
            //"autoplay": autoPlay ? 1 : 0,

            //???showinfo: 0,

            controls: element.Controls ? 1 : 0,
            origin: window.location.origin
        }

        if (element.Loop) {
            playerVars.loop = 1
            playerVars.playlist = youtubeId
        }

        let volumeTimeout = undefined
        let positionTimeout = undefined

        let player = new YT.Player(iframe.id, {
            videoId: youtubeId,
            host: "http://www.youtube-nocookie.com",
            playerVars: playerVars,
            events: {
                onReady: () => {                    
                    /*if (currentPosition !== 0) {
                        player.seekTo(currentPosition, true)
                    }*/

                    element.PositionActual = () => {
                        clearTimeout(positionTimeout)

                        if (!element.Player)
                            return undefined

                        let result = element.Player.getCurrentTime()
                        if (element.PlayActual) {
                            positionTimeout = window.setTimeout(() => {
                                element.Reactive.PositionActual.makeDirty()
                            }, 50)
                        }
                        return result
                    }



                    element.VolumeActual = () => {
                        clearTimeout(volumeTimeout)

                        if (!element.Player)
                            return undefined

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
        
        
        playerDestructor = function () {
            clearTimeout(volumeTimeout)
            clearTimeout(positionTimeout)
            
            playerDestructor = undefined

            player.destroy()
            element.removeChild(positioner)

            let d = Reaction.beginDeferred()
            element.Player = undefined
            d.end()
        }        
    }



    function InitializeHtml5Video() {
        element.VideoPlayerType = "html5"

        if (playerDestructor) {
            playerDestructor()
        }

        let player = document.createElement("video")
        element.appendChild(player)
        element.Positioner = player

        player.src = sources[1].url
        player.muted = true

        player.setAttribute("playsinline", "true")
        player.setAttribute("webkit-playsinline", "true")

        player.setAttribute("x5-video-player-type", "h5")
        //player.setAttribute("autoplay", "true")
        

        player.ontimeupdate = () => element.PositionActual = element.Player.currentTime

        function onPlayOrPause() {
            if (element.Play == element.Player.paused) {//anction is done by user
                window.UserInteracted = true
            }
        }

        player.onplay = () => {
            let d = Reaction.beginDeferred()
            element.PlayActual = true
            onPlayOrPause();
            d.end()
        }
        player.onpause = () => {
            let d = Reaction.beginDeferred()
            element.PlayActual = false
            onPlayOrPause()
            d.end()
        }


        element.VolumeActual = player.volume
        player.onvolumechange = () => {
            let d = Reaction.beginDeferred()
            //window.UserInteracted = true TODO
            element.VolumeActual = player.volume
            element.MuteActual = player.muted
            d.end()
        }

        player.oncanplay = () => {
            element.Player = player
        }

        
        playerDestructor = function () {
            playerDestructor = undefined

            player.ontimeupdate = undefined
            player.onpause = undefined
            player.onplay = undefined
            player.onvolumechange = undefined

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

    /*if (window.history && history.pushState) { // check for history api support
        window.addEventListener('load', function () {
            // create history states
            history.pushState(-1, null); // back state
            history.pushState(0, null); // main state
            history.pushState(1, null); // forward state
            history.go(-1); // start in main state

            this.addEventListener('popstate', function (event, state) {
                // check history state and fire custom events
                if (state = event.state) {

                    event = document.createEvent('Event');
                    event.initEvent(state > 0 ? 'next' : 'previous', true, true);
                    this.dispatchEvent(event);

                    // reset state
                    history.go(-state);
                }
            }, false);
        }, false);
    }*/

    this.addEventListener('popstate', function (event, state) {
        console.log("popstate")

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

            window.UserInteracted //for wechat

            if (element.Play == element.Player.paused) {
                element.Play ? element.Player.play() : element.Player.pause()
            }

            //element.Player.play()
            

            
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
                    var value = (!window.UserInteracted) || element.Mute
                    element.Player.muted = value
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

}
