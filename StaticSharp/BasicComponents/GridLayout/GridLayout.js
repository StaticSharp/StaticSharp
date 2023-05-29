









var GridLayoutOrder = {
    LeftToRight_TopToBottom: "LeftToRight_TopToBottom",
}

var GridLayoutType = {
    Stack: "Stack",
    EqualSpace: "EqualSpace"
}


function GridLayout(element) {
    BlockWithChildren(element)
    element.isGridLayout = true

    function transpose(rows) {

        //let numChildren = grid.length
        if (rows.length == 0)
            return []

        if (rows.length == 1 && rows[0].length == 1)
            return rows

        let numColumns = rows[0].length
        let numRows = rows.length

        let result = Array(numColumns)
        let lastRow = rows[rows.length - 1]

        for (let x = 0; x < numColumns; x++) {
            let fillColumn = lastRow.length >= (x+1)
            let numItemsInColumn = fillColumn ? numRows : (numRows - 1) 
            result[x] = Array(numItemsInColumn)

            for (let y = 0; y < numItemsInColumn; y++) {
                result[x][y] = rows[y][x];
            }
        }
        return result
    }




    element.Reactive = {


        NumColumns: 2,
        NumExistingChildren: e => e.ExistingChildren.Count(),

        CalculatedNumRows: e => Math.ceil(e.NumExistingChildren / e.NumColumns),

        NumRows: e => e.CalculatedNumRows,

        LayoutTypeX: GridLayoutType.EqualSpace,
        LayoutTypeY: GridLayoutType.Stack,

        GapX: 0,
        GapY: 0,

        GravityX: undefined,
        GravityY: undefined,
       

        Rows: e => {
            
            let children = e.ExistingChildren.ToArray()
            let numChildren = children.length
            let result = []
            if (numChildren == 0)
                return result

            let NumColumns = e.NumColumns
            let numRows = e.NumRows

            for (let y = 0; y < numRows; y++) {
                var row = children.splice(0, NumColumns)
                result.push(row)
            }

            return result
        },

        RowsTransposed: e => transpose(e.Rows),

        CellWidth : 100,
        CellHeight : 100,

        InternalWidth: e => {
            if (element.LayoutTypeX == GridLayoutType.EqualSpace) {
                return LayoutAlgorithms.Grid.EqualSpace.Measure(element, false, element.RowsTransposed, element.GapX, element.CellWidth)
            } else {
                return LayoutAlgorithms.Grid.Stack.Measure(element, false, element.RowsTransposed, element.GravityX, element.GapX)
            }
        },

            //e.Vertical ? e.InternalSecondarySize : e.InternalPrimarySize,
        InternalHeight: e => {

            
            if (element.LayoutTypeY == GridLayoutType.EqualSpace) {
                return LayoutAlgorithms.Grid.EqualSpace.Measure(element, true, element.Rows, element.GapY, element.CellHeight)
            } else {
                return LayoutAlgorithms.Grid.Stack.Measure(element, true, element.Rows, element.GravityY, element.GapY)
            }
        },


        Width: e => e.InternalWidth,
        Height: e => e.InternalHeight,
    }

    element.HtmlNodesOrdered = new Enumerable(function* () {
        yield* element.ExistingChildren
        yield* element.UnmanagedChildren
    })


    



    



    










    //Y
    new Reaction(() => {

        if (element.LayoutTypeY == GridLayoutType.EqualSpace) {
            LayoutAlgorithms.Grid.EqualSpace.Layout(element, true, element.Rows, element.GravityY, element.GapY)
        } else {
            LayoutAlgorithms.Grid.Stack.Layout(element, true, element.Rows, element.GravityY, element.GapY)
        }



        //LayoutDirection(element, true, element.Rows, element.GravityY, element.GapY)
        
        //RegularGridLayout(element, element.Vertical, element.OrderedChildren, element.PrimaryGravity, element.PrimaryGap)

    })







    //X
    new Reaction(() => {

        //console.log("element.RowsTransposed", element.RowsTransposed)

        if (element.LayoutTypeX == GridLayoutType.EqualSpace) {
            LayoutAlgorithms.Grid.EqualSpace.Layout(element, false, element.RowsTransposed, element.GravityX, element.GapX)
        } else {
            LayoutAlgorithms.Grid.Stack.Layout(element, false, element.RowsTransposed, element.GravityX, element.GapX)
        }
        
    })

    



}
