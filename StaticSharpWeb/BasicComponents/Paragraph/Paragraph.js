
function ParagraphInitialization(element) {

    BlockInitialization(element)

    let content = element.children[0]

    element.Reactive = {
        Width: () => Min(element.LayoutWidth, element.MaxContentWidth),

        Height: undefined,

        MaxContentWidth: undefined,
        MinContentWidth: undefined,

        MaxContentHeight: undefined,
        MinContentHeight: undefined,


    }

    element.Margin.Left = 10
    element.Margin.Right = 10

    element.Margin.Top = 8
    element.Margin.Bottom = 8

}

function ParagraphBefore(element) {
    BlockBefore(element)
}

function ParagraphAfter(element) {

    BlockAfter(element)



    let paragraphDeferred = function () {

        const testFontSize = 100;

        let content = element.children[0]
        content.style.fontSize = testFontSize + "px";
        content.style.width = "min-content"
        var minWidthRect = content.getBoundingClientRect()
        content.style.width = "max-content"
        var maxWidthRect = content.getBoundingClientRect()

        content.style.fontSize = ""
        content.style.width = ""

        element.MaxContentWidth = () => element.Modifier.HierarchyFontSize / testFontSize * maxWidthRect.width
        element.MinContentWidth = () => element.Modifier.HierarchyFontSize / testFontSize * minWidthRect.width
        element.MaxContentHeight = () => element.Modifier.HierarchyFontSize / testFontSize * minWidthRect.height
        element.MinContentHeight = () => element.Modifier.HierarchyFontSize / testFontSize * maxWidthRect.height

        //element.title = maxWidthRect.width + " - " + minWidthRect.width


        new Reaction(() => {
            element.style.width = element.Width + "px"
            if (element.MinContentWidth > element.Width) {
                content.style.width = "min-content"
                content.style.transformOrigin = "top left"

                let scale = element.Width / element.MinContentWidth
                content.style.transform = `scale(${scale}, ${scale})`
                element.Height = element.MaxContentHeight * scale
                return
            }
            if (element.Width == element.MaxContentWidth) {
                element.Height = element.MinContentHeight
                content.style.width = "max-content"
                return
            }

            content.style.transformOrigin = ""
            content.style.transform = ""
            content.style.width = ""
            var rect = content.getBoundingClientRect()

            element.Height = rect.height


        })

        HeightToStyle(element)
    }



    document.fonts.ready
        .then(() => {
            paragraphDeferred()
        })

    /*element.onclick = function () {
    };*/


}