using StaticSharp.Gears;

namespace StaticSharp;




public interface JGridLayout : JBlockWithChildren {

    //GridLayout.Order ItemsOrder { get; set; }
    int NumColumns { get; set; }
    int NumRows { get; set; }
    int CalculatedNumRows { get; }

    GridLayout.LayoutType LayoutTypeX { get; set; }
    GridLayout.LayoutType LayoutTypeY { get; set; }


    double GapX { get; set; }

    double GapY { get; set; }

    double GravityX { get; set; }
    double GravityY { get; set; }


    double CellWidth { get; set; }
    double CellHeight { get; set; }

    double InternalWidth { get; }
    double InternalHeight { get; }
}






[ConstructorJs]
[Scripts.LayoutAlgorithms.Grid.EqualSpace]
[Scripts.LayoutAlgorithms.Grid.Stack]
public partial class GridLayout : BlockWithChildren {
    /*public enum Order {
        LeftToRight_TopToBottom,
    }*/

    public enum LayoutType {
        Stack,
        EqualSpace
    }
}