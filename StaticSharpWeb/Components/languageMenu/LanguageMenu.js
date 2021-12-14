function LanguageMenu(element) {
    element.width = 30;
    let topOffset = 5;
    element.position = '';

    const marker = document.getElementById("RightMarker");
    const glass = document.getElementById("Glass");
    const icon = document.getElementById("RightIcon");

    function extended() {
        setMarkerHidden();
        element.position = 'extended';
        element.css({
            paddingTop: '10px',
            margin: '0px',
            borderRadius: '0px',
            transform: 'unset',
            translate: "0px",
            zIndex: "20"
        });
    }

    function hidden() {
        setGlassHidden();
        element.position = 'hidden';
        element.css({
            borderRadius: "10px 10px 10px 10px",
            transform: "translateX(50px)",
            visibility: "visible"
        });
        setMarkerVisible();
    }

    function setGlassVisible() {
        glass.css({
            visibility: "visible",
            opacity: "0.7",
            zIndex: "11",
        })
    }

    function setGlassHidden() {
        glass.css({
            visibility: "hidden"
        })
    }

    function setMarkerVisible() {
        marker.css({
            visibility: "visible"
        });
    }

    function setMarkerHidden() {
        marker.css({
            visibility: "hidden"
        });
    }

    function circleMarker() {
        marker.css({
            width: "40px",
            height: "40px",
            borderRadius: "50%",
            left: "-50px",
        });
        icon.css({
            content: "url(https://api.iconify.design/ic/baseline-translate.svg?color=white&width=24&height=24)",
        });
    }

    function standartMarker() {
        marker.css({
            width: "",
            height: "",
            borderRadius: "",
            left: "",
            content: "",
        });
        icon.css({
            content: "",
        });
    }

    marker.addEventListener("click", (evt) => {
        setGlassVisible();
        disableScrolling();
        extended();
        if (window.scrollY != 0)
            setMarkerVisible();
    })

    glass.addEventListener("click", (evn) => {
        enableScrolling();
        hidden();
    })

    function disableScrolling() {
        // TopScroll = window.pageYOffset || document.documentElement.scrollTop;
        // LeftScroll = window.pageXOffset || document.documentElement.scrollLeft,
        //     window.addEventListener("scroll", (evt) => {
        //         window.scrollTo(LeftScroll, TopScroll);
        //     })
        document.documentElement.style.height = "100vh";
        document.documentElement.style.overflow = "hidden";
        marker.css({
            transition: "",
        });
    };

    function enableScrolling() {
        document.documentElement.style.height = "";
        document.documentElement.style.overflow = "";
        window.addEventListener("scroll", (evt) => {
            if (window.scrollY == 0)
                circleMarker();
            else standartMarker();
        });
        window.addEventListener("load", (evt) => {
            if (window.scrollY == 0)
                circleMarker();
            else standartMarker();
        })
        marker.css({
            transition: "all 0.05s, opacity 0s",
        });
    }

    const parent = element.parentElement.parentElement;
    element.updateWidth = () => {
        if (parent.anchors.wideAnchorsCollapsed) {
            enableScrolling();
            hidden();
        } else {
            extended();
        }
    }
    parent.onAnchorsChanged.push(element.updateWidth);
}