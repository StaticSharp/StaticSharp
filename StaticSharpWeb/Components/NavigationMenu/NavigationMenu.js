function NavigationMenu(element) {
    element.width = 200;
    let topOffset = 5;
    element.position = '';

    function extend() {
        element.position = 'extend';
        element.css({
            padding: '10px',
            margin: '0px',
            borderRadius: '0px',
            //backgroundColor: 'rgb(227, 227, 227)',
            backgroundColor: '#3b424d',
            top: '5px',
            height: "100vh",
            transform: 'unset',
            //width: "auto",
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
                //onscrollRight();
                if (element.parentElement.style.visibility == "hidden") {
                    window.scrollTo(LeftScroll, TopScroll);
                }
            };
        //}
    }

    function enableScrolling() {
        window.onscroll = function() {
            onscrollRight();
            console.log(element.parentElement.style.visibility);
            if (element.parentElement.style.visibility == "hidden") {
                if (window.scrollY == 0) {
                    console.log("top left");
                    element.css({
                        //visibility: "hidden",
                        transform: "translateX(0%)",
                        visibility: "visible",
                        width: "25px",
                        height: "25px",
                        borderRadius: "50%",
                    })
                } else {
                    console.log("not the top left");
                    element.css({
                        borderRadius: "10px 10px 10px 10px",
                        height: "auto",
                        transform: "translateX(-90%)",
                        top: topOffset + "px",
                        width: "auto",
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
            borderRadius: "10px 10px 10px 10px",
            height: "auto",
            transform: "translateX(-90%)",
            backgroundColor: 'rgb(227, 227, 227)',
            top: topOffset + "px",
            right: "0",
            visibility: "visible"
        });
        hideChildren();
        if (element.parentElement.style.visibility == "hidden") {
            element.onclick = (e) => {
                //e.preventDefault();
                console.log("haha");
                element.css({
                    width: "auto",
                });
                //console.log(element.parentElement);
                disableScrolling();
                //hideBackground();
                extend();
            }
        }
    }

    document.addEventListener("click", (evt) => {
        if (element.parentElement.style.visibility == "hidden") {
            element.css({
                width: "auto",
            });
            const flyoutElement = document.getElementById("leftBar");
            let targetElement = evt.target;

            while (targetElement) {
                if (targetElement == flyoutElement) {
                    console.log("inside left");
                    return;
                }
                targetElement = targetElement.parentNode;
            };
            console.log("outside left");
            enableScrolling();
            minimizedRight();
        }
    });



    window.onscroll = function() {
        onscrollRight();
        console.log(element.parentElement.style.visibility);
        if (element.parentElement.style.visibility == "hidden") {
            if (window.scrollY == 0) {
                console.log("top left");
                element.css({
                    //visibility: "hidden",
                    transform: "translateX(0%)",
                    visibility: "visible",
                    width: "25px", //auto
                    height: "25px",
                    borderRadius: "50%",
                })
            } else {
                console.log("not the top left");
                element.css({
                    borderRadius: "10px 10px 10px 10px",
                    height: "auto",
                    transform: "translateX(-90%)",
                    top: topOffset + "px",
                    right: "0",
                    width: "auto",
                    visibility: "visible"
                })
            }
        }
    }

    window.onload = function() {
        console.log("loaded");
        onloadRight();
        // getscript("LanguageMenu.js", function() {
        //     testr();
        // });
        if (element.parentElement.style.visibility == "hidden") {
            if (window.scrollY == 0) {
                console.log("top left");
                element.css({
                    //visibility: "hidden",
                    transform: "translateX(0%)",
                    visibility: "visible",
                    width: "25px",
                    height: "25px",
                    borderRadius: "50%",
                })
            } else {
                console.log("not the top left");
                element.css({
                    borderRadius: "10px 0 0 10px",
                    height: "auto",
                    transform: "translateX(-60%)",
                    top: topOffset + "px",
                    right: "0",
                    width: "auto",
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
            element.css({
                width: "auto", //auto
            })
            extend();
        }
    }

    extend();
    parent.onAnchorsChanged.push(element.updateWidth);
}