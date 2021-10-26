namespace DemoWebsite.Content {
	public partial class Material : CsmlEngine.IRepresentative {
		protected virtual ProtoNode VirtualNode => Node;
		CsmlRoot Node => new(Language);
		CsmlEngine.INode CsmlEngine.IRepresentative.Node => Node;
		public global::DemoWebsite.Language Language{get; init;}
		public Material(global::DemoWebsite.Language language) {
			Language = language;
		}
	}
	namespace Index {
		public partial class Common : CsmlEngine.IRepresentative {
			protected override ProtoNode VirtualNode => Node;
			CsmlRoot.αIndex Node => new(Language);
			CsmlEngine.INode CsmlEngine.IRepresentative.Node => Node;
			public Common(global::DemoWebsite.Language language) : base(language) {
			}
		}
		public partial class En : CsmlEngine.IRepresentative {
			protected override ProtoNode VirtualNode => Node;
			CsmlRoot.αIndex Node => new(Language);
			CsmlEngine.INode CsmlEngine.IRepresentative.Node => Node;
			public En(global::DemoWebsite.Language language) : base(language) {
			}
		}
		public partial class Ru : CsmlEngine.IRepresentative {
			protected override ProtoNode VirtualNode => Node;
			CsmlRoot.αIndex Node => new(Language);
			CsmlEngine.INode CsmlEngine.IRepresentative.Node => Node;
			public Ru(global::DemoWebsite.Language language) : base(language) {
			}
		}
		namespace Articles {
			namespace Terms {
				public partial class Common : CsmlEngine.IRepresentative {
					protected override ProtoNode VirtualNode => Node;
					CsmlRoot.αIndex.αArticles.αTerms Node => new(Language);
					CsmlEngine.INode CsmlEngine.IRepresentative.Node => Node;
					public Common(global::DemoWebsite.Language language) : base(language) {
					}
				}
				public partial class En : CsmlEngine.IRepresentative {
					protected override ProtoNode VirtualNode => Node;
					CsmlRoot.αIndex.αArticles.αTerms Node => new(Language);
					CsmlEngine.INode CsmlEngine.IRepresentative.Node => Node;
					public En(global::DemoWebsite.Language language) : base(language) {
					}
				}
				public partial class Ru : CsmlEngine.IRepresentative {
					protected override ProtoNode VirtualNode => Node;
					CsmlRoot.αIndex.αArticles.αTerms Node => new(Language);
					CsmlEngine.INode CsmlEngine.IRepresentative.Node => Node;
					public Ru(global::DemoWebsite.Language language) : base(language) {
					}
				}
			}
		}
	}
}
