using StaticSharp.Gears;
using System.Runtime.CompilerServices;

namespace StaticSharp {

    public enum CursorOption {
        Default,
        None,
        ContextMenu,
        Help,
        Pointer,
        Progress,
        Wait,
        Cell,
        Crosshair,
        Text,
        VerticalText,
        Alias,
        Copy,
        Move,
        NoDrop,
        NotAllowed,
        Grab,
        Grabbing,
        EResize,
        NResize,
        NeResize,
        NwResize,
        SResize,
        SeResize,
        SwResize,
        WResize,
        EwResize,
        NsResize,
        NeswResize,
        NwseResize
    }
    public interface JCursor : JModifier {
        public CursorOption Option { get; set; }
    }


    [ConstructorJs]
    public partial class Cursor : Modifier {        
        public Cursor(CursorOption option, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
            : this(callerLineNumber, callerFilePath) {
            Option = option;
        }
    }


}


