using StaticSharp.Gears;
using StaticSharp.Html;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaticSharp {

    namespace Gears {
        public static class HtmlModifierStatic {
            public static Tag AddAsChild(this Tag tag, string name = "") {
                tag["data-child"] = name;
                return tag;
            }
            public static Tag AssignParentProperty(this Tag tag, string name = "") {
                tag["data-property"] = name;
                return tag;
            }
        }
    }

    public class HtmlModifier: List<Func<Tag, Task>> {
        public async Task Apply(Tag input) {
            foreach (var action in this) {
                await action(input);
            }
        }
        public void Add(Action<Tag> action) {
            Add((input) => {
                action(input);
                return Task.CompletedTask;
            });
        }

        /*public HtmlModifier AddAsChild(string name = "") {
            Add((input) => input.AddAsChild(name));
            return this;
        }*/
        
        public HtmlModifier AssignParentProperty(string name = "") {
            Add((input) => input.AssignParentProperty(name));
            return this;
        }

    }
}