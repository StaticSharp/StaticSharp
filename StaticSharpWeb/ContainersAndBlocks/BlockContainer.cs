using System.Collections;
using System.Collections.Generic;

namespace StaticSharpWeb;

public class BlockContainer : IEnumerable<IBlock>, IBlockContainer {
    private readonly List<IBlock> _items = new();

    IEnumerator IEnumerable.GetEnumerator() {
        return _items.GetEnumerator();
    }

    public IEnumerator<IBlock> GetEnumerator() {
        return _items.GetEnumerator();
    }

    public void AddBlock(IBlock block) {
        _items.Add(block);
    }

}
