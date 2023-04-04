
/**
 * @constructor
 * @template T
 */
function Enumerable(source) {
    if (typeof source === "function") {
        this[Symbol.iterator] = source;
        return;
    }
    if (typeof source[Symbol.iterator] === "function") {
        this[Symbol.iterator] = source[Symbol.iterator].bind(source);
        return;
    }

    throw new Error("Invalid source type");
}

/**
 * @return {Array<T>}
 */
Enumerable.prototype.ToArray = function() {
    return [...this]
}

/**
 * @param {number} start 
 * @param {number} count 
 * @return {Enumerable<number>}
 */
Enumerable.Range = function(start, count) {
    return new Enumerable(function* () {
        for (let i = start; i < start + count; i++)
            yield i
    })
}

/**
 * @return {Enumerable<*>}
 */
Enumerable.FromArguments = function () {
    return new Enumerable(arguments)
}



/**
 * @param {function(T): boolean} func
 * @return {Enumerable<T>}
 */
Enumerable.prototype.Where = function (func) {
    let _this = this;
    let generator = function*() {
        for (let i of _this) {
            if (func(i)) yield i;
        }
    };
    return new Enumerable(generator);
};




/**
 * @template R
 * @param {function(T): R} func
 * @return {Enumerable<R>}
 */
Enumerable.prototype.Select = function (func) {
    let _this = this;
    let generator = function*() {
        for (let i of _this) {
            yield func(i);
        }
    };
    return new Enumerable(generator);
};



/**
 * @param {function(T): boolean} [func]
 * @param {function(): T} [notFound]
 * @return {T}
 */
Enumerable.prototype.First = function (func = () => true, notFound = () => {throw new Error("Out of range")}) {
    let _this = this;
    let enumerator = _this[Symbol.iterator]();

    while (true) {
        var item = enumerator.next();
        if (item.done) {
            return notFound();
        }
        if (func(item.value)) {
            return item.value;
        }        
    }
}

/**
 * @param {function(T): boolean} [func]
 * @param {function(): T} [notFound]
 * @return {T}
 */
Enumerable.prototype.Last = function (func = () => true, notFound = () => { throw new Error("Out of range") }) {
    let array = [...this]
    if (array.length > 0) {
        for (let i = array.length - 1; i >= 0; i--) {
            if (func(array[i])) {
                return array[i];
            }
        }
    }
}


/**
 * @param {function(T): boolean} [func]
 * @return {number}
 */
Enumerable.prototype.Count = function (func) {
    let array = [...this]
    if (func === undefined) {
        return array.length
    }
    
    let result = 0
    for (let i = 0; i<array.length; i++){
        if (func(array[i])) {
            result++
        } 
    }
    return result
}

/**
 * @param {function(T): boolean} [func]
 * @return {boolean}
 */
Enumerable.prototype.Any = function (func = () => true) {
    let _this = this;
    let enumerator = _this[Symbol.iterator]();

    if (func === undefined) {
        var item = enumerator.next();
        return !item.done
    }

    while (true) {
        var item = enumerator.next();
        if (item.done) {
            return false;
        }
        if (func(item.value)) {
            return true;
        }        
    }
}



/**
 * @param {function(R,T): R} func
 * @param {R} [initialValue]
 * @return {R}
 */
Enumerable.prototype.Aggregate = function(func, initialValue) {
    let array = [...this]
    let startIndex = 0
    if (arguments.length == 1){        
        if (array.length == 0) {
            return undefined
        }
        startIndex = 1
        initialValue = array[0]
    }
    
    for (let i = startIndex; i<array.length; i++){
        initialValue = func(initialValue,array[i])
    }

    return initialValue
}

/**
 * @return {T}
 */
Enumerable.prototype.Max = function() {
    return this.Aggregate((a, b) => a > b ? a : b)
}

/**
 * @return {T}
 */
Enumerable.prototype.Min = function() {
    return this.Aggregate((a, b) => a < b ? a : b)
}

/**
 * @return {T}
 */
Enumerable.prototype.Sum = function () {
    return this.Aggregate((a,b)=>a+b)
}


/**
 * @param {function(T,T): number} [compareFunc]
 * @return {Enumerable<T>}
 */
Enumerable.prototype.Order = function (compareFunc = (a, b) => a == b ? 0 : (a < b ? -1 : 1)) {
    let array = [...this]
    array.sort(compareFunc)
    return new Enumerable(array)
}
/**
 * @template R
 * @param {function(T): R} keySelector
 * @param {function(R,R): number} [compareFunc]
 * @return {Enumerable<T>}
 */
Enumerable.prototype.OrderBy = function(
    keySelector,
    compareFunc = function(a,b){        
        let ka = keySelector(a)
        let kb = keySelector(b)
        if (ka==kb) return 0
        return ka<kb?-1:1
    }
    )
{
    return this.Order(compareFunc)
}

/**
 * @return {Enumerable<T>}
 */
Enumerable.prototype.Reverse = function () {
    let array = [...this]
    let generator = function* () {
        for (let i = array.length - 1; i >= 0; i--) {
            yield array[i]
        }
    };
    return new Enumerable(generator);
}


/**
 * @param {number} count
 * @return {Enumerable<T>}
 */
Enumerable.prototype.Skip = function (count) {
    let _this = this;
    let generator = function*() {
        for (let i of _this) {
            if (count>0){
                count--
            }else{
                yield i
            }
        }
    };
    return new Enumerable(generator);
};

/**
 * @param {number} count
 * @return {Enumerable<T>}
 */
Enumerable.prototype.Take = function (count) {
    let _this = this;
    let generator = function*() {
        for (let i of _this) {
            if (count>0){
                yield i
                count--
            }else{
                break
            }
        }
    };
    return new Enumerable(generator);
};


/*
console.log(Enumerable.Range(0,9).OrderBy(x=>x%3).ToArray())
console.log(Enumerable.Range(0,10).Skip(3).Take(3).ToArray())

console.log(Enumerable.Range(0,10).Where(x=>x%2===0).Select(x=>x+"Hi").Last(x=>x.startsWith("4"),()=>"Not found"))
console.log(Enumerable.Range(0,10).Where(x=>x%2===0).Select(x=>x+"Hi").Aggregate((a,b)=>a+b,"___"))
console.log(Enumerable.Range(0,10).Where(x=>x%2===0).Select(x=>x+"Hi").Sum())

console.log(Enumerable.FromArguments(10,9,6,3,2,5,8,7,4,1,0).Order().Select(x=>x+"Hi").ToArray())
console.log(Enumerable.FromArguments("a","c","b").Order().Select(x=>x+"Hi").ToArray())*/