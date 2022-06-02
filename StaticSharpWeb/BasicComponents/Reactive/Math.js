function Max(a,b) {
    if (a == undefined) return b
    if (b == undefined) return a
    return Math.max(a,b)
}

function Min(a, b) {
    if (a == undefined) return b
    if (b == undefined) return a
    return Math.min(a, b)
}

function Sum() {
    let resultValid = false
    let result = 0
    for (let i of arguments) {
        if (isNaN(i)) continue;
        resultValid = true
        result += i
    }
    return resultValid ? result : undefined
}