namespace StaticSharpDemo.Root {
	public partial class Material : StaticSharpEngine.IRepresentative {
		protected virtual ProtoNode VirtualNode => Node;
		αRoot Node => new(Language);
		StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
		public global::StaticSharpDemo.Language Language{get; init;}
		public Material(global::StaticSharpDemo.Language language) {
			Language = language;
		}
	}
	public partial class Ru : StaticSharpEngine.IRepresentative {
		protected override ProtoNode VirtualNode => Node;
		αRoot Node => new(Language);
		StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
		public Ru(global::StaticSharpDemo.Language language) : base(language) {
		}
	}
	namespace Components {
		public partial class Ru : StaticSharpEngine.IRepresentative {
			protected override ProtoNode VirtualNode => Node;
			αRoot.αComponents Node => new(Language);
			StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
			public Ru(global::StaticSharpDemo.Language language) : base(language) {
			}
		}
	}
	namespace Customization {
		namespace HowToCreateNewComponent {
			public partial class Ru : StaticSharpEngine.IRepresentative {
				protected override ProtoNode VirtualNode => Node;
				αRoot.αCustomization.αHowToCreateNewComponent Node => new(Language);
				StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
				public Ru(global::StaticSharpDemo.Language language) : base(language) {
				}
			}
		}
	}
	namespace Legacy {
		public partial class Common : StaticSharpEngine.IRepresentative {
			protected override ProtoNode VirtualNode => Node;
			αRoot.αLegacy Node => new(Language);
			StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
			public Common(global::StaticSharpDemo.Language language) : base(language) {
			}
		}
		public partial class En : StaticSharpEngine.IRepresentative {
			protected override ProtoNode VirtualNode => Node;
			αRoot.αLegacy Node => new(Language);
			StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
			public En(global::StaticSharpDemo.Language language) : base(language) {
			}
		}
		public partial class Ru : StaticSharpEngine.IRepresentative {
			protected override ProtoNode VirtualNode => Node;
			αRoot.αLegacy Node => new(Language);
			StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
			public Ru(global::StaticSharpDemo.Language language) : base(language) {
			}
		}
		namespace Articles {
			public partial class _ : StaticSharpEngine.IRepresentative {
				protected override ProtoNode VirtualNode => Node;
				αRoot.αLegacy.αArticles Node => new(Language);
				StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
				public _(global::StaticSharpDemo.Language language) : base(language) {
				}
			}
			namespace Terms {
				public partial class Common : StaticSharpEngine.IRepresentative {
					protected override ProtoNode VirtualNode => Node;
					αRoot.αLegacy.αArticles.αTerms Node => new(Language);
					StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
					public Common(global::StaticSharpDemo.Language language) : base(language) {
					}
				}
				public partial class En : StaticSharpEngine.IRepresentative {
					protected override ProtoNode VirtualNode => Node;
					αRoot.αLegacy.αArticles.αTerms Node => new(Language);
					StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
					public En(global::StaticSharpDemo.Language language) : base(language) {
					}
				}
				public partial class Fr : StaticSharpEngine.IRepresentative {
					protected override ProtoNode VirtualNode => Node;
					αRoot.αLegacy.αArticles.αTerms Node => new(Language);
					StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
					public Fr(global::StaticSharpDemo.Language language) : base(language) {
					}
				}
				public partial class Ru : StaticSharpEngine.IRepresentative {
					protected override ProtoNode VirtualNode => Node;
					αRoot.αLegacy.αArticles.αTerms Node => new(Language);
					StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
					public Ru(global::StaticSharpDemo.Language language) : base(language) {
					}
				}
			}
		}
		namespace Katya {
			public partial class Common : StaticSharpEngine.IRepresentative {
				protected override ProtoNode VirtualNode => Node;
				αRoot.αLegacy.αKatya Node => new(Language);
				StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
				public Common(global::StaticSharpDemo.Language language) : base(language) {
				}
			}
			public partial class MyPage : StaticSharpEngine.IRepresentative {
				protected override ProtoNode VirtualNode => Node;
				αRoot.αLegacy.αKatya Node => new(Language);
				StaticSharpEngine.INode StaticSharpEngine.IRepresentative.Node => Node;
				public MyPage(global::StaticSharpDemo.Language language) : base(language) {
				}
			}
		}
	}
}
