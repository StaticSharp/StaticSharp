using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StaticSharp {


    public class Group: CallerInfo,  IBlockCollector {//TODO: delete? use Blocks?

        public Blocks Children { get; } = new();

        public Group([CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
            : base(callerFilePath, callerLineNumber) {
        
        }        

        public void Add(string? id, IBlock? value) {
            if (value!=null)
                Children.Add(id,value);
        }


    }
}
