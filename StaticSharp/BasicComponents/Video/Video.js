

function EventPropertyDriver(readProperty, writePoperty, writeSource) {

    let entered = false
    new Reaction(() => {
        let value = readProperty()
        //console.log("property changed", value, entered)
        if (entered) {
            entered = false
        } else {
            //console.log("write from property", value)
            entered = true
            writeSource(value)
        }
    })

    this.setValueFromEvent = function (value) {
        //console.log("event fired", value, entered)
        if (entered) {
            entered = false
        } else {
            //console.log("write from event", value)
            entered = true
            writePoperty(value)
        }        
    }

    this.DoNotWaitForEvent = function () {
        entered = false
    }

}



StaticSharpClass("StaticSharp.Video", (element) => {
    StaticSharp.AspectBlockResizableContent(element)


    let player = element.content

    element.Reactive = {
        Controls: true,
        Loop: false,
        Play: false,
        CurrentTime: 0,
        Mute: false,
        Volume: 1,


    }


    //To implement later: This is the way to check if the "loop" flag has been changed by the user.It doesn't work with the "controls" attribute.
    /*var observer = new MutationObserver(function (mutations) {
        mutations.forEach((mutation) => {
            if (mutation.type === "attributes") {
                console.log(mutation.attributeName);
            }
        })
    })
    observer.observe(player, {
        attributes: true
    });*/



    let baseHtmlNodesOrdered = element.HtmlNodesOrdered
    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield element.content
        yield* baseHtmlNodesOrdered
    })

    new Reaction(() => {
        player.controls = element.Controls
    })

    new Reaction(() => {
        player.loop = element.Loop
    })

    


    let playOnUserInteracted = false
    new Reaction(() => {
        if (window.UserInteracted) {
            if (playOnUserInteracted) {
                player.play()
            }
        }
    })


    function play_EnableEvents() {
        player.onplay = () => {
            playReaction.enabled = false
            element.Play = true
            playReaction.enabled = true
        }

        player.onpause = () => {
            playReaction.enabled = false
            element.Play = false
            playReaction.enabled = true            
        }
    }

    function play_DisableEvents() {
        player.onplay = null
        player.onpause = null
    }


    /*new Reaction(() => {
        console.log("Transient Sticky", navigator.userActivation.isActive, navigator.userActivation.hasBeenActive)
        window.AnimationFrameTime
    })*/

    const waitingForUserInteractionCssClassName = "player-waiting-for-user-interaction";
    function beginWaitingForUserInteraction() {
        element.classList.add(waitingForUserInteractionCssClassName);
    }
    function endWaitingForUserInteraction() {
        element.classList.remove(waitingForUserInteractionCssClassName);
    }


    const playReaction = new Reaction(() => {
        if (element.Play == !player.paused)
            return

        if (window.UserInteracted) {
            endWaitingForUserInteraction()
        }

        play_DisableEvents()

        if (element.Play) {

            player.play()
                .then(() => {
                    play_EnableEvents()
                })
                .catch(() => {
                    //console.log("play failed")
                    beginWaitingForUserInteraction()
                    //play_EnableEvents()
                })


        } else {
            player.pause()
        }
    })


    let muteDriver = new EventPropertyDriver(
        () => element.Mute,
        x => element.Mute = x,
        muted => {
            if (player.muted == muted) {
                muteDriver.DoNotWaitForEvent()
            }
            try {
                player.muted = muted
            } catch {
                console.log("F")
            }
        }
    )

    let volumeDriver = new EventPropertyDriver(
        () => element.Volume,
        x => element.Volume = x,
        volume => {
            if (player.volume == volume) {
                volumeDriver.DoNotWaitForEvent()
            }
            player.volume = volume
        }
    )

    let oldVolume = player.volume
    let oldMuted = player.muted    

    player.onvolumechange = () => {
        if (oldVolume != player.volume) {
            //console.log('volumechange', player.volume);
            volumeDriver.setValueFromEvent(player.volume)
            oldVolume = player.volume
        }
        if (oldMuted != player.muted) {
            //console.log('volumechange', player.muted);
            muteDriver.setValueFromEvent(player.muted)
            oldMuted = player.muted
        }
    }


    let currentTimeDriver = new EventPropertyDriver(
        () => element.CurrentTime,
        x => element.CurrentTime = x,
        currentTime => {
            player.currentTime = currentTime
        }
    )

    player.ontimeupdate = () => currentTimeDriver.setValueFromEvent(player.currentTime)


})
