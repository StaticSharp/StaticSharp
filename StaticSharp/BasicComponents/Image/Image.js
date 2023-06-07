StaticSharpClass("StaticSharp.Image", (element) => {
    StaticSharp.AspectBlock(element)

    let baseHtmlNodesOrdered = element.HtmlNodesOrdered
    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield element.content
        yield* baseHtmlNodesOrdered        
    })


    new Reaction(() => {
        FitImage(
            element,
            element.content, element.NativeAspect,
            element.Fit, element.GravityVertical, element.GravityHorizontal
        )
    })


    new Reaction(() => {
        let img = element.querySelector("img")
        
        var svgBackground =
            `<svg xmlns="http://www.w3.org/2000/svg" viewBox ="0 0 ${element.thumbnailData.width} ${element.thumbnailData.height}" preserveAspectRatio="none">
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
    <image href="${element.thumbnailData.src}" filter="url(#filter)"/>
    <circle  cx="50%" cy="50%" r="10%" fill="red" />
</svg>`

        const svgEncoded = encodeURIComponent(svgBackground)
        img.style.backgroundImage = `url('data:image/svg+xml;utf8,${svgEncoded}')`;
        img.style.backgroundSize = '100% 100%';
        img.src = ""

        console.log(img)
    })


    /*new Reaction(() => {
        let content = element.children[0]
        let thumbnail = content.querySelector("#thumbnail")
        if (thumbnail) {
            thumbnail.style.display = "block"
        }
    })*/

    

    /*element.AfterChildren = function () {
        let thumbnail = element.content.querySelector("#thumbnail")
        if (thumbnail) {
            thumbnail.style.display = "block"
        }
    }*/

    
})
