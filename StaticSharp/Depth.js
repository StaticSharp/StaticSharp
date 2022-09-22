
var Depth = function(){

    let depth = {}
    depth.min = -2147483648

    let nestingBits = 7
    let overlayBits = 32 - nestingBits

    depth.maxNesting = 2 ** nestingBits
    depth.nestingIncrement = 2 ** overlayBits

    console.log(depth.nestingIncrement)
    
    depth.calcDepth = function(element){


    }

    return depth
}()