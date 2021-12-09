var youTubeIFrameApiIsReady = false;
var youTubeIFrameApiWaitList = [];
var youTubeIFrameID = 0;

function getUniqueYouTubeIFrameID() {

    youTubeIFrameID++;
    return "youTubeIFrameID" + youTubeIFrameID;

}

function callWhenYouTubeIFrameApiReady(callback) {

    if (youTubeIFrameApiIsReady) {
        callback()
    } else {
        youTubeIFrameApiWaitList.push(callback);
    }
}

function onYouTubeIframeAPIReady() {
    youTubeIFrameApiWaitList.forEach(element => element());
}

function Video(element, code, aspect, showControls, autoPlay, loop, sound, mips, poster) {

    this.element = element;
    this.element.onAnchorsChanged = [];
    let parent = element.parentElement;
    element.updateWidth = function() {
        let left = parent.anchors.textLeft;
        let right = parent.anchors.textRight;
        element.style.marginLeft = left + "px";
        element.style.width = right - left + "px";
        //element.style.backgroundColor = "red";
    }
    parent.onAnchorsChanged.push(this.element.updateWidth);

    var ResizeContainer = function() {
        element.style.height = (element.offsetWidth / aspect) + "px";
    }
    window.addEventListener("resize", ResizeContainer);

    this.onYoutubePlayerReady = function(event) {
        /*if (this.autoPlay) {
          this.player.playVideo();
        }*/
    }

    function onYoutubePlayerStateChange(event) {
        /*if (event.data == YT.PlayerState.PLAYING) {
            //this.player.unMute()
        }*/
        if (event.data == YT.PlayerState.ENDED) {
            if (loop) {
                player.seekTo(0);
                player.playVideo();
            }
            return;
        }

    }

    function CreateYoutubeIFrame() {
        callWhenYouTubeIFrameApiReady(function() {
            var iframe = document.createElement("div");
            iframe.className = "VideoInner";
            if (!showControls) {
                //iframe.style.top = "-100%";
                //iframe.style.height = "300%";
                iframe.style.pointerEvents = "none";
            }

            iframe.id = getUniqueYouTubeIFrameID();
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
                    'onReady': function(event) {
                        onYoutubePlayerReady(event)
                    },
                    'onStateChange': function(event) { onYoutubePlayerStateChange(event) }
                }
            });


        })

    }

    function CreateVideoTag(withPoster) {
        var video = document.createElement("video");
        video.className = "VideoInner";
        video.poster = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";
        video.controls = showControls;
        video.autoplay = autoPlay;
        video.muted = autoPlay || (!sound);
        video.loop = loop;
        element.appendChild(video);
        video.style.height = element.style.height;
        var previousContainerWidth = element.offsetWidth * window.devicePixelRatio;

        var SelectClosestMip = function(elementWidth) {
            for (const [key] of Object.entries(mips)) {
                var width = key * aspect;
                if (width > 0.95 * elementWidth) return mips[key];
            }
            return Object.values(mips).at(-1);
        }
        let closestMip = SelectClosestMip(element.offsetWidth * window.devicePixelRatio);
        video.src = closestMip.Value;

        var onResize = function() {

            var width = element.offsetWidth * window.devicePixelRatio;

            if (previousContainerWidth == width) {
                return;
            }
            previousContainerWidth = width;
            var newClosestMip = SelectClosestMip(width);

            if (newClosestMip.Key != closestMip.Key) {
                closestMip = newClosestMip;
                var time = video.currentTime;
                video.src = closestMip.Value;
                video.currentTime = time;
            }
        }
        window.addEventListener("resize", onResize);

    }

    function CreateYouTubeIFrameApiScript() {
        const id = "YouTubeIFrameApiScript";
        var apiElement = document.getElementById(id);

        if (apiElement === null) {

            apiElement = document.createElement("script");
            apiElement.id = id;

            apiElement.src = "https://www.youtube.com/iframe_api";
            window.YouTubeIFrameApiStatus = "loading";
            window.YouTubeIFrameApiStatusTimeout = setTimeout(() => {
                apiElement.onerror && apiElement.onerror()
            }, 1000);

            var head = document.getElementsByTagName('head')[0];
            head.appendChild(apiElement);

        }



        if (poster) {
            CreateVideoTag(poster);

            return;
        }

        if (window.YouTubeIFrameApiStatus === "loading") {

            var prewOnLoad = apiElement.onload;
            apiElement.onload = function() {
                apiElement.onload = null;
                apiElement.onerror = null;

                if (window.YouTubeIFrameApiStatus === "loading") {
                    window.YouTubeIFrameApiStatus = "ready"
                }
                if (apiElement.onerror !== null) apiElement.onerror = null;
                prewOnLoad && prewOnLoad();
                CreateYoutubeIFrame();
            };

            var prewOnError = apiElement.onerror;
            apiElement.onerror = function() {
                apiElement.onload = null;
                apiElement.onerror = null;

                if (window.YouTubeIFrameApiStatus === "loading") {
                    window.YouTubeIFrameApiStatus = "failed"
                }
                prewOnError && prewOnError();
                CreateVideoTag();
            };

        } else {
            if (window.YouTubeIFrameApiStatus === "ready") {
                CreateYoutubeIFrame();
            }
            if (window.YouTubeIFrameApiStatus === "failed") {
                CreateVideoTag();
            }
        }
    }

    function OnDOMContentLoaded(event) {

    }

    document.addEventListener("DOMContentLoaded", function() {
        ResizeContainer();

        //if (!showControls || autoPlay) {
        //   CreateVideoTag()
        //} else {
        CreateYouTubeIFrameApiScript();
        //}
    });

}