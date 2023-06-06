
var Num = {}

Num.ValidNumber = function (value) {
    if (typeof (value) !== "number") return false
    return !isNaN(value)
}

Num.IsNaNOrNull = function (value) {
    if (value === null) return true
    return isNaN(value)
}

Num.Max = function () {
    let result = undefined
    for (let i of arguments) {
        if (Num.IsNaNOrNull(i)) continue;
        if (result === undefined) {
            result = i
        } else {
            result = Math.max(result, i)
        }
    }
    return result
}

Num.Min = function () {
    let result = undefined
    for (let i of arguments) {
        if (Num.IsNaNOrNull(i)) continue;
        if (result === undefined) {
            result = i
        } else {
            result = Math.min(result, i)
        }
    }
    return result
}

Num.Clamp = function (value, min, max) {
    return Num.Max(min, Num.Min(max, value))
}

Num.Sum = function () {
    let resultValid = false
    let result = 0
    for (let i of arguments) {
        if (Num.IsNaNOrNull(i)) continue;
        resultValid = true
        result += i
    }
    return resultValid ? result : undefined
}


Num.First = function () {
    for (let i of arguments) {
        if (i === undefined) continue;
        if (isNaN(i)) continue;
        if (i === null) continue;
        return i
    }
    return undefined
}

Num.PiecewiseLinearInterpolation = function (x, ...keyframes) {

    if (x == undefined)
        return undefined

    if (keyframes.length === 0) {
        throw new Error("Keyframes list must not be empty.");
    }

    if (keyframes.length === 1) {
        return keyframes[0][1];
    }

    // Sort keyframes by x.
    keyframes.sort((a, b) => a[0] - b[0]);
    
    if (x < keyframes[0][0]) {
        return keyframes[0][1];
    }

    if (x >= keyframes[keyframes.length - 1][0]) {
        return keyframes[keyframes.length - 1][1];
    }

    for (let i = 1; i < keyframes.length; i++) {
        if (x < keyframes[i][0]) {
            const deltaX = keyframes[i][0] - keyframes[i - 1][0];
            const alpha = (x - keyframes[i - 1][0]) / deltaX;
            const deltaY = keyframes[i][1] - keyframes[i - 1][1];
            return keyframes[i - 1][1] + deltaY * alpha;
        }
    }

    // This should never happen if the keyframes are sorted correctly.
    throw new Error("Keyframes are not sorted correctly.");
}