/**
 * @constructor
 * @param {boolean} vertical
*/
function LayoutPropertiesNames(vertical) {

    /**@type {Array<string>}*/
    this.side = [
        vertical ? "Top" : "Left",
        vertical ? "Bottom" : "Right"
    ]
    /**@type {"Y" | "X"}*/
    this.cordinate = vertical ? "Y" : "X"

    /**@type {"Height" | "Width"}*/
    this.dimension = vertical ? "Height" : "Width"
}



function LayoutBorder(sideIndex, layoutPrpertiesNames, marginStop = 0, bodyStop = 0) {
    this.layoutPrpertiesNames = layoutPrpertiesNames
    this.sideIndex = sideIndex
    this.oppositeSideIndex = (sideIndex + 1) % 2
    //this.sideOpposite = layoutPrpertiesNames.side[(sideIndex+1)%2]
    this.marginStop = marginStop
    this.bodyStop = bodyStop
}

LayoutBorder.prototype.ShiftMaxOf = function (children) {

    let marginStop = 0
    let bodyStop = 0

    for (let child of children) {
        let margin = child["Margin" + this.layoutPrpertiesNames.side[this.sideIndex]] || 0
        let marginOpposite = child["Margin" + this.layoutPrpertiesNames.side[this.oppositeSideIndex]] || 0
        let size = child.Layer[this.layoutPrpertiesNames.dimension] || 0

        let position = Math.max(this.bodyStop, this.marginStop + margin)

        marginStop = Math.max(marginStop, position + size)
        bodyStop = Math.max(bodyStop, this.marginStop + marginOpposite)
    }

    this.marginStop = marginStop
    this.bodyStop = bodyStop
}

LayoutBorder.prototype.Shift = function (child, size = undefined) {


    let margin = child["Margin" + this.layoutPrpertiesNames.side[this.sideIndex]] || 0
    let marginOpposite = child["Margin" + this.layoutPrpertiesNames.side[this.oppositeSideIndex]] || 0
    if (size == undefined)
        size = child.Layer[this.layoutPrpertiesNames.dimension] || 0

    let position = Math.max(this.bodyStop, this.marginStop + margin)

    this.marginStop = position + size
    this.bodyStop = this.marginStop + marginOpposite

    return position
}

LayoutBorder.prototype.ShiftByPixels = function (pixels) {
    this.marginStop += pixels
    this.bodyStop += pixels
}




function LinearLayoutRegion(vertical) {
    let names = new LayoutPropertiesNames(vertical)
    this.vertical = vertical
    this.border = [
        new LayoutBorder(0, names),
        new LayoutBorder(1, names),
    ]
}

LinearLayoutRegion.prototype.GetSize = function () {
    return Math.max(
        this.border[0].marginStop + this.border[1].bodyStop,
        this.border[1].marginStop + this.border[0].bodyStop
    )
}


LinearLayoutRegion.formContainer = function (container, vertical) {
    let result = new LinearLayoutRegion(vertical)

    let names = new LayoutPropertiesNames(vertical)

    /**@type {Array<number>}*/
    let padding = [
        container["Padding" + names.side[0]],
        container["Padding" + names.side[1]],
    ]

    for (let i = 0; i < 2; i++) {

        result.border[i].bodyStop = 0
        result.border[i].marginStop = 0
        if (padding[i] != undefined) {
            result.border[i].bodyStop = padding[i]
        } else {
            let margin = container["Margin" + names.side[i]]
            if (margin !== undefined)
                result.border[i].marginStop = -margin
        }
    }
    return result
}