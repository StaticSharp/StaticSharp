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
				yield return Legacy;
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
		public virtual αLegacy Legacy => new(Language);
		public class αLegacy : ProtoNode, StaticSharpEngine.ITypedRepresentativeProvider<global::StaticSharpDemo.Root.Legacy.Common> {
			public αLegacy(global::StaticSharpDemo.Language language) : base(language) {
			}
			public override αLegacy WithLanguage(global::StaticSharpDemo.Language language) {
				return new αLegacy(language);
			}
			public override ProtoNode Parent => new αRoot(Language);
			public override αRoot Root => new αRoot(Language);
			public override string[] Path => new string[]{"Legacy"};
			public override string Name => "Legacy";
			public static implicit operator global::StaticSharpDemo.Root.Legacy.Common(αLegacy α) => α.Representative;
			public override global::StaticSharpDemo.Root.Legacy.Common Representative => SelectRepresentative(Representatives);
			public IEnumerable<global::StaticSharpDemo.Root.Legacy.Common> Representatives {
				get {
					yield return new global::StaticSharpDemo.Root.Legacy.En(Language);
					yield return new global::StaticSharpDemo.Root.Legacy.Ru(Language);
				}
			}
			public override IEnumerable<ProtoNode> Children {
				get {
					yield return Articles;
					yield return Katya;
				}
			}
			public virtual αArticles Articles => new(Language);
			public class αArticles : ProtoNode, StaticSharpEngine.ITypedRepresentativeProvider<global::StaticSharpDemo.Root.Material> {
				public αArticles(global::StaticSharpDemo.Language language) : base(language) {
				}
				public override αArticles WithLanguage(global::StaticSharpDemo.Language language) {
					return new αArticles(language);
				}
				public override ProtoNode Parent => new αLegacy(Language);
				public override αRoot Root => new αRoot(Language);
				public override string[] Path => new string[]{"Legacy","Articles"};
				public override string Name => "Articles";
				public static implicit operator global::StaticSharpDemo.Root.Material(αArticles α) => α.Representative;
				public override global::StaticSharpDemo.Root.Material Representative => new global::StaticSharpDemo.Root.Legacy.Articles._(Language);
				public override IEnumerable<ProtoNode> Children {
					get {
						yield return Terms;
					}
				}
				public virtual αTerms Terms => new(Language);
				public class αTerms : ProtoNode, StaticSharpEngine.ITypedRepresentativeProvider<global::StaticSharpDemo.Root.Legacy.Articles.Terms.Common> {
					public αTerms(global::StaticSharpDemo.Language language) : base(language) {
					}
					public override αTerms WithLanguage(global::StaticSharpDemo.Language language) {
						return new αTerms(language);
					}
					public override ProtoNode Parent => new αArticles(Language);
					public override αRoot Root => new αRoot(Language);
					public override string[] Path => new string[]{"Legacy","Articles","Terms"};
					public override string Name => "Terms";
					public static implicit operator global::StaticSharpDemo.Root.Legacy.Articles.Terms.Common(αTerms α) => α.Representative;
					public override global::StaticSharpDemo.Root.Legacy.Articles.Terms.Common Representative => SelectRepresentative(Representatives);
					public IEnumerable<global::StaticSharpDemo.Root.Legacy.Articles.Terms.Common> Representatives {
						get {
							yield return new global::StaticSharpDemo.Root.Legacy.Articles.Terms.En(Language);
							yield return new global::StaticSharpDemo.Root.Legacy.Articles.Terms.Fr(Language);
							yield return new global::StaticSharpDemo.Root.Legacy.Articles.Terms.Ru(Language);
						}
					}
					public override IEnumerable<ProtoNode> Children => Enumerable.Empty<ProtoNode>();
				}
			}
			public virtual αKatya Katya => new(Language);
			public class αKatya : ProtoNode, StaticSharpEngine.ITypedRepresentativeProvider<global::StaticSharpDemo.Root.Legacy.Katya.Common> {
				public αKatya(global::StaticSharpDemo.Language language) : base(language) {
				}
				public override αKatya WithLanguage(global::StaticSharpDemo.Language language) {
					return new αKatya(language);
				}
				public override ProtoNode Parent => new αLegacy(Language);
				public override αRoot Root => new αRoot(Language);
				public override string[] Path => new string[]{"Legacy","Katya"};
				public override string Name => "Katya";
				public static implicit operator global::StaticSharpDemo.Root.Legacy.Katya.Common(αKatya α) => α.Representative;
				public override global::StaticSharpDemo.Root.Legacy.Katya.Common Representative => new global::StaticSharpDemo.Root.Legacy.Katya.MyPage(Language);
				public override IEnumerable<ProtoNode> Children => Enumerable.Empty<ProtoNode>();
			}
		}
	}
}
