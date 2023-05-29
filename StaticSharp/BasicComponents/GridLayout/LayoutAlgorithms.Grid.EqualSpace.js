if (!window.LayoutAlgorithms) window.LayoutAlgorithms = {}
if (!window.LayoutAlgorithms.Grid) window.LayoutAlgorithms.Grid = {}
if (!window.LayoutAlgorithms.Grid.EqualSpace) window.LayoutAlgorithms.Grid.EqualSpace = {}

window.LayoutAlgorithms.Grid.EqualSpace.Measure = function (e, vertical, children, gap, cellSize) {
    let names = new LayoutPropertiesNames(vertical)

    let numRows = children.length

    let paddingStart = (e["Padding" + names.side[0]] || 0)
    let paddingEnd = (e["Padding" + names.side[1]] || 0)

    return paddingStart + numRows * cellSize + (numRows - 1) * gap + paddingEnd
}


window.LayoutAlgorithms.Grid.EqualSpace.Layout = function (e, vertical, children, gravity, gap) {
    let names = new LayoutPropertiesNames(vertical)

    let containerSize = e[names.dimension]
    if (Num.IsNaNOrNull(containerSize))
        return

    let numRows = children.length

    let paddingStart = (e["Padding" + names.side[0]] || 0)
    let paddingEnd = (e["Padding" + names.side[1]] || 0)

    let innerSize = e[names.dimension] - paddingStart - paddingEnd

    let rowSize = (innerSize - (numRows - 1) * gap) / numRows

    for (let y = 0; y < numRows; y++) {

        let row = children[y]

        for (let x = 0; x < row.length; x++) {
            let item = row[x]

            let startA = 0
            let startB = 0

            let endA = rowSize
            let endB = rowSize
            
            let size = item.Layer[names.dimension] || 0
            
            if (gravity != undefined) {

                let bias = 0.5 * gravity + 0.5
                let extraPixels = (rowSize - size)
                console.log("extraPixels", extraPixels)
                if (extraPixels > 0) {
                    startA = extraPixels * bias
                    endA -= extraPixels * (1 - bias)
                }
            }

            if (y == 0) {
                let offset = CalcOffset(e, item, names.side[0]) - paddingStart
                startB = offset
            } else {
                let offset = Math.max(item["Margin" + names.side[0]] || 0, gap) - gap
                startB = offset
            }

            if (y == (numRows - 1)) {
                endB -= CalcOffset(e, item, names.side[1]) - paddingEnd
            } else {

                let margin = item["Margin" + names.side[1]] || 0
                endB -= Math.max(gap, margin) - gap

            }

            endA = Math.max(endA, startB + size)
            startA = Math.min(startA, endB - size)

            let start = Math.max(startA, startB)
            let end = Math.min(endA, endB)

            start += y * (gap + rowSize) + paddingStart
            end += y * (gap + rowSize) + paddingStart

            let itemSize = end - start
            item.Layer[names.coordinate] = start
            item.Layer[names.dimension] = itemSize
        }
    }
}
