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

Object.setPrototypeOf(DomLinkedList.prototype, Enumerable.prototype);

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
    for (item of itemsArray) {
        item.Parent = this.firstChildProperty.object
    }

    d.end()
}


DomLinkedList.prototype.RemoveRange = function (startIndex, count) {
    if (count < 1 || startIndex < 0) {
        throw new Error(`Range to remove is out of collection range. "startIndex" : ${startIndex}, "count" : ${count}`)
    }

    //let currentItem = element[firstChildPropertyName]
    let currentItemProperty = this.firstChildProperty
    let currentIndex = 0

    //if (currentItem == undefined) {
    if (!currentItemProperty.getValue()) {
        throw new Error(`Range to remove is out of collection range. "startIndex" : ${startIndex}, "count" : ${count}`)
    }

    console.log("startIndex, count = ", startIndex, count)

    //let firstItemToRemove = currentItem
    let firstItemToRemoveProperty = currentItemProperty
    let itemsToRemove = [currentItemProperty.getValue()]
    while (currentIndex < startIndex + count - 1) {
        currentIndex++
        //currentItem = currentItem.NextSibling
        currentItemProperty = currentItemProperty.getValue().__NextSibling
        //if (currentItem == undefined) {
        if (!currentItemProperty.getValue()) {
            throw new Error(`Range to remove is out of collection range. "startIndex" : ${startIndex}, "count" : ${count}`)
        }

        if (currentIndex == startIndex) {
            //firstItemToRemove = currentItem
            firstItemToRemoveProperty = currentItemProperty
            itemsToRemove = [currentItemProperty.getValue()]
        }

        if (currentIndex >= startIndex) {
            //itemsToRemove.push(currentItem)
            itemsToRemove.push(currentItemProperty.getValue())
        }
    }

    let d = Reaction.beginDeferred()
    let lastItemToRemove = currentItemProperty.getValue()
    firstItemToRemoveProperty.setValue(lastItemToRemove.NextSibling)
    lastItemToRemove.NextSibling = undefined
    for (item of itemsToRemove) {
        item.Parent = undefined
    }

    d.end()

    return itemsToRemove
}


DomLinkedList.prototype.Insert = function (index, item) {
    this.InsertRange(index, [item])
}

DomLinkedList.prototype.RemoveAt = function (index) {
    var result = this.RemoveRange(index, 1)

    console.log("RemoveAt() index, result", index, result);

    return result[0]
}
