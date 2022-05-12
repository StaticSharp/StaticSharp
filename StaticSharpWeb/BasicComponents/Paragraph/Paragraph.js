
function ParagraphInitialization(element) {

    BlockInitialization(element)

    element.Reactive = {
        ContentHeight: undefined,
        Height: () => element.ContentHeight,

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

    element.style.width = 0
    element.criticalWidth = element.scrollWidth
    element.style.width = element.criticalWidth+"px"
    element.criticalHeight = element.offsetHeight

    element.ContentHeight = () => {
        if (element.Width < element.criticalWidth) {
            element.style.transformOrigin = "top left"
            let scale = element.Width / element.criticalWidth
            element.style.transform = `scale(${scale}, ${scale})`
            element.style.width = element.criticalWidth + "px"
            return element.criticalHeight * scale
        } else {
            element.style.width = element.Width + "px"
            element.style.transformOrigin = ""
            element.style.transform = ""
            return element.offsetHeight
        }
    };


}