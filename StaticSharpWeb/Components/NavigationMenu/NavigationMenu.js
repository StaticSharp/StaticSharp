function NavigationMenu(element) {
    element.width = 200;
    let topOffset = 5;
    element.position = '';

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

    function enableScrolling() {
        window.onscroll = function() {
            myEnableScrolling();
            if (element.parentElement.style.visibility == "hidden") {
                if (window.scrollY == 0) {
                    console.log("top left");
                    element.css({
                        transform: "translateX(0%)",
                        visibility: "visible",
                        width: "25px",
                        height: "25px",
                        borderRadius: "50%",
                        content: "url(https://api.iconify.design/ci/hamburger.svg?color=white)",
                    })
                } else {
                    element.css({
                        width: "auto",
                        content: "",
                    })
                    minimizedLeft();
                }
            }
        }
    }

    document.addEventListener("click", (evt) => {
        if (element.parentElement.style.visibility == "hidden") {
            const flyoutElement = document.getElementById("leftMenu");
            const flyoutElementRight = document.getElementById("rightMenu");
            const glassElement = document.getElementById("Glass");
            let targetElement = evt.target;
            console.log(targetElement);
            while (targetElement) {
                if (targetElement == flyoutElement) {
                    console.log("inside left");
                    glassElement.css({
                        visibility: "visible",
                        opacity: "0.3",
                        zIndex: "3"
                    })
                    targetElement.css({
                        zIndex: "3",
                        padding: "",
                        paddintTop: "10px"
                    })
                    extend();
                    disableScrolling();
                    return;
                } else if (targetElement == flyoutElementRight) {
                    console.log("inside right");
                    glassElement.css({
                            visibility: "visible",
                            opacity: "0.3",
                            zIndex: "3"
                        })
                        // targetElement.css({
                        //         zIndex: "1"
                        //     })
                        //myExtend();
                    disableScrolling();
                    return;
                }
                targetElement = targetElement.parentNode;
                //console.log(targetElement); 
            };
            console.log("outside left");
            enableScrolling();
            glassElement.css({
                visibility: "hidden",
                opacity: "0"
            })
            element.css({
                width: "auto",
                zIndex: "-2"
                    //content: ""
            })
            if (window.scrollY == 0) {
                element.css({
                    width: "auto"
                        //content: url('https://api.iconify.design/ci/hamburger.svg'),
                })
            }
            minimizedLeft();
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

    function minimizedLeft() {
        console.log("minimized");
        element.position = 'minimizedRight';
        if (window.scrollY != 0) {
            element.css({
                borderRadius: "10px 10px 10px 10px",
                height: "auto",
                transform: "translateX(-90%)",
                backgroundColor: 'rgb(172, 196, 53)',
                top: topOffset + "px",
                right: "0",
                visibility: "visible",
                padding: '10px',
                zIndex: "-2"
                    //content: url('https://api.iconify.design/ci/hamburger.svg')
            });
        } else {
            element.css({
                transform: "translateX(0%)",
                visibility: "visible",
                width: "25px",
                height: "25px",
                backgroundColor: 'rgb(172, 196, 53)',
                borderRadius: "50%",
                padding: "10px",
                content: "url(https://api.iconify.design/ci/hamburger.svg?color=white)",
                zIndex: "-2"
                    //content: url('https://api.iconify.design/ci/hamburger.svg')
            })
        }
        hideChildren();
    }

    function extend() {
        console.log("exetended");
        element.position = 'extend';
        element.css({
            paddingTop: '10px',
            margin: '0px',
            borderRadius: '0px',
            backgroundColor: '#3b424d',
            top: '5px',
            height: "100vh",
            transform: 'unset',
            content: ""
        });
        if (element.parentElement.style.visibility == "hidden") {
            element.css({
                width: "auto",
                //content: ""
            });
        }

        Array.from(element.children).forEach(x => {
            x.css({
                visibility: "visible",
                display: "block",
                paddingTop: "5px",
                textDecoration: "none",
                fontSize: "20px",
                color: "black",
            });
        });
    }

    window.onload = function() {
        myOnLoad();
        if (element.parentElement.style.visibility == "hidden") {
            if (window.scrollY == 0) {
                console.log("top left");
                element.css({
                    transform: "translateX(0%)",
                    visibility: "visible",
                    width: "25px",
                    height: "25px",
                    borderRadius: "50%",
                    //content: url('https://api.iconify.design/ci/hamburger.svg')
                })
            } else {
                element.css({
                    width: "auto",
                    content: ""
                })
                minimizedLeft();
            }
        }
    }

    const parent = element.parentElement.parentElement;
    element.updateWidth = () => {
        if (parent.anchors.wideAnchorsCollapsed) {
            enableScrolling();
            element.css({
                width: "auto",
                //content: url('https://api.iconify.design/ci/hamburger.svg')
            })
            minimizedLeft();
        } else {
            extend();
        }
    }

    parent.onAnchorsChanged.push(element.updateWidth);
}