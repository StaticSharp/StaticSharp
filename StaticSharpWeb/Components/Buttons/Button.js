
/*
var CustomButton = function () {
    console.log("created")
    return Reflect.construct(HTMLElement, [], CustomButton)
}

CustomButton.prototype = Object.create(HTMLElement.prototype)

CustomButton.prototype.connectedCallback = function () {
    console.log("connected")
    this.style.display = "flex"
    this.style.flexDirection = "column"
    this.style.justifyContent = "center"

    this.style.backgroundColor = "darkviolet";
    this.style.padding = "16px";
    this.style.textAlign = "center";
}

customElements.define('custom-button', CustomButton);*/


/*customElements.define("object-v1", CEo)



class CustomButton extends HTMLElement {
    constructor() {
        super()
        
    }

    connectedCallback() { // (2)
        this.style.display = "flex"
        this.style.flexDirection = "column"
        this.style.justifyContent = "center"

        this.style.backgroundColor = "darkviolet";
        this.style.padding = "16px";
        this.style.textAlign = "center";


    }

}*/




function Button() {

    let element = this
    //element.innerText = navigator.userAgent
    element.innerText = 321//navigator.userAgent

    /*let root = {}

    let A = new Property(0)
        .attach(root, "A")

    let B = new Property(0)
        .attach(root, "B")

    let C = new Property(0)
        .attach(root, "C")

    let D = new Property(0)
        .attach(root, "D")

    let rABC = new Reaction(() => {
        console.log("reaction abc", root.A, root.B, root.C)
        //console.log("a+b+c = ", root.C + root.A + root.B)
    })

    


    let rBC = new Reaction(() => {
        console.log("reaction bc->d")
        root.D = root.B + root.C
    })


    let rB = new Reaction(() => {
        console.log("reaction b->c")
        root.C = root.B
    })

    let rA = new Reaction(() => {
        console.log("reaction a->b")
        root.B = root.A
    })*/


    /*window.addEventListener('scroll', function (e) {
        console.log(window.scrollY)
        element.style.position = "absolute"
        element.style.top = window.scrollY+"px"
    });*/

    

    /*let rAB = new Reaction(() => {
        console.log("reaction ab")
        root.C = root.A + root.B + 1
    })*/

    


    /*let rC = new Reaction(() => {
        console.log("reaction c")
        console.log("c = ",root.C)
    })
    console.log(rC)*/


/*
    console.group("root.B = 8")
    root.B = 8
    console.groupEnd()

    console.group("root.B = 8")
    root.B = 9
    console.groupEnd()

    console.group("root.B = 8")
    root.B = 10
    console.groupEnd()

    window.onresize = function (event) {
        let d = Reaction.beginDeferred()
        window.InnerWidth = window.innerWidth
        window.InnerHeight = window.innerHeight
        d.end()
        element.innerText = window.innerHeight

    }

    let width = 400


    element.innerText = "0000000000000000011111111111111111111122222222222222222222222333333333333333"
    element.style.wordBreak = "break-word"

    element.onclick = () => {
        //var t0 = performance.now()
        //console.log("Call to doSomething took " + (performance.now() - t0) + " milliseconds.");

        
        element.style.width = width + "px";

        width -= 10

        console.log(element.offsetHeight)

    }*/
    



    /*new Reaction(() => {
        element.style.backgroundColor = `hsl(${window.InnerWidth},50%,60%)`
        element.innerText = window.InnerWidth
    })*/





    /*


    

    new Property(8).attach(root, "a")

    new Property(() => {
        console.log("calc b")
        return root.a + 2
    }).attach(root, "b")


    let property = new Property(8)
    console.log(root.b)*/

    


    let parent = element.parentElement;

    element.billboard = true;

    let previousIsBillboard = element.previousElementSibling?.billboard;
    if (!previousIsBillboard) {
        element.style.marginTop = "16px"
    }





    


    //element.onAnchorsChanged = []
    //element.anchors = {}

    element.style.display = "flex"
    element.style.flexDirection = "column"
    element.style.justifyContent = "center"


    //element.updateWidth = function () {
    //    let left = parent.anchors.fillLeft;
    //    let right = parent.anchors.fillRight;
    //    element.style.left = left + "px";
    //    //element.style.width = right - left + "px";
    //
    //
    //    //element.style.backgroundColor = "red";
    //
    //    /*let contentSpace = parent.anchors.textRight - parent.anchors.textLeft;
    //    let contentWidth = Math.min(MaxContentWidth, contentSpace);
    //    let horizontalMargin = 0.5 * (contentSpace - contentWidth);
    //
    //    element.anchors.textLeft = parent.anchors.textLeft - left + horizontalMargin;
    //    element.anchors.textRight = element.anchors.textLeft + contentWidth;
    //
    //    element.onAnchorsChanged.map(x => x());*/
    //}
    //parent.onAnchorsChanged.push(element.updateWidth);

}