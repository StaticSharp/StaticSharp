function LanguageMenu(element) {
    element.width = 30;
    let topOffset = 5;
    element.position = '';



    function swipeAction(direction, swipe, touchEnd, e) {
        let getTranslate = () => {
            var value = direction == 'left' || direction == 'right' ? swipe.swipeX : swipe.swipeY;
            let percent = toPercents(Math.abs(value), element.offsetWidth);
            let translate = percent > 0 ? percent : 0;
            return translate;
        }
        let horizontal = direction == 'left' || direction == 'right';
        let translate = getTranslate();
        if (element.position == 'minimizedRight' && direction == 'left') {
            showChildren();
            translate = 100 - translate;
            element.css({
                transform: `translateX(clamp(0%, ${translate}%, 70%))`
            });
        } else if (element.position == 'extend' && direction == 'right') {
            let translate = getTranslate();
            element.css({
                transform: `translateX(clamp(0%, ${translate}%, 70%))`
            });
        }
        if (horizontal && touchEnd) {
            if (translate < 40) extend();
            else minimizedRight();
        }
    }

    function extend() {
        element.position = 'extend';
        element.css({
            padding: '10px',
            margin: '0px',
            borderRadius: '0px',
            backgroundColor: 'rgb(227, 227, 227)',
            top: '5px',
            height: "auto",
            transform: 'unset',
        });

        Array.from(element.children).forEach(x => {
            x.css({
                visibility: "visible",
                display: "block",
                padding: "5px",
                textDecoration: "none",
                fontSize: "20px",
                color: "black",
            });
        });
        if (!parent.anchors.wideAnchorsCollapsed) {
            //element.onclick = element.onmouseleave = null;
        }
    }

    function disableScrolling() {
        //if (element.parentElement.style.visibility == "hidden") {
        TopScroll = window.pageYOffset || document.documentElement.scrollTop;
        LeftScroll = window.pageXOffset || document.documentElement.scrollLeft,
            window.onscroll = function() {
                if (element.parentElement.style.visibility == "hidden") {
                    window.scrollTo(LeftScroll, TopScroll);
                }
            };
        //}
    }

    function enableScrolling() {
        window.onscroll = function() {
            console.log(element.parentElement.style.visibility);
            if (element.parentElement.style.visibility == "hidden") {
                if (window.scrollY == 0) {
                    console.log("top right");
                    element.css({
                        //visibility: "hidden",
                        transform: "translateX(0%)",
                        visibility: "visible",
                        width: "25px",
                        height: "25px",
                        borderRadius: "50%",
                    })
                } else {
                    console.log("not the top right");
                    element.css({
                        borderRadius: "10px 0 0 10px",
                        height: "auto",
                        transform: "translateX(70%)",
                        top: topOffset + "px",
                        right: "0",
                        visibility: "visible"
                    })
                }
            }
        };

    }

    function hideChildren() {
        Array.from(element.children).forEach(x => {
            x.css({ visibility: "hidden" });
        });
    }

    this.showChildren = function() {
        Array.from(element.children).forEach(x => {
            x.css({ visibility: "visible" });
        });
    }

    function minimizedRight() {
        //extend();
        element.position = 'minimizedRight';
        element.css({
            borderRadius: "10px 0 0 10px",
            height: "auto",
            transform: "translateX(70%)",
            top: topOffset + "px",
            right: "0",
            visibility: "visible"
        });
        hideChildren();
        if (element.parentElement.style.visibility == "hidden") {
            element.onclick = (e) => {
                //e.preventDefault();
                console.log("haha");
                disableScrolling();
                //hideBackground();
                extend();
            }
        }
    }

    document.addEventListener("click", (evt) => {
        if (element.parentElement.style.visibility == "hidden") {
            const flyoutElement = document.getElementById("rightBar");
            let targetElement = evt.target;

            while (targetElement) {
                if (targetElement == flyoutElement) {
                    console.log("inside right");
                    return;
                }
                targetElement = targetElement.parentNode;
            };
            console.log("outside right");
            enableScrolling();
            minimizedRight();
        }
    });

    window.onscroll = function() {
        console.log(element.parentElement.style.visibility);
        if (element.parentElement.style.visibility == "hidden") {
            if (window.scrollY == 0) {
                console.log("top right");
                element.css({
                    //visibility: "hidden",
                    transform: "translateX(0%)",
                    visibility: "visible",
                    width: "25px",
                    height: "25px",
                    borderRadius: "50%",
                })
            } else {
                console.log("not the top right");
                element.css({
                    borderRadius: "10px 0 0 10px",
                    height: "auto",
                    transform: "translateX(70%)",
                    top: topOffset + "px",
                    right: "0",
                    visibility: "visible"
                })
            }
        }
    }

    window.onloadRight = function() {
        console.log("loaded");
        if (element.parentElement.style.visibility == "hidden") {
            if (window.scrollY == 0) {
                console.log("top right");
                element.css({
                    //visibility: "hidden",
                    transform: "translateX(0%)",
                    visibility: "visible",
                    width: "25px",
                    height: "25px",
                    borderRadius: "50%",
                })
            } else {
                console.log("not the top right");
                element.css({
                    borderRadius: "10px 0 0 10px",
                    height: "auto",
                    transform: "translateX(70%)",
                    top: topOffset + "px",
                    right: "0",
                    visibility: "visible"
                })
            }
        }
    }

    window.onscrollRight = function() {
        console.log(element.parentElement.style.visibility);
        if (element.parentElement.style.visibility == "hidden") {
            if (window.scrollY == 0) {
                console.log("top right");
                element.css({
                    //visibility: "hidden",
                    transform: "translateX(0%)",
                    visibility: "visible",
                    width: "25px",
                    height: "25px",
                    borderRadius: "50%",
                })
            } else {
                console.log("not the top right");
                element.css({
                    borderRadius: "10px 0 0 10px",
                    height: "auto",
                    transform: "translateX(70%)",
                    top: topOffset + "px",
                    right: "0",
                    visibility: "visible"
                })
            }
        }
    }

    window.onload = function() {
        console.log("loaded");
        if (element.parentElement.style.visibility == "hidden") {
            if (window.scrollY == 0) {
                console.log("top right");
                element.css({
                    //visibility: "hidden",
                    transform: "translateX(0%)",
                    visibility: "visible",
                    width: "25px",
                    height: "25px",
                    borderRadius: "50%",
                })
            } else {
                console.log("not the top right");
                element.css({
                    borderRadius: "10px 0 0 10px",
                    height: "auto",
                    transform: "translateX(70%)",
                    top: topOffset + "px",
                    right: "0",
                    visibility: "visible"
                })
            }
        }
    }

    const parent = element.parentElement.parentElement;
    element.updateWidth = () => {
        if (parent.anchors.wideAnchorsCollapsed) {
            minimizedRight();
        } else {
            extend();
        }
    }

    extend();
    parent.onAnchorsChanged.push(element.updateWidth);

    // function hideBackground() {
    //     if (!$(event.target).is('menu')) {
    //         console.log("body");
    //     }
    // }
}