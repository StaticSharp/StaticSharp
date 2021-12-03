// function NavigationMenu(element) {
//     element.width = 200;
//     let topOffset = 5;
//     element.position = '';

//     function extend() {
//         element.position = 'extend';
//         element.css({
//             padding: '10px',
//             margin: '0px',
//             borderRadius: '0px',
//             //backgroundColor: 'rgb(227, 227, 227)',
//             backgroundColor: '#3b424d',
//             top: '5px',
//             height: "100vh",
//             transform: 'unset',
//             //width: "auto",
//         });

//         Array.from(element.children).forEach(x => {
//             x.css({
//                 visibility: "visible",
//                 display: "block",
//                 padding: "5px",
//                 textDecoration: "none",
//                 fontSize: "20px",
//                 color: "black",
//             });
//         });
//         if (!parent.anchors.wideAnchorsCollapsed) {
//             //element.onclick = element.onmouseleave = null;
//         }
//     }

//     function disableScrolling() {
//         //if (element.parentElement.style.visibility == "hidden") {
//         console.log("disable");
//         ondisableRight();
//         TopScroll = window.pageYOffset || document.documentElement.scrollTop;
//         LeftScroll = window.pageXOffset || document.documentElement.scrollLeft,
//             window.onscroll = function() {
//                 //onscrollRight();

//                 console.log("try to scroll");
//                 if (element.parentElement.style.visibility == "hidden") {
//                     console.log("bad try");
//                     window.scrollTo(LeftScroll, TopScroll);
//                 }
//             };
//         //}
//     }

//     function enableScrolling() {
//         window.onscroll = function() {
//             onscrollRight();
//             console.log(element.parentElement.style.visibility);
//             if (element.parentElement.style.visibility == "hidden") {
//                 if (window.scrollY == 0) {
//                     console.log("top left");
//                     element.css({
//                         //visibility: "hidden",
//                         transform: "translateX(0%)",
//                         visibility: "visible",
//                         width: "25px",
//                         height: "25px",
//                         borderRadius: "50%",
//                     })
//                 } else {
//                     console.log("not the top left");
//                     element.css({
//                         borderRadius: "10px 10px 10px 10px",
//                         height: "auto",
//                         transform: "translateX(-90%)",
//                         top: topOffset + "px",
//                         width: "auto",
//                         right: "0",
//                         visibility: "visible"
//                     })
//                 }
//             }
//         };

//     }

//     function hideChildren() {
//         Array.from(element.children).forEach(x => {
//             x.css({ visibility: "hidden" });
//         });
//     }

//     this.showChildren = function() {
//         Array.from(element.children).forEach(x => {
//             x.css({ visibility: "visible" });
//         });
//     }

//     function minimizedRight() {
//         //extend();
//         element.position = 'minimizedRight';
//         element.css({
//             borderRadius: "10px 10px 10px 10px",
//             height: "auto",
//             transform: "translateX(-90%)",
//             backgroundColor: 'rgb(172, 196, 53)',
//             top: topOffset + "px",
//             right: "0",
//             visibility: "visible"
//         });
//         hideChildren();
//         if (element.parentElement.style.visibility == "hidden") {
//             element.onclick = (e) => {
//                 //e.preventDefault();
//                 console.log("clicked left");
//                 element.css({
//                     width: "auto",
//                 });
//                 //console.log(element.parentElement);
//                 disableScrolling();
//                 //hideBackground();
//                 extend();
//             }
//         }
//     }

//     document.addEventListener("click", (evt) => {
//         if (element.parentElement.style.visibility == "hidden") {
//             element.css({
//                 width: "auto",
//             });
//             const flyoutElement = document.getElementById("leftBar");
//             let targetElement = evt.target;

//             while (targetElement) {
//                 if (targetElement == flyoutElement) {
//                     console.log("inside left");
//                     return;
//                 }
//                 targetElement = targetElement.parentNode;
//             };
//             console.log("outside left");
//             enableScrolling();
//             minimizedRight();
//         }
//     });



