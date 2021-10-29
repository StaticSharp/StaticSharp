using System.Collections.Generic;
using System.Linq;
using System;
#pragma warning disable IDE1006 // Naming Styles
namespace DemoWebsite.Content {
	public class CsmlRoot : ProtoNode {
		public CsmlRoot(global::DemoWebsite.Language language) : base(language) {
		}
		public override CsmlRoot WithLanguage(global::DemoWebsite.Language language) {
			return new CsmlRoot(language);
		}
		public override ProtoNode Parent => null;
		public override CsmlRoot Root => new CsmlRoot(Language);
		public override string[] Path => new string[]{};
		public override string Name => "Content";
		public override CsmlEngine.IRepresentative Representative => null;
		public override IEnumerable<ProtoNode> Children {
			get {
				yield return Index;
			}
		}
		public virtual αIndex Index => new(Language);
		public class αIndex : ProtoNode, CsmlEngine.ITypedRepresentativeProvider<global::DemoWebsite.Content.Index.Common> {
			public αIndex(global::DemoWebsite.Language language) : base(language) {
			}
			public override αIndex WithLanguage(global::DemoWebsite.Language language) {
				return new αIndex(language);
			}
			public override ProtoNode Parent => new CsmlRoot(Language);
			public override CsmlRoot Root => new CsmlRoot(Language);
			public override string[] Path => new string[]{"Index"};
			public override string Name => "Index";
			public static implicit operator global::DemoWebsite.Content.Index.Common(αIndex α) => α.Representative;
			public override global::DemoWebsite.Content.Index.Common Representative => SelectRepresentative(Representatives);
			public IEnumerable<global::DemoWebsite.Content.Index.Common> Representatives {
				get {
					yield return new global::DemoWebsite.Content.Index.En(Language);
					yield return new global::DemoWebsite.Content.Index.Ru(Language);
				}
			}
			public override IEnumerable<ProtoNode> Children {
				get {
					yield return Articles;
					yield return Katya;
				}
			}
			public virtual αArticles Articles => new(Language);
			public class αArticles : ProtoNode {
				public αArticles(global::DemoWebsite.Language language) : base(language) {
				}
				public override αArticles WithLanguage(global::DemoWebsite.Language language) {
					return new αArticles(language);
				}
				public override ProtoNode Parent => new αIndex(Language);
				public override CsmlRoot Root => new CsmlRoot(Language);
				public override string[] Path => new string[]{"Index","Articles"};
				public override string Name => "Articles";
				public override CsmlEngine.IRepresentative Representative => null;
				public override IEnumerable<ProtoNode> Children {
					get {
						yield return Terms;
					}
				}
				public virtual αTerms Terms => new(Language);
				public class αTerms : ProtoNode, CsmlEngine.ITypedRepresentativeProvider<global::DemoWebsite.Content.Index.Articles.Terms.Common> {
					public αTerms(global::DemoWebsite.Language language) : base(language) {
					}
					public override αTerms WithLanguage(global::DemoWebsite.Language language) {
						return new αTerms(language);
					}
					public override ProtoNode Parent => new αArticles(Language);
					public override CsmlRoot Root => new CsmlRoot(Language);
					public override string[] Path => new string[]{"Index","Articles","Terms"};
					public override string Name => "Terms";
					public static implicit operator global::DemoWebsite.Content.Index.Articles.Terms.Common(αTerms α) => α.Representative;
					public override global::DemoWebsite.Content.Index.Articles.Terms.Common Representative => SelectRepresentative(Representatives);
					public IEnumerable<global::DemoWebsite.Content.Index.Articles.Terms.Common> Representatives {
						get {
							yield return new global::DemoWebsite.Content.Index.Articles.Terms.En(Language);
							yield return new global::DemoWebsite.Content.Index.Articles.Terms.Fr(Language);
							yield return new global::DemoWebsite.Content.Index.Articles.Terms.Ru(Language);
						}
					}
					public override IEnumerable<ProtoNode> Children => Enumerable.Empty<ProtoNode>();
				}
			}
			public virtual αKatya Katya => new(Language);
			public class αKatya : ProtoNode, CsmlEngine.ITypedRepresentativeProvider<global::DemoWebsite.Content.Index.Katya.Common> {
				public αKatya(global::DemoWebsite.Language language) : base(language) {
				}
				public override αKatya WithLanguage(global::DemoWebsite.Language language) {
					return new αKatya(language);
				}
				public override ProtoNode Parent => new αIndex(Language);
				public override CsmlRoot Root => new CsmlRoot(Language);
				public override string[] Path => new string[]{"Index","Katya"};
				public override string Name => "Katya";
				public static implicit operator global::DemoWebsite.Content.Index.Katya.Common(αKatya α) => α.Representative;
				public override global::DemoWebsite.Content.Index.Katya.Common Representative => new global::DemoWebsite.Content.Index.Katya.MyPage(Language);
				public override IEnumerable<ProtoNode> Children => Enumerable.Empty<ProtoNode>();
			}
		}
	}
}
