function Material(parameters) {

    //console.log(this.$.A)

    document.body.style.backgroundColor = "violet"


    PropertyTest()

    window.Reactive = {
        InnerWidth: window.innerWidth,
        InnerHeight: window.innerHeight,
    }

    window.onresize = function (event) {
        let d = Reaction.beginDeferred()
        window.InnerWidth = window.innerWidth
        window.InnerHeight = window.innerHeight
        d.end()
    }

    this.Reactive = {
        Content: undefined,
    }


    /*let previous = this.Content
    let onChanged = function (previous, current) {
        if (current) {
            this.Content.InnerWidth =
                () => Math.min(this.Content.Width, parameters.ContentWidth)
        }
    }
    new Reaction(() => {
        let current = this.Content
        if (current != previous) {
            onChanged(previous, current)
            previous = current
        }
    })*/

    this.Reactive.Content.OnChanged((previous, current) => {
        if (current) {
            this.Content.InnerWidth =
                () => Math.min(this.Content.Width, parameters.ContentWidth)
            this.Content.PaddingLeft =
                () => (this.Content.Width - parameters.ContentWidth) * 0.5
        }
    })



    new Reaction(() => {
        this.innerText = window.InnerWidth
        const LeftBarSize = 200
        const RightBarSize = 50

        if (this.Content) {
            console.log("content assigned")

            
                


            this.Content.style.left = LeftBarSize + "px"

            this.Content.MaxInnerWidth = parameters.ContentWidth
            this.Content.Width = window.InnerWidth - LeftBarSize - RightBarSize
        }
    })

    document.title = "!!";
    

    console.log(parameters.ContentWidth)

    element = this

    /*new Property().attach(element, "Content")

    new Property(window.innerWidth).attach(window, "InnerWidth")
    new Property(window.innerHeight).attach(window, "InnerHeight")


    const LeftBarSize = 200
    const RightBarSize = 50

    new Reaction(() => {
        if (element.Content) {

            element.Content.style.left = LeftBarSize+"px"

            element.Content.MaxInnerWidth = parameters.ContentWidth
            element.Content.Width = window.InnerWidth - LeftBarSize - RightBarSize
        }
    })


    document.addEventListener("DOMContentLoaded", function () {
        element.style.display = "";
    })

    */


    /*element.css({
        margin: "0",
    })
    let leftBarWidth = 0;
    let rightBarWidth = 0;

    //contentWidth = 800;

    element.onAnchorsChanged = []
    element.anchors = {}
    var leftBar = element.querySelector("#leftBar");
    var rightBar = document.getElementById("#rightBar");

    leftBarWidth = leftBar == null ? 0 : leftBar.width;
    rightBarWidth = rightBar == null ? 0 : rightBar.width;

    function updateHeaderWidth() {
        let left = element.anchors.textLeft;
        let right = element.anchors.textRight;
        header.style.marginLeft = left + "px";
        header.style.width = right - left + "px";
    }



    function updateAnchors() {
        

        var leftBar = element.querySelector("#leftBar");
        var rightBar = element.querySelector("#rightBar");
        leftBarWidth = leftBar == null ? 0 : leftBar.offsetWidth;
        rightBarWidth = rightBar == null ? 0 : rightBar.offsetWidth;
        const textMargin = 12;
        let width = window.innerWidth;
        let wideAnchorsCollapsed = width < (leftBarWidth + rightBarWidth + contentWidth);
        element.anchors.wideLeft = wideAnchorsCollapsed ? 0 : leftBarWidth;
        element.anchors.wideRight = wideAnchorsCollapsed ? width : width - rightBarWidth;
        element.anchors.wideAnchorsCollapsed = wideAnchorsCollapsed;
        let fillAnchorsCollapsed = width < contentWidth;
        let center = 0.5 * (element.anchors.wideLeft + element.anchors.wideRight);
        element.anchors.fillLeft = fillAnchorsCollapsed ? 0 : (center - 0.5 * contentWidth);
        element.anchors.fillRight = fillAnchorsCollapsed ? width : (center + 0.5 * contentWidth);

        element.anchors.textLeft = Math.max(element.anchors.fillLeft, element.anchors.wideLeft + textMargin);
        element.anchors.textRight = Math.min(element.anchors.fillRight, element.anchors.wideRight - textMargin);

        element.onAnchorsChanged.map(x => x());
        if (wideAnchorsCollapsed) {
            document.getElementById("rightBar").style.visibility = "hidden";
            document.getElementById("leftBar").style.visibility = "hidden";
        } else {
            document.getElementById("rightBar").style.visibility = "";
            document.getElementById("leftBar").style.visibility = "";
        }

    }

    DetectSwipe(element, swiper);

    function swiper(direction, swipe, touchEnd, event) {
        let getTranslate = (offsetWidth) => {
            var value = direction == 'left' || direction == 'right' ? swipe.swipeX : swipe.swipeY;
            let percent = toPercents(Math.abs(value), offsetWidth);
            let translate = percent > 0 ? percent : 0;
            return translate;
        }

        let showChildren = (element) => Array.from(element.children).forEach(x => {
            x.css({ visibility: "visible" });
        });

        if (direction == 'left') {
            let allSwiped = event.path.map(x => x.scrollLeft == null ? true : x.scrollLeft === x.scrollWidth - x.clientWidth).every(x => x == true);
            if (allSwiped) {
                let startY = swipe.clientStartY;
                let touchedElementSpace = menusHitBoxes.find(x => x.top <= startY && x.bottom >= startY);
                if (touchedElementSpace == null) { return; }
                let selectedMenu = touchedElementSpace.element
                if (selectedMenu.position == 'minimizedRight') {
                    showChildren(selectedMenu);
                    let translate = 100 - getTranslate(selectedMenu.offsetWidth);
                    selectedMenu.css({ transform: `translateX(clamp(0%, ${translate}%, 70%))` });
                }
            }
        } else {
            //menusHitBoxes.forEach(x => x.element.css({ transform: 'translateX(70%)' }));
        }
    }

    menusHitBoxes = [];


    window.addEventListener("resize", updateAnchors);
    document.addEventListener("DOMContentLoaded", function() {

        element.style.display = "";
        rightBarWidth = document.getElementById("rightBar").scrollWidth;
        leftBarWidth = document.getElementById("leftBar").scrollWidth;
        header = document.getElementById("Header");
        if (header != null) {
            element.onAnchorsChanged.push(updateHeaderWidth);
        }
        updateAnchors()
        var rightBar = document.getElementById('rightBar');
        var children = Array.from(rightBar.children)
        children.forEach(x => {
            var rect = x.getBoundingClientRect();
            let element = {
                element: x,
                top: rect.top,
                bottom: rect.bottom
            }
            menusHitBoxes.push(element);
        });
    });*/

}