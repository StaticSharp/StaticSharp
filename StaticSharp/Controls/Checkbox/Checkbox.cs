using StaticSharp.Gears;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StaticSharp {

    namespace Js {
        public interface Checkbox {
            public bool Enabled { get; }
            public bool Value { get; }
            public bool ValueActual { get; }
        }
    }

    public class CheckboxBindings<FinalJs> : Bindings<FinalJs> {
        public Binding<bool> Enabled { set { Apply(value); } }
        public Binding<bool> Value { set { Apply(value); } }

    }

    public static class Checkbox { 
         
    }
}