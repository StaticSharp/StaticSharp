
var Num = {}


function IsNaNOrNull(value) {
    if (value === null) return true
    return isNaN(value)
}

Num.Max = function () {
    let result = undefined
    for (let i of arguments) {
        if (IsNaNOrNull(i)) continue;
        if (result === undefined) {
            result = i
        } else {
            result = Math.max(result, i)
        }
    }
    return result
}

function Min() {
    let result = undefined
    for (let i of arguments) {
        if (IsNaNOrNull(i)) continue;
        if (result === undefined) {
            result = i
        } else {
            result = Math.min(result, i)
        }
    }
    return result
}

function Clamp(value, min, max) {
    return Math.max(min, Math.min(max, value))
}

function Sum() {
    let resultValid = false
    let result = 0
    for (let i of arguments) {
        if (IsNaNOrNull(i)) continue;
        resultValid = true
        result += i
    }
    return resultValid ? result : undefined
}


function First() {
    for (let i of arguments) {
        if (i === undefined) continue;
        if (isNaN(i)) continue;
        if (i === null) continue;
        return i
    }
    return undefined
}

