using System.Collections.Generic;
using System.Linq;
using System;
#pragma warning disable IDE1006 // Naming Styles
namespace RoutingSg.TestProject.Root {
	public class αRoot : ProtoNode {
		public αRoot(global::RoutingSg.TestProject.Root.Language language) : base(language) {
		}
		public override αRoot WithLanguage(global::RoutingSg.TestProject.Root.Language language) {
			return new αRoot(language);
		}
		public override ProtoNode Parent => null;
		public override αRoot Root => new αRoot(Language);
		public override string[] Path => new string[]{};
		public override string Name => "Root";
		public override StaticSharp.Page Representative => null;
		public override IEnumerable<ProtoNode> Children => Enumerable.Empty<ProtoNode>();
	}
}
