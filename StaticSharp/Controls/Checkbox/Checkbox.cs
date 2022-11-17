using StaticSharp.Gears;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StaticSharp {

    namespace Js {
        public class Checkbox {
            public bool Enabled => NotEvaluatableValue<bool>();
            public bool Value => NotEvaluatableValue<bool>();
            public bool ValueActual => NotEvaluatableValue<bool>();
        }
    }

    public class CheckboxBindings<FinalJs> : Bindings<FinalJs> where FinalJs : new() {
        public Binding<bool> Enabled { set { Apply(value); } }
        public Binding<bool> Value { set { Apply(value); } }

    }

    public static class Checkbox { 
         
    }
}