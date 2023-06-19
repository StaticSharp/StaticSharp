
StaticSharp.SetThumbnailBackground = function (img, preloadLinkId, width, height) {

    let src = document.getElementById(preloadLinkId).href

    var svgBackground =
`<svg xmlns="http://www.w3.org/2000/svg" width="${width}" height="${height}" viewBox ="0 0 ${width} ${height}" preserveAspectRatio="none">
    <filter id="filter">
        <feGaussianBlur stdDeviation="1 0" in="SourceGraphic" result="hBlur"></feGaussianBlur>
        <feMerge result="hBlurAndImage">
            <feMergeNode in="SourceGraphic"></feMergeNode>
            <feMergeNode in="hBlur"></feMergeNode>
        </feMerge>
        <feGaussianBlur stdDeviation="0 1" in="hBlurAndImage" result="vBlur"></feGaussianBlur>
        <feMerge>
            <feMergeNode in="SourceGraphic"></feMergeNode>
                <feMergeNode in="hBlur"></feMergeNode>
                <feMergeNode in="vBlur"></feMergeNode>
            </feMerge>
        </filter>
    <image href="${src}" filter="url(#filter)"/>
</svg>` 
    const svgEncoded = encodeURIComponent(svgBackground)
    //<circle  cx="50%" cy="50%" r="10%" fill="red" />

    img.style.backgroundImage = `url('data:image/svg+xml;utf8,${svgEncoded}')`
    img.style.backgroundSize = '100% 100%'

    function loaded() {
        img.style.backgroundImage = ""
        img.style.backgroundSize = ""
    }

    if (img.complete) {
        loaded()
    } else {
        img.addEventListener('load', loaded)
        /*img.addEventListener('error', function () {
            alert('error')
        })*/
    }
}


StaticSharpClass("StaticSharp.Image", (element) => {
    StaticSharp.AspectBlock(element)

    let baseHtmlNodesOrdered = element.HtmlNodesOrdered
    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield element.img
        yield* baseHtmlNodesOrdered        
    })


    new Reaction(() => {
        FitImage(
            element,
            element.img, element.NativeAspect,
            element.Fit, element.GravityVertical, element.GravityHorizontal
        )
    })


    
})
