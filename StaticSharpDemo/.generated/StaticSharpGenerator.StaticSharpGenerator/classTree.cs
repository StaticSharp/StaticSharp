using System.Collections.Generic;
using System.Linq;
using System;
#pragma warning disable IDE1006 // Naming Styles
namespace StaticSharpDemo.Root {
	public class αRoot : ProtoNode, StaticSharpEngine.ITypedRepresentativeProvider<global::StaticSharpDemo.Root.Material> {
		public αRoot(global::StaticSharpDemo.Language language) : base(language) {
		}
		public override αRoot WithLanguage(global::StaticSharpDemo.Language language) {
			return new αRoot(language);
		}
		public override ProtoNode Parent => null;
		public override αRoot Root => new αRoot(Language);
		public override string[] Path => new string[]{};
		public override string Name => "Root";
		public static implicit operator global::StaticSharpDemo.Root.Material(αRoot α) => α.Representative;
		public override global::StaticSharpDemo.Root.Material Representative => new global::StaticSharpDemo.Root.Ru(Language);
		public override IEnumerable<ProtoNode> Children {
			get {
				yield return Components;
				yield return Customization;
			}
		}
		public virtual αComponents Components => new(Language);
		public class αComponents : ProtoNode, StaticSharpEngine.ITypedRepresentativeProvider<global::StaticSharpDemo.Root.Material> {
			public αComponents(global::StaticSharpDemo.Language language) : base(language) {
			}
			public override αComponents WithLanguage(global::StaticSharpDemo.Language language) {
				return new αComponents(language);
			}
			public override ProtoNode Parent => new αRoot(Language);
			public override αRoot Root => new αRoot(Language);
			public override string[] Path => new string[]{"Components"};
			public override string Name => "Components";
			public static implicit operator global::StaticSharpDemo.Root.Material(αComponents α) => α.Representative;
			public override global::StaticSharpDemo.Root.Material Representative => new global::StaticSharpDemo.Root.Components.Ru(Language);
			public override IEnumerable<ProtoNode> Children => Enumerable.Empty<ProtoNode>();
		}
		public virtual αCustomization Customization => new(Language);
		public class αCustomization : ProtoNode {
			public αCustomization(global::StaticSharpDemo.Language language) : base(language) {
			}
			public override αCustomization WithLanguage(global::StaticSharpDemo.Language language) {
				return new αCustomization(language);
			}
			public override ProtoNode Parent => new αRoot(Language);
			public override αRoot Root => new αRoot(Language);
			public override string[] Path => new string[]{"Customization"};
			public override string Name => "Customization";
			public override StaticSharpEngine.IRepresentative Representative => null;
			public override IEnumerable<ProtoNode> Children {
				get {
					yield return HowToCreateNewComponent;
				}
			}
			public virtual αHowToCreateNewComponent HowToCreateNewComponent => new(Language);
			public class αHowToCreateNewComponent : ProtoNode, StaticSharpEngine.ITypedRepresentativeProvider<global::StaticSharpDemo.Root.Material> {
				public αHowToCreateNewComponent(global::StaticSharpDemo.Language language) : base(language) {
				}
				public override αHowToCreateNewComponent WithLanguage(global::StaticSharpDemo.Language language) {
					return new αHowToCreateNewComponent(language);
				}
				public override ProtoNode Parent => new αCustomization(Language);
				public override αRoot Root => new αRoot(Language);
				public override string[] Path => new string[]{"Customization","HowToCreateNewComponent"};
				public override string Name => "HowToCreateNewComponent";
				public static implicit operator global::StaticSharpDemo.Root.Material(αHowToCreateNewComponent α) => α.Representative;
				public override global::StaticSharpDemo.Root.Material Representative => new global::StaticSharpDemo.Root.Customization.HowToCreateNewComponent.Ru(Language);
				public override IEnumerable<ProtoNode> Children => Enumerable.Empty<ProtoNode>();
			}
		}
	}
}
