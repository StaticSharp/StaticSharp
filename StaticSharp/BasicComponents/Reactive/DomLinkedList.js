/**
 * @constructor - Inherits Enumerable, represents a linked list of DOM elements. Has modification methods
 * @param {Property} firstChildProperty - Name of collection property
 */
function DomLinkedList(firstChildProperty) {

    this.firstChildProperty = firstChildProperty

    //// function* copied from Children definition in Hierarchical.js
    Enumerable.call(this,
        function* () {
            var i = firstChildProperty.getValue()
            while (i != undefined) {
                yield i
                i = i.NextSibling
            }
        })
}

Object.setPrototypeOf(DomLinkedList.prototype, Enumerable.prototype)

DomLinkedList.prototype.InsertRange = function (startIndex, itemsArray) {
    if (startIndex < 0) {
        throw new Error(`"startIndex" out of range : ${startIndex}`)
    }

    let currentItemProperty = this.firstChildProperty
    let currentIndex = 0

    while (currentIndex < startIndex) {
        if (!currentItemProperty.getValue()) {
            throw new Error(`Range to remove is out of collection range. "startIndex" : ${startIndex}, "count" : ${count}`)
        }

        currentIndex++
        currentItemProperty = currentItemProperty.getValue().__NextSibling
    }

    var nextItem = currentItemProperty.getValue()


    let d = Reaction.beginDeferred()
    currentItemProperty.setValue(itemsArray[0])
    itemsArray[itemsArray.length - 1].NextSibling = nextItem
    for (let [i, item] of itemsArray.entries()) {
        if (i < itemsArray.length - 1) {
            item.NextSibling = itemsArray[i + 1]
        }
    }

    d.end()
}


DomLinkedList.prototype.RemoveRange = function (startIndex, count) {
    if (count < 1 || startIndex < 0) {
        throw new Error(`Range to remove is out of collection range. "startIndex" : ${startIndex}, "count" : ${count}`)
    }

    let currentItemProperty = this.firstChildProperty

    let firstItemToRemoveProperty = currentItemProperty
    let lastItemToRemove = undefined
    let itemsToRemove = []
    for (let i = 0; i < startIndex + count; i++) {
        if (!currentItemProperty.getValue()) {
            throw new Error(`Range to remove is out of collection range. "startIndex" : ${startIndex}, "count" : ${count}`)
        }

        if (i == startIndex) {
            firstItemToRemoveProperty = currentItemProperty
        }

        if (i >= startIndex) {
            itemsToRemove.push(currentItemProperty.getValue())
        }

        if (i == startIndex + count - 1) {
            lastItemToRemove = currentItemProperty.getValue()
        }

        currentItemProperty = currentItemProperty.getValue().__NextSibling
    }

    let d = Reaction.beginDeferred()
    firstItemToRemoveProperty.setValue(lastItemToRemove.NextSibling)
    lastItemToRemove.NextSibling = undefined

    d.end()
    return itemsToRemove
}


DomLinkedList.prototype.Insert = function (index, item) {
    this.InsertRange(index, [item])
}

DomLinkedList.prototype.RemoveAt = function (index) {
    var result = this.RemoveRange(index, 1)
    return result[0]
}
