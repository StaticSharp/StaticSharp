if (!window.LayoutAlgorithms) window.LayoutAlgorithms = {}
if (!window.LayoutAlgorithms.Grid) window.LayoutAlgorithms.Grid = {}
if (!window.LayoutAlgorithms.Grid.Stack) window.LayoutAlgorithms.Grid.Stack = {}

LayoutAlgorithms.Grid.Stack.Measure = function (e, vertical, children, gravity, gap) {

    return window.LayoutAlgorithms.Grid.Stack.Common(false, e, vertical, children, gravity, gap)
}

LayoutAlgorithms.Grid.Stack.Layout = function (e, vertical, children, gravity, gap) {
    window.LayoutAlgorithms.Grid.Stack.Common(true, e, vertical, children, gravity, gap)
}

LayoutAlgorithms.Grid.Stack.Common = function (layout, e, vertical, children, gravity, gap) {

    let numRows = children.length
    let numColumns = children[0].length
    var names = new LayoutPropertiesNames(vertical)

    let regions = Array.from({ length: numColumns }, () => LinearLayoutRegion.formContainer(e, vertical));

    let stretch = gravity == undefined
    let baselineLocation = stretch ? 0 : (0.5 * gravity + 0.5)

    for (let y = 0; y < numRows; y++) {
        let row = children[y]
        let positions = Array(row.length)
        let sizes = row.map(x => x.Layer[names.dimension] || 0)

        let maxBaselinePosition = 0

        let size = undefined
        if (stretch) {
            size = Math.max(...sizes)
            if (layout)
                row.forEach(x => x.Layer[names.dimension] = size)
        }

        for (let x = 0; x < row.length; x++) {
            positions[x] = regions[x].border[0].Shift(row[x], size)
            let baselinePosition = positions[x] + sizes[x] * baselineLocation
            //console.log("result", sizes[x])
            maxBaselinePosition = Math.max(maxBaselinePosition, baselinePosition)
        }

        for (let x = 0; x < row.length; x++) {
            let position = maxBaselinePosition - (sizes[x] * baselineLocation)
            let offset = position - positions[x]
            regions[x].border[0].ShiftByPixels(offset)
            if (layout)
                row[x].Layer[names.coordinate] = position
            
        }

        if (y == (numRows - 1)) { //last row
            if (!layout) {
                
                let maxBodyStop = Math.max(...regions.map(x => x.border[0].bodyStop))
                regions[0].bodyStop = maxBodyStop
                let result = regions[0].GetSize()
                
                return result
            }
        } else {
            let maxBodyStop = Math.max(...regions.map(x => Math.max(x.border[0].bodyStop, gap + x.border[0].marginStop)))
            regions.map(x => x.border[0].bodyStop = maxBodyStop)
        }       

    }

}
