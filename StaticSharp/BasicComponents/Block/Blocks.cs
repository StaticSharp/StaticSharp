using StaticSharp.Gears;
using System.Collections.Generic;

namespace StaticSharp {


    public class Blocks : List<KeyValuePair<string?, IBlock>>, IBlockCollector {
        public Blocks(): base() { }
        public Blocks(Blocks other): base(other) {}
        public void Add(string? id, IBlock? block) {
            if (block != null) {
                Add(new KeyValuePair<string?, IBlock>(id, block));
            }            
        }
    }
    

    /*public sealed class Item : Item<Symbolic.Item> {
        public Item([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) { }
    }*/
}