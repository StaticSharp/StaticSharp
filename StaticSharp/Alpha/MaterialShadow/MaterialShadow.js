
function MaterialShadow(element) {
    AbstractBoxShadow.call(this, element)
    this.isMaterialShadow = true
    let modifier = this

    modifier.Reactive = {
        Elevation: 10
    }


    function ambientShadow(e) {
        return `0px ${0.25 * e}px ${2 * e}px ${-0.5 * e}px rgba(0, 0, 0, 0.5)`
    }
    function hardShadow(e) {
        let spread = 0.15
        let x = 0.4
        let y = 0.8
        return `${x * e}px ${y * e}px ${2 * spread * e}px ${-spread * e}px rgba(0, 0, 0, 0.2)`
    }

    modifier.getBoxShadow = function () {
        let e = modifier.Elevation
        return ambientShadow(e) + "," + hardShadow(e)
    }



}