function LanguageMenu(element) {
    element.width = 30;
    let topOffset = 5;
    element.position = '';

    element.onclick = function() {
        alert("BBB");
    }

    function disableScrolling() {
        console.log("disabled");
        TopScroll = window.pageYOffset || document.documentElement.scrollTop;
        LeftScroll = window.pageXOffset || document.documentElement.scrollLeft,
            window.onscroll = function() {
                if (element.parentElement.style.visibility == "hidden") {
                    window.scrollTo(LeftScroll, TopScroll);
                }
            };
    }
    window.myEnableScrolling = function() {
        if (element.parentElement.style.visibility == "hidden") {
            if (window.scrollY == 0) {
                console.log("top left");
                element.css({
                    transform: "translateX(0%)",
                    visibility: "visible",
                    width: "25px",
                    height: "25px",
                    borderRadius: "50%",
                    content: "url(https://api.iconify.design/ic/baseline-translate.svg?color=white)"
                })
            } else {
                element.css({
                    width: "auto",
                    content: ""
                })
                minimizedRight();
            }
        }
    };

    function enableScrolling() {
        window.onscroll = function() {
            if (element.parentElement.style.visibility == "hidden") {
                if (window.scrollY == 0) {
                    console.log("top left");
                    element.css({
                        transform: "translateX(0%)",
                        visibility: "visible",
                        width: "25px",
                        height: "25px",
                        borderRadius: "50%",
                        content: "url(https://api.iconify.design/ic/baseline-translate.svg?color=white)",
                    })
                } else {
                    element.css({
                        width: "auto",
                        content: "",
                    })
                    minimizedRight();
                }
            }
        }
    }

    document.addEventListener("click", (evt) => {
        if (element.parentElement.style.visibility == "hidden") {
            const flyoutElement = document.getElementById("rightBar");
            let targetElement = evt.target;

            while (targetElement) {
                if (targetElement == flyoutElement) {
                    console.log("inside left");
                    extend();
                    disableScrolling();
                    return;
                }
                targetElement = targetElement.parentNode;
            };
            console.log("outside left");
            enableScrolling();
            element.css({
                width: "auto",
                content: ""
            })
            minimizedRight();
        }
    });

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
    window.myMinimizedRight = minimizedRight();

    function minimizedRight() {
        console.log("minimized");
        element.position = 'minimizedRight';
        element.css({
            borderRadius: "10px 10px 10px 10px",
            height: "auto",
            transform: "translateX(70%)",
            backgroundColor: 'rgb(88, 131, 204)',
            top: topOffset + "px",
            right: "0",
            visibility: "visible",
            padding: '10px',
        });
        hideChildren();
    }
    window.myExtend = extend();

    function extend() {
        console.log("exetended");
        element.position = 'extend';
        element.css({
            padding: '10px',
            margin: '0px',
            borderRadius: '0px',
            backgroundColor: '#3b424d',
            top: '5px',
            height: "auto",
            transform: 'unset',
            content: ""
        });
        if (element.parentElement.style.visibility == "hidden") {
            element.css({
                width: "auto",
                content: ""
            });
        }

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
    }

    window.myOnLoad = function() {
        if (element.parentElement.style.visibility == "hidden") {
            if (window.scrollY == 0) {
                console.log("top left");
                element.css({
                    transform: "translateX(0%)",
                    visibility: "visible",
                    width: "25px",
                    height: "25px",
                    borderRadius: "50%",
                })
            } else {
                element.css({
                    width: "auto",
                })
                minimizedRight();
            }
        }
    }

    window.onload = function() {
        if (window.scrollY == 0) {
            console.log("top left");
            element.css({
                transform: "translateX(0%)",
                visibility: "visible",
                width: "25px",
                height: "25px",
                borderRadius: "50%",
            })
        } else {
            element.css({
                width: "auto",
            })
            minimizedRight();
        }
    }

    const parent = element.parentElement.parentElement;
    element.updateWidth = () => {
        if (parent.anchors.wideAnchorsCollapsed) {
            enableScrolling();
            minimizedRight();
        } else {
            extend();
        }
    }

    parent.onAnchorsChanged.push(element.updateWidth);
}