//     window.onscroll = function() {
//         onscrollRight();
//         console.log(element.parentElement.style.visibility);
//         if (element.parentElement.style.visibility == "hidden") {
//             if (window.scrollY == 0) {
//                 console.log("top left");
//                 element.css({
//                     //visibility: "hidden",
//                     transform: "translateX(0%)",
//                     visibility: "visible",
//                     width: "25px", //auto
//                     height: "25px",
//                     borderRadius: "50%",
//                 })
//             } else {
//                 console.log("not the top left");
//                 element.css({
//                     borderRadius: "10px 10px 10px 10px",
//                     height: "auto",
//                     transform: "translateX(-90%)",
//                     top: topOffset + "px",
//                     right: "0",
//                     width: "auto",
//                     visibility: "visible"
//                 })
//             }
//         }
//     }

//     window.onload = function() {
//         console.log("loaded");
//         onloadRight();
//         // getscript("LanguageMenu.js", function() {
//         //     testr();
//         // });
//         if (element.parentElement.style.visibility == "hidden") {
//             if (window.scrollY == 0) {
//                 console.log("top left");
//                 element.css({
//                     //visibility: "hidden",
//                     transform: "translateX(0%)",
//                     visibility: "visible",
//                     width: "25px",
//                     height: "25px",
//                     borderRadius: "50%",
//                 })
//             } else {
//                 console.log("not the top left");
//                 element.css({
//                     borderRadius: "10px 0 0 10px",
//                     height: "auto",
//                     transform: "translateX(-60%)",
//                     top: topOffset + "px",
//                     right: "0",
//                     width: "auto",
//                     visibility: "visible"
//                 })
//             }
//         }
//     }

//     const parent = element.parentElement.parentElement;
//     element.updateWidth = () => {
//         if (parent.anchors.wideAnchorsCollapsed) {
//             minimizedRight();
//         } else {
//             element.css({
//                 width: "auto", //auto
//             })
//             extend();
//         }
//     }

//     extend();
//     parent.onAnchorsChanged.push(element.updateWidth);
// }

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
                    })
                } else {
                    element.css({
                        width: "auto",
                    })
                    minimizedLeft();
                }
            }
        }
    }

    document.addEventListener("click", (evt) => {
        if (element.parentElement.style.visibility == "hidden") {
            const flyoutElement = document.getElementById("leftBar");
            const flyoutElementRight = document.getElementById("rightBar");
            const glassElement = document.getElementById("Glass");
            let targetElement = evt.target;
            console.log(targetElement);
            while (targetElement) {
                if (targetElement == flyoutElement) {
                    console.log("inside left");
                    extend();
                    disableScrolling();
                    return;
                } else if (targetElement == flyoutElementRight) {
                    console.log("inside right");
                    myExtend();
                    disableScrolling();
                    return;
                }
                targetElement = targetElement.parentNode;
                //console.log(targetElement);
            };
            console.log("outside left");
            enableScrolling();
            element.css({
                width: "auto",
            })
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
        element.css({
            borderRadius: "10px 10px 10px 10px",
            height: "auto",
            transform: "translateX(-90%)",
            backgroundColor: 'rgb(172, 196, 53)',
            top: topOffset + "px",
            right: "0",
            visibility: "visible",
            padding: '10px',
        });
        hideChildren();
    }

    function extend() {
        console.log("exetended");
        element.position = 'extend';
        element.css({
            padding: '10px',
            margin: '0px',
            borderRadius: '0px',
            backgroundColor: '#3b424d',
            top: '5px',
            height: "100vh",
            transform: 'unset',
        });
        if (element.parentElement.style.visibility == "hidden") {
            element.css({
                width: "auto",
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
                })
            } else {
                element.css({
                    width: "auto",
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
            })
            minimizedLeft();
        } else {
            extend();
        }
    }

    parent.onAnchorsChanged.push(element.updateWidth);
